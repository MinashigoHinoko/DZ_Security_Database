namespace DZ_Security_DataBase
{
    partial class cEquipmentRent
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
            label11 = new Label();
            lbTotalCount = new Label();
            label10 = new Label();
            lbCheckedOut = new Label();
            label8 = new Label();
            lbCheckedIn = new Label();
            label6 = new Label();
            label4 = new Label();
            stop_Work_Timestamp = new Button();
            start_Work_Timestamp = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(159, 53);
            label11.Name = "label11";
            label11.Size = new Size(54, 20);
            label11.TabIndex = 30;
            label11.Text = "Anzahl";
            // 
            // lbTotalCount
            // 
            lbTotalCount.AutoSize = true;
            lbTotalCount.Location = new Point(159, 73);
            lbTotalCount.Name = "lbTotalCount";
            lbTotalCount.Size = new Size(87, 20);
            lbTotalCount.TabIndex = 29;
            lbTotalCount.Text = "Placeholder";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(69, 73);
            label10.Name = "label10";
            label10.Size = new Size(79, 20);
            label10.TabIndex = 28;
            label10.Text = "Insgesamt:";
            // 
            // lbCheckedOut
            // 
            lbCheckedOut.AutoSize = true;
            lbCheckedOut.Location = new Point(159, 113);
            lbCheckedOut.Name = "lbCheckedOut";
            lbCheckedOut.Size = new Size(87, 20);
            lbCheckedOut.TabIndex = 27;
            lbCheckedOut.Text = "Placeholder";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(69, 113);
            label8.Name = "label8";
            label8.Size = new Size(93, 20);
            label8.TabIndex = 26;
            label8.Text = "Ausgeliehen:";
            // 
            // lbCheckedIn
            // 
            lbCheckedIn.AutoSize = true;
            lbCheckedIn.Location = new Point(159, 93);
            lbCheckedIn.Name = "lbCheckedIn";
            lbCheckedIn.Size = new Size(87, 20);
            lbCheckedIn.TabIndex = 25;
            lbCheckedIn.Text = "Placeholder";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(69, 93);
            label6.Name = "label6";
            label6.Size = new Size(82, 20);
            label6.TabIndex = 24;
            label6.Text = "Ausleihbar:";
            label6.Click += label6_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(115, 9);
            label4.Name = "label4";
            label4.Size = new Size(76, 25);
            label4.TabIndex = 23;
            label4.Text = "Statistik";
            // 
            // stop_Work_Timestamp
            // 
            stop_Work_Timestamp.AutoSize = true;
            stop_Work_Timestamp.Cursor = Cursors.Hand;
            stop_Work_Timestamp.Location = new Point(187, 164);
            stop_Work_Timestamp.Margin = new Padding(3, 4, 3, 4);
            stop_Work_Timestamp.Name = "stop_Work_Timestamp";
            stop_Work_Timestamp.Size = new Size(150, 33);
            stop_Work_Timestamp.TabIndex = 22;
            stop_Work_Timestamp.Text = "Abgeben";
            stop_Work_Timestamp.UseVisualStyleBackColor = true;
            // 
            // start_Work_Timestamp
            // 
            start_Work_Timestamp.AutoSize = true;
            start_Work_Timestamp.Cursor = Cursors.Hand;
            start_Work_Timestamp.Location = new Point(15, 164);
            start_Work_Timestamp.Margin = new Padding(3, 4, 3, 4);
            start_Work_Timestamp.Name = "start_Work_Timestamp";
            start_Work_Timestamp.Size = new Size(142, 33);
            start_Work_Timestamp.TabIndex = 21;
            start_Work_Timestamp.Text = "Ausleihen";
            start_Work_Timestamp.UseVisualStyleBackColor = true;
            start_Work_Timestamp.Click += start_Work_Timestamp_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(284, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 31;
            label1.Text = "@Mobinoko";
            // 
            // cEquipmentRent
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 206);
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
            Name = "cEquipmentRent";
            Text = "FestivalManager Ausrüstung Ausleih Menü";
            Load += fEquipmentRent_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label11;
        private Label lbTotalCount;
        private Label label10;
        private Label lbCheckedOut;
        private Label label8;
        private Label lbCheckedIn;
        private Label label6;
        private Label label4;
        private Button stop_Work_Timestamp;
        private Button start_Work_Timestamp;
        private Label label1;
    }
}