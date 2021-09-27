 
var storage = window.localStorage;
var ActiveList = {};
var draggedElement = 0;
var cThemeToggle = new ejs.buttons.Switch({ checked: false });
cThemeToggle.appendTo('#ChangeTheme');
var data;
var LastActiveElement;
// Sidebar Initialization
var sidebarMenu = new ej.navigations.Sidebar({
    width: '290px',
    target: '.main-content',
    mediaQuery: '(min-width: 600px)',
});
sidebarMenu.appendTo('#sidebar-treeview');
//end of Sidebar initialization

//if (storage.getItem("Theme") !== undefined) {
//    SetTheme(storage.getItem("Theme"));
//}
//else {
//    storage.setItem("Theme", false);
//}

// Toggle the Sidebar
document.getElementById('hamburger').addEventListener('click', function () {
    sidebarMenu.toggle();
});
// open new tab


function ChangeTheme(args) {
    cThemeToggle.click();

    if (cThemeToggle.checked === true) {
        $("#themeLabel").html("Dark");
        document.getElementById('theme').href = "/css/Themes/Dark/fabric-dark.min.css";
        document.getElementById('themeAdjustments').href = "/css/Shared/Dark/_LayoutDashboard.css";
        storage.setItem("Theme", true);


    }
    else {
        $("#themeLabel").html("Light");
        document.getElementById('theme').href = "/css/Themes/Light/material.min.css";
        document.getElementById('themeAdjustments').href = "/css/Shared/Light/_LayoutDashboard.css";
        storage.setItem("Theme", false);

    }
}

function SetTheme(type) {
    if (type === "true") {
        cThemeToggle.click();

        $("#themeLabel").html("Dark");
        document.getElementById('theme').href = "/css/Themes/Dark/fabric-dark.min.css";
        document.getElementById('themeAdjustments').href = "/css/Shared/Dark/_LayoutDashboard.css";

    }
    else {
        $("#themeLabel").html("Light");
        document.getElementById('theme').href = "/css/Themes/Light/material.min.css";
        document.getElementById('themeAdjustments').href = "/css/Shared/Light/_LayoutDashboard.css";
    }
}


function CallUrl(url) {
    window.location.href = url;
}

function ShowLoader() {
    $("#spinner").show();
    $("#Content").hide();
}
function ShowContent() {
    $("#spinner").hide();
    $("#Content").show();
}
 

function gettoken() {
    var token = '@Html.AntiForgeryToken()';
    token = $(token).val();
    return token;
}

function SetDraggableStartingIndex(index) {
    draggedElement = index + 1;
}

function ClearActiveList() {
    ActiveList = {};
}

function GenerateWidgetAt(component, cDraggable) {
    var getElement = document.createElement("div");
    getElement.setAttribute("id", "yes-drop_" + cDraggable);
    getElement.setAttribute("data-value", component.componentName);
    getElement.setAttribute("data-info", component.elementName);
    getElement.classList.add("popup");


    //getElement.classList.add("resize-drag");

    $("#outer-dropzone").append(getElement);

    $.ajax({
        url: "/Home/GetComponent?componentName=" + component.componentName,
        method: "GET",
        success: function (data) {


            getElement.onmouseup = ElementReleased;
            $("#yes-drop_" + cDraggable).html(data);
            getElement.setAttribute("drag-id", cDraggable);

            getElement.style.cssText = component.translate;

            //getElement.setAttribute("data-x", component.clientX);
            draggedElement = cDraggable;
            //getElement.setAttribute("data-y", component.clientY);
            RedRaw();
           // initDragElement();
    
            ActiveList[component.elementName] = {
                ElementName: component.elementName,
                ComponentName: component.componentName,
                Translate: component.translate,
                Base64: component.base64
            };

            if (component.base64 !== null && component.base64 !== "") {
                SetContent(component.base64);
            }

            getElement.onmousemove = divMove;
            getElement.onmousedown = StartDrag;
            initResizeElement();
         }
    });
}

function GenerateWidget(target, componentName) {
    draggedElement++;

    var getElement = document.createElement("div");
    getElement.setAttribute("id", "yes-drop_" + draggedElement);
    getElement.setAttribute("data-value", componentName);

    getElement.classList.add("popup");

  //  getElement.classList.add("resize-drag");

    
    getElement.style.setProperty("top", "20");
    getElement.style.setProperty("left", "20");
    getElement.style.setProperty("width", "200px");
    getElement.style.setProperty("height", "200px");
    getElement.style.setProperty("position", "absolute");

    $.ajax({
        url: "/Home/GetComponent?componentName=" + componentName,
        method: "GET",
        success: function (data) {
            AddLayoutElement(null, data);
           // getElement.onmouseup = ElementReleased;
           // $("#yes-drop_" + draggedElement).html(data);
           // getElement.setAttribute("drag-id", draggedElement);

           // getElement.setAttribute("data-info", componentName + "_" + draggedElement);
           //// initDragElement();
           // getElement.onmousemove = divMove;
           // getElement.onmousedown = StartDrag;
           // initResizeElement();
        }
    });
    $("#outer-dropzone").append(getElement);
}


function UpdatePlaceholderContent(id, content) {
    var element = document.getElementById(id);
    var parentId = element.offsetParent.getAttribute("data-info");
    var newList = {};
    for (var item in ActiveList) {
        if (item !== parentId)
            newList[item] = ActiveList[item];
        else
        {
            var currentItem = ActiveList[item];
            currentItem.Base64 = content;
            newList[item] = currentItem;
        }
    }

    ActiveList = newList;
}

function RemoveElement(id) {
    var element = document.getElementById(id);
    var parentId = element.offsetParent.getAttribute("data-info");
    element.offsetParent.remove();
    var newList = {};
    for (var item in ActiveList) {
        if (item !== parentId)
            newList[item] = ActiveList[item];
        else {
            var dto = {
                Layout: GetActiveFileName(),
                File: ActiveList[item].ElementName
            }
            $.ajax({
                method: "POST",
                contentType: "application/json",
                url: "/Home/RemoveItemFromLayout",
                data: JSON.stringify(dto)
            }).done(function (msg) {
                ShowInfo("Layout saved!");
            });
        }
    }

    ActiveList = newList;
}

var resizing = false;
 

function ElementReleased(args) {
    var transform = args.currentTarget.style.getPropertyValue("transform");
    var name = args.currentTarget.getAttribute("data-info");
    var dragId = args.currentTarget.getAttribute("drag-id");
    var cName = args.currentTarget.getAttribute("data-value");

    if (ActiveList[name] === undefined)
        ActiveList[name] = {
            ElementName: name,
            ComponentName: cName,
            ClientY: "Depricated",
            OffsetX: "Depricated",
            OffsetY: "Depricated",
            Width: "Depricated",
            Height: "Depricated",
            Translate: args.currentTarget.style.cssText,
            Base64: ""
        };
    else
        ActiveList[name] = {
            ElementName: name,
            ComponentName: cName,
            ClientY: "Depricated",
            OffsetX: "Depricated",
            OffsetY: "Depricated",
            Width: "Depricated",
            Height: "Depricated",
            Translate: args.currentTarget.style.cssText,
            Base64: ActiveList[name].Base64
        };
}

function ActivateEvent(id) {
    var element = document.getElementById(id);
    var parentId = element.offsetParent.getAttribute("data-value");
    var arrayId = element.offsetParent.getAttribute("data-info");
    LastActiveElement = arrayId;
    var newList = {};
   // ShowLoader();
    for (var item in ActiveList) {
        if (item !== arrayId)
            newList[item] = ActiveList[item];
        else
        {
            var data = ActiveList[item];
            data.Events = [
                {Type: 0, Relation : "" }
            ];
            newList[item] = data;
        }
    }
    ToggleSetting();


    $.ajax({
        url: "/Home/GetComponentPanel?componentName=Element&&args=FlowDiagram",
        method: "GET",
        success: function (data) {
       
    
            $("#SlidingElement").html(data);
            setTimeout(function () {
                DrawFlowDiagram();
                SetCurrentActiveEvent(id);
                AddShape(id, "Event", "node1");
                InitLayouts();
            }, 1000);
         }
    });
    ActiveList = newList;
}

function AssociateTransitionEvent(key, screen) {
    var newList = {};
    
    // ShowLoader();
    for (var item in ActiveList)
    {
        if (item !== LastActiveElement)
            newList[item] = ActiveList[item];
        else
        {
            var data = ActiveList[item];
            if (data.Events === undefined) {
                data.Events = [
                    { Type: 0, Relation: screen }
                ];
            }
            else
            {
                var tpm = []; 
                for (var event in data.Events)
                {
                    if (data.Events[event].Type === 0) {
                        data.Events[event].Relation = screen;
                        tpm.push(data.Events[event]);
                    }
                    else
                    {
                        tpm.push(data.Events[event]);
                    }
                }
                data.Events = tpm;
            }
           
            newList[item] = data;
        }
    }
    ActiveList = newList;

}

