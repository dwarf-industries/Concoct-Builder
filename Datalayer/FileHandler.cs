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
                            case 9:
                                pageElement.Events = new List<Event>();
                                var events = x.Split("_");
                                events.ToList().ForEach(y =>
                                {
                                    if (!string.IsNullOrEmpty(y))
                                    {
                                        var source = y.Split(",");
                                        pageElement.Events.Add(new Event
                                        {
                                            Type = int.Parse(source[0]),
                                            Relation = source[1]
                                        });
                                    }
                                });
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
                if(x.Events != null)
                {
                    x.Events.ForEach(y =>
                    {
                        result += $"{y.Type},{y.Relation}_";
                    });
                    result += Environment.NewLine;
                }
               
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

        public List<Widget> GetAllWidgets()
        {
            return new List<Widget>{
                new Widget
                {
                    ComponentName = "AreaChart",
                    DisplayName = "Area Chart",
                    Icon = "fa-area-chart"
                },
                new Widget
                {
                    ComponentName = "BarChart",
                    DisplayName = "Bar Chart",
                    Icon = "fa-bar-chart"
                },
                new Widget
                {
                    ComponentName = "Barcode",
                    DisplayName = "Barcode",
                    Icon = "fa-barcode"
                },
                new Widget
                {
                    ComponentName = "Button",
                    DisplayName = "Button",
                    Icon = "fa-caret-square-o-right"
                },
                new Widget
                {
                    ComponentName = "Calendar",
                    DisplayName = "Calendar",
                    Icon = "fa-calendar"
                },
                new Widget
                {
                    ComponentName = "Chip",
                    DisplayName = "Chip",
                    Icon = "Example of id-badge fa-id-badge"
                },
                new Widget
                {
                    ComponentName = "DateRangePicker",
                    DisplayName = "Date Range Picker",
                    Icon = "fa-calendar-o"
                },
                new Widget
                {
                    ComponentName = "Diagram",
                    DisplayName = "Diagram",
                    Icon = "fa-paint-brush"
                },
                new Widget
                {
                    ComponentName = "Document",
                    DisplayName = "Document",
                    Icon = "fa-file"
                },
                new Widget
                {
                    ComponentName = "Grantt",
                    DisplayName = "Grantt Chart",
                    Icon = "fa-clipboard"
                },
                new Widget
                {
                    ComponentName = "Grid",
                    DisplayName = "Grid",
                    Icon = "fa-th"
                },
                new Widget
                {
                    ComponentName = "Guage",
                    DisplayName = "Guage",
                    Icon = "fa-circle-o-notch"
                },
                new Widget
                {
                    ComponentName = "HeatmapCalendar",
                    DisplayName = "Heatmap Calendar",
                    Icon = "fa-globe"
                },
                new Widget
                {
                    ComponentName = "Kanban",
                    DisplayName = "Kanban Board",
                    Icon = "fa-graduation-cap"
                },
                new Widget
                {
                    ComponentName = "LinearGuage",
                    DisplayName = "Linear Guage",
                    Icon = "fa-linode"
                },
                new Widget
                {
                    ComponentName = "LineChart",
                    DisplayName = "Line Chart",
                    Icon = "fa-line-chart"
                },
                new Widget
                {
                    ComponentName = "Map",
                    DisplayName = "Map",
                    Icon = "fa-map"
                },
                new Widget
                {
                    ComponentName = "PivotTable",
                    DisplayName = "Pivot Table",
                    Icon = "fa-table"
                },
                new Widget
                {
                    ComponentName = "ScatterChart",
                    DisplayName = "SCatter Chart",
                    Icon = "fa-circle"
                },
                new Widget
                {
                    ComponentName = "Scheduler",
                    DisplayName = "Scheduler",
                    Icon = "fa-calendar"
                },
                new Widget
                {
                    ComponentName = "Spreadsheet",
                    DisplayName = "Spreadsheet",
                    Icon = "fa-file-excel-o"
                },
                new Widget
                {
                    ComponentName = "StockChart",
                    DisplayName = "Stock Chart",
                    Icon = "fa-money"
                },
                new Widget
                {
                    ComponentName = "Temperature",
                    DisplayName = "Temperature Guage",
                    Icon = "fa-thermometer-empty"
                },
                new Widget
                {
                    ComponentName = "TreeGrid",
                    DisplayName = "Tree Grid",
                    Icon = "fa-th-large"
                },
                new Widget
                {
                   ComponentName = "TreeMap",
                   DisplayName ="Tree Map",
                   Icon = "fa-map-marker"
                },
                new Widget
                {
                   ComponentName = "Placeholder",
                   DisplayName = "Placeholder (Image/Custom)",
                   Icon = "fa-calendar-o"
                },
                new Widget
                {
                   ComponentName = "InPlaceEditor",
                   DisplayName = "In Place Editor",
                   Icon = "fa-edit"
                },
                new Widget
                {
                   ComponentName = "TimePicker",
                   DisplayName = "Time Picker",
                   Icon = "fa-history"
                },
                new Widget
                {
                   ComponentName = "Dropdown",
                   DisplayName = "Dropdown",
                   Icon = "fa-th-list"
                },
                new Widget
                {
                   ComponentName = "MultiSelect",
                   DisplayName = "Multi SElect",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "ListBox",
                   DisplayName = "List Box",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Accordion",
                   DisplayName = "Accordion",
                   Icon = "fa-list-alt"
                },
                 new Widget
                {
                   ComponentName = "MenuBar",
                   DisplayName = "Menu Bar",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Tab",
                   DisplayName = "Tab",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "FileExplorer",
                   DisplayName = "File Explorer",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Badge",
                   DisplayName = "Badge",
                   Icon = "fa-list-alt"
                },
                 new Widget
                {
                   ComponentName = "Toast",
                   DisplayName = "Toast",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "FileUploader",
                   DisplayName = "File Uploader",
                   Icon = "fa-list-alt"
                },
                 new Widget
                {
                   ComponentName = "ColorPicker",
                   DisplayName = "Color Picker",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Input",
                   DisplayName = "Text Input",
                   Icon = "fa-list-alt"
                },
            };
        }
    }
}
