 
var storage = window.localStorage;
var ActiveList = {};
var draggedElement = 0;
var cThemeToggle = new ejs.buttons.Switch({ checked: false });
cThemeToggle.appendTo('#ChangeTheme');
var data;
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

function GenerateWidgetAt(component, cDraggable) {
    var getElement = document.createElement("div");
    getElement.setAttribute("id", "yes-drop_" + cDraggable);
    getElement.setAttribute("data-value", component.elementName);
    getElement.setAttribute("data-info", component.elementName + "_" + cDraggable);


    getElement.classList.add("resize-drag");

    $("#outer-dropzone").append(getElement);

    $.ajax({
        url: "/Home/GetComponent?componentName=" + component.elementName,
        method: "GET",
        success: function (data) {


            getElement.onmouseup = ElementReleased;
            $("#yes-drop_" + cDraggable).html(data);
            if (component.base64 !== "" && component.base64 !== undefined && component.base64 !== null) {
                SetContent(component.base64);
            }
            getElement.style.setProperty("width", component.width);
            getElement.style.setProperty("height", component.height);
            getElement.style.setProperty("transform", component.translate);
            //getElement.setAttribute("data-x", component.clientX);
            draggedElement = cDraggable;
            //getElement.setAttribute("data-y", component.clientY);
            RedRaw();

            ActiveList[component.elementName + "_" + cDraggable] = {
                ElementName: component.elementName,
                ClientX: component.clientX,
                ClientY: component.clientY,
                Width: component.width,
                Height: component.height,
                Translate: component.Translate,
                Base64: component.base64
            };
         }
    });
}

function GenerateWidget(target, componentName) {
     
    var getElement = document.createElement("div");
    getElement.setAttribute("id", "yes-drop_" + draggedElement);
    getElement.setAttribute("data-value", componentName);


    getElement.classList.add("resize-drag");
    getElement.style.setProperty("top", "20");
    getElement.style.setProperty("left", "20");
    getElement.style.setProperty("width", "200px");
    getElement.style.setProperty("height", "200px");

    $.ajax({
        url: "/Home/GetComponent?componentName=" + componentName,
        method: "GET",
        success: function (data) {
             
            getElement.onmouseup = ElementReleased;
            $("#yes-drop_" + draggedElement).html(data);
            draggedElement++;
            getElement.setAttribute("data-info", componentName + "_" + draggedElement);

        }
    });
    $("#outer-dropzone").append(getElement);
}


function UpdatePlaceholderContent(id, content) {
    var element = document.getElementById(id);
    var parentId = element.offsetParent.getAttribute("data-value");
    var newList = {};
    for (var item in ActiveList) {
        if (item !== parentId)
            newList[item] = ActiveList[item];
        else
        {
            var item = ActiveList[item];
            item.Base64 = content;
        }
    }

    ActiveList = newList;
}

function RemoveElement(id) {
    var element = document.getElementById(id);
    var parentId = element.offsetParent.getAttribute("data-value");
    element.offsetParent.remove();
    var newList = {};
    for (var item in ActiveList) {
        if (item !== parentId)
            newList[item] = ActiveList[item];
    }

    ActiveList = newList;
}

var resizing = false;
interact('.resize-drag')
    .resizable({
        // resize from all edges and corners
        edges: { left: true, right: true, bottom: true, top: true },

        listeners: {
            move(event) {
                var target = event.target
                var x = (parseFloat(target.getAttribute('data-x')) || 0)
                var y = (parseFloat(target.getAttribute('data-y')) || 0)

                // update the element's style
                target.style.width = event.rect.width + 'px'
                target.style.height = event.rect.height + 'px'

                // translate when resizing from top or left edges
                x += event.deltaRect.left
                y += event.deltaRect.top

                target.style.transform = 'translate(' + x + 'px,' + y + 'px)'

                target.setAttribute('data-x', x)
                target.setAttribute('data-y', y)
                if (!resizing) {
                    resizing = true;
                    setTimeout(function () {
                        RedRaw();
                        resizing = false;
                    }, 600)
                }
                 //  target.textContent = Math.round(event.rect.width) + '\u00D7' + Math.round(event.rect.height)
            }
        },
        modifiers: [
            // keep the edges inside the parent
            interact.modifiers.restrictEdges({
                outer: 'parent'
            }),
            
            // minimum size
            interact.modifiers.restrictSize({
                min: { width: 100, height: 50 }
            })
        ],

        inertia: true
    })
    .draggable({
        listeners: { move: window.dragMoveListener },
        inertia: true,
        modifiers: [
            interact.modifiers.restrictRect({
                restriction: 'parent',
                endOnly: true
            })
        ]
    })

 

function ElementReleased(args) {
    var transform = args.currentTarget.style.getPropertyValue("transform");
    var name = args.currentTarget.getAttribute("data-value");
    if (ActiveList[name + "_" + draggedElement] === undefined)
        ActiveList[name + "_" + draggedElement] = {
            ElementName: name,
            ClientX: "2",
            ClientY: "2",
            OffsetX: args.offsetX.toString(),
            OffsetY: args.offsetY.toString(),
            Width: args.currentTarget.style.getPropertyValue("width"),
            Height: args.currentTarget.style.getPropertyValue("height"),
            Translate: transform,
            Base64: ""
        };
    else
        ActiveList[name + "_" + draggedElement] = {
            ElementName: name,
            ClientX: "2",
            ClientY: "2",
            OffsetX: args.offsetX.toString(),
            OffsetY: args.offsetY.toString(),
            Width: args.currentTarget.style.getPropertyValue("width"),
            Height: args.currentTarget.style.getPropertyValue("height"),
            Translate: transform,
            base64: ActiveList[name + "_" + draggedElement].Base64
        };
}

function ActivateEvent(id) {
    var element = document.getElementById(id);
    var parentId = element.offsetParent.getAttribute("data-value");
    var arrayId = element.offsetParent.getAttribute("data-info");
    var newList = {};
   // ShowLoader();
    for (var item in ActiveList) {
        if (item !== arrayId)
            newList[item] = ActiveList[item];
        else
        {
            var data = ActiveList[item];
            data.Events = [
                {Type: 0, relation : "" }
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
                AddShape(id);
            }, 1000);
         }
    });
    ActiveList = newList;
}

function dragMoveListener(event) {
    var target = event.target,
        // keep the dragged position in the data-x/data-y attributes
        x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
        y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

    // translate the element
    target.style.webkitTransform =
        target.style.transform =
        'translate(' + x + 'px, ' + y + 'px)';

    // update the posiion attributes
    target.setAttribute('data-x', x);
    target.setAttribute('data-y', y);
}