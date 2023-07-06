namespace DZ_Security_DataBase
{
    partial class cAdminView
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
            bExcelExport = new Button();
            bPrint = new Button();
            bToolOverlay = new Button();
            bToolBorrow = new Button();
            bCheckIn = new Button();
            SuspendLayout();
            // 
            // bExcelExport
            // 
            bExcelExport.Location = new Point(12, 191);
            bExcelExport.Name = "bExcelExport";
            bExcelExport.Size = new Size(415, 64);
            bExcelExport.TabIndex = 9;
            bExcelExport.Text = "Excel Export";
            bExcelExport.UseVisualStyleBackColor = true;
            bExcelExport.Click += bExcelExport_Click;
            // 
            // bPrint
            // 
            bPrint.Location = new Point(242, 12);
            bPrint.Name = "bPrint";
            bPrint.Size = new Size(185, 64);
            bPrint.TabIndex = 8;
            bPrint.Text = "Ausdruck";
            bPrint.UseVisualStyleBackColor = true;
            // 
            // bToolOverlay
            // 
            bToolOverlay.Location = new Point(242, 102);
            bToolOverlay.Name = "bToolOverlay";
            bToolOverlay.Size = new Size(185, 64);
            bToolOverlay.TabIndex = 7;
            bToolOverlay.Text = "Equipment Übersicht";
            bToolOverlay.UseVisualStyleBackColor = true;
            // 
            // bToolBorrow
            // 
            bToolBorrow.Location = new Point(12, 102);
            bToolBorrow.Name = "bToolBorrow";
            bToolBorrow.Size = new Size(185, 64);
            bToolBorrow.TabIndex = 6;
            bToolBorrow.Text = "Equipment Ausleihe";
            bToolBorrow.UseVisualStyleBackColor = true;
            // 
            // bCheckIn
            // 
            bCheckIn.Location = new Point(12, 12);
            bCheckIn.Name = "bCheckIn";
            bCheckIn.Size = new Size(185, 64);
            bCheckIn.TabIndex = 5;
            bCheckIn.Text = "Check-In";
            bCheckIn.UseVisualStyleBackColor = true;
            bCheckIn.Click += bCheckin_Click;
            // 
            // cAdminView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(438, 276);
            Controls.Add(bExcelExport);
            Controls.Add(bPrint);
            Controls.Add(bToolOverlay);
            Controls.Add(bToolBorrow);
            Controls.Add(bCheckIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "cAdminView";
            Text = "DZ Security Admin Menü";
            Load += cAdminMenu_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button bExcelExport;
        private Button bPrint;
        private Button bToolOverlay;
        private Button bToolBorrow;
        private Button bCheckIn;
    }
}