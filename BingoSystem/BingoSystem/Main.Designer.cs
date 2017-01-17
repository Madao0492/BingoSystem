namespace BingoSystem
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lotteryNumber = new System.Windows.Forms.TextBox();
            this.lotteryButton = new System.Windows.Forms.Button();
            this.selectedNumber = new System.Windows.Forms.RichTextBox();
            this.continueButton = new System.Windows.Forms.Button();
            this.soundCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lotteryNumber
            // 
            this.lotteryNumber.BackColor = System.Drawing.Color.White;
            this.lotteryNumber.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 99.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lotteryNumber.Location = new System.Drawing.Point(806, 12);
            this.lotteryNumber.Name = "lotteryNumber";
            this.lotteryNumber.ReadOnly = true;
            this.lotteryNumber.Size = new System.Drawing.Size(166, 140);
            this.lotteryNumber.TabIndex = 1;
            this.lotteryNumber.TabStop = false;
            this.lotteryNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lotteryButton
            // 
            this.lotteryButton.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lotteryButton.Location = new System.Drawing.Point(806, 320);
            this.lotteryButton.Name = "lotteryButton";
            this.lotteryButton.Size = new System.Drawing.Size(166, 113);
            this.lotteryButton.TabIndex = 0;
            this.lotteryButton.Text = "抽　選";
            this.lotteryButton.UseVisualStyleBackColor = true;
            this.lotteryButton.Click += new System.EventHandler(this.lotteryButton_Click);
            // 
            // selectedNumber
            // 
            this.selectedNumber.BackColor = System.Drawing.Color.White;
            this.selectedNumber.Font = new System.Drawing.Font("ＭＳ ゴシック", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.selectedNumber.Location = new System.Drawing.Point(13, 13);
            this.selectedNumber.Name = "selectedNumber";
            this.selectedNumber.ReadOnly = true;
            this.selectedNumber.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.selectedNumber.Size = new System.Drawing.Size(787, 420);
            this.selectedNumber.TabIndex = 0;
            this.selectedNumber.TabStop = false;
            this.selectedNumber.Text = "";
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(806, 291);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(166, 23);
            this.continueButton.TabIndex = 2;
            this.continueButton.Text = "前回の続きから";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // soundCheckBox
            // 
            this.soundCheckBox.AutoSize = true;
            this.soundCheckBox.Checked = true;
            this.soundCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.soundCheckBox.Location = new System.Drawing.Point(912, 269);
            this.soundCheckBox.Name = "soundCheckBox";
            this.soundCheckBox.Size = new System.Drawing.Size(60, 16);
            this.soundCheckBox.TabIndex = 3;
            this.soundCheckBox.Text = "効果音";
            this.soundCheckBox.UseVisualStyleBackColor = true;
            this.soundCheckBox.CheckedChanged += new System.EventHandler(this.soundCheckBox_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 445);
            this.Controls.Add(this.soundCheckBox);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.selectedNumber);
            this.Controls.Add(this.lotteryButton);
            this.Controls.Add(this.lotteryNumber);
            this.Name = "Main";
            this.Text = "MainView - BingoSystem";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox lotteryNumber;
        private System.Windows.Forms.Button lotteryButton;
        private System.Windows.Forms.RichTextBox selectedNumber;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.CheckBox soundCheckBox;
    }
}

