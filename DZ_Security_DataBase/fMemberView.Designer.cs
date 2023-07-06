namespace DZ_Security_DataBase
{
    partial class fMemberView
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
            SuspendLayout();
            // 
            // bCheckIn
            // 
            bCheckIn.Location = new Point(6, 11);
            bCheckIn.Margin = new Padding(3, 2, 3, 2);
            bCheckIn.Name = "bCheckIn";
            bCheckIn.Size = new Size(162, 48);
            bCheckIn.TabIndex = 0;
            bCheckIn.Text = "Check-In";
            bCheckIn.UseVisualStyleBackColor = true;
            bCheckIn.Click += bCheckin_Click;
            // 
            // bToolBorrow
            // 
            bToolBorrow.Location = new Point(208, 11);
            bToolBorrow.Margin = new Padding(3, 2, 3, 2);
            bToolBorrow.Name = "bToolBorrow";
            bToolBorrow.Size = new Size(162, 48);
            bToolBorrow.TabIndex = 3;
            bToolBorrow.Text = "Equipment Ausleihe";
            bToolBorrow.UseVisualStyleBackColor = true;
            // 
            // bPrintReceipt
            // 
            bPrintReceipt.Location = new Point(6, 63);
            bPrintReceipt.Margin = new Padding(3, 2, 3, 2);
            bPrintReceipt.Name = "bPrintReceipt";
            bPrintReceipt.Size = new Size(365, 48);
            bPrintReceipt.TabIndex = 4;
            bPrintReceipt.Text = "Laufzettel manuell drucken";
            bPrintReceipt.UseVisualStyleBackColor = true;
            bPrintReceipt.Click += bPrintReceipt_Click;
            // 
            // fMemberView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(385, 118);
            Controls.Add(bPrintReceipt);
            Controls.Add(bToolBorrow);
            Controls.Add(bCheckIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            Name = "fMemberView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DZ Security Member Menü";
            Load += cMenu_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button bCheckIn;
        private Button bToolBorrow;
        private Button bPrintReceipt;
    }
}