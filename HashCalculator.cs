using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IntegrityVerificationTool
{
    public class HashCalculator
    {
        private HashHandler _hashHandler;
        private FolderManager _folderManager;
        public HashCalculator(HashHandler handler, FolderManager folderManager)
        {
            _hashHandler = handler;
            _folderManager = folderManager;
        }

        // 計算資料夾的 Hash 值
        public string CalculateFolderHash(string folderPath, out List<string> changedFiles, bool isFirstRun)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                StringBuilder combinedHash = new StringBuilder();
                changedFiles = new List<string>();

                // 獲取資料夾中所有檔案，但過濾掉排除的資料夾內的檔案
                var currentFiles = new HashSet<string>(Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .Where(file => !_folderManager.ExcludedFolders.Any(excludedFolder => file.StartsWith(excludedFolder))));

                // 檢查現有檔案的 Hash 值
                foreach (string file in currentFiles)
                {
                    byte[] fileBytes = File.ReadAllBytes(file);
                    byte[] fileHash = sha256.ComputeHash(fileBytes);
                    string fileHashString = BitConverter.ToString(fileHash).Replace("-", "");

                    // 檔案新增或修改
                    if (!_hashHandler.HasHash(file))
                    {
                        changedFiles.Add(file + " (新增/包含)");
                        _hashHandler.UpdateHash(file, fileHashString);
                    }
                    else if (_hashHandler.GetHash(file) != fileHashString)
                    {
                        changedFiles.Add(file + " (修改)");
                        _hashHandler.UpdateHash(file, fileHashString);
                    }

                    combinedHash.Append(fileHashString);
                }

                // 檢查已刪除的檔案
                var deletedFiles = _hashHandler.FileHashes.Keys.Except(currentFiles).ToList();
                foreach (var deletedFile in deletedFiles)
                {
                    changedFiles.Add(deletedFile + " (已刪除/已排除)");
                    _hashHandler.RemoveHash(deletedFile);
                }

                byte[] folderHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedHash.ToString()));

                // 即使是初始化階段，仍然返回實際的 Hash 值
                string finalHash = BitConverter.ToString(folderHash).Replace("-", "");

                // 如果是初始化階段，將 "初始化" 標註添加到變更檔案列表
                if (isFirstRun)
                {
                    changedFiles.Add("初始化");
                }

                return finalHash;
            }
        }



    }
}
