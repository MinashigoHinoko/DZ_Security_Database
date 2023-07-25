namespace Festival_Manager
{
    partial class cEquipmentOverview
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
            lbBlack = new Label();
            label36 = new Label();
            lbRed = new Label();
            label28 = new Label();
            lbBlue = new Label();
            label30 = new Label();
            label32 = new Label();
            lbDefect = new Label();
            label20 = new Label();
            lbBestand = new Label();
            cbEquipment = new ComboBox();
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
            bAddWorker.Click += bAddWorker_Click;
            // 
            // lbBlack
            // 
            lbBlack.AutoSize = true;
            lbBlack.Location = new Point(234, 109);
            lbBlack.Name = "lbBlack";
            lbBlack.Size = new Size(17, 20);
            lbBlack.TabIndex = 92;
            lbBlack.Text = "0";
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
            // lbRed
            // 
            lbRed.AutoSize = true;
            lbRed.Location = new Point(234, 89);
            lbRed.Name = "lbRed";
            lbRed.Size = new Size(17, 20);
            lbRed.TabIndex = 90;
            lbRed.Text = "0";
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
            // lbBlue
            // 
            lbBlue.AutoSize = true;
            lbBlue.Location = new Point(234, 69);
            lbBlue.Name = "lbBlue";
            lbBlue.Size = new Size(17, 20);
            lbBlue.TabIndex = 88;
            lbBlue.Text = "0";
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
            // lbDefect
            // 
            lbDefect.AutoSize = true;
            lbDefect.Location = new Point(123, 69);
            lbDefect.Name = "lbDefect";
            lbDefect.Size = new Size(17, 20);
            lbDefect.TabIndex = 78;
            lbDefect.Text = "0";
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
            // lbBestand
            // 
            lbBestand.AutoSize = true;
            lbBestand.Location = new Point(123, 49);
            lbBestand.Name = "lbBestand";
            lbBestand.Size = new Size(17, 20);
            lbBestand.TabIndex = 60;
            lbBestand.Text = "0";
            // 
            // cbEquipment
            // 
            cbEquipment.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbEquipment.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbEquipment.Cursor = Cursors.Hand;
            cbEquipment.FormattingEnabled = true;
            cbEquipment.Location = new Point(124, 6);
            cbEquipment.Margin = new Padding(3, 4, 3, 4);
            cbEquipment.Name = "cbEquipment";
            cbEquipment.Size = new Size(138, 28);
            cbEquipment.Sorted = true;
            cbEquipment.TabIndex = 59;
            cbEquipment.SelectedIndexChanged += cbEquipment_SelectedIndexChanged;
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
            // cEquipmentOverview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(326, 220);
            Controls.Add(label2);
            Controls.Add(bAddWorker);
            Controls.Add(lbBlack);
            Controls.Add(label36);
            Controls.Add(lbRed);
            Controls.Add(label28);
            Controls.Add(lbBlue);
            Controls.Add(label30);
            Controls.Add(label32);
            Controls.Add(lbDefect);
            Controls.Add(label20);
            Controls.Add(lbBestand);
            Controls.Add(cbEquipment);
            Controls.Add(Name);
            Controls.Add(label1);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Festival Manager Equipment Übersicht";
            FormClosed += cEquipmentRent_FormClosed;
            Load += cEquipmentOverview_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button bAddWorker;
        private Label lbBlack;
        private Label label36;
        private Label lbRed;
        private Label label28;
        private Label lbBlue;
        private Label label30;
        private Label label32;
        private Label lbDefect;
        private Label label20;
        private Label lbBestand;
        public ComboBox cbEquipment;
        private Label Name;
        private Label label1;
        private Label label2;
    }
}