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
            var context = new ConcoctbuilderDbContext();
            var layout = context.Layouts.FirstOrDefault(x => x.Name == path);
            if (layout != null)
                return context.LayoutData.Where(x => x.LayoutId == layout.Id).Select(x => new PageElement
                {
                    Base64 = x.Base64,
                    ElementName = x.ElementName,
                    Translate = x.Translate,
                    ComponentName = x.ComponentName
                }).ToList();
            
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

        public void WriteFile(string path, List<PageElement> content, string layoutDetails)
        {
            var context = new ConcoctbuilderDbContext();
            var layout = context.Layouts.FirstOrDefault(x => x.Name == path);
            var activeSetting = context.UserSettings.FirstOrDefault(x => x.IsActive == 1);
            if (layout == null)
            {
                layout = context.Layouts.Add(new Layouts
                {
                    Name = path,
                    CreatedAt = DateTime.Now.ToFileTimeUtc().ToString(),
                    Owner = activeSetting.OrganizationName,
                    UpdatedAt = "",
                    UserSetting = activeSetting.Id,
                    LayoutThumbnail = layoutDetails,
                    

                }).Entity;
                context.SaveChanges();
            }
            else
            {
                layout.LayoutThumbnail = layoutDetails;
                context.Attach(layout);
                context.Update(layout);
                context.SaveChanges();
            }

            content.ForEach(x =>
            {
                var existingEvent = default(long); 
           
                var attachedEvent = x.Events != null ? x.Events.FirstOrDefault() : null;

                if (attachedEvent != null)
                    existingEvent = context.Layouts.FirstOrDefault(y => y.Name == attachedEvent.Relation).Id;

                var existing = context.LayoutData.FirstOrDefault(y => y.ElementName == x.ElementName && y.LayoutId == layout.Id);
                if (existing != null)
                {
                    existing.Translate = x.Translate;
                    existing.Base64 = x.Base64;
                    existing.Width = x.Width;
                    existing.Height = x.Height;
                    existing.OffsetY = x.OffsetY;
                    existing.OffsetX = x.OffsetX;

                    if(existingEvent != 0)
                        existing.RefereenceScreen = existingEvent;

                    context.Attach(existing);
                    context.Update(existing);
                    context.SaveChanges();
                }
                else
                {
                    if(existingEvent == default(long))
                        context.LayoutData.Add(new LayoutData
                        {
                            ElementName = x.ElementName,
                            Base64 = x.Base64,
                            Translate = x.Translate,
                            LayoutId = layout.Id,
                            ComponentName =  x.ComponentName,
                            OffsetX = x.OffsetX,
                            OffsetY = x.OffsetY,
                            Height = x.Height,
                            Width = x.Width
                        });
                    else
                        context.LayoutData.Add(new LayoutData
                        {
                            ElementName = x.ElementName,
                            Base64 = x.Base64,
                            Translate = x.Translate,
                            LayoutId = layout.Id,
                            ComponentName = x.ComponentName,
                            RefereenceScreen = existingEvent,
                            OffsetX = x.OffsetX,
                            OffsetY = x.OffsetY,
                            Height = x.Height,
                            Width = x.Width
                        });
                    context.SaveChanges();
                }    
            });
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

        public void RemoveLayoutFile(string layout, string file)
        {
            var context = new ConcoctbuilderDbContext();
            var currentLayout = context.Layouts.FirstOrDefault(x => x.Name == layout);
            if (currentLayout == null)
                return;

            var item = context.LayoutData.FirstOrDefault(x => x.LayoutId == currentLayout.Id && x.ElementName == file);
            if (item == null)
                return;

            context.LayoutData.Remove(item);
            context.SaveChanges();
        }

        public List<UserSettings> GetUserSettings()
        {
            var context = new ConcoctbuilderDbContext();
            return context.UserSettings.ToList();
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
