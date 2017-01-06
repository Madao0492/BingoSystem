using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
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
        int lotteryIndex = 0; //今何番目の数字なのかのインデックス

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MakeAndRandomizeNumber(); //ビンゴの数字生成＆並び替え
        }

        private async void lotteryButton_Click(object sender, EventArgs e)
        {
            lotteryButton.Enabled = false;

            if (lotteryIndex < 75) //抽選範囲内
            {
                lotteryNumber.ForeColor = Color.Black;
                for (int i = 0; i < 30; i++) //最初の3秒は速い画面変化
                {
                    await Task.Run(() => PlaySound("button71.mp3"));
                    ShowRandomNum(100);
                    lotteryNumber.Refresh();
                }
                for (int i = 0; i < 20; i++) //あとの2秒は遅い画面変化
                {
                    await Task.Run(() => PlaySound("button71.mp3"));
                    ShowRandomNum(300);
                    lotteryNumber.Refresh();
                }

                lotteryNumber.ForeColor = Color.Red;
                lotteryNumber.Text = lNumber[lotteryIndex].ToString();

                PlaySound("one35.mp3");

                InsertLotteryNumToSelectedNumber(lotteryIndex);

                lotteryButton.Focus();

                lotteryIndex++;
            }

            if(lotteryIndex >= 75)
            {
                MessageBox.Show("抽選はすべて終了しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            lotteryButton.Enabled = true;
        }

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

        private void InsertLotteryNumToSelectedNumber(int index)
        {
            string tmpNum;

            if(lNumber[index] < 10)
            {
                tmpNum = " " + lNumber[index];
            }
            else
            {
                tmpNum = lNumber[index].ToString();
            }

            selectedNumber.AppendText(tmpNum);

            if(index % 6 == 5) //数字6個ごとに改行
            {
                selectedNumber.AppendText(Environment.NewLine);
            }
            else //改行しない場合は文字列後ろにスペース挿入
            {
                selectedNumber.AppendText(" ");
            }
        }

        private void PlaySound(string fileName)
        {
            WindowsMediaPlayer mp = new WMPLib.WindowsMediaPlayer(); //音楽再生用変数
            mp.URL = fileName;
            mp.settings.volume = 100;
            mp.controls.play();
        }
    }
}
