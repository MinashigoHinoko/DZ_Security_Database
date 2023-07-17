
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SQLite;


namespace DZ_Security_DataBase
{
    public partial class cPrintOutView : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        bool isAdmin = false;
        public cPrintOutView(bool isAdmin)
        {
            this.isAdmin = isAdmin;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, Vorname, Nachname,Geburtsname,Gender,Geburtsdatum,Geburtsort,Wohnort FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FileName = "export_OrdnungsAmt.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Mitarbeiter");

                            // Überschriften
                            IRow row = sheet.CreateRow(0);
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                            }

                            // Daten
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                row = sheet.CreateRow(i + 1);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                                }
                            }

                            // Speichern
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(stream, false);
                            }

                        }
                    }
                }
                conn.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FileName = "export_Eigener.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Mitarbeiter");

                            // Überschriften
                            IRow row = sheet.CreateRow(0);
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                            }

                            // Daten
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                row = sheet.CreateRow(i + 1);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                                }
                            }

                            // Speichern
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(stream, false);
                            }

                        }
                    }
                }
                conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT Vorname, Nachname,Firma,Position,CheckInState FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FileName = "export_ZollAmt.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Mitarbeiter");

                            // Überschriften
                            IRow row = sheet.CreateRow(0);
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                            }

                            // Daten
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                row = sheet.CreateRow(i + 1);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                                }
                            }

                            // Speichern
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(stream, false);
                            }

                        }
                    }
                }
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT Vorname, Nachname,Firma,Position,CheckInState FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FileName = "export_Polizei.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Mitarbeiter");

                            // Überschriften
                            IRow row = sheet.CreateRow(0);
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                            }

                            // Daten
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                row = sheet.CreateRow(i + 1);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                                }
                            }

                            // Speichern
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(stream, false);
                            }

                        }
                    }
                }
                conn.Close();
            }
        }
        private void cEquipmentRent_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            if (this.isAdmin)
            {
                cAdminView cAdminView = new cAdminView();
                cAdminView.ShowDialog();
            }
            else
            {
                cBookingView cMemberView = new cBookingView();
                cMemberView.ShowDialog();
            }
        }

        private void cPrintOutView_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
