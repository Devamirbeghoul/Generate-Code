namespace Generate_Code.Generate
{
    partial class fmSetInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmSetInfo));
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.btnBrowse = new Guna.UI2.WinForms.Guna2Button();
            this.tbPath = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCheckAll = new System.Windows.Forms.CheckBox();
            this.clTables = new System.Windows.Forms.CheckedListBox();
            this.cbDB = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Animated = true;
            this.btnNext.BorderColor = System.Drawing.Color.Silver;
            this.btnNext.BorderRadius = 18;
            this.btnNext.BorderThickness = 1;
            this.btnNext.CheckedState.Parent = this.btnNext;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.CustomImages.Parent = this.btnNext;
            this.btnNext.FillColor = System.Drawing.Color.Transparent;
            this.btnNext.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.HoverState.Parent = this.btnNext;
            this.btnNext.Location = new System.Drawing.Point(174, 379);
            this.btnNext.Name = "btnNext";
            this.btnNext.ShadowDecoration.Parent = this.btnNext;
            this.btnNext.Size = new System.Drawing.Size(174, 34);
            this.btnNext.TabIndex = 16;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Animated = true;
            this.btnBrowse.BorderColor = System.Drawing.Color.Silver;
            this.btnBrowse.BorderRadius = 18;
            this.btnBrowse.BorderThickness = 1;
            this.btnBrowse.CheckedState.Parent = this.btnBrowse;
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.CustomImages.Parent = this.btnBrowse;
            this.btnBrowse.FillColor = System.Drawing.Color.Transparent;
            this.btnBrowse.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.Black;
            this.btnBrowse.HoverState.Parent = this.btnBrowse;
            this.btnBrowse.Location = new System.Drawing.Point(17, 164);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.ShadowDecoration.Parent = this.btnBrowse;
            this.btnBrowse.Size = new System.Drawing.Size(105, 51);
            this.btnBrowse.TabIndex = 15;
            this.btnBrowse.TabStop = false;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPath
            // 
            this.tbPath.Animated = true;
            this.tbPath.BorderRadius = 18;
            this.tbPath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPath.DefaultText = "";
            this.tbPath.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbPath.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbPath.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbPath.DisabledState.Parent = this.tbPath;
            this.tbPath.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbPath.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbPath.FocusedState.Parent = this.tbPath;
            this.tbPath.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPath.ForeColor = System.Drawing.Color.Black;
            this.tbPath.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbPath.HoverState.Parent = this.tbPath;
            this.tbPath.Location = new System.Drawing.Point(138, 324);
            this.tbPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPath.Name = "tbPath";
            this.tbPath.PasswordChar = '\0';
            this.tbPath.PlaceholderText = "";
            this.tbPath.SelectedText = "";
            this.tbPath.ShadowDecoration.Parent = this.tbPath;
            this.tbPath.Size = new System.Drawing.Size(261, 34);
            this.tbPath.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(58, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 26);
            this.label2.TabIndex = 13;
            this.label2.Text = "Path :";
            // 
            // cbCheckAll
            // 
            this.cbCheckAll.AutoSize = true;
            this.cbCheckAll.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckAll.Location = new System.Drawing.Point(138, 62);
            this.cbCheckAll.Name = "cbCheckAll";
            this.cbCheckAll.Size = new System.Drawing.Size(94, 25);
            this.cbCheckAll.TabIndex = 12;
            this.cbCheckAll.Text = "check all";
            this.cbCheckAll.UseVisualStyleBackColor = true;
            this.cbCheckAll.CheckedChanged += new System.EventHandler(this.cbCheckAll_CheckedChanged);
            // 
            // clTables
            // 
            this.clTables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clTables.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clTables.FormattingEnabled = true;
            this.clTables.Location = new System.Drawing.Point(138, 93);
            this.clTables.Name = "clTables";
            this.clTables.Size = new System.Drawing.Size(261, 218);
            this.clTables.TabIndex = 11;
            // 
            // cbDB
            // 
            this.cbDB.BackColor = System.Drawing.Color.Transparent;
            this.cbDB.BorderRadius = 18;
            this.cbDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDB.FocusedColor = System.Drawing.Color.Empty;
            this.cbDB.FocusedState.Parent = this.cbDB;
            this.cbDB.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbDB.FormattingEnabled = true;
            this.cbDB.HoverState.Parent = this.cbDB;
            this.cbDB.ItemHeight = 30;
            this.cbDB.ItemsAppearance.Parent = this.cbDB;
            this.cbDB.Location = new System.Drawing.Point(138, 20);
            this.cbDB.Name = "cbDB";
            this.cbDB.ShadowDecoration.Parent = this.cbDB;
            this.cbDB.Size = new System.Drawing.Size(261, 36);
            this.cbDB.TabIndex = 10;
            this.cbDB.SelectedIndexChanged += new System.EventHandler(this.cbDB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 26);
            this.label1.TabIndex = 9;
            this.label1.Text = "Database :";
            // 
            // OpenFolder
            // 
            this.OpenFolder.Description = "Select a folder";
            this.OpenFolder.SelectedPath = "C:\\";
            // 
            // fmSetInfo
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(247)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(441, 431);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCheckAll);
            this.Controls.Add(this.clTables);
            this.Controls.Add(this.cbDB);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fmSetInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Info";
            this.Load += new System.EventHandler(this.fmSetInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnBrowse;
        private Guna.UI2.WinForms.Guna2TextBox tbPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbCheckAll;
        private System.Windows.Forms.CheckedListBox clTables;
        private Guna.UI2.WinForms.Guna2ComboBox cbDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog OpenFolder;
    }
}