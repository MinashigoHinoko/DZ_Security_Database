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
            bWorkerOverview = new Button();
            label1 = new Label();
            bPrintReceipt = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // bExcelExport
            // 
            bExcelExport.Location = new Point(242, 191);
            bExcelExport.Name = "bExcelExport";
            bExcelExport.Size = new Size(185, 64);
            bExcelExport.TabIndex = 9;
            bExcelExport.Text = "Excel Export";
            bExcelExport.UseVisualStyleBackColor = true;
            bExcelExport.Click += bExcelExport_Click;
            // 
            // bPrint
            // 
            bPrint.Location = new Point(12, 191);
            bPrint.Name = "bPrint";
            bPrint.Size = new Size(185, 64);
            bPrint.TabIndex = 8;
            bPrint.Text = "Ausdruck";
            bPrint.UseVisualStyleBackColor = true;
            bPrint.Click += bPrint_Click;
            // 
            // bToolOverlay
            // 
            bToolOverlay.Location = new Point(242, 102);
            bToolOverlay.Name = "bToolOverlay";
            bToolOverlay.Size = new Size(185, 64);
            bToolOverlay.TabIndex = 7;
            bToolOverlay.Text = "Equipment Übersicht";
            bToolOverlay.UseVisualStyleBackColor = true;
            bToolOverlay.Click += bToolOverlay_Click;
            // 
            // bToolBorrow
            // 
            bToolBorrow.Location = new Point(12, 102);
            bToolBorrow.Name = "bToolBorrow";
            bToolBorrow.Size = new Size(185, 64);
            bToolBorrow.TabIndex = 6;
            bToolBorrow.Text = "Equipment Ausleihe";
            bToolBorrow.UseVisualStyleBackColor = true;
            bToolBorrow.Click += bToolBorrow_Click;
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
            // bWorkerOverview
            // 
            bWorkerOverview.Location = new Point(242, 12);
            bWorkerOverview.Name = "bWorkerOverview";
            bWorkerOverview.Size = new Size(185, 64);
            bWorkerOverview.TabIndex = 10;
            bWorkerOverview.Text = "Personal Übersicht";
            bWorkerOverview.UseVisualStyleBackColor = true;
            bWorkerOverview.Click += bWorkerOverview_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(433, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 16;
            label1.Text = "@Mobinoko";
            // 
            // bPrintReceipt
            // 
            bPrintReceipt.Location = new Point(10, 271);
            bPrintReceipt.Name = "bPrintReceipt";
            bPrintReceipt.Size = new Size(187, 64);
            bPrintReceipt.TabIndex = 17;
            bPrintReceipt.Text = "Laufzettel manuell drucken";
            bPrintReceipt.UseVisualStyleBackColor = true;
            bPrintReceipt.Click += bPrintReceipt_Click;
            // 
            // button1
            // 
            button1.Location = new Point(242, 271);
            button1.Name = "button1";
            button1.Size = new Size(185, 64);
            button1.TabIndex = 18;
            button1.Text = "Liste Inportieren";
            button1.UseVisualStyleBackColor = true;
            // 
            // cAdminView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 355);
            Controls.Add(button1);
            Controls.Add(bPrintReceipt);
            Controls.Add(label1);
            Controls.Add(bWorkerOverview);
            Controls.Add(bExcelExport);
            Controls.Add(bPrint);
            Controls.Add(bToolOverlay);
            Controls.Add(bToolBorrow);
            Controls.Add(bCheckIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "cAdminView";
            Text = "DZ Security Admin Menü";
            FormClosed += cAdminView_FormClosed;
            Load += cAdminView_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bExcelExport;
        private Button bPrint;
        private Button bToolOverlay;
        private Button bToolBorrow;
        private Button bCheckIn;
        private Button bWorkerOverview;
        private Label label1;
        private Button bPrintReceipt;
        private Button button1;
    }
}