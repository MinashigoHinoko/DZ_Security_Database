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
            lbRented = new Label();
            label8 = new Label();
            lbRentable = new Label();
            label6 = new Label();
            label4 = new Label();
            bReturn = new Button();
            bRent = new Button();
            label1 = new Label();
            button1 = new Button();
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
            // lbRented
            // 
            lbRented.AutoSize = true;
            lbRented.Location = new Point(159, 113);
            lbRented.Name = "lbRented";
            lbRented.Size = new Size(87, 20);
            lbRented.TabIndex = 27;
            lbRented.Text = "Placeholder";
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
            // lbRentable
            // 
            lbRentable.AutoSize = true;
            lbRentable.Location = new Point(159, 93);
            lbRentable.Name = "lbRentable";
            lbRentable.Size = new Size(87, 20);
            lbRentable.TabIndex = 25;
            lbRentable.Text = "Placeholder";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(69, 93);
            label6.Name = "label6";
            label6.Size = new Size(82, 20);
            label6.TabIndex = 24;
            label6.Text = "Ausleihbar:";
            label6.Click += return_Click;
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
            // bReturn
            // 
            bReturn.AutoSize = true;
            bReturn.Cursor = Cursors.Hand;
            bReturn.Location = new Point(187, 164);
            bReturn.Margin = new Padding(3, 4, 3, 4);
            bReturn.Name = "bReturn";
            bReturn.Size = new Size(150, 33);
            bReturn.TabIndex = 22;
            bReturn.Text = "Abgeben";
            bReturn.UseVisualStyleBackColor = true;
            bReturn.Click += return_Click;
            // 
            // bRent
            // 
            bRent.AutoSize = true;
            bRent.Cursor = Cursors.Hand;
            bRent.Location = new Point(15, 164);
            bRent.Margin = new Padding(3, 4, 3, 4);
            bRent.Name = "bRent";
            bRent.Size = new Size(142, 33);
            bRent.TabIndex = 21;
            bRent.Text = "Ausleihen";
            bRent.UseVisualStyleBackColor = true;
            bRent.Click += rent_Click;
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
            // button1
            // 
            button1.AutoSize = true;
            button1.Cursor = Cursors.Hand;
            button1.Location = new Point(125, 205);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(92, 30);
            button1.TabIndex = 32;
            button1.Text = "Funkgeräte";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // cEquipmentRent
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 244);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(label11);
            Controls.Add(lbTotalCount);
            Controls.Add(label10);
            Controls.Add(lbRented);
            Controls.Add(label8);
            Controls.Add(lbRentable);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(bReturn);
            Controls.Add(bRent);
            Name = "cEquipmentRent";
            Text = "FestivalManager Ausrüstung Ausleih Menü";
            FormClosed += cEquipmentRent_FormClosed;
            Load += cEquipmentRent_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label11;
        private Label lbTotalCount;
        private Label label10;
        private Label lbRented;
        private Label label8;
        private Label lbRentable;
        private Label label6;
        private Label label4;
        private Button bReturn;
        private Button bRent;
        private Label label1;
        private Button button1;
    }
}