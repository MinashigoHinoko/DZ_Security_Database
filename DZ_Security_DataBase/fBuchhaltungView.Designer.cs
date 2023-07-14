namespace DZ_Security_DataBase
{
    partial class cBookingView
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
            label1 = new Label();
            SuspendLayout();
            // 
            // bExcelExport
            // 
            bExcelExport.Location = new Point(203, 12);
            bExcelExport.Name = "bExcelExport";
            bExcelExport.Size = new Size(185, 64);
            bExcelExport.TabIndex = 11;
            bExcelExport.Text = "Excel Export";
            bExcelExport.UseVisualStyleBackColor = true;
            bExcelExport.Click += bExcelExport_Click;
            // 
            // bPrint
            // 
            bPrint.Location = new Point(12, 12);
            bPrint.Name = "bPrint";
            bPrint.Size = new Size(185, 64);
            bPrint.TabIndex = 10;
            bPrint.Text = "Ausdruck";
            bPrint.UseVisualStyleBackColor = true;
            bPrint.Click += bPrint_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(394, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 17;
            label1.Text = "@Mobinoko";
            // 
            // cBookingView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(455, 81);
            Controls.Add(label1);
            Controls.Add(bExcelExport);
            Controls.Add(bPrint);
            Name = "cBookingView";
            Text = "cBuchHaltung";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bExcelExport;
        private Button bPrint;
        private Label label1;
    }
}