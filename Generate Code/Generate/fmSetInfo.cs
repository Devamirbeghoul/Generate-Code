using Generate_Code.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generate_Code.Generate
{
    public partial class fmSetInfo : Form
    {
        public fmSetInfo()
        {
            InitializeComponent();
        }

        private void _ChangeAllTablesCheck(bool IsCheck)
        {
            for (int i = 0; i < clTables.Items.Count; i++)
                clTables.SetItemChecked(i, IsCheck);
        }

        private async void _FillTables(string DB)
        {
            clTables.Items.Clear();

            List<string> tables = await clsConnection.GetAllTables(DB);

            if (tables == null || tables.Count == 0)
                MessageBox.Show($"Cannot Load tables list from {DB}", "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                foreach (string table in tables)
                    clTables.Items.Add(table);
            }

                
        }

        private async void _FillDB()
        {
            List<string> list = await clsConnection.GetAllDB();

            if (list == null || list.Count == 0)
                MessageBox.Show("Cannot Load DB list", "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                foreach (string item in list)
                    cbDB.Items.Add(item);
            }

            if (cbDB.Items.Count > 0)
                cbDB.SelectedIndex = 0;


        }

        private void fmSetInfo_Load(object sender, EventArgs e)
        {
            _FillDB(); 
        }

        private void cbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            _ChangeAllTablesCheck(cbCheckAll.Checked);
        }

        private void cbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDB.Items == null)
                return;

            _FillTables(cbDB.Text.Trim());
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (OpenFolder.ShowDialog() == DialogResult.OK)
                tbPath.Text = OpenFolder.SelectedPath;
        }

        private List<string> _GetTablesSelected()
        {
            List<string> list = new List<string>();

            foreach (string item in clTables.CheckedItems)
                list.Add(item);

            return list;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string SelectedPath = tbPath.Text.Trim();

            if (string.IsNullOrEmpty(SelectedPath))
            {
                MessageBox.Show("Enter a valid path!", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(SelectedPath))
            {
                MessageBox.Show("The selected path does not exist. choose an existing directory.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SelectedPath.Any(c => Path.GetInvalidPathChars().Contains(c)))
            {
                MessageBox.Show("The path contains invalid characters.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Path.IsPathRooted(SelectedPath))
            {
                MessageBox.Show("Please choose a valid root path.", "Invalid Path Root", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fmGenerate Generate = new fmGenerate(SelectedPath, cbDB.Text.Trim(), _GetTablesSelected());

            Generate.ShowDialog();
        }
    }
}
