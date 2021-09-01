const { get } = require("jquery");

var isFullScreen = false;

function KillWindow() {
        $.ajax({
            url: "/Home/CloseApp",
            method: "GET",
            success: function (data) {
                 
 
            }
        });
}

function Minimize() {
    $.ajax({
        url: "/Home/MinimizeApp",
        method: "GET",
        success: function (data) {


        }
    });
}



function LoadLayout() {
    debugger

    var getElement = document.getElementById("FileManager") !== undefined ? document.getElementById("FileManager") : document.createElement("input");
    getElement.setAttribute("id", "fileManage");
    getElement.type = "file";
    getElement.setAttribute("style", "display:none");
    getElement.click();
    getElement.onchange = FileSelected;
}


function FileSelected(event) {
    debugger
    var dto = {
        Path: event.path[0].files[0].path,
        Name: event.path[0].files[0].name
    }

    $.ajax({
        method: "POST",
        url: "/Home/LoadFromFile",
        data: dto
    })
    .done(function (msg) {
        alert("Data Saved: " + msg);
    });
}