namespace IntegrityVerificationTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSelectFolder = new Button();
            listBoxFolders = new ListBox();
            btnSelectExcludeFolder = new Button();
            listBoxExcludedFolders = new ListBox();
            dataGridViewChanges = new DataGridView();
            btnStopDetection = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChanges).BeginInit();
            SuspendLayout();
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(12, 12);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(137, 23);
            btnSelectFolder.TabIndex = 0;
            btnSelectFolder.Text = "選擇要偵測的資料夾";
            btnSelectFolder.UseVisualStyleBackColor = true;
            btnSelectFolder.Click += btnSelectFolder_Click;
            // 
            // listBoxFolders
            // 
            listBoxFolders.FormattingEnabled = true;
            listBoxFolders.ItemHeight = 15;
            listBoxFolders.Location = new Point(12, 41);
            listBoxFolders.Name = "listBoxFolders";
            listBoxFolders.Size = new Size(585, 79);
            listBoxFolders.TabIndex = 1;
            listBoxFolders.MouseDoubleClick += listBoxFolders_MouseDoubleClick;
            // 
            // btnSelectExcludeFolder
            // 
            btnSelectExcludeFolder.Location = new Point(11, 139);
            btnSelectExcludeFolder.Name = "btnSelectExcludeFolder";
            btnSelectExcludeFolder.Size = new Size(138, 23);
            btnSelectExcludeFolder.TabIndex = 2;
            btnSelectExcludeFolder.Text = "選擇排除偵測的資料夾";
            btnSelectExcludeFolder.UseVisualStyleBackColor = true;
            btnSelectExcludeFolder.Click += btnSelectExcludeFolder_Click;
            // 
            // listBoxExcludedFolders
            // 
            listBoxExcludedFolders.FormattingEnabled = true;
            listBoxExcludedFolders.ItemHeight = 15;
            listBoxExcludedFolders.Location = new Point(12, 178);
            listBoxExcludedFolders.Name = "listBoxExcludedFolders";
            listBoxExcludedFolders.Size = new Size(585, 79);
            listBoxExcludedFolders.TabIndex = 3;
            listBoxExcludedFolders.MouseDoubleClick += listBoxExcludedFolders_MouseDoubleClick;
            // 
            // dataGridViewChanges
            // 
            dataGridViewChanges.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewChanges.Location = new Point(12, 304);
            dataGridViewChanges.Name = "dataGridViewChanges";
            dataGridViewChanges.Size = new Size(585, 150);
            dataGridViewChanges.TabIndex = 4;
            // 
            // btnStopDetection
            // 
            btnStopDetection.Location = new Point(598, 310);
            btnStopDetection.Name = "btnStopDetection";
            btnStopDetection.Size = new Size(75, 23);
            btnStopDetection.TabIndex = 5;
            btnStopDetection.Text = "停止偵測";
            btnStopDetection.UseVisualStyleBackColor = true;
            btnStopDetection.Click += btnStopDetection_Click;
            // 
            // button2
            // 
            button2.Location = new Point(598, 354);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "開始偵測";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 619);
            Controls.Add(button2);
            Controls.Add(btnStopDetection);
            Controls.Add(dataGridViewChanges);
            Controls.Add(listBoxExcludedFolders);
            Controls.Add(btnSelectExcludeFolder);
            Controls.Add(listBoxFolders);
            Controls.Add(btnSelectFolder);
            Name = "Form1";
            Text = "完整性驗證工具";
            FormClosing += Form1_FormClosing;
            ((System.ComponentModel.ISupportInitialize)dataGridViewChanges).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnSelectFolder;
        private ListBox listBoxFolders;
        private Button btnSelectExcludeFolder;
        private ListBox listBoxExcludedFolders;
        private DataGridView dataGridViewChanges;
        private Button btnStopDetection;
        private Button button2;
    }
}
