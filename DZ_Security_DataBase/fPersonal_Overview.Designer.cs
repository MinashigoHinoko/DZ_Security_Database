namespace DZ_Security_DataBase
{
    partial class cPersonalOverview
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
            label1 = new Label();
            Name = new Label();
            Position = new Label();
            cbMitarbeiterID = new ComboBox();
            label3 = new Label();
            lbCheckedIn = new Label();
            label16 = new Label();
            label18 = new Label();
            label20 = new Label();
            label22 = new Label();
            label24 = new Label();
            label26 = new Label();
            label32 = new Label();
            label30 = new Label();
            label28 = new Label();
            label36 = new Label();
            label34 = new Label();
            bAddWorker = new Button();
            cbCompany = new ComboBox();
            button1 = new Button();
            label2 = new Label();
            tbSurName = new TextBox();
            tbName = new TextBox();
            tbBirthday = new TextBox();
            tbLiving = new TextBox();
            tbBirthPlace = new TextBox();
            tbChip = new TextBox();
            cbGender = new ComboBox();
            tbLanguage = new TextBox();
            cbOtherLanguage = new ComboBox();
            tbNumber = new TextBox();
            tbContact = new TextBox();
            tbPosition = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 12);
            label1.Name = "label1";
            label1.Size = new Size(71, 20);
            label1.TabIndex = 12;
            label1.Text = "Personal: ";
            // 
            // Name
            // 
            Name.AutoSize = true;
            Name.Location = new Point(40, 78);
            Name.Name = "Name";
            Name.Size = new Size(83, 20);
            Name.TabIndex = 13;
            Name.Text = "Nachname:";
            // 
            // Position
            // 
            Position.AutoSize = true;
            Position.Location = new Point(472, 239);
            Position.Name = "Position";
            Position.Size = new Size(64, 20);
            Position.TabIndex = 14;
            Position.Text = "Position:";
            // 
            // cbMitarbeiterID
            // 
            cbMitarbeiterID.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbMitarbeiterID.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbMitarbeiterID.Cursor = Cursors.Hand;
            cbMitarbeiterID.DropDownWidth = 200;
            cbMitarbeiterID.FormattingEnabled = true;
            cbMitarbeiterID.Location = new Point(128, 9);
            cbMitarbeiterID.Margin = new Padding(3, 4, 3, 4);
            cbMitarbeiterID.Name = "cbMitarbeiterID";
            cbMitarbeiterID.Size = new Size(139, 28);
            cbMitarbeiterID.Sorted = true;
            cbMitarbeiterID.TabIndex = 16;
            cbMitarbeiterID.SelectedIndexChanged += cbMitarbeiterID_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 245);
            label3.Name = "label3";
            label3.Size = new Size(113, 20);
            label3.TabIndex = 19;
            label3.Text = "Check-in Status:";
            // 
            // lbCheckedIn
            // 
            lbCheckedIn.AutoSize = true;
            lbCheckedIn.Location = new Point(149, 245);
            lbCheckedIn.Name = "lbCheckedIn";
            lbCheckedIn.Size = new Size(102, 20);
            lbCheckedIn.TabIndex = 20;
            lbCheckedIn.Text = "Keine Angabe";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(13, 272);
            label16.Name = "label16";
            label16.Size = new Size(106, 20);
            label16.TabIndex = 31;
            label16.Text = "Chip-Nummer:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(70, 44);
            label18.Name = "label18";
            label18.Size = new Size(49, 20);
            label18.TabIndex = 33;
            label18.Text = "Firma:";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(52, 111);
            label20.Name = "label20";
            label20.Size = new Size(71, 20);
            label20.TabIndex = 35;
            label20.Text = "Vorname:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(17, 144);
            label22.Name = "label22";
            label22.Size = new Size(106, 20);
            label22.TabIndex = 37;
            label22.Text = "Geburtsdatum:";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(50, 177);
            label24.Name = "label24";
            label24.Size = new Size(69, 20);
            label24.TabIndex = 39;
            label24.Text = "Wohnort:";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(27, 212);
            label26.Name = "label26";
            label26.Size = new Size(92, 20);
            label26.TabIndex = 41;
            label26.Text = "Geburtsland:";
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(453, 72);
            label32.Name = "label32";
            label32.Size = new Size(83, 20);
            label32.TabIndex = 43;
            label32.Text = "Geschlecht:";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(429, 106);
            label30.Name = "label30";
            label30.Size = new Size(107, 20);
            label30.TabIndex = 45;
            label30.Text = "Muttersprache:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(402, 139);
            label28.Name = "label28";
            label28.Size = new Size(134, 20);
            label28.TabIndex = 47;
            label28.Text = "Sonstige Sprachen:";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new Point(452, 172);
            label36.Name = "label36";
            label36.Size = new Size(84, 20);
            label36.TabIndex = 49;
            label36.Text = "Telefon Nr.:";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(415, 206);
            label34.Name = "label34";
            label34.Size = new Size(121, 20);
            label34.TabIndex = 51;
            label34.Text = "Ansprechpartner:";
            // 
            // bAddWorker
            // 
            bAddWorker.Location = new Point(6, 343);
            bAddWorker.Name = "bAddWorker";
            bAddWorker.Size = new Size(217, 65);
            bAddWorker.TabIndex = 53;
            bAddWorker.Text = "Neues Personal Hinzufügen";
            bAddWorker.UseVisualStyleBackColor = true;
            bAddWorker.Click += bAddWorker_Click;
            // 
            // cbCompany
            // 
            cbCompany.FormattingEnabled = true;
            cbCompany.Location = new Point(128, 41);
            cbCompany.Name = "cbCompany";
            cbCompany.Size = new Size(139, 28);
            cbCompany.TabIndex = 54;
            cbCompany.SelectedIndexChanged += cbCompany_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(474, 343);
            button1.Name = "button1";
            button1.Size = new Size(220, 65);
            button1.TabIndex = 55;
            button1.Text = "Änderungen Speichern";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(639, 9);
            label2.Name = "label2";
            label2.Size = new Size(53, 12);
            label2.TabIndex = 56;
            label2.Text = "@Mobinoko";
            // 
            // tbSurName
            // 
            tbSurName.Location = new Point(128, 75);
            tbSurName.Name = "tbSurName";
            tbSurName.Size = new Size(139, 27);
            tbSurName.TabIndex = 57;
            // 
            // tbName
            // 
            tbName.Location = new Point(128, 108);
            tbName.Name = "tbName";
            tbName.Size = new Size(139, 27);
            tbName.TabIndex = 58;
            // 
            // tbBirthday
            // 
            tbBirthday.Location = new Point(128, 141);
            tbBirthday.Name = "tbBirthday";
            tbBirthday.Size = new Size(139, 27);
            tbBirthday.TabIndex = 59;
            // 
            // tbLiving
            // 
            tbLiving.Location = new Point(128, 174);
            tbLiving.Name = "tbLiving";
            tbLiving.Size = new Size(139, 27);
            tbLiving.TabIndex = 60;
            // 
            // tbBirthPlace
            // 
            tbBirthPlace.Location = new Point(128, 209);
            tbBirthPlace.Name = "tbBirthPlace";
            tbBirthPlace.Size = new Size(139, 27);
            tbBirthPlace.TabIndex = 61;
            // 
            // tbChip
            // 
            tbChip.Location = new Point(128, 269);
            tbChip.Name = "tbChip";
            tbChip.Size = new Size(139, 27);
            tbChip.TabIndex = 62;
            // 
            // cbGender
            // 
            cbGender.FormattingEnabled = true;
            cbGender.Location = new Point(550, 69);
            cbGender.Name = "cbGender";
            cbGender.Size = new Size(139, 28);
            cbGender.TabIndex = 63;
            // 
            // tbLanguage
            // 
            tbLanguage.Location = new Point(550, 103);
            tbLanguage.Name = "tbLanguage";
            tbLanguage.Size = new Size(139, 27);
            tbLanguage.TabIndex = 64;
            // 
            // cbOtherLanguage
            // 
            cbOtherLanguage.FormattingEnabled = true;
            cbOtherLanguage.Location = new Point(550, 136);
            cbOtherLanguage.Name = "cbOtherLanguage";
            cbOtherLanguage.Size = new Size(140, 28);
            cbOtherLanguage.TabIndex = 65;
            // 
            // tbNumber
            // 
            tbNumber.Location = new Point(550, 170);
            tbNumber.Name = "tbNumber";
            tbNumber.Size = new Size(140, 27);
            tbNumber.TabIndex = 66;
            // 
            // tbContact
            // 
            tbContact.Location = new Point(550, 203);
            tbContact.Name = "tbContact";
            tbContact.Size = new Size(140, 27);
            tbContact.TabIndex = 67;
            // 
            // tbPosition
            // 
            tbPosition.Location = new Point(550, 236);
            tbPosition.Name = "tbPosition";
            tbPosition.Size = new Size(140, 27);
            tbPosition.TabIndex = 68;
            // 
            // button2
            // 
            button2.Location = new Point(242, 343);
            button2.Name = "button2";
            button2.Size = new Size(217, 65);
            button2.TabIndex = 69;
            button2.Text = "Personal Löschen";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // cPersonalOverview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(703, 416);
            Controls.Add(button2);
            Controls.Add(tbPosition);
            Controls.Add(tbContact);
            Controls.Add(tbNumber);
            Controls.Add(cbOtherLanguage);
            Controls.Add(tbLanguage);
            Controls.Add(cbGender);
            Controls.Add(tbChip);
            Controls.Add(tbBirthPlace);
            Controls.Add(tbLiving);
            Controls.Add(tbBirthday);
            Controls.Add(tbName);
            Controls.Add(tbSurName);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(cbCompany);
            Controls.Add(bAddWorker);
            Controls.Add(label34);
            Controls.Add(label36);
            Controls.Add(label28);
            Controls.Add(label30);
            Controls.Add(label32);
            Controls.Add(label26);
            Controls.Add(label24);
            Controls.Add(label22);
            Controls.Add(label20);
            Controls.Add(label18);
            Controls.Add(label16);
            Controls.Add(lbCheckedIn);
            Controls.Add(label3);
            Controls.Add(cbMitarbeiterID);
            Controls.Add(Position);
            Controls.Add(Name);
            Controls.Add(label1);
            Text = "Festival Manager Personal Übersicht";
            FormClosed += cEquipmentRent_FormClosed;
            Load += cPersonalOverview_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label Name;
        private Label Position;
        public ComboBox cbMitarbeiterID;
        private Label label3;
        private Label lbCheckedIn;
        private Label label16;
        private Label label18;
        private Label label20;
        private Label label22;
        private Label label24;
        private Label label26;
        private Label label32;
        private Label label30;
        private Label label28;
        private Label label36;
        private Label label34;
        private Button bAddWorker;
        private ComboBox cbCompany;
        private Button button1;
        private Label label2;
        private TextBox tbSurName;
        private TextBox tbName;
        private TextBox tbBirthday;
        private TextBox tbLiving;
        private TextBox tbBirthPlace;
        private TextBox tbChip;
        private ComboBox cbGender;
        private TextBox tbLanguage;
        private ComboBox cbOtherLanguage;
        private TextBox tbNumber;
        private TextBox tbContact;
        private TextBox tbPosition;
        private Button button2;
    }
}