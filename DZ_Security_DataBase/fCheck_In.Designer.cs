namespace DZ_Security_DataBase
{
    partial class cCheckIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            start_Work_Timestamp = new Button();
            stop_Work_Timestamp = new Button();
            label1 = new Label();
            Vorname = new Label();
            Position = new Label();
            label4 = new Label();
            label5 = new Label();
            dgvArbeitszeit = new DataGridView();
            cbMitarbeiterID = new ComboBox();
            lbSurname = new Label();
            lbPosition = new Label();
            lbName = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvArbeitszeit).BeginInit();
            SuspendLayout();
            // 
            // start_Work_Timestamp
            // 
            start_Work_Timestamp.AutoSize = true;
            start_Work_Timestamp.Cursor = Cursors.Hand;
            start_Work_Timestamp.Location = new Point(14, 412);
            start_Work_Timestamp.Margin = new Padding(3, 4, 3, 4);
            start_Work_Timestamp.Name = "start_Work_Timestamp";
            start_Work_Timestamp.Size = new Size(142, 33);
            start_Work_Timestamp.TabIndex = 0;
            start_Work_Timestamp.Text = "Arbeitszeit Starten";
            start_Work_Timestamp.UseVisualStyleBackColor = true;
            start_Work_Timestamp.Click += button1_Click;
            // 
            // stop_Work_Timestamp
            // 
            stop_Work_Timestamp.AutoSize = true;
            stop_Work_Timestamp.Cursor = Cursors.Hand;
            stop_Work_Timestamp.Enabled = false;
            stop_Work_Timestamp.Location = new Point(401, 412);
            stop_Work_Timestamp.Margin = new Padding(3, 4, 3, 4);
            stop_Work_Timestamp.Name = "stop_Work_Timestamp";
            stop_Work_Timestamp.Size = new Size(150, 33);
            stop_Work_Timestamp.TabIndex = 1;
            stop_Work_Timestamp.Text = "Arbeitszeit Stoppen";
            stop_Work_Timestamp.UseVisualStyleBackColor = true;
            stop_Work_Timestamp.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 57);
            label1.Name = "label1";
            label1.Size = new Size(67, 20);
            label1.TabIndex = 2;
            label1.Text = "Personal:";
            // 
            // Vorname
            // 
            Vorname.AutoSize = true;
            Vorname.Location = new Point(14, 86);
            Vorname.Name = "Vorname";
            Vorname.Size = new Size(71, 20);
            Vorname.TabIndex = 3;
            Vorname.Text = "Vorname:";
            // 
            // Position
            // 
            Position.AutoSize = true;
            Position.Location = new Point(14, 128);
            Position.Name = "Position";
            Position.Size = new Size(64, 20);
            Position.TabIndex = 4;
            Position.Text = "Position:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(14, 155);
            label4.Name = "label4";
            label4.Size = new Size(197, 25);
            label4.TabIndex = 5;
            label4.Text = "Arbeitszeit Datenbank";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(14, 12);
            label5.Name = "label5";
            label5.Size = new Size(205, 25);
            label5.TabIndex = 7;
            label5.Text = "Ausgewähltes Personal";
            // 
            // dgvArbeitszeit
            // 
            dgvArbeitszeit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvArbeitszeit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArbeitszeit.Cursor = Cursors.No;
            dgvArbeitszeit.Location = new Point(14, 185);
            dgvArbeitszeit.Margin = new Padding(3, 4, 3, 4);
            dgvArbeitszeit.Name = "dgvArbeitszeit";
            dgvArbeitszeit.RowHeadersWidth = 51;
            dgvArbeitszeit.RowTemplate.Height = 25;
            dgvArbeitszeit.Size = new Size(537, 200);
            dgvArbeitszeit.TabIndex = 8;
            // 
            // cbMitarbeiterID
            // 
            cbMitarbeiterID.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbMitarbeiterID.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbMitarbeiterID.Cursor = Cursors.Hand;
            cbMitarbeiterID.FormattingEnabled = true;
            cbMitarbeiterID.Location = new Point(109, 53);
            cbMitarbeiterID.Margin = new Padding(3, 4, 3, 4);
            cbMitarbeiterID.Name = "cbMitarbeiterID";
            cbMitarbeiterID.Size = new Size(138, 28);
            cbMitarbeiterID.Sorted = true;
            cbMitarbeiterID.TabIndex = 9;
            cbMitarbeiterID.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // lbSurname
            // 
            lbSurname.AutoSize = true;
            lbSurname.Location = new Point(83, 86);
            lbSurname.Name = "lbSurname";
            lbSurname.Size = new Size(0, 20);
            lbSurname.TabIndex = 10;
            // 
            // lbPosition
            // 
            lbPosition.AutoSize = true;
            lbPosition.Location = new Point(81, 128);
            lbPosition.Name = "lbPosition";
            lbPosition.Size = new Size(0, 20);
            lbPosition.TabIndex = 11;
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.Location = new Point(91, 106);
            lbName.Name = "lbName";
            lbName.Size = new Size(0, 20);
            lbName.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 106);
            label3.Name = "label3";
            label3.Size = new Size(83, 20);
            label3.TabIndex = 12;
            label3.Text = "Nachname:";
            // 
            // cCheckIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            ClientSize = new Size(559, 456);
            Controls.Add(lbName);
            Controls.Add(label3);
            Controls.Add(lbPosition);
            Controls.Add(lbSurname);
            Controls.Add(cbMitarbeiterID);
            Controls.Add(dgvArbeitszeit);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(Position);
            Controls.Add(Vorname);
            Controls.Add(label1);
            Controls.Add(stop_Work_Timestamp);
            Controls.Add(start_Work_Timestamp);
            Margin = new Padding(3, 4, 3, 4);
            Name = "cCheckIn";
            Text = "Placeholder";
            FormClosed += fCheckin_FormClosed;
            Load += cCheckIn_Load;
            ((System.ComponentModel.ISupportInitialize)dgvArbeitszeit).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button start_Work_Timestamp;
        private Button stop_Work_Timestamp;
        private Label label1;
        private Label Vorname;
        private Label Position;
        private Label label4;
        private Label label5;
        private DataGridView dgvArbeitszeit;
        private Label lbSurname;
        private Label lbPosition;
        public ComboBox cbMitarbeiterID;
        private Button convert_Excel_Button;
        private Label lbName;
        private Label label3;
    }
}