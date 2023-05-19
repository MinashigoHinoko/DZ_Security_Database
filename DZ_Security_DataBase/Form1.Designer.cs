namespace DZ_Security_DataBase
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Name = new System.Windows.Forms.Label();
            this.Position = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvArbeitszeit = new System.Windows.Forms.DataGridView();
            this.cbMitarbeiterID = new System.Windows.Forms.ComboBox();
            this.lbName = new System.Windows.Forms.Label();
            this.lbPosition = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArbeitszeit)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Location = new System.Drawing.Point(12, 309);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Arbeitszeit Starten";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Location = new System.Drawing.Point(142, 309);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Arbeitszeit Stoppen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "MitarbeiterID: ";
            // 
            // Name
            // 
            this.Name.AutoSize = true;
            this.Name.Location = new System.Drawing.Point(12, 72);
            this.Name.Name = "Name";
            this.Name.Size = new System.Drawing.Size(112, 15);
            this.Name.TabIndex = 3;
            this.Name.Text = "Vor und Nachname:";
            // 
            // Position
            // 
            this.Position.AutoSize = true;
            this.Position.Location = new System.Drawing.Point(12, 92);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(53, 15);
            this.Position.TabIndex = 4;
            this.Position.Text = "Position:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(12, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Arbeitszeit Datenbank";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(177, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Ausgewählter Mitarbeiter";
            // 
            // dgvArbeitszeit
            // 
            this.dgvArbeitszeit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvArbeitszeit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArbeitszeit.Cursor = System.Windows.Forms.Cursors.No;
            this.dgvArbeitszeit.Location = new System.Drawing.Point(12, 139);
            this.dgvArbeitszeit.Name = "dgvArbeitszeit";
            this.dgvArbeitszeit.RowTemplate.Height = 25;
            this.dgvArbeitszeit.Size = new System.Drawing.Size(470, 150);
            this.dgvArbeitszeit.TabIndex = 8;
            // 
            // cbMitarbeiterID
            // 
            this.cbMitarbeiterID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbMitarbeiterID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbMitarbeiterID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbMitarbeiterID.FormattingEnabled = true;
            this.cbMitarbeiterID.Location = new System.Drawing.Point(95, 40);
            this.cbMitarbeiterID.Name = "cbMitarbeiterID";
            this.cbMitarbeiterID.Size = new System.Drawing.Size(121, 23);
            this.cbMitarbeiterID.Sorted = true;
            this.cbMitarbeiterID.TabIndex = 9;
            this.cbMitarbeiterID.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(130, 72);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(0, 15);
            this.lbName.TabIndex = 10;
            // 
            // lbPosition
            // 
            this.lbPosition.AutoSize = true;
            this.lbPosition.Location = new System.Drawing.Point(71, 92);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(0, 15);
            this.lbPosition.TabIndex = 11;
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Location = new System.Drawing.Point(342, 309);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(140, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Zu Excel Konvertieren";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Cursor = System.Windows.Forms.Cursors.Help;
            this.button4.Location = new System.Drawing.Point(481, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(135, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "Equipment Ausleihen";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Cursor = System.Windows.Forms.Cursors.Help;
            this.button5.Location = new System.Drawing.Point(481, 41);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(135, 23);
            this.button5.TabIndex = 14;
            this.button5.Text = "Equipment Übersicht";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(628, 399);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.lbPosition);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.cbMitarbeiterID);
            this.Controls.Add(this.dgvArbeitszeit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.Name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Text = "DZ Security CheckIn-Tool";
            ((System.ComponentModel.ISupportInitialize)(this.dgvArbeitszeit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label Name;
        private Label Position;
        private Label label4;
        private Label label5;
        private DataGridView dgvArbeitszeit;
        private Label lbName;
        private Label lbPosition;
        public ComboBox cbMitarbeiterID;
        private Button button3;
        private Button button4;
        private Button button5;
    }
}