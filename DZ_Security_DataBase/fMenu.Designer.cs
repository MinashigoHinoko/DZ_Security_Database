namespace DZ_Security_DataBase
{
    partial class cMenu
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
            bToolOverlay = new Button();
            bPrint = new Button();
            bExcelExport = new Button();
            SuspendLayout();
            // 
            // bCheckIn
            // 
            bCheckIn.Location = new Point(12, 12);
            bCheckIn.Name = "bCheckIn";
            bCheckIn.Size = new Size(185, 64);
            bCheckIn.TabIndex = 0;
            bCheckIn.Text = "Check-In";
            bCheckIn.UseVisualStyleBackColor = true;
            bCheckIn.Click += bCheckin_Click;
            // 
            // bToolBorrow
            // 
            bToolBorrow.Location = new Point(12, 102);
            bToolBorrow.Name = "bToolBorrow";
            bToolBorrow.Size = new Size(185, 64);
            bToolBorrow.TabIndex = 1;
            bToolBorrow.Text = "Equipment Ausleihe";
            bToolBorrow.UseVisualStyleBackColor = true;
            bToolBorrow.Click += button2_Click;
            // 
            // bToolOverlay
            // 
            bToolOverlay.Location = new Point(242, 102);
            bToolOverlay.Name = "bToolOverlay";
            bToolOverlay.Size = new Size(185, 64);
            bToolOverlay.TabIndex = 2;
            bToolOverlay.Text = "Equipment Übersicht";
            bToolOverlay.UseVisualStyleBackColor = true;
            // 
            // bPrint
            // 
            bPrint.Location = new Point(242, 12);
            bPrint.Name = "bPrint";
            bPrint.Size = new Size(185, 64);
            bPrint.TabIndex = 3;
            bPrint.Text = "Ausdruck";
            bPrint.UseVisualStyleBackColor = true;
            bPrint.Click += button4_Click;
            // 
            // bExcelExport
            // 
            bExcelExport.Location = new Point(12, 191);
            bExcelExport.Name = "bExcelExport";
            bExcelExport.Size = new Size(415, 64);
            bExcelExport.TabIndex = 4;
            bExcelExport.Text = "Excel Export";
            bExcelExport.UseVisualStyleBackColor = true;
            // 
            // cMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(439, 269);
            Controls.Add(bExcelExport);
            Controls.Add(bPrint);
            Controls.Add(bToolOverlay);
            Controls.Add(bToolBorrow);
            Controls.Add(bCheckIn);
            Name = "cMenu";
            Text = "DZ Security Member Menü";
            Load += cMenu_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button bCheckIn;
        private Button bToolBorrow;
        private Button bToolOverlay;
        private Button bPrint;
        private Button bExcelExport;
    }
}