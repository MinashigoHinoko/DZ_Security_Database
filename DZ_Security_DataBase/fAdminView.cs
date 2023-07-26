using System.Data;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cAdminView : Form
    {
        private cViewManager viewManager = new();
        private string username;
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public cAdminView(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e, username);
        }


        private void bPrint_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.printOut(sender, e, true, username);
        }

        private void bToolOverlay_Click(object sender, EventArgs e)
        {

            Form dataForm = new();
            dataForm.Text = "Ausrüstungs Übersicht";
            dataForm.Width = 800;
            dataForm.Height = 600;

            DataGridView dataGridView = new() { Dock = DockStyle.Fill, ReadOnly = true };

            TextBox searchBox = new() { Dock = DockStyle.Top, Height = 40 };
            Button searchButton = new() { Text = "Suchen", Dock = DockStyle.Top, Height = 40 };

            // Summary Labels
            Label totalLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };
            Label checkedInLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };
            Label rentStateLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };
            Label nightShiftLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };

            Panel labelPanel = new() { Dock = DockStyle.Right, Width = 500 };
            labelPanel.Controls.AddRange(new Control[] { totalLabel, checkedInLabel, rentStateLabel, nightShiftLabel });
            Panel searchPanel = new() { Dock = DockStyle.Left, Height = 300 };
            searchPanel.Controls.AddRange(new Control[] { searchButton, searchBox });
            Panel overlayPanel = new() { Dock = DockStyle.Top };
            overlayPanel.Controls.AddRange(new Control[] { searchPanel, labelPanel });

            dataForm.Controls.Add(dataGridView);
            dataForm.Controls.Add(overlayPanel);

            void LoadData(string search = "")
            {
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();
                    string query = @"
            SELECT *
            FROM Ausruestung
            GROUP BY MitarbeiterID";

                    if (!string.IsNullOrEmpty(search))
                    {
                        IEnumerable<string> searchTerms = search.Split(';').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t));
                        List<string> conditions = new();

                        foreach (string? term in searchTerms)
                        {
                            conditions.Add($@"
                (ID LIKE '%{term}%' 
                OR Art LIKE '%{term}%' 
                OR Farbe LIKE '%{term}%'
                OR Zustand LIKE '%{term}%'
                OR MitarbeiterID LIKE '%{term}%')");
                        }

                        if (conditions.Count > 0)
                        {
                            query += " HAVING " + string.Join(" AND ", conditions);
                        }
                    }

                    SQLiteDataAdapter da = new(query, conn);
                    DataTable dt = new();
                    da.Fill(dt);

                    dataGridView.DataSource = dt;

                    // Update summary labels
                    totalLabel.Text = "Wie viele Ausrüstungsteile sind Ausgeliehen: " + dt.Rows.Count;
                    checkedInLabel.Text = "Wie viele Ausrüstungsteile sind Blau: " + dt.Select("Farbe = 'blau'").Length;
                    rentStateLabel.Text = "Wie viele Ausrüstungsteile sind Rot: " + dt.Select("Farbe = 'rot'").Length;
                    nightShiftLabel.Text = "Wie viele Ausrüstungsteile sind Schwarz: " + dt.Select("Farbe = 'schwarz'").Length;
                }
            }

            searchButton.Click += (sender, e) =>
            {
                LoadData(searchBox.Text);
            };

            LoadData(); // Load all data at first

            dataForm.ShowDialog();
        }

        private void bWorkerOverview_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.workerOverview(sender, e, true, username);
        }

        private void cAdminView_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void cAdminView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            cLoginMenu cLoginMenu = new();
            cLoginMenu.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            UserManagementForm userManagementForm = new(username);
            userManagementForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            fInportData inportData = new(true, username);
            inportData.ShowDialog();
        }

        private void bWorker_Click(object sender, EventArgs e)
        {
            Form dataForm = new();
            dataForm.Text = "Mitarbeiter Übersicht";
            dataForm.Width = 800;
            dataForm.Height = 600;

            DataGridView dataGridView = new() { Dock = DockStyle.Fill, ReadOnly = true };

            TextBox searchBox = new() { Dock = DockStyle.Top, Height = 40 };
            Button searchButton = new() { Text = "Suchen", Dock = DockStyle.Top, Height = 40 };

            // Summary Labels
            Label totalLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };
            Label checkedInLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };
            Label rentStateLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };
            Label nightShiftLabel = new() { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight };

            Panel labelPanel = new() { Dock = DockStyle.Right, Width = 400 };
            labelPanel.Controls.AddRange(new Control[] { totalLabel, checkedInLabel, rentStateLabel, nightShiftLabel });
            Panel searchPanel = new() { Dock = DockStyle.Left, Height = 300 };
            searchPanel.Controls.AddRange(new Control[] { searchButton, searchBox });
            Panel overlayPanel = new() { Dock = DockStyle.Top };
            overlayPanel.Controls.AddRange(new Control[] { searchPanel, labelPanel });

            dataForm.Controls.Add(dataGridView);
            dataForm.Controls.Add(overlayPanel);

            void LoadData(string search = "")
            {
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();
                    string query = @"
            SELECT M.*, GROUP_CONCAT(MS.Sprache, ', ') AS Sprachen
            FROM Mitarbeiter M
            LEFT JOIN MitarbeiterSprachen MS ON M.MitarbeiterID = MS.MitarbeiterID
            GROUP BY M.MitarbeiterID";

                    if (!string.IsNullOrEmpty(search))
                    {
                        IEnumerable<string> searchTerms = search.Split(';').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t));
                        List<string> conditions = new();

                        foreach (string? term in searchTerms)
                        {
                            conditions.Add($@"
                (Firma LIKE '%{term}%' 
                OR Vorname LIKE '%{term}%' 
                OR Nachname LIKE '%{term}%'
                OR Sprachen LIKE '%{term}%'
                OR Position LIKE '%{term}%')");
                        }

                        if (conditions.Count > 0)
                        {
                            query += " HAVING " + string.Join(" AND ", conditions);
                        }
                    }

                    SQLiteDataAdapter da = new(query, conn);
                    DataTable dt = new();
                    da.Fill(dt);

                    dataGridView.DataSource = dt;

                    // Update summary labels
                    totalLabel.Text = "Wie viele Mitarbeiter sind in der Ansicht: " + dt.Rows.Count;
                    checkedInLabel.Text = "Wie viele sind Eingechecked: " + dt.Select("CheckInState = 'true'").Length;
                    rentStateLabel.Text = "Wie viele Mitarbeiter haben etwas ausgeliehen: " + dt.Select("RentState = 'true'").Length;

                    // Query for night shift workers
                    SQLiteCommand cmd = new("SELECT COUNT(*) FROM ArbeitszeitenSoll WHERE Nacht = 'true' AND MitarbeiterID IN (" + string.Join(", ", dt.AsEnumerable().Select(r => r.Field<long>("MitarbeiterID"))) + ")", conn);
                    nightShiftLabel.Text = "Nacht schicht: " + cmd.ExecuteScalar();
                }
            }

            searchButton.Click += (sender, e) =>
            {
                LoadData(searchBox.Text);
            };

            LoadData(); // Load all data at first

            dataForm.ShowDialog();

        }
    }
}
