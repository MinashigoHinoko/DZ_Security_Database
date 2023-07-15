namespace DZ_Security_DataBase
{
    partial class cFunk
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbRadio = new Label();
            label14 = new Label();
            lbHidden = new Label();
            label12 = new Label();
            lbBatteries = new Label();
            label10 = new Label();
            lbShaver = new Label();
            label8 = new Label();
            lbMicky = new Label();
            label6 = new Label();
            SuspendLayout();
            // 
            // lbRadio
            // 
            lbRadio.AutoSize = true;
            lbRadio.Location = new Point(326, 147);
            lbRadio.Name = "lbRadio";
            lbRadio.Size = new Size(102, 20);
            lbRadio.TabIndex = 40;
            lbRadio.Text = "Keine Angabe";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(177, 147);
            label14.Name = "label14";
            label14.Size = new Size(141, 20);
            label14.TabIndex = 39;
            label14.Text = "Funkgerät-Nummer:";
            // 
            // lbHidden
            // 
            lbHidden.AutoSize = true;
            lbHidden.Location = new Point(312, 207);
            lbHidden.Name = "lbHidden";
            lbHidden.Size = new Size(59, 20);
            lbHidden.TabIndex = 38;
            lbHidden.Text = "Ja/Nein";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(234, 207);
            label12.Name = "label12";
            label12.Size = new Size(66, 20);
            label12.TabIndex = 37;
            label12.Text = "Tarn-Set:";
            // 
            // lbBatteries
            // 
            lbBatteries.AutoSize = true;
            lbBatteries.Location = new Point(312, 229);
            lbBatteries.Name = "lbBatteries";
            lbBatteries.Size = new Size(102, 20);
            lbBatteries.TabIndex = 36;
            lbBatteries.Text = "Keine Angabe";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(234, 229);
            label10.Name = "label10";
            label10.Size = new Size(72, 20);
            label10.TabIndex = 35;
            label10.Text = "Batterien:";
            // 
            // lbShaver
            // 
            lbShaver.AutoSize = true;
            lbShaver.Location = new Point(306, 187);
            lbShaver.Name = "lbShaver";
            lbShaver.Size = new Size(59, 20);
            lbShaver.TabIndex = 34;
            lbShaver.Text = "Ja/Nein";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(235, 187);
            label8.Name = "label8";
            label8.Size = new Size(65, 20);
            label8.TabIndex = 33;
            label8.Text = "Rasierer:";
            // 
            // lbMicky
            // 
            lbMicky.AutoSize = true;
            lbMicky.Location = new Point(326, 167);
            lbMicky.Name = "lbMicky";
            lbMicky.Size = new Size(59, 20);
            lbMicky.TabIndex = 32;
            lbMicky.Text = "Ja/Nein";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(235, 167);
            label6.Name = "label6";
            label6.Size = new Size(85, 20);
            label6.TabIndex = 31;
            label6.Text = "MickyMaus:";
            // 
            // cFunk
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lbRadio);
            Controls.Add(label14);
            Controls.Add(lbHidden);
            Controls.Add(label12);
            Controls.Add(lbBatteries);
            Controls.Add(label10);
            Controls.Add(lbShaver);
            Controls.Add(label8);
            Controls.Add(lbMicky);
            Controls.Add(label6);
            Name = "cFunk";
            Text = "cFunk";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbRadio;
        private Label label14;
        private Label lbHidden;
        private Label label12;
        private Label lbBatteries;
        private Label label10;
        private Label lbShaver;
        private Label label8;
        private Label lbMicky;
        private Label label6;
    }
}