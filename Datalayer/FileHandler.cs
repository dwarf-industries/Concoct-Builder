using Concoct_Builder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
                return context.LayoutData.Include(x=>x.RefereenceScreenNavigation).Where(x => x.LayoutId == layout.Id).Select(x => new PageElement
                {
                    Base64 = x.Base64,
                    ElementName = x.ElementName,
                    Translate = x.Translate,
                    ComponentName = x.ComponentName,
                    Width = x.Width,
                    Height = x.Height,
                    OffsetX = x.OffsetX,
                    OffsetY = x.OffsetY,
                    IsTrigger = (int)x.IsTrigger == 0 ? false:true,
                    hoPercent = x.HoPercent,
                    hPercent = x.HPercent,
                    woPercent = x.WoPercent,
                    wPercent = x.WPercent,
                    Events = new List<Event>{
                        new   Event{
                            Relation = x.RefereenceScreenNavigation.Name,
                            Type = 0
                        }
                    }
                }).ToList();
            
            return null;
        }

        internal List<WorkItems> GetAllWorkItemsInProjects(int id)
        {
            var context = new ConcoctbuilderDbContext();
            if(id != 0)
                return context.WorkItems.Where(x => x.ProjectId == id).ToList();

            return context.WorkItems.ToList();
        }

        internal int GetActiveOrganization()
        {
            var context = new ConcoctbuilderDbContext();
            return (int)context.UserSettings.FirstOrDefault(x => x.IsActive == 1).Id;
        }

        internal List<WorkItems> GetAllWorkItemsInProjects(List<long> list)
        {
            var context = new ConcoctbuilderDbContext();
            var workItemResult = new List<WorkItems>();
            list.ForEach(x =>
            {
                workItemResult.AddRange(context.WorkItems.Where(y=>y.ProjectId == (int)x));
            });
            return workItemResult;
        }

        internal List<Layouts> GetAllLayoutsByOrganization(int id)
        {
            var context = new ConcoctbuilderDbContext();
            return context.Layouts.Include(x=>x.AssociatedTags).ThenInclude(AssociatedTags=> AssociatedTags.Tag).Where(x => x.UserSetting == 0 || x.UserSetting == id).ToList();
        }

        internal List<Tags> GetAllOrganizationTags(int id)
        {
            var context = new ConcoctbuilderDbContext();
            var organization = context.UserSettings.FirstOrDefault(x => x.Id == id);
            return context.AssociatedTags.Include(x => x.Tag)
                                         .Where(x => x.Organization == organization.OrganizationName)
                                         .Select(x => x.Tag)
                                         .ToList();
        }

        internal List<Projects> GetAllProjetsForOrganization(int id)
        {
            var context = new ConcoctbuilderDbContext();
            var organizationName = context.UserSettings.FirstOrDefault(x => x.Id == id);
            if (organizationName == null)
                return null;

            return context.Projects.Where(x => x.Organization == organizationName.OrganizationName).ToList();
        }

        internal List<UserSettings> GetAllOrganizations()
        {
            var context = new ConcoctbuilderDbContext();
            return context.UserSettings.ToList();
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

        public void WriteFile(string path, List<PageElement> content, string layoutDetails, int projectId, int workItemId)
        {
            var context = new ConcoctbuilderDbContext();
            var layout = context.Layouts.FirstOrDefault(x => x.Name == path);
            var activeSetting = context.UserSettings.FirstOrDefault(x => x.IsActive == 1);
            if (layout == null)
            {
                if(projectId != 0 && workItemId != 0)
                    layout = context.Layouts.Add(new Layouts
                    {
                        Name = path,
                        CreatedAt = DateTime.Now.ToFileTimeUtc().ToString(),
                        Owner = activeSetting.OrganizationName,
                        UpdatedAt = "",
                        UserSetting = activeSetting.Id,
                        LayoutThumbnail = layoutDetails,
                        ProjectId = projectId,
                        WorkItemId = workItemId

                    }).Entity;
                else
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
                if (projectId != 0 && workItemId != 0)
                    layout.ProjectId = projectId;
                if (workItemId != 0 && projectId != 0)
                    layout.WorkItemId = workItemId;
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
                    existing.IsTrigger = x.IsTrigger ? 1:0;
                    existing.HoPercent = x.hoPercent;
                    existing.WoPercent = x.woPercent;
                    existing.HPercent = x.hPercent;
                    existing.WPercent = x.wPercent;
 
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
                            Width = x.Width,
                            IsTrigger = x.IsTrigger?1:0,
                            WoPercent = x.woPercent,
                            HoPercent = x.hoPercent,
                            WPercent = x.wPercent,
                            HPercent = x.hPercent
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
                            Width = x.Width,
                            IsTrigger = x.IsTrigger?1:0,
                            WoPercent = x.woPercent,
                            HoPercent = x.hoPercent,
                            WPercent = x.wPercent,
                            HPercent = x.hPercent
                        });
                    context.SaveChanges();
                }    
            });
        }

        internal string PushUpStream(int settingId)
        {
            var context = new ConcoctbuilderDbContext();
            var layouts = context.Layouts.Where(x => x.UserSetting == settingId).ToList();
            var tags = context.Tags.Where(x => x.IsNew == 1).ToList();
            dynamic cResult = new System.Dynamic.ExpandoObject();
            cResult.Layouts = layouts;
            cResult.Take = tags;
            dynamic eResult = new System.Dynamic.ExpandoObject();
            eResult.Phase = System.Text.Json.JsonSerializer.Serialize(layouts);
            var pack = System.Text.Json.JsonSerializer.Serialize(eResult);

            return pack;
        }



        internal UserSettings GetSettingByLayout(string name)
        {
            var context = new ConcoctbuilderDbContext();
            var userSetting = context.Layouts.Include(x => x.UserSettingNavigation).FirstOrDefault(x => x.Name == name);
            if (userSetting == null)
                return null;

            return userSetting.UserSettingNavigation;
        }

        internal void UpdateCompiled(string name, string layoutDetail)
        {
            var context = new ConcoctbuilderDbContext();
            var layout = context.Layouts.FirstOrDefault(x => x.Name == name);
            if (layout == null)
                return;

            layout.WorkItemResult = layoutDetail;
            context.Attach(layout);
            context.Update(layout);
            context.SaveChanges();
        }

        internal bool SetLayoutActive(int id)
        {
            var context = new ConcoctbuilderDbContext();
            var oldActive = context.UserSettings.FirstOrDefault(x => x.IsActive == 1);
            var layout = context.UserSettings.FirstOrDefault(x => x.Id == id);
            if (layout == null)
                return false;

            oldActive.IsActive = 0;
            context.Attach(oldActive);
            context.Update(oldActive);
            context.SaveChanges();

            layout.IsActive = 1;
            context.Attach(layout);
            context.Update(layout);
            context.SaveChanges();
            Program.ActiveUserSetting = (int)layout.Id;

            return true;
        }

        internal void SyncContentDownStream(IncomingServerResponse parseToObject, AuthenicationRequest request)
        {
            var context = new ConcoctbuilderDbContext();
            parseToObject.item3.ForEach(x =>
            {
                var getOrganizationByName = context.UserSettings.FirstOrDefault(y => y.OrganizationName.ToLower() == x.ToLower());
                if (getOrganizationByName == null)
                {
                    var lastElement = context.UserSettings.ToList().LastOrDefault();
                    context.UserSettings.Add(new UserSettings
                    {
                        Id = lastElement.Id + 1,
                        ConnectionType = request.AuthType ? 1 : 0,
                        Endpoint = request.Instance,
                        InstanceAddress = $"{request.Instance}/OutboundDetails/AuthenicateCB",
                        Key = request.Token,
                        Username = request.Username,
                        Password = request.Password,
                        OrganizationName = x.ToLower()
                    });
                    context.SaveChanges();
                }
            });
            parseToObject.item2.ForEach(x =>
            {
                var getOrganizationByName = context.UserSettings.FirstOrDefault(x => x.OrganizationName.ToLower() == x.OrganizationName.ToLower());
                if (getOrganizationByName != null)
                {
                    var project = context.Projects.FirstOrDefault(y => y.InternalId == x.ProjectId);
                    if (project == null)
                    {

                        project = context.Projects.Add(new Projects
                        {
                            InternalId = x.ProjectId,
                            Organization = x.OrganizationName.ToLower(),
                            ProjectName = x.ProjectName
                        }).Entity;
                        context.SaveChanges();
                    }

                    x.WorkItems.ForEach(y =>
                    {
                        var workItem = context.WorkItems.FirstOrDefault(z => z.InternalId == y.id);
                        if (workItem == null)
                        {
                            workItem = context.WorkItems.Add(new WorkItems
                            {
                                InternalId = y.id,
                                ProjectId = project.Id,
                                Title = y.title,
                                SprintId = y.iteration,
                                WorkItemType = y.workItemType
                            }).Entity;
                            context.SaveChanges();
                        }
                        else
                        {
                            workItem.Title = y.title;
                            context.Attach(workItem);
                            context.Update(workItem);
                            context.SaveChanges();
                        }
                    });
                }
            });
            parseToObject.item4.ForEach(x =>
            {
                x.Tags.ForEach(y =>
                {
                    var tag = context.Tags.FirstOrDefault(z => z.Name == y);
                    if (tag == null)
                    {
                        var tagId =context.Tags.Add(new Tags
                        {
                            Name = y
                        }).Entity;
                        context.SaveChanges();
                        context.AssociatedTags.Add(new AssociatedTags
                        {
                            Organization = x.Organization,
                            TagId = tagId.Id
                        });
                        context.SaveChanges();
                    }
                });
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
