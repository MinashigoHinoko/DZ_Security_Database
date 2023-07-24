namespace Festival_Manager
{
    partial class fInportData
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
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Location = new Point(172, 12);
            button2.Name = "button2";
            button2.Size = new Size(154, 60);
            button2.TabIndex = 1;
            button2.Text = "Einlesen Positionen mit Beschreibung";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 12);
            button3.Name = "button3";
            button3.Size = new Size(154, 60);
            button3.TabIndex = 2;
            button3.Text = "Einlesen Mitarbeiter Stammdaten";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(346, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 12);
            label1.TabIndex = 4;
            label1.Text = "@Mobinoko";
            // 
            // button1
            // 
            button1.Location = new Point(6, 78);
            button1.Name = "button1";
            button1.Size = new Size(320, 60);
            button1.TabIndex = 0;
            button1.Text = "Einlesen Arbeitsplan";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // fInportData
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(411, 146);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "fInportData";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FestivalManager Daten Einlese";
            FormClosed += fInportData_FormClosed;
            Load += fInportData_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button2;
        private Button button3;
        private Label label1;
        private Button button1;
    }
}