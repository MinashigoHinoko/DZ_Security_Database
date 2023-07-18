namespace Festival_Manager
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
            button2 = new Button();
            SuspendLayout();
            // 
            // bExcelExport
            // 
            bExcelExport.Cursor = Cursors.Hand;
            bExcelExport.Location = new Point(242, 182);
            bExcelExport.Name = "bExcelExport";
            bExcelExport.Size = new Size(185, 64);
            bExcelExport.TabIndex = 9;
            bExcelExport.Text = "Excel Export";
            bExcelExport.UseVisualStyleBackColor = true;
            bExcelExport.Click += bExcelExport_Click;
            // 
            // bPrint
            // 
            bPrint.Cursor = Cursors.Hand;
            bPrint.Location = new Point(12, 182);
            bPrint.Name = "bPrint";
            bPrint.Size = new Size(185, 64);
            bPrint.TabIndex = 8;
            bPrint.Text = "Ausdruck";
            bPrint.UseVisualStyleBackColor = true;
            bPrint.Click += bPrint_Click;
            // 
            // bToolOverlay
            // 
            bToolOverlay.Cursor = Cursors.Hand;
            bToolOverlay.Location = new Point(242, 95);
            bToolOverlay.Name = "bToolOverlay";
            bToolOverlay.Size = new Size(185, 64);
            bToolOverlay.TabIndex = 7;
            bToolOverlay.Text = "Equipment Übersicht";
            bToolOverlay.UseVisualStyleBackColor = true;
            bToolOverlay.Click += bToolOverlay_Click;
            // 
            // bToolBorrow
            // 
            bToolBorrow.Cursor = Cursors.Hand;
            bToolBorrow.Location = new Point(12, 95);
            bToolBorrow.Name = "bToolBorrow";
            bToolBorrow.Size = new Size(185, 64);
            bToolBorrow.TabIndex = 6;
            bToolBorrow.Text = "Equipment Ausleihe";
            bToolBorrow.UseVisualStyleBackColor = true;
            bToolBorrow.Click += bToolBorrow_Click;
            // 
            // bCheckIn
            // 
            bCheckIn.Cursor = Cursors.Hand;
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
            bWorkerOverview.Cursor = Cursors.Hand;
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
            bPrintReceipt.Cursor = Cursors.Hand;
            bPrintReceipt.Location = new Point(10, 262);
            bPrintReceipt.Name = "bPrintReceipt";
            bPrintReceipt.Size = new Size(187, 64);
            bPrintReceipt.TabIndex = 17;
            bPrintReceipt.Text = "Laufzettel manuell drucken";
            bPrintReceipt.UseVisualStyleBackColor = true;
            bPrintReceipt.Click += bPrintReceipt_Click;
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.Location = new Point(242, 262);
            button1.Name = "button1";
            button1.Size = new Size(185, 64);
            button1.TabIndex = 18;
            button1.Text = "Liste Inportieren";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.Location = new Point(10, 353);
            button2.Name = "button2";
            button2.Size = new Size(417, 64);
            button2.TabIndex = 19;
            button2.Text = "Login Daten Anpassen";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // cAdminView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 441);
            Controls.Add(button2);
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
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Festival Manager Admin Menü";
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
        private Button button2;
    }
}