namespace DZ_Security_DataBase
{
    partial class fEquipmentOverview
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
            bAddWorker = new Button();
            lbMobileNumber = new Label();
            label36 = new Label();
            lbLanguageLvL = new Label();
            label28 = new Label();
            lbLanguage = new Label();
            label30 = new Label();
            label32 = new Label();
            lbName = new Label();
            label20 = new Label();
            lbSurName = new Label();
            cbMitarbeiterID = new ComboBox();
            Name = new Label();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // bAddWorker
            // 
            bAddWorker.Location = new Point(82, 147);
            bAddWorker.Name = "bAddWorker";
            bAddWorker.Size = new Size(128, 61);
            bAddWorker.TabIndex = 95;
            bAddWorker.Text = "Defekt Melden";
            bAddWorker.UseVisualStyleBackColor = true;
            // 
            // lbMobileNumber
            // 
            lbMobileNumber.AutoSize = true;
            lbMobileNumber.Location = new Point(234, 109);
            lbMobileNumber.Name = "lbMobileNumber";
            lbMobileNumber.Size = new Size(17, 20);
            lbMobileNumber.TabIndex = 92;
            lbMobileNumber.Text = "1";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new Point(162, 109);
            label36.Name = "label36";
            label36.Size = new Size(66, 20);
            label36.TabIndex = 91;
            label36.Text = "Schwarz:";
            // 
            // lbLanguageLvL
            // 
            lbLanguageLvL.AutoSize = true;
            lbLanguageLvL.Location = new Point(234, 89);
            lbLanguageLvL.Name = "lbLanguageLvL";
            lbLanguageLvL.Size = new Size(17, 20);
            lbLanguageLvL.TabIndex = 90;
            lbLanguageLvL.Text = "2";
            lbLanguageLvL.Click += lbLanguageLvL_Click;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(189, 89);
            label28.Name = "label28";
            label28.Size = new Size(35, 20);
            label28.TabIndex = 89;
            label28.Text = "Rot:";
            // 
            // lbLanguage
            // 
            lbLanguage.AutoSize = true;
            lbLanguage.Location = new Point(234, 69);
            lbLanguage.Name = "lbLanguage";
            lbLanguage.Size = new Size(17, 20);
            lbLanguage.TabIndex = 88;
            lbLanguage.Text = "1";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(187, 69);
            label30.Name = "label30";
            label30.Size = new Size(41, 20);
            label30.TabIndex = 87;
            label30.Text = "Blau:";
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label32.Location = new Point(187, 41);
            label32.Name = "label32";
            label32.Size = new Size(75, 28);
            label32.TabIndex = 85;
            label32.Text = "Farben:";
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.Location = new Point(123, 69);
            lbName.Name = "lbName";
            lbName.Size = new Size(17, 20);
            lbName.TabIndex = 78;
            lbName.Text = "2";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(11, 69);
            label20.Name = "label20";
            label20.Size = new Size(64, 20);
            label20.TabIndex = 77;
            label20.Text = "Defekte:";
            // 
            // lbSurName
            // 
            lbSurName.AutoSize = true;
            lbSurName.Location = new Point(123, 49);
            lbSurName.Name = "lbSurName";
            lbSurName.Size = new Size(17, 20);
            lbSurName.TabIndex = 60;
            lbSurName.Text = "4";
            // 
            // cbMitarbeiterID
            // 
            cbMitarbeiterID.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbMitarbeiterID.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbMitarbeiterID.Cursor = Cursors.Hand;
            cbMitarbeiterID.FormattingEnabled = true;
            cbMitarbeiterID.Location = new Point(124, 6);
            cbMitarbeiterID.Margin = new Padding(3, 4, 3, 4);
            cbMitarbeiterID.Name = "cbMitarbeiterID";
            cbMitarbeiterID.Size = new Size(138, 28);
            cbMitarbeiterID.Sorted = true;
            cbMitarbeiterID.TabIndex = 59;
            // 
            // Name
            // 
            Name.AutoSize = true;
            Name.Location = new Point(11, 49);
            Name.Name = "Name";
            Name.Size = new Size(65, 20);
            Name.TabIndex = 57;
            Name.Text = "Bestand:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 56;
            label1.Text = "Equipment:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(268, 6);
            label2.Name = "label2";
            label2.Size = new Size(53, 12);
            label2.TabIndex = 97;
            label2.Text = "@Mobinoko";
            // 
            // fEquipmentOverview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(326, 220);
            Controls.Add(label2);
            Controls.Add(bAddWorker);
            Controls.Add(lbMobileNumber);
            Controls.Add(label36);
            Controls.Add(lbLanguageLvL);
            Controls.Add(label28);
            Controls.Add(lbLanguage);
            Controls.Add(label30);
            Controls.Add(label32);
            Controls.Add(lbName);
            Controls.Add(label20);
            Controls.Add(lbSurName);
            Controls.Add(cbMitarbeiterID);
            Controls.Add(Name);
            Controls.Add(label1);
            Text = "fEquipmentOverview";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button bAddWorker;
        private Label lbMobileNumber;
        private Label label36;
        private Label lbLanguageLvL;
        private Label label28;
        private Label lbLanguage;
        private Label label30;
        private Label label32;
        private Label lbName;
        private Label label20;
        private Label lbSurName;
        public ComboBox cbMitarbeiterID;
        private Label Name;
        private Label label1;
        private Label label2;
    }
}