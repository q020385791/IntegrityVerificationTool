using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrityVerificationTool
{
    public class FolderManager
    {
        public List<string> DetectedFolders { get; private set; } = new List<string>();
        public List<string> ExcludedFolders { get; private set; } = new List<string>();

        // 載入資料夾清單
        public void LoadFoldersFromFile()
        {
            if (!File.Exists("folders.txt"))
                return;

            using (StreamReader reader = new StreamReader("folders.txt"))
            {
                string line;
                bool isDetectedFolders = false;
                bool isExcludedFolders = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "[DetectedFolders]")
                    {
                        isDetectedFolders = true;
                        isExcludedFolders = false;
                    }
                    else if (line == "[ExcludedFolders]")
                    {
                        isDetectedFolders = false;
                        isExcludedFolders = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (isDetectedFolders)
                        {
                            DetectedFolders.Add(line);
                        }
                        else if (isExcludedFolders)
                        {
                            ExcludedFolders.Add(line);
                        }
                    }
                }
            }
        }

        // 保存資料夾清單
        public void SaveFoldersToFile()
        {
            using (StreamWriter writer = new StreamWriter("folders.txt"))
            {
                writer.WriteLine("[DetectedFolders]");
                foreach (var folder in DetectedFolders)
                {
                    writer.WriteLine(folder);
                }

                writer.WriteLine("[ExcludedFolders]");
                foreach (var folder in ExcludedFolders)
                {
                    writer.WriteLine(folder);
                }
            }
        }

        // 選擇資料夾
        public void SelectFolder(ListBox listBoxFolders)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "請選擇要偵測的資料夾";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;

                    // 確保該資料夾未被排除，並且不重複
                    if (!ExcludedFolders.Contains(selectedFolder) && !DetectedFolders.Contains(selectedFolder))
                    {
                        DetectedFolders.Add(selectedFolder);
                        listBoxFolders.Items.Add(selectedFolder); // 顯示偵測資料夾
                    }
                    else if (ExcludedFolders.Contains(selectedFolder))
                    {
                        MessageBox.Show("該資料夾已經被排除，無法選擇", "排除資料夾", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("該資料夾已經添加過", "重複資料夾", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // 移除資料夾
        public void RemoveFolder(ListBox listBoxFolders)
        {
            int index = listBoxFolders.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                DetectedFolders.RemoveAt(index);
                listBoxFolders.Items.RemoveAt(index);
            }
        }

        // 選擇要排除的資料夾
        public void SelectExcludeFolder(ListBox listBoxExcludedFolders)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "請選擇要排除的資料夾";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;

                    // 確保該資料夾不在已偵測的資料夾內且未重複
                    if (!ExcludedFolders.Contains(selectedFolder) && !DetectedFolders.Contains(selectedFolder))
                    {
                        ExcludedFolders.Add(selectedFolder);
                        listBoxExcludedFolders.Items.Add(selectedFolder); // 顯示排除資料夾
                    }
                    else if (DetectedFolders.Contains(selectedFolder))
                    {
                        MessageBox.Show("該資料夾已經在偵測列表中，無法排除", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("該資料夾已經排除過", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        // 移除排除資料夾
        public void RemoveExcludedFolder(ListBox listBoxExcludedFolders)
        {
            int index = listBoxExcludedFolders.SelectedIndex;
            if (index >= 0)
            {
                string folderToRemove = listBoxExcludedFolders.Items[index].ToString();
                ExcludedFolders.Remove(folderToRemove);
                listBoxExcludedFolders.Items.RemoveAt(index);
            }
        }
    }
}
