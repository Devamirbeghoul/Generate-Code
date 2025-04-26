using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Generate_Code.Global_Classes;

namespace Generate_Code.Generate
{
    public partial class fmGenerate : Form
    {
        private string _Path, _DB;
        private List<string> _Tables;
        public fmGenerate(string Path, string DB, List<string> Tables)
        {
            InitializeComponent();
            _Path = Path;
            _Tables = Tables;
            _DB = DB;
        }

        private void fmGenerate_Load(object sender, EventArgs e)
        {
            dgvTables.Rows.Clear();
            dgvTables.Columns.Clear();

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn()
            {
                Name = "FileName",
                HeaderText = "File Name",
                ReadOnly = true
            };

            dgvTables.Columns.Add(column);

            foreach (string table in _Tables)
            {
                dgvTables.Rows.Add($"cls{table}.cs");
                dgvTables.Rows.Add($"cls{table}Data.cs");
            }

            dgvTables.Rows.Add("clsSettings.cs");

            lbCount.Text = dgvTables.Rows.Count.ToString() + " File(s)";

        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            string DataLayerPath = Path.Combine(_Path, _DB + "_DataAccessLayer");
            string BusinessLayerPath = Path.Combine(_Path, _DB + "_BusinessLayer");

            Directory.CreateDirectory(DataLayerPath);
            Directory.CreateDirectory(BusinessLayerPath);

            List<Task> Tasks = new List<Task>();
            List<string> Warnings = new List<string>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (string Table in _Tables)
            {
                Tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var Columns = await clsConnection.GetColumnsInfoOfTable(Table);

                        if (Columns == null || Columns.Count == 0)
                        {
                            lock (Warnings)
                            {
                                Warnings.Add($"⚠ Warning: No columns found for table {Table}");
                            }
                            return;
                        }

                        string BusinessLayer = clsGenerate.BusinessLayer(Table , Columns);
                        string DataLayer = clsGenerate.DataLayer(Table , Columns);

                        string BusinessPath = Path.Combine(BusinessLayerPath, "cls" + Table + ".cs");
                        string DataPath = Path.Combine(DataLayerPath, "cls" + Table + "Data.cs");

                        clsSystem.SaveToFile(BusinessPath , BusinessLayer);
                        clsSystem.SaveToFile(DataPath , DataLayer);
                    }
                    catch (Exception ex)
                    {
                        clsSystem.ErrorLog(ex);
                        lock (Warnings)
                        {
                            Warnings.Add($"❌ Error processing table {Table}: {ex.Message}");
                        }
                    }
                }));
            }

            await Task.WhenAll(Tasks);

            string SettingsPath = Path.Combine(DataLayerPath, "clsSettings.cs");
            clsSystem.SaveToFile(SettingsPath , clsGenerate.Settings());

            stopwatch.Stop();

            if (Warnings.Count > 0)
            {
                MessageBox.Show(string.Join("\n", Warnings), "Warnings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            MessageBox.Show($"✅ All files have been generated successfully in {stopwatch.ElapsedMilliseconds} ms.",
                             "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
