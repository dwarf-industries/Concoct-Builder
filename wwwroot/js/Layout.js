 
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
 

function GenerateWidget(target, componentName) {
    draggedElement++;
 

    $.ajax({
        url: "/Home/GetComponent?componentName=" + componentName,
        method: "GET",
        success: function (data) {
            AddLayoutElement(componentName, data);
 
        }
    });
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
 
 

 

function ActivateEvent(id) {
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
 }

function AssociateTransitionEvent(key, screen) {
    for (var index in nodes) {
        var node = nodes[index];
        if (node.id === activeNode) {
            if (node.Events !== undefined) {
                node.Events[0] = {
                    Type: 0,
                    Relation: screen
                }
            }
            else {
                node.Events = [{
                    Type: 0,
                    Relation: screen
                }]
            }

            nodes[index] = node;
        }
    }
}

