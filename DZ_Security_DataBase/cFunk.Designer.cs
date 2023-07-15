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
            bReturn = new Button();
            bRent = new Button();
            label11 = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // lbRadio
            // 
            lbRadio.AutoSize = true;
            lbRadio.Location = new Point(181, 35);
            lbRadio.Name = "lbRadio";
            lbRadio.Size = new Size(17, 20);
            lbRadio.TabIndex = 40;
            lbRadio.Text = "0";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(93, 35);
            label14.Name = "label14";
            label14.Size = new Size(77, 20);
            label14.TabIndex = 39;
            label14.Text = "Funkgerät:";
            // 
            // lbHidden
            // 
            lbHidden.AutoSize = true;
            lbHidden.Location = new Point(181, 95);
            lbHidden.Name = "lbHidden";
            lbHidden.Size = new Size(17, 20);
            lbHidden.TabIndex = 38;
            lbHidden.Text = "0";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(104, 95);
            label12.Name = "label12";
            label12.Size = new Size(66, 20);
            label12.TabIndex = 37;
            label12.Text = "Tarn-Set:";
            // 
            // lbBatteries
            // 
            lbBatteries.AutoSize = true;
            lbBatteries.Location = new Point(181, 117);
            lbBatteries.Name = "lbBatteries";
            lbBatteries.Size = new Size(17, 20);
            lbBatteries.TabIndex = 36;
            lbBatteries.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(29, 115);
            label10.Name = "label10";
            label10.Size = new Size(141, 20);
            label10.TabIndex = 35;
            label10.Text = "Benötigte Batterien:";
            label10.Click += label10_Click;
            // 
            // lbShaver
            // 
            lbShaver.AutoSize = true;
            lbShaver.Location = new Point(181, 75);
            lbShaver.Name = "lbShaver";
            lbShaver.Size = new Size(17, 20);
            lbShaver.TabIndex = 34;
            lbShaver.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(105, 75);
            label8.Name = "label8";
            label8.Size = new Size(65, 20);
            label8.TabIndex = 33;
            label8.Text = "Rasierer:";
            // 
            // lbMicky
            // 
            lbMicky.AutoSize = true;
            lbMicky.Location = new Point(181, 55);
            lbMicky.Name = "lbMicky";
            lbMicky.Size = new Size(17, 20);
            lbMicky.TabIndex = 32;
            lbMicky.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(85, 55);
            label6.Name = "label6";
            label6.Size = new Size(85, 20);
            label6.TabIndex = 31;
            label6.Text = "MickyMaus:";
            // 
            // bReturn
            // 
            bReturn.AutoSize = true;
            bReturn.Cursor = Cursors.Hand;
            bReturn.Location = new Point(181, 161);
            bReturn.Margin = new Padding(3, 4, 3, 4);
            bReturn.Name = "bReturn";
            bReturn.Size = new Size(150, 33);
            bReturn.TabIndex = 42;
            bReturn.Text = "Abgeben";
            bReturn.UseVisualStyleBackColor = true;
            bReturn.Click += bReturn_Click;
            // 
            // bRent
            // 
            bRent.AutoSize = true;
            bRent.Cursor = Cursors.Hand;
            bRent.Location = new Point(9, 161);
            bRent.Margin = new Padding(3, 4, 3, 4);
            bRent.Name = "bRent";
            bRent.Size = new Size(142, 33);
            bRent.TabIndex = 41;
            bRent.Text = "Ausleihen";
            bRent.UseVisualStyleBackColor = true;
            bRent.Click += bRent_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(129, 9);
            label11.Name = "label11";
            label11.Size = new Size(54, 20);
            label11.TabIndex = 43;
            label11.Text = "Anzahl";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(278, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 44;
            label1.Text = "@Mobinoko";
            // 
            // cFunk
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(352, 206);
            Controls.Add(label1);
            Controls.Add(label11);
            Controls.Add(bReturn);
            Controls.Add(bRent);
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
            Load += cFunk_Load;
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
        private Button bReturn;
        private Button bRent;
        private Label label11;
        private Label label1;
    }
}