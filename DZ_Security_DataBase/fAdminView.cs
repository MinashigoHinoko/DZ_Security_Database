using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_Security_DataBase
{
    public partial class cAdminView : Form
    {
        private bool firstLoad = true;
        public cAdminView()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            string folderPath = cDataBase.DbPath;
            string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM Arbeitszeiten", conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        try
                        {
                            adapter.Fill(dt);

                        }
                        catch (Exception)
                        {
                            DialogResult result = MessageBox.Show("Die Datei ist nicht Speicherbar mit fehlenden Stop Zeitstempeln", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FileName = "export.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Arbeitszeiten");

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

        private void cAdminMenu_Load(object sender, EventArgs e)
        {
            if (firstLoad)
            {
                cDataBase.createDatabase();
                cDataBase.editDatabase();
                firstLoad = false;
            }
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            cCheckIn checkIn = new cCheckIn();
            checkIn.ShowDialog();
        }
    }
}
