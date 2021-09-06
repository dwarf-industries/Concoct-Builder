using Concoct_Builder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                if (pageElements.Count != i)
                    result += "@";
                i++;
            });
            return result;
        }
    }
}
