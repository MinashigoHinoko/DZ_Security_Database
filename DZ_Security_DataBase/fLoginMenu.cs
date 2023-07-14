using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class cLoginMenu : Form
    {
        private bool firstLoad = true;
        public cLoginMenu()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void bRegister_Click(object sender, EventArgs e)
        {
            // Erstellen Sie eine neue Instanz der Login-Klasse.
            cPasswordManager.Login loginManager = new cPasswordManager.Login();

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
            Form registrationForm = new Form();
            registrationForm.Width = 300;
            registrationForm.Height = 250;
            registrationForm.Text = "Benutzerregistrierung";

            // Erzeugt Labels für Benutzername, Passwort und Benutzerrolle
            Label lblUsername = new Label() { Left = 50, Top = 0, Width = 200, Text = "Nutzername:" };
            Label lblPassword = new Label() { Left = 50, Top = 50, Width = 200, Text = "Passwort:" };
            Label lblRole = new Label() { Left = 50, Top = 100, Width = 200, Text = "Benutzerrolle:" };

            // Erzeugt TextBoxen für Benutzername und Passwort
            TextBox txtUsername = new TextBox() { Left = 50, Top = 20, Width = 200 };
            TextBox txtPassword = new TextBox() { Left = 50, Top = 70, Width = 200, PasswordChar = '*' };

            // Erzeugt ComboBox für die Benutzerrolle
            ComboBox cbRole = new ComboBox() { Left = 50, Top = 120, Width = 200 };
            cbRole.Items.AddRange(new string[] { "admin", "member", "booking" });

            // Erzeugt einen neuen Button zum Einreichen der Benutzerregistrierung
            Button registerButton = new Button() { Text = "Registrieren", Dock = DockStyle.Bottom };
            registerButton.Width = 100; // Setzt die Breite
            registerButton.Height = 30; // Setzt die Höhe

            registerButton.Click += (sender, e) =>
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                    string role = cbRole.SelectedItem.ToString(); // Nehmen Sie die ausgewählte Rolle

                    // Erzeugt eine neue Instanz von cPasswordManager.Registration und versucht, den Benutzer zu registrieren
                    cPasswordManager.Registration registrationManager = new cPasswordManager.Registration();

                    try
                    {
                        registrationManager.RegisterUser(username, password, role); // Passen Sie Ihre RegisterUser-Methode an, um eine Rolle zu akzeptieren
                        MessageBox.Show("Benutzer erfolgreich registriert!");
                        registrationForm.Close();
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


            // Setzt die AcceptButton-Eigenschaft des Formulars
            registrationForm.AcceptButton = registerButton;

            // Fügt die Labels, TextBoxen, ComboBox und den Button zum Formular hinzu
            registrationForm.Controls.Add(lblUsername);
            registrationForm.Controls.Add(txtUsername);
            registrationForm.Controls.Add(lblPassword);
            registrationForm.Controls.Add(txtPassword);
            registrationForm.Controls.Add(lblRole);
            registrationForm.Controls.Add(cbRole);
            registrationForm.Controls.Add(registerButton);

            // Zeigt das Formular an
            registrationForm.TopMost = true;
            registrationForm.ShowDialog();
        }


        private void cLoginMenu_Load(object sender, EventArgs e)
        {
            if (firstLoad)
            {
                cDataBase.createDatabase();
                cDataBase.editDatabase();
                firstLoad = false;
            }
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            // Nehmen Sie den Benutzernamen und das Passwort aus Ihren Textboxen.
            string username = tbUsername.Text;
            string password = tbPassword.Text;

            // Erstellen Sie eine neue Instanz der Login-Klasse.
            cPasswordManager.Login loginManager = new cPasswordManager.Login();

            // Überprüfen Sie die Anmeldedaten des Benutzers.
            string role = loginManager.AuthenticateUser(username, password);

            if (role != null)
            {
                // Anmeldung war erfolgreich.
                this.Hide();
                if (role == "admin")
                {
                    cAdminView admin = new cAdminView();
                    admin.Show();
                }
                else if (role == "member")
                {
                    cMemberView member = new cMemberView();
                    member.Show();
                }
                else if (role == "booking")
                {
                    cBookingView booking = new cBookingView();
                    booking.Show();
                }
            }
            else
            {
                // Anmeldung war nicht erfolgreich. Zeigen Sie eine Fehlermeldung an.
                MessageBox.Show("Fehlerhafte Anmeldedaten, bitte versuchen Sie es erneut.");
            }
        }
    }
}
