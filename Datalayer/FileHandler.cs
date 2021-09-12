using Concoct_Builder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Concoct_Builder.Datalayer
{
    public class FileHandler : IFileHandler
    {
        public void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public List<PageElement> ReadFile(string path)
        {

            var contents = System.IO.File.ReadAllText(path);

            if (!string.IsNullOrEmpty(contents))
                return DeserializeData(contents);

            return null;
        }

        public string ReadFileRaw(string filePath)
        {
            if(!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File doesn't exist, creating empty file.");
                System.IO.File.Create(filePath).Dispose();

                return "";
            }

            return System.IO.File.ReadAllText(filePath);
        }

        public void WriteFile(string path, List<PageElement> content)
        {
            var serialized = SerializeData(content);
            System.IO.File.WriteAllText(path, serialized);
        }

        public List<PageElement> DeserializeData(string data)
        {
            var i = 0;
            var result = new List<PageElement>();
            var pageElement = default(PageElement);
            data.Split('@').ToList().ForEach( dataRow =>
            {
                pageElement = new PageElement();
                dataRow.Split(Environment.NewLine).ToList().ForEach(x =>
                {
                    if(x != "")
                    {
                        switch (i)
                        {
                            case 0:
                                pageElement.ElementName = x;
                                break;
                            case 1:
                                pageElement.Width = x;
                                break;
                            case 2:
                                pageElement.Height = x;
                                break;
                            case 3:
                                pageElement.ClientX = x;
                                break;
                            case 4:
                                pageElement.ClientY = x;
                                break;
                            case 5:
                                pageElement.OffsetX = x;
                                break;
                            case 6:
                                pageElement.OffsetY = x;
                                break;
                            case 7:
                                pageElement.Translate = x;
                                break;
                            case 8:
                                pageElement.Base64 = x;
                                break;

                        }
                    }
                    i++;
                });
                result.Add(pageElement);
                i = 0;


            });
            return result;
        }

        public string SerializeData(List<PageElement> pageElements)
        {
            var result = string.Empty;

            var i = 1;
            pageElements.ForEach(x =>
            {
                result += x.ElementName + Environment.NewLine;
                result += x.Width + Environment.NewLine;
                result += x.Height + Environment.NewLine;
                result += x.ClientX + Environment.NewLine;
                result += x.ClientY + Environment.NewLine;
                result += x.OffsetX + Environment.NewLine;
                result += x.OffsetY + Environment.NewLine;
                result += x.Translate + Environment.NewLine;
                result += x.Base64 + Environment.NewLine;
                if (pageElements.Count != i)
                    result += "@";
                i++;
            });
            return result;
        }

        public string SaveDirectoryFile(string files, IncomingFileRequest file)
        {
            var result = files;
            result += $"{file.Path}{Startup.Settings.SystemFolderDelimiter}{file.Name}" + Environment.NewLine;
            result += file.Name + Environment.NewLine;
            result += "@";


            return result;
        }

        public Settings ReadConfig(string filePath)
        {
            try
            {
                var result = new Settings();
                var i = 0;
                filePath.Split('@').ToList().ForEach(dataRow =>
                {
                    dataRow.Split(Environment.NewLine).ToList().ForEach(x =>
                    {
                        if (x != "")
                        {
                            switch (i)
                            {
                                case 0:
                                    result.SingInType = x;
                                    break;
                                case 1:
                                    result.ConcoctInstance = x;
                                    break;
                                case 2:
                                    result.UserName = x;
                                    break;
                                case 3:
                                    result.Password = x;
                                    break;
                                case 4:
                                    result.APIKey = x;
                                    break;
                                case 5:
                                    result.AssocaitedFileLocation = x;
                                    break;
                            }
                        }
                        i++;
                    });
                });
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read configuration, please reinstall or fix the configuration file. Settings.cf");
                Console.WriteLine(ex.ToString());
                return null;
            }
           
        }

        public List<IncomingFileRequest> ReadDirectoryFile(string assocaitedFileLocation)
        {
            var i = 0;
            var result = new List<IncomingFileRequest>();
            var directory = default(IncomingFileRequest);

            if (string.IsNullOrEmpty(assocaitedFileLocation))
                return new List<IncomingFileRequest>();

            assocaitedFileLocation.Split('@').ToList().ForEach(dataRow =>
            {
                directory = new IncomingFileRequest();
                dataRow.Split(Environment.NewLine).ToList().ForEach(x =>
                {
                    if (x != "")
                    {
                        switch (i)
                        {
                            case 0:
                                directory.Path = x;
                                break;
                            case 1:
                                directory.Name = x;
                                break;                        
                        }
                    }
                    i++;
                });
                result.Add(directory);
                i = 0;
            });
            return result;

        }

        public string ConvertTobase64(string path)
        {
            var bytes = System.IO.File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        public void CreateFile(string path, string data)
        {
            System.IO.File.WriteAllText(path, data);
        }
    }
}
