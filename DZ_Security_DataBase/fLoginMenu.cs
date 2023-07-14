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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void showRegistrationForm()
        {
            // Erzeugt ein neues Formular
            Form registrationForm = new Form();
            registrationForm.Width = 300;
            registrationForm.Height = 200;
            registrationForm.Text = "Benutzerregistrierung";

            // Erzeugt TextBoxen für Benutzername und Passwort
            TextBox txtUsername = new TextBox() { Left = 50, Top = 20, Width = 200 };
            TextBox txtPassword = new TextBox() { Left = 50, Top = 60, Width = 200, PasswordChar = '*' };

            // Erzeugt einen neuen Button zum Einreichen der Benutzerregistrierung
            Button registerButton = new Button() { Text = "Registrieren", Dock = DockStyle.Bottom };
            registerButton.Width = 100; // Setzt die Breite
            registerButton.Height = 30; // Setzt die Höhe

            registerButton.Click += (sender, e) =>
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                // Erzeugt eine neue Instanz von cPasswordManager.Registration und versucht, den Benutzer zu registrieren
                cPasswordManager.Registration registrationManager = new cPasswordManager.Registration();

                try
                {
                    registrationManager.RegisterUser(username, password);
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

            // Fügt die TextBoxen und den Button zum Formular hinzu
            registrationForm.Controls.Add(txtUsername);
            registrationForm.Controls.Add(txtPassword);
            registrationForm.Controls.Add(registerButton);

            // Zeigt das Formular an
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
            //showRegistrationForm();
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            // Nehmen Sie den Benutzernamen und das Passwort aus Ihren Textboxen.
            string username = tbUsername.Text;
            string password = tbPassword.Text;

            // Erstellen Sie eine neue Instanz der Login-Klasse.
            cPasswordManager.Login loginManager = new cPasswordManager.Login();

            // Überprüfen Sie die Anmeldedaten des Benutzers.
            bool isAuthenticated = loginManager.AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                // Anmeldung war erfolgreich. Hier könnten Sie beispielsweise das Anmeldefenster schließen und das Hauptfenster Ihrer Anwendung öffnen.
                this.Hide();
                if (username == "admin")
                {
                    cAdminView admin = new cAdminView();
                    admin.Show();
                }
                if (username == "member")
                {
                    cMemberView member = new cMemberView();
                    member.Show();
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
