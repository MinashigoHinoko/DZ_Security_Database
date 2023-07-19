namespace Festival_Manager
{
    public class UserManagementForm : Form
    {
        private cPasswordManager.UserManagement _userManagement;
        private Label _lblTargetUsername;
        private ComboBox _cbTargetUsername;
        private Label _lblRights;
        private Label _lblNewPassword;
        private TextBox _txtNewPassword;
        private ComboBox _cbRole;
        private CheckBox _chbPin;
        private Label _lblNewPin;
        private TextBox _txtNewPin;
        private Button _btnSaveChanges;
        private Button _btnDeleteUser;
        string username;

        public UserManagementForm(string username)
        {
            username = username;
            _userManagement = new cPasswordManager.UserManagement();

            this.Text = "Festival Manager Account Bearbeiter";
            this.Size = new Size(400, 380);
            this.StartPosition = FormStartPosition.CenterScreen;

            _lblTargetUsername = new Label() { Text = "Account:", Left = 60, Top = 50, Width = 70 };
            _cbTargetUsername = new ComboBox() { Left = 150, Top = 50, Width = 200 };
            LoadUsernames();
            _lblNewPassword = new Label() { Text = "Neues Passwort:", Left = 30, Top = 100, Width = 120 };
            _txtNewPassword = new TextBox() { Left = 150, Top = 100, Width = 200 };
            _cbTargetUsername.DropDownStyle = ComboBoxStyle.DropDownList;

            _lblRights = new Label() { Text = "Rechte:", Left = 60, Top = 150, Width = 60 };
            _cbRole = new ComboBox() { Left = 150, Top = 150, Width = 200 };
            _cbRole.Items.AddRange(new string[] { "admin", "member", "booking" });
            _cbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            _cbTargetUsername.SelectedIndexChanged += (sender, e) =>
            {
                cPasswordManager.CheckRights checkRights = new cPasswordManager.CheckRights();
                _cbRole.SelectedItem = checkRights.rightCheck(_cbTargetUsername.SelectedItem.ToString());
                _txtNewPin.Text = null;
            };
            _lblNewPin = new Label() { Text = "Neue PIN:", Left = 50, Top = 250, Width = 100 };
            _txtNewPin = new TextBox() { Left = 150, Top = 250, Width = 200 };
            _chbPin = new CheckBox() { Left = 130, Top = 200, Width = 220, Text = "Darf der Nutzer Editieren?" };
            _lblNewPin.Visible = false;
            _txtNewPin.Visible = false;

            // Ereignishandler für die CheckBox
            _chbPin.Click += (sender, e) =>
            {
                // Zeigt das PIN-Eingabefeld an, wenn die CheckBox aktiviert ist; andernfalls versteckt es
                _lblNewPin.Visible = _chbPin.Checked;
                _txtNewPin.Visible = _chbPin.Checked;
            };

            _btnSaveChanges = new Button() { Text = "Speicher Nutzer", Left = 50, Width = 150, Dock = DockStyle.Left };
            _btnSaveChanges.Click += OnSaveChangesClicked;

            _btnDeleteUser = new Button() { Text = "Lösche Nutzer", Left = 210, Width = 150, Dock = DockStyle.Right };
            _btnDeleteUser.Click += OnDeleteUserClicked;

            Panel bottomPanel = new Panel() { Dock = DockStyle.Bottom, Height = 50 };
            bottomPanel.Controls.Add(_btnSaveChanges);
            bottomPanel.Controls.Add(_btnDeleteUser);

            this.Controls.Add(_lblTargetUsername);
            this.Controls.Add(_cbTargetUsername);
            this.Controls.Add(_lblNewPassword);
            this.Controls.Add(_txtNewPassword);
            this.Controls.Add(_lblRights);
            this.Controls.Add(_cbRole);
            this.Controls.Add(_chbPin);
            this.Controls.Add(_lblNewPin);
            this.Controls.Add(_txtNewPin);
            this.Controls.Add(bottomPanel);
        }
        private void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (_cbTargetUsername.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle ein Account aus!");
                return;
            }

            string targetUsername = _cbTargetUsername.SelectedItem.ToString();

            if (username == targetUsername)
            {
                MessageBox.Show("Der momentan eingeloggte Account kann nicht gelöscht werden.");
                return;
            }

            var result = MessageBox.Show($"Sind Sie sich sicher, dass Sie den Account '{targetUsername}' Löschen möchten ?", "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _userManagement.DeleteUser(targetUsername);
                LoadUsernames();
                cLogger.LogDatabaseChange($"Lösche Account: {targetUsername}", username);
            }
        }
        private void LoadUsernames()
        {
            _cbTargetUsername.Items.Clear();
            List<string> usernames = _userManagement.GetUsernames();
            _cbTargetUsername.Items.AddRange(usernames.ToArray());
        }

        private void OnSaveChangesClicked(object sender, EventArgs e)
        {
            if (_cbTargetUsername.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle ein Account aus!");
                return;
            }
            string targetUsername = _cbTargetUsername.SelectedItem.ToString();
            string rights = _cbRole.SelectedItem.ToString();
            string newPin = _txtNewPin.Text;
            string newPassword = _txtNewPassword.Text;

            var result = MessageBox.Show($"Sind Sie sich sicher, dass Sie den Account '{targetUsername}' Bearbeiten möchten ?", "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                cLogger.LogDatabaseChange($"Speichere Änderungen an: {targetUsername}", username);
                _userManagement.ChangeUserRights(username, targetUsername, rights);
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    _userManagement.ChangeUserPassword(username, targetUsername, newPassword);
                }
                if (_chbPin.Checked)
                {
                    _userManagement.ChangeUserPin(username, targetUsername, newPin);
                }
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // UserManagementForm
            // 
            ClientSize = new Size(282, 253);
            Name = "UserManagementForm";
            Load += UserManagementForm_Load_1;
            ResumeLayout(false);
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
        }

        private void UserManagementForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
