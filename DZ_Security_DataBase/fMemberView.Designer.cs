namespace DZ_Security_DataBase
{
    partial class cMemberView
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
            bCheckIn = new Button();
            bToolBorrow = new Button();
            bPrintReceipt = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // bCheckIn
            // 
            bCheckIn.Location = new Point(7, 15);
            bCheckIn.Name = "bCheckIn";
            bCheckIn.Size = new Size(185, 64);
            bCheckIn.TabIndex = 0;
            bCheckIn.Text = "Check-In";
            bCheckIn.UseVisualStyleBackColor = true;
            bCheckIn.Click += bCheckin_Click;
            // 
            // bToolBorrow
            // 
            bToolBorrow.Location = new Point(238, 15);
            bToolBorrow.Name = "bToolBorrow";
            bToolBorrow.Size = new Size(185, 64);
            bToolBorrow.TabIndex = 3;
            bToolBorrow.Text = "Equipment Ausleihe";
            bToolBorrow.UseVisualStyleBackColor = true;
            bToolBorrow.Click += bToolBorrow_Click;
            // 
            // bPrintReceipt
            // 
            bPrintReceipt.Location = new Point(7, 84);
            bPrintReceipt.Name = "bPrintReceipt";
            bPrintReceipt.Size = new Size(417, 64);
            bPrintReceipt.TabIndex = 4;
            bPrintReceipt.Text = "Laufzettel manuell drucken";
            bPrintReceipt.UseVisualStyleBackColor = true;
            bPrintReceipt.Click += bPrintReceipt_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(429, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 16;
            label1.Text = "@Mobinoko";
            // 
            // cMemberView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(488, 157);
            Controls.Add(label1);
            Controls.Add(bPrintReceipt);
            Controls.Add(bToolBorrow);
            Controls.Add(bCheckIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "cMemberView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Festival Manager Member Menü";
            FormClosed += cMemberView_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bCheckIn;
        private Button bToolBorrow;
        private Button bPrintReceipt;
        private Label label1;
    }
}