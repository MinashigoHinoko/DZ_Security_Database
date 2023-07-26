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
        private string username;

        public UserManagementForm(string username)
        {
            FormClosed += UserManagementForm_FormClosed;
            this.username = username;
            _userManagement = new cPasswordManager.UserManagement();

            Text = "Festival Manager Account Bearbeiter";
            Size = new Size(400, 380);
            StartPosition = FormStartPosition.CenterScreen;

            _lblTargetUsername = new Label() { Text = "Account:", Left = 60, Top = 50, Width = 70 };
            _cbTargetUsername = new ComboBox() { Left = 150, Top = 50, Width = 200 };
            LoadUsernames();
            _lblNewPassword = new Label() { Text = "Neues Passwort:", Left = 30, Top = 100, Width = 120 };
            _txtNewPassword = new TextBox() { Left = 150, Top = 100, Width = 200 };
            _cbTargetUsername.DropDownStyle = ComboBoxStyle.DropDown;

            _lblRights = new Label() { Text = "Rechte:", Left = 60, Top = 150, Width = 60 };
            _cbRole = new ComboBox() { Left = 150, Top = 150, Width = 200 };
            _cbRole.Items.AddRange(new string[] { "admin", "member", "booking" });
            _cbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            _cbTargetUsername.SelectedIndexChanged += (sender, e) =>
            {
                cPasswordManager.CheckRights checkRights = new();
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

            Panel bottomPanel = new() { Dock = DockStyle.Bottom, Height = 50 };
            bottomPanel.Controls.Add(_btnSaveChanges);
            bottomPanel.Controls.Add(_btnDeleteUser);

            Controls.Add(_lblTargetUsername);
            Controls.Add(_cbTargetUsername);
            Controls.Add(_lblNewPassword);
            Controls.Add(_txtNewPassword);
            Controls.Add(_lblRights);
            Controls.Add(_cbRole);
            Controls.Add(_chbPin);
            Controls.Add(_lblNewPin);
            Controls.Add(_txtNewPin);
            Controls.Add(bottomPanel);
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

            DialogResult result = MessageBox.Show($"Sind Sie sich sicher, dass Sie den Account '{targetUsername}' Löschen möchten ?", "Confirmation", MessageBoxButtons.YesNo);

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

            DialogResult result = MessageBox.Show($"Sind Sie sich sicher, dass Sie den Account '{targetUsername}' Bearbeiten möchten ?", "Confirmation", MessageBoxButtons.YesNo);

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
            ResumeLayout(false);
        }

        private void UserManagementForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            cAdminView cAdminView = new(username);
            cAdminView.ShowDialog();
        }
    }
}
