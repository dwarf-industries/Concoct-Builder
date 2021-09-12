using Concoct_Builder.Models;
using System.Collections.Generic;

namespace Concoct_Builder.Datalayer
{
    public interface IFileHandler
    {
        public List<PageElement> ReadFile(string path);
        public void WriteFile(string path, List<PageElement> content);
        public void DeleteFile(string path);
        public string SerializeData(List<PageElement> pageElements);
        public List<PageElement> DeserializeData(string data);
        Settings ReadConfig(string filePath);
        public string SaveDirectoryFile(string files, IncomingFileRequest file);
        public List<IncomingFileRequest> ReadDirectoryFile(string assocaitedFileLocation);
        public string ReadFileRaw(string filePath);
        public string ConvertTobase64(string path);
        void CreateFile(string path, string data);
    }
}
