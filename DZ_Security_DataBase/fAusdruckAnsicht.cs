
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SQLite;


namespace Festival_Manager
{
    public partial class cPrintOutView : Form
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        private bool isAdmin = false;
        private string username;
        public cPrintOutView(bool isAdmin, string username)
        {
            this.isAdmin = isAdmin;
            InitializeComponent();
            this.username = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT MitarbeiterID, Vorname, Nachname,Geburtsname,Gender,Geburtsdatum,Geburtsort,Wohnort FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new();
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
                            using (FileStream stream = new(sfd.FileName, FileMode.Create, FileAccess.Write))
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
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT * FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new();
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
                            using (FileStream stream = new(sfd.FileName, FileMode.Create, FileAccess.Write))
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
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT Mitarbeiter.Vorname, Mitarbeiter.Nachname, Mitarbeiter.Firma, Mitarbeiter.Position, Position.Quadrant, Mitarbeiter.CheckInState FROM Mitarbeiter LEFT JOIN Position ON Mitarbeiter.Position = Position.Nr", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new();
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
                                    object cellValue = dt.Rows[i][j];
                                    if (cellValue != null)
                                    {
                                        row.CreateCell(j).SetCellValue(cellValue.ToString());
                                    }
                                    else
                                    {
                                        row.CreateCell(j).SetCellValue("NULL");
                                    }
                                }
                            }

                            // Speichern
                            using (FileStream stream = new(sfd.FileName, FileMode.Create, FileAccess.Write))
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

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT Vorname, Nachname,Firma,Position,CheckInState FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        adapter.Fill(dt);

                        SaveFileDialog sfd = new();
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
                            using (FileStream stream = new(sfd.FileName, FileMode.Create, FileAccess.Write))
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
            Hide();
            if (isAdmin)
            {
                cAdminView cAdminView = new(username);
                cAdminView.ShowDialog();
            }
            else
            {
                cBookingView cMemberView = new(username);
                cMemberView.ShowDialog();
            }
        }

        private void cPrintOutView_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
