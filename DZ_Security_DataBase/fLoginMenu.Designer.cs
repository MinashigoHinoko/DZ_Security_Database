namespace DZ_Security_DataBase
{
    partial class cLoginMenu
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
            tbUsername = new TextBox();
            label1 = new Label();
            label2 = new Label();
            tbPassword = new TextBox();
            bLogin = new Button();
            bRegister = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // tbUsername
            // 
            tbUsername.Location = new Point(107, 8);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(125, 27);
            tbUsername.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 8);
            label1.Name = "label1";
            label1.Size = new Size(93, 20);
            label1.TabIndex = 1;
            label1.Text = "Nutzername:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 38);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 3;
            label2.Text = "Passwort:";
            // 
            // tbPassword
            // 
            tbPassword.Location = new Point(107, 38);
            tbPassword.Name = "tbPassword";
            tbPassword.PasswordChar = '*';
            tbPassword.Size = new Size(125, 27);
            tbPassword.TabIndex = 2;
            // 
            // bLogin
            // 
            bLogin.Location = new Point(12, 74);
            bLogin.Name = "bLogin";
            bLogin.Size = new Size(111, 29);
            bLogin.TabIndex = 4;
            bLogin.Text = "Login";
            bLogin.UseVisualStyleBackColor = true;
            bLogin.Click += bLogin_Click;
            // 
            // bRegister
            // 
            bRegister.Location = new Point(177, 74);
            bRegister.Name = "bRegister";
            bRegister.Size = new Size(111, 29);
            bRegister.TabIndex = 5;
            bRegister.Text = "Registrieren";
            bRegister.UseVisualStyleBackColor = true;
            bRegister.Click += bRegister_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(238, 8);
            label3.Name = "label3";
            label3.Size = new Size(53, 12);
            label3.TabIndex = 32;
            label3.Text = "@Mobinoko";
            // 
            // cLoginMenu
            // 
            AcceptButton = bLogin;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bRegister;
            ClientSize = new Size(300, 114);
            Controls.Add(label3);
            Controls.Add(bRegister);
            Controls.Add(bLogin);
            Controls.Add(label2);
            Controls.Add(tbPassword);
            Controls.Add(label1);
            Controls.Add(tbUsername);
            Name = "cLoginMenu";
            Text = "Mobinoko\n Festival Manager LogIn";
            Load += cLoginMenu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbUsername;
        private Label label1;
        private Label label2;
        private TextBox tbPassword;
        private Button bLogin;
        private Button bRegister;
        private Label label3;
    }
}