using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BingoSystem
{
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

        private void lotteryButton_Click(object sender, EventArgs e)
        {
            if (lotteryIndex < 75) //抽選範囲内
            {
                lotteryNumber.ForeColor = Color.Black;
                for (int i = 0; i < 50; i++) //5秒で抽選結果が出てくる
                {
                    ShowRandomNum();
                    lotteryNumber.Refresh();
                }

                lotteryNumber.ForeColor = Color.Red;
                lotteryNumber.Text = lNumber[lotteryIndex].ToString();

                InsertLotteryNumToSelectedNumber(lotteryIndex);

                lotteryIndex++;
            }

            if(lotteryIndex >= 75)
            {
                MessageBox.Show("抽選はすべて終了しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MakeAndRandomizeNumber()
        {
            for (int i = 1; i <= 75; i++) //ビンゴは1～75までで抽選する
            {
                lNumber.Add(i);
            }

            lNumber = lNumber.OrderBy(i => Guid.NewGuid()).ToList(); //リストの中身をかき混ぜる
        }

        private void ShowRandomNum()
        {
            Random rnd = new Random();
            int tmpNumber = rnd.Next(74) + 1; //0～74を1～75に変化

            lotteryNumber.Text = tmpNumber.ToString();
            Thread.Sleep(100);
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
    }
}
