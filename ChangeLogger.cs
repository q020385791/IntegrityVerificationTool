using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrityVerificationTool
{
    public class ChangeLogger
    {
        public void LogChange(string folder, string changeType, string filePath)
        {
            // 如果變更類型是初始化，不顯示 MessageBox
            if (changeType == "初始化")
                return;

            string[] validChangeTypes = ["新增", "修改", "已刪除"];
            // 判斷如果有變更才做動作
            if (validChangeTypes.Contains(changeType))
            {
                // TODO: 執行想要的變更動作
                Console.WriteLine($"變更發生: {changeType} - {filePath} in {folder}");
            }

            // 記錄變更到日誌
            File.AppendAllText("change_log.txt", $"{DateTime.Now}: [{changeType}] {filePath} in {folder}\n");
        }
    }
}
