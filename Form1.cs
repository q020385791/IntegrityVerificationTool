using System.Security.Cryptography;
using System.Text;

namespace IntegrityVerificationTool
{
    public partial class Form1 : Form
    {
        private HashHandler hashHandler;
        private FolderManager folderManager;
        private HashCalculator hashCalculator;
        private ChangeLogger changeLogger;
        private System.Timers.Timer hashCheckTimer;
        private bool isFirstRunAfterStartup = true;
        public Form1()
        {
            InitializeComponent();

            hashHandler = new HashHandler();
            folderManager = new FolderManager();
            hashCalculator = new HashCalculator(hashHandler, folderManager);
            changeLogger = new ChangeLogger();

            folderManager.LoadFoldersFromFile();
            hashHandler.LoadHashRecords();
            InitializeDataGridView();

            // 添加已載入的資料夾到 ListBox
            foreach (var folder in folderManager.DetectedFolders)
            {
                listBoxFolders.Items.Add(folder);
            }

            foreach (var folder in folderManager.ExcludedFolders)
            {
                listBoxExcludedFolders.Items.Add(folder);
            }

            hashCheckTimer = new System.Timers.Timer(10000); // 每 5 分鐘執行一次檢測
            hashCheckTimer.Elapsed += OnTimedEvent;
            hashCheckTimer.AutoReset = true;
            hashCheckTimer.Start();
        }
        private void InitializeDataGridView()
        {
            dataGridViewChanges.Columns.Add("Time", "時間");
            dataGridViewChanges.Columns.Add("HashValue", "Hash 值");
            dataGridViewChanges.Columns.Add("FilePath", "相異的檔案路徑");

            dataGridViewChanges.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewChanges.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewChanges.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            folderManager.SelectFolder(listBoxFolders);
            // 當使用者選擇資料夾後立即進行 Hash 檢測
            foreach (var folder in folderManager.DetectedFolders)
            {
                string currentHash = hashCalculator.CalculateFolderHash(folder, out List<string> changedFiles, isFirstRunAfterStartup);
                AddResultToDataGridView(folder, currentHash, changedFiles);
            }
        }

        private void listBoxFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            folderManager.RemoveFolder(listBoxFolders);
        }

        private void btnSelectExcludeFolder_Click(object sender, EventArgs e)
        {
            folderManager.SelectExcludeFolder(listBoxExcludedFolders);

            // 更新排除資料夾後，馬上更新檢測邏輯，確保這些資料夾不再參與檢測
            foreach (var folder in folderManager.DetectedFolders)
            {
                if (!folderManager.ExcludedFolders.Contains(folder))
                {
                    string currentHash = hashCalculator.CalculateFolderHash(folder, out List<string> changedFiles, isFirstRunAfterStartup);
                    AddResultToDataGridView(folder, currentHash, changedFiles);
                }
            }
        }

        // 雙擊 ListBox 項目移除排除資料夾
        private void listBoxExcludedFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            folderManager.RemoveExcludedFolder(listBoxExcludedFolders);

            // 移除排除清單後，重新進行檢測
            foreach (var folder in folderManager.DetectedFolders)
            {
                if (!folderManager.ExcludedFolders.Contains(folder))
                {
                    string currentHash = hashCalculator.CalculateFolderHash(folder, out List<string> changedFiles, isFirstRunAfterStartup);
                    AddResultToDataGridView(folder, currentHash, changedFiles);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFoldersToFile();
        }

        // 保存資料夾清單至檔案
        private void SaveFoldersToFile()
        {
            using (StreamWriter writer = new StreamWriter("folders.txt"))
            {
                writer.WriteLine("[DetectedFolders]");
                foreach (var folder in folderManager.DetectedFolders)
                {
                    writer.WriteLine(folder);
                }

                writer.WriteLine("[ExcludedFolders]");
                foreach (var folder in folderManager.ExcludedFolders)
                {
                    writer.WriteLine(folder);
                }
            }
        }


        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var folder in folderManager.DetectedFolders)
            {
                // 第一次執行時會自動由 CalculateFolderHash 添加 "初始化"
                string currentHash = hashCalculator.CalculateFolderHash(folder, out List<string> changedFiles, isFirstRunAfterStartup);

                AddResultToDataGridView(folder, currentHash, changedFiles);

                foreach (var change in changedFiles)
                {
                    changeLogger.LogChange(folder, "變更", change);
                }
            }

            // 第一次執行完成後，設置標記為 false
            if (isFirstRunAfterStartup)
            {
                isFirstRunAfterStartup = false;
            }

            hashHandler.SaveHashRecords();
            folderManager.SaveFoldersToFile();
        }


        // 將結果加入到 DataGridView
        private void AddResultToDataGridView(string folder, string hashValue, List<string> changedFiles)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 如果沒有變更的檔案，記錄為 "無"
            if (changedFiles.Count == 0)
            {
                changedFiles.Add("無");
            }

            // 合併所有變更的檔案路徑，用 Environment.NewLine 分隔
            string combinedChangedFiles = string.Join(Environment.NewLine, changedFiles);

            dataGridViewChanges.Invoke((MethodInvoker)delegate
            {
                dataGridViewChanges.Rows.Add(timestamp, hashValue, combinedChangedFiles);

                // 將 DataGridView 滾動到最後一行
                int rowIndex = dataGridViewChanges.Rows.Count - 1;
                if (rowIndex >= 0)
                {
                    dataGridViewChanges.FirstDisplayedScrollingRowIndex = rowIndex;
                }
            });
        }

        private void btnStopDetection_Click(object sender, EventArgs e)
        {
            hashCheckTimer.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hashCheckTimer.Start();
        }
    }
}
