using Concoct_Builder.Models;
using System.Collections.Generic;

namespace Concoct_Builder.Datalayer
{
    public interface IFileHandler
    {
        public List<PageElement> ReadFile(string path);
        public void WriteFile(string path, List<PageElement> content, string layoutDetails, int projectId, int workItemId);
        public void DeleteFile(string path);
         public string ReadFileRaw(string filePath);
        public string ConvertTobase64(string path);
        void CreateFile(string path, string data);
        public List<Widget> GetAllWidgets();
        List<UserSettings> GetUserSettings();
        public void RemoveLayoutFile(string layout, string file);
    }
}
