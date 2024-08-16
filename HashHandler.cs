using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrityVerificationTool
{
    public class HashHandler
    {
        private string hashFilePath = "hash_records.txt";
        public Dictionary<string, string> FileHashes { get; private set; }

        public HashHandler()
        {
            FileHashes = new Dictionary<string, string>();
        }
        public void LoadHashRecords()
        {
            if (File.Exists(hashFilePath))
            {
                using (StreamReader reader = new StreamReader(hashFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(new[] { '|' }, 2);
                        if (parts.Length == 2)
                        {
                            FileHashes[parts[0]] = parts[1];  // 載入檔案的 Hash
                        }
                    }
                }
            }
        }
        // 保存 Hash 紀錄
        public void SaveHashRecords()
        {
            using (StreamWriter writer = new StreamWriter(hashFilePath))
            {
                foreach (var entry in FileHashes)
                {
                    writer.WriteLine($"{entry.Key}|{entry.Value}");  // 儲存檔案路徑與對應的 Hash
                }
            }
        }
        // 更新 Hash 值
        public void UpdateHash(string filePath, string hashValue)
        {
            FileHashes[filePath] = hashValue;
        }

        // 刪除檔案 Hash
        public void RemoveHash(string filePath)
        {
            if (FileHashes.ContainsKey(filePath))
            {
                FileHashes.Remove(filePath);
            }
        }
        // 檢查是否存在 Hash 紀錄
        public bool HasHash(string filePath)
        {
            return FileHashes.ContainsKey(filePath);
        }

        // 獲取檔案的 Hash 值
        public string GetHash(string filePath)
        {
            return FileHashes.ContainsKey(filePath) ? FileHashes[filePath] : null;
        }
    }
}
