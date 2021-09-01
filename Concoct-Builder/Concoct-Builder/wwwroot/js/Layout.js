var storage = window.localStorage;
var ActiveList = {};
var draggedElement = 0;
var IsOpen = 0;
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
    sidebarChat.toggle();
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

function ChatToggle(val) {
    if (IsOpen == 1) {
        $("#PlatformElement").slideToggle();
        $("#ChatElement").slideToggle();
    }
    else {
        $("#PlatformElement").slideToggle();
        $("#ChatElement").slideToggle();
    }
    IsOpen = val;
}

function gettoken() {
    var token = '@Html.AntiForgeryToken()';
    token = $(token).val();
    return token;
}

function GenerateWidget(target, componentName) {
    debugger
    var getElement = document.createElement("div");
    getElement.setAttribute("id", "yes-drop_" + draggedElement);
    getElement.setAttribute("data-value", componentName);


    getElement.classList.add("resize-drag");
    getElement.style.setProperty("top", "20");
    getElement.style.setProperty("left", "20");

    $.ajax({
        url: "/Home/GetComponent?componentName=" + componentName,
        method: "GET",
        success: function (data) {
            debugger

            getElement.onmouseup = ElementReleased;
            $("#yes-drop_" + draggedElement).html(data)
            draggedElement++;
        }
    });
    $("#outer-dropzone").append(getElement);
}


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
    debugger
    var name = args.currentTarget.getAttribute("data-value");
    ActiveList[name + "_" + draggedElement] = {
        ElementName: name,
        InnerX: args.currentTarget,
        ClientX: args.ClientX,
        ClientY: args.clientY,
        OffsetX: args.offsetX,
        OffsetY: args.offsetY,
        Width: args.currentTarget.offsetHeight,
        Height: args.currentTarget.offsetHeight
    }
    
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