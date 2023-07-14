namespace DZ_Security_DataBase
{
    partial class cCheckIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            start_Work_Timestamp = new Button();
            stop_Work_Timestamp = new Button();
            label1 = new Label();
            label4 = new Label();
            label6 = new Label();
            lbCheckedIn = new Label();
            label8 = new Label();
            lbCheckedOut = new Label();
            label10 = new Label();
            lbTotalCount = new Label();
            label11 = new Label();
            SuspendLayout();
            // 
            // start_Work_Timestamp
            // 
            start_Work_Timestamp.AutoSize = true;
            start_Work_Timestamp.Cursor = Cursors.Hand;
            start_Work_Timestamp.Location = new Point(19, 164);
            start_Work_Timestamp.Margin = new Padding(3, 4, 3, 4);
            start_Work_Timestamp.Name = "start_Work_Timestamp";
            start_Work_Timestamp.Size = new Size(150, 33);
            start_Work_Timestamp.TabIndex = 0;
            start_Work_Timestamp.Text = "Einchecken";
            start_Work_Timestamp.UseVisualStyleBackColor = true;
            start_Work_Timestamp.Click += button1_Click;
            // 
            // stop_Work_Timestamp
            // 
            stop_Work_Timestamp.AutoSize = true;
            stop_Work_Timestamp.Cursor = Cursors.Hand;
            stop_Work_Timestamp.Location = new Point(175, 164);
            stop_Work_Timestamp.Margin = new Padding(3, 4, 3, 4);
            stop_Work_Timestamp.Name = "stop_Work_Timestamp";
            stop_Work_Timestamp.Size = new Size(150, 33);
            stop_Work_Timestamp.TabIndex = 1;
            stop_Work_Timestamp.Text = "Auschecken";
            stop_Work_Timestamp.UseVisualStyleBackColor = true;
            stop_Work_Timestamp.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(277, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 21;
            label1.Text = "@Mobinoko";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(121, 9);
            label4.Name = "label4";
            label4.Size = new Size(76, 25);
            label4.TabIndex = 5;
            label4.Text = "Statistik";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(75, 93);
            label6.Name = "label6";
            label6.Size = new Size(91, 20);
            label6.TabIndex = 14;
            label6.Text = "Eingecheckt:";
            // 
            // lbCheckedIn
            // 
            lbCheckedIn.AutoSize = true;
            lbCheckedIn.Location = new Point(165, 93);
            lbCheckedIn.Name = "lbCheckedIn";
            lbCheckedIn.Size = new Size(87, 20);
            lbCheckedIn.TabIndex = 15;
            lbCheckedIn.Text = "Placeholder";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(75, 113);
            label8.Name = "label8";
            label8.Size = new Size(95, 20);
            label8.TabIndex = 16;
            label8.Text = "Ausgecheckt:";
            // 
            // lbCheckedOut
            // 
            lbCheckedOut.AutoSize = true;
            lbCheckedOut.Location = new Point(165, 113);
            lbCheckedOut.Name = "lbCheckedOut";
            lbCheckedOut.Size = new Size(87, 20);
            lbCheckedOut.TabIndex = 17;
            lbCheckedOut.Text = "Placeholder";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(75, 73);
            label10.Name = "label10";
            label10.Size = new Size(79, 20);
            label10.TabIndex = 18;
            label10.Text = "Insgesamt:";
            // 
            // lbTotalCount
            // 
            lbTotalCount.AutoSize = true;
            lbTotalCount.Location = new Point(165, 73);
            lbTotalCount.Name = "lbTotalCount";
            lbTotalCount.Size = new Size(87, 20);
            lbTotalCount.TabIndex = 19;
            lbTotalCount.Text = "Placeholder";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(165, 53);
            label11.Name = "label11";
            label11.Size = new Size(91, 20);
            label11.TabIndex = 20;
            label11.Text = "Personalzahl";
            // 
            // cCheckIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            ClientSize = new Size(342, 207);
            Controls.Add(label1);
            Controls.Add(label11);
            Controls.Add(lbTotalCount);
            Controls.Add(label10);
            Controls.Add(lbCheckedOut);
            Controls.Add(label8);
            Controls.Add(lbCheckedIn);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(stop_Work_Timestamp);
            Controls.Add(start_Work_Timestamp);
            Margin = new Padding(3, 4, 3, 4);
            Name = "cCheckIn";
            Text = "FestivalManager CheckIn Menü";
            FormClosed += fCheckin_FormClosed;
            Load += cCheckIn_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button start_Work_Timestamp;
        private Button stop_Work_Timestamp;
        private Label label1;
        public ComboBox cbMitarbeiterID;
        private Button convert_Excel_Button;
        private Label label4;
        private Label label6;
        private Label lbCheckedIn;
        private Label label8;
        private Label lbCheckedOut;
        private Label label10;
        private Label lbTotalCount;
        private Label label11;
    }
}