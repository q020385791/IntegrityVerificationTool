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

            // �K�[�w���J����Ƨ��� ListBox
            foreach (var folder in folderManager.DetectedFolders)
            {
                listBoxFolders.Items.Add(folder);
            }

            foreach (var folder in folderManager.ExcludedFolders)
            {
                listBoxExcludedFolders.Items.Add(folder);
            }

            hashCheckTimer = new System.Timers.Timer(10000); // �C 5 ��������@���˴�
            hashCheckTimer.Elapsed += OnTimedEvent;
            hashCheckTimer.AutoReset = true;
            hashCheckTimer.Start();
        }
        private void InitializeDataGridView()
        {
            dataGridViewChanges.Columns.Add("Time", "�ɶ�");
            dataGridViewChanges.Columns.Add("HashValue", "Hash ��");
            dataGridViewChanges.Columns.Add("FilePath", "�۲����ɮ׸��|");

            dataGridViewChanges.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewChanges.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewChanges.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            folderManager.SelectFolder(listBoxFolders);
            // ��ϥΪ̿�ܸ�Ƨ���ߧY�i�� Hash �˴�
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

            // ��s�ư���Ƨ���A���W��s�˴��޿�A�T�O�o�Ǹ�Ƨ����A�ѻP�˴�
            foreach (var folder in folderManager.DetectedFolders)
            {
                if (!folderManager.ExcludedFolders.Contains(folder))
                {
                    string currentHash = hashCalculator.CalculateFolderHash(folder, out List<string> changedFiles, isFirstRunAfterStartup);
                    AddResultToDataGridView(folder, currentHash, changedFiles);
                }
            }
        }

        // ���� ListBox ���ز����ư���Ƨ�
        private void listBoxExcludedFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            folderManager.RemoveExcludedFolder(listBoxExcludedFolders);

            // �����ư��M���A���s�i���˴�
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

        // �O�s��Ƨ��M����ɮ�
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
                // �Ĥ@������ɷ|�۰ʥ� CalculateFolderHash �K�[ "��l��"
                string currentHash = hashCalculator.CalculateFolderHash(folder, out List<string> changedFiles, isFirstRunAfterStartup);

                AddResultToDataGridView(folder, currentHash, changedFiles);

                foreach (var change in changedFiles)
                {
                    changeLogger.LogChange(folder, "�ܧ�", change);
                }
            }

            // �Ĥ@�����槹����A�]�m�аO�� false
            if (isFirstRunAfterStartup)
            {
                isFirstRunAfterStartup = false;
            }

            hashHandler.SaveHashRecords();
            folderManager.SaveFoldersToFile();
        }


        // �N���G�[�J�� DataGridView
        private void AddResultToDataGridView(string folder, string hashValue, List<string> changedFiles)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // �p�G�S���ܧ��ɮסA�O���� "�L"
            if (changedFiles.Count == 0)
            {
                changedFiles.Add("�L");
            }

            // �X�֩Ҧ��ܧ��ɮ׸��|�A�� Environment.NewLine ���j
            string combinedChangedFiles = string.Join(Environment.NewLine, changedFiles);

            dataGridViewChanges.Invoke((MethodInvoker)delegate
            {
                dataGridViewChanges.Rows.Add(timestamp, hashValue, combinedChangedFiles);

                // �N DataGridView �u�ʨ�̫�@��
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
