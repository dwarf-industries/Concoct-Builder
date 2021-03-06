
var resizing = false;

function initResizeElement() {
    var popups = document.getElementsByClassName("popup");
    var element = null;
    var startX, startY, startWidth, startHeight;

    for (var i = 0; i < popups.length; i++) {
        var p = popups[i];

        var right = document.createElement("div");
        right.className = "resizer-right";
        p.appendChild(right);
        right.addEventListener("mousedown", initDrag, false);
        right.parentPopup = p;

        var bottom = document.createElement("div");
        bottom.className = "resizer-bottom";
        p.appendChild(bottom);
        bottom.addEventListener("mousedown", initDrag, false);
        bottom.parentPopup = p;

        var both = document.createElement("div");
        both.className = "resizer-both";
        p.appendChild(both);
        both.addEventListener("mousedown", initDrag, false);
        both.parentPopup = p;
    }

    function initDrag(e) {
        element = this.parentPopup;
        dragItem = null;
        startX = e.clientX;
        startY = e.clientY;
        startWidth = parseInt(
            document.defaultView.getComputedStyle(element).width,
            10
        );
        startHeight = parseInt(
            document.defaultView.getComputedStyle(element).height,
            10
        );
        document.documentElement.addEventListener("mousemove", doDrag, false);
        document.documentElement.addEventListener("mouseup", stopDrag, false);
    }

    function doDrag(e) {
        resizing = true;
        element.style.width = startWidth + e.clientX - startX + "px";
        element.style.height = startHeight + e.clientY - startY + "px";
    }

    function stopDrag() {
        resizing = false;
        RedRaw();
        document.documentElement.removeEventListener("mousemove", doDrag, false);
        document.documentElement.removeEventListener("mouseup", stopDrag, false);
    }
}


 
var dragItem = null;
var container = document.querySelector("#outer-dropzone");
window.addEventListener("mouseup", mouseUp, true);
   
function StartDrag(id) {
    if (dragItem === null || dragItem !== id.currentTarget) {
        dragItem = id.currentTarget;
        startX = id.currentTarget.onmousedown.arguments[0].offsetX;
        startY = id.currentTarget.onmousedown.arguments[0].offsetY;
        window.addEventListener('mousemove', divMove, true);
    }
 }


function mouseUp() {
    if (dragItem !== null) {
        startY = undefined;
        startX = undefined;
        dragItem = null;
        window.removeEventListener('mousemove', divMove, true);
    }
}

function mouseDown(e) {
    window.addEventListener('mousemove', divMove, true);
}

function divMove(e) {
    if (dragItem !== null && !resizing) {
        dragItem.style.position = 'absolute';
        dragItem.style.top = (e.clientY - startY)  + 'px';
        dragItem.style.left = (e.clientX - startX)  + 'px';
    }
}
 

    