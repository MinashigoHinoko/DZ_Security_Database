using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cLoginMenu : Form
    {
        private bool firstLoad = true;
        public cLoginMenu()
        {
            InitializeComponent();
            TopMost = true;
        }

        private void bRegister_Click(object sender, EventArgs e)
        {
            // Erstellen Sie eine neue Instanz der Login-Klasse.
            cPasswordManager.Login loginManager = new();

            // Überprüfen Sie, ob die Datenbank leer ist.
            bool isDatabaseEmpty = loginManager.IsDatabaseEmpty();

            if (isDatabaseEmpty)
            {
                // Wenn die Datenbank leer ist, können Sie direkt zum Registrierungsformular gehen.
                showRegistrationForm();
            }
            else
            {
                // Nehmen Sie den Benutzernamen und das Passwort aus Ihren Textboxen.
                string username = tbUsername.Text;
                string password = tbPassword.Text;

                // Überprüfen Sie die Anmeldedaten des Benutzers.
                string role = loginManager.AuthenticateUser(username, password);

                if (role != null)
                {
                    // Anmeldung war erfolgreich. Hier könnten Sie beispielsweise das Anmeldefenster schließen und das Hauptfenster Ihrer Anwendung öffnen.
                    if (role == "admin")
                    {
                        showRegistrationForm();
                    }
                    else
                    {
                        MessageBox.Show("Bitte Logge dich als admin ein.");
                    }
                }
                else
                {
                    // Anmeldung war nicht erfolgreich. Zeigen Sie eine Fehlermeldung an.
                    MessageBox.Show("Fehlerhafte Anmeldedaten, bitte versuchen Sie es erneut.");
                }
            }
        }


        public void showRegistrationForm()
        {
            // Erzeugt ein neues Formular
            Form registrationForm = new();
            registrationForm.Width = 300;
            registrationForm.Height = 300;
            registrationForm.Text = "Benutzerregistrierung";
            registrationForm.StartPosition = FormStartPosition.CenterScreen;

            // Erzeugt Labels für Benutzername, Passwort und Benutzerrolle
            Label lblUsername = new() { Left = 50, Top = 0, Width = 200, Text = "Nutzername:" };
            Label lblPassword = new() { Left = 50, Top = 50, Width = 200, Text = "Passwort:" };
            Label lblRole = new() { Left = 50, Top = 100, Width = 200, Text = "Benutzerrolle:" };
            CheckBox chbPin = new() { Left = 50, Top = 150, Width = 220, Text = "Darf der Nutzer Editieren?" };
            Label lblPin = new() { Left = 50, Top = 170, Width = 200, Text = "PIN:" };
            lblPin.Visible = false;

            // Erzeugt TextBoxen für Benutzername und Passwort
            TextBox txtUsername = new() { Left = 50, Top = 20, Width = 200 };
            TextBox txtPassword = new() { Left = 50, Top = 70, Width = 200, PasswordChar = '*' };
            // Erzeugt ein TextBox für die PIN-Eingabe
            TextBox txtPin = new() { Left = 50, Top = 190, Width = 200 };
            txtPin.Visible = false; // Anfangs unsichtbar

            // Erzeugt ComboBox für die Benutzerrolle
            ComboBox cbRole = new() { Left = 50, Top = 120, Width = 200 };
            cbRole.Items.AddRange(new string[] { "admin", "member", "booking" });
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRole.SelectedIndex = 1;


            // Ereignishandler für die CheckBox
            chbPin.Click += (sender, e) =>
            {
                // Zeigt das PIN-Eingabefeld an, wenn die CheckBox aktiviert ist; andernfalls versteckt es
                txtPin.Visible = chbPin.Checked;
                lblPin.Visible = chbPin.Checked;
            };
            // Erzeugt einen neuen Button zum Einreichen der Benutzerregistrierung
            Button registerButton = new() { Text = "Registrieren", Dock = DockStyle.Bottom };
            registerButton.Width = 100; // Setzt die Breite
            registerButton.Height = 30; // Setzt die Höhe
            registerButton.Click += (sender, e) =>
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string role = cbRole.SelectedItem.ToString(); // Nehmen Sie die ausgewählte Rolle
                bool canEdit = chbPin.Checked;
                string pin = txtPin.Text;

                // Erzeugt eine neue Instanz von cPasswordManager.Registration und versucht, den Benutzer zu registrieren
                cPasswordManager.Registration registrationManager = new();

                try
                {
                    bool success = registrationManager.RegisterUser(username, password, role, canEdit, pin); // Passen Sie Ihre RegisterUser-Methode an, um eine Rolle zu akzeptieren
                    if (success)
                    {
                        cLogger.LogDatabaseChange($"Registriert Nutzer mit {role} rechte", username);
                        MessageBox.Show("Benutzer erfolgreich registriert!");
                        registrationForm.Close();
                    }
                }
                catch (Exception ex) // Ändern Sie den Exception-Typ je nach verwendeter Datenbank
                {
                    if (ex is SQLiteException sqliteEx && sqliteEx.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        // Dieser Fehler tritt auf, wenn ein Benutzername, der bereits in der Datenbank existiert, verwendet wird
                        MessageBox.Show("Der Benutzername existiert bereits, bitte wählen Sie einen anderen.");
                    }
                    else
                    {
                        // Für alle anderen Fehler
                        MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                    }
                }
            };

            txtUsername.Cursor = Cursors.IBeam;
            txtPassword.Cursor = Cursors.IBeam;
            cbRole.Cursor = Cursors.Hand;
            chbPin.Cursor = Cursors.Hand;
            txtPin.Cursor = Cursors.IBeam;
            registerButton.Cursor = Cursors.Hand;
            // Setzt die AcceptButton-Eigenschaft des Formulars
            registrationForm.AcceptButton = registerButton;

            // Fügt die Labels, TextBoxen, ComboBox und den Button zum Formular hinzu
            registrationForm.Controls.Add(lblUsername);
            registrationForm.Controls.Add(txtUsername);
            registrationForm.Controls.Add(lblPassword);
            registrationForm.Controls.Add(txtPassword);
            registrationForm.Controls.Add(lblRole);
            registrationForm.Controls.Add(cbRole);
            registrationForm.Controls.Add(chbPin);
            registrationForm.Controls.Add(txtPin);
            registrationForm.Controls.Add(lblPin);
            registrationForm.Controls.Add(registerButton);

            // Zeigt das Formular an
            registrationForm.TopMost = true;
            registrationForm.ShowDialog();
        }


        private void cLoginMenu_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
            if (firstLoad)
            {
                cDataBase.createDatabase();
                firstLoad = false;
            }
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            // Nehmen Sie den Benutzernamen und das Passwort aus Ihren Textboxen.
            string username = tbUsername.Text;
            string password = tbPassword.Text;

            // Erstellen Sie eine neue Instanz der Login-Klasse.
            cPasswordManager.Login loginManager = new();

            // Überprüfen Sie die Anmeldedaten des Benutzers.
            string role = loginManager.AuthenticateUser(username, password);

            if (role != null)
            {
                cLogger.LogDatabaseChange($"Login {role}", username);
                // Anmeldung war erfolgreich.
                Hide();
                if (role == "admin")
                {
                    cAdminView admin = new(username);
                    admin.Show();
                }
                else if (role == "member")
                {
                    cMemberView member = new(username);
                    member.Show();
                }
                else if (role == "booking")
                {
                    cBookingView booking = new(username);
                    booking.Show();
                }
            }
            else
            {
                // Anmeldung war nicht erfolgreich. Zeigen Sie eine Fehlermeldung an.
                MessageBox.Show("Fehlerhafte Anmeldedaten, bitte versuchen Sie es erneut.");
            }
        }

        private void cLoginMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }
    }
}
