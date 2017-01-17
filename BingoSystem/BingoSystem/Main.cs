using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace BingoSystem
{
    /* 音素材入手先 */
    /* http://www.kurage-kosho.info/ */

    public partial class Main : Form
    {
        List<int> lNumber = new List<int>(); //抽選数字が挿入されるリスト
        int lotteryIndex = -1; //今何番目の数字なのかのインデックス

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0x000B;

        public Main()
        {
            InitializeComponent();
        }

        #region Formイベント
        private void Form1_Load(object sender, EventArgs e)
        {
            MakeAndRandomizeNumber(); //ビンゴの数字生成＆並び替え
            selectedNumber.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            lotteryNumber.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            lotteryButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            continueButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            selectedNumber.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            InsertSelectedNumber();
            FitNumberToLotteryNum();
        }

        private async void lotteryButton_Click(object sender, EventArgs e)
        {
            lotteryButton.Enabled = false;
            lotteryIndex++;

            continueButton.Enabled = false;

            if (lotteryIndex < 75) //抽選範囲内
            {
                lotteryNumber.ForeColor = Color.Black;
                for (int i = 0; i < 30; i++) //最初の3秒は速い画面変化
                {
                    await Task.Run(() => PlaySound("button71.mp3"));
                    ShowRandomNum(100);
                    lotteryNumber.Refresh();
                }
                for (int i = 0; i < 5; i++) //あとの2秒は遅い画面変化
                {
                    await Task.Run(() => PlaySound("button71.mp3"));
                    ShowRandomNum(400);
                    lotteryNumber.Refresh();
                }

                lotteryNumber.ForeColor = Color.Red;
                lotteryNumber.Text = lNumber[lotteryIndex].ToString();

                await Task.Run(() => PlaySound("one35.mp3"));

                RefreshSelectedNumber(lotteryIndex);
                SaveLottery();
                lotteryButton.Focus();

            }

            if(lotteryIndex >= 75)
            {
                MessageBox.Show("抽選はすべて終了しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            lotteryButton.Enabled = true;
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            string logFileName = "bingo.log";

            if (File.Exists(logFileName))
            {
                if (!RestoreLottery(logFileName))
                {
                    MessageBox.Show("前回終了時の状態を復元しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ログファイルが不正です", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("ログファイルが見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            FitNumberToLotteryNum();
        }
        #endregion

        #region ビンゴ番号生成（リスト・ランダム）
        private void MakeAndRandomizeNumber()
        {
            for (int i = 1; i <= 75; i++) //ビンゴは1～75までで抽選する
            {
                lNumber.Add(i);
            }

            lNumber = lNumber.OrderBy(i => Guid.NewGuid()).ToList(); //リストの中身をかき混ぜる
        }

        private void ShowRandomNum(int interval)
        {
            Random rnd = new Random();
            int tmpNumber = rnd.Next(74) + 1; //0～74を1～75に変化

            lotteryNumber.Text = tmpNumber.ToString();
            Thread.Sleep(interval);
        }
        #endregion

        #region 抽選済み番号表示
        private void InsertSelectedNumber()
        {
            selectedNumber.Text = "";
            string tmpNum = "";

            BeginControlUpdate(selectedNumber);

            for(int i = 1; i <= 75; i++)
            {
                if (i < 10)
                {
                    tmpNum = "0" + i;
                }
                else
                {
                    tmpNum = i.ToString();
                }

                selectedNumber.AppendText(tmpNum);

                if (i % 10 == 0) //数字10個ごとに改行
                {
                    selectedNumber.AppendText(Environment.NewLine);
                }
                else //改行しない場合は文字列後ろにスペース挿入
                {
                    selectedNumber.AppendText(" ");
                }
            }

            EndControlUpdate(selectedNumber);
        }

        private void RefreshSelectedNumber(int index)
        {

            if (index < 0)
            {
                return;
            }
            else
            {
                //現在の選択状態を覚えておく
                int currentSelectionStart = selectedNumber.SelectionStart;
                int currentSelectionLength = selectedNumber.SelectionLength;

                string selectedNum = "";

                for (int i = 0; i <= index; i++)
                {
                    if (lNumber[i] < 10)
                    {
                        selectedNum = "0" + lNumber[i];
                    }
                    else
                    {
                        selectedNum = lNumber[i].ToString();
                    }
                    selectedNumber.Find(selectedNum, 0, RichTextBoxFinds.None);
                    selectedNumber.SelectionColor = Color.Red;
                }

                //選択状態を元に戻す
                selectedNumber.Select(currentSelectionStart, currentSelectionLength);

            }
        }
        #endregion

        #region フォントサイズの更新
        private void FitNumberToLotteryNum()
        {
            Graphics g = selectedNumber.CreateGraphics();

            Font realFont = new Font(selectedNumber.Font.FontFamily, 100);

            SizeF size = g.MeasureString(selectedNumber.Text, realFont);
            while (size.Width >= selectedNumber.Width || size.Height >= selectedNumber.Height)
            {
                if (realFont.Size <= 1)
                    break;

                realFont = new Font(realFont.FontFamily, realFont.Size - 1);
                size = g.MeasureString(selectedNumber.Text, realFont);
            }

            selectedNumber.Font = realFont;

            RefreshSelectedNumber(lotteryIndex);
            
        }
        #endregion

        #region コントロールの更新抑制
        /// <summary>
        /// コントロールの再描画を停止させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void BeginControlUpdate(Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// コントロールの再描画を再開させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EndControlUpdate(Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            control.Invalidate();
        }
        #endregion

        #region 音再生
        private void PlaySound(string fileName)
        {
            try
            {
                WindowsMediaPlayer mp = new WindowsMediaPlayer();
                mp.URL = fileName;
                mp.settings.volume = 100;
                mp.controls.play();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region ビンゴ設定の復元
        private bool RestoreLottery(string str)
        {
            string logFileName = str;
            bool isError = false;

            lNumber.Clear();
            try
            {
                lNumber = GetLotterySequence(logFileName);
                lotteryIndex = GetLotteryIndex(logFileName); //本当はJSONのほうが効率的だが…
                RefreshSelectedNumber(lotteryIndex);
            }
            catch (Exception)
            {
                isError = true;
                MakeAndRandomizeNumber();
                lotteryIndex = -1;
            }

            return isError;
        }

        private List<int> GetLotterySequence(string str)
        {
            StreamReader sr = new StreamReader(str, Encoding.GetEncoding("shift_jis"));
            int line = 1;

            string[] lNumber_tmp = new string[75];
            string str_tmp;

            List<int> lNumber = new List<int>();

            while (sr.Peek() > -1)
            {
                str_tmp = sr.ReadLine();
                if(line == 3)
                {
                    lNumber_tmp = str_tmp.Split(',');
                    break;
                }
                line++;
            }
            sr.Close();

            for(int i = 0; i < lNumber_tmp.Length; i++)
            {
                lNumber.Add(int.Parse(lNumber_tmp[i]));
            }

            if(lNumber.Count != 75)
            {
                throw new FormatException();
            }

            return lNumber;
        }

        private int GetLotteryIndex(string str)
        {
            StreamReader sr = new StreamReader(str, Encoding.GetEncoding("shift_jis"));
            int line = 1;

            string str_tmp;

            int lotteryIndex = -1;

            while (sr.Peek() > -1)
            {
                str_tmp = sr.ReadLine();
                if (line == 1)
                {
                    lotteryIndex = int.Parse(str_tmp.Remove(0,15));
                    break;
                }
                line++;
            }
            sr.Close();

            if(lotteryIndex < -1)
            {
                throw new FormatException();
            }

            return lotteryIndex;
        }
        #endregion

        #region ビンゴ設定の保存
        private void SaveLottery()
        {
            StreamWriter sw = new StreamWriter("bingo.log", false, Encoding.GetEncoding("shift_jis"));

            string lNumber_tmp = "";

            sw.WriteLine("lotteryIndex = " + lotteryIndex);
            sw.WriteLine("lNumber = {");

            for(int i = 0;i < lNumber.Count; i++)
            {
                lNumber_tmp += lNumber[i] + ",";
            }

            lNumber_tmp = lNumber_tmp.Remove(lNumber_tmp.Length - 1, 1); //行末,を除去

            sw.WriteLine(lNumber_tmp);
            sw.WriteLine("}");

            sw.Close();
        }
        #endregion
    }
}
