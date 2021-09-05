const { get } = require("jquery");

var isFullScreen = false;
var fileName = "Untiteled.cf";

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

var mode = true;

function SaveChanges() {
    mode = false;
    var getElement;
    getElement = document.getElementById("FileManager");
    if (getElement === null) {
        getElement = document.createElement("input");
    }
    getElement.setAttribute("id", "fileManage");
    getElement.setAttribute("webkitdirectory", "");
    getElement.setAttribute("directory", "");
    getElement.type = "file";
    getElement.setAttribute("style", "display:none");
    getElement.click();
    getElement.onchange = FileSelect;
}

function LoadLayout() {
    
    mode = true;
    var getElement;
    getElement = document.getElementById("FileManager");
    if (getElement === null) {
        getElement = document.createElement("input");
    }
    getElement.setAttribute("id", "fileManage");
    getElement.type = "file";
    getElement.setAttribute("style", "display:none");
    getElement.click();
    getElement.onchange = FileSelect;
}


function FileSelect(event) {
     
    var dto;
    if (mode) {

        dto = {
            Path: event.path[0].files[0].path,
            Name: event.path[0].files[0].name
        }
    }
    else {
        var folders = event.path[0].files[0].path.split("\\");
        var i = 0;
        var path = "";
        for (var item in folders) {
            if (i < folders.length - 1) {
                path += folders[item] + "\\";
            }
            i++;
        }
        dto = {
            Path: path,
            Name: fileName
        }
    }

    FileSelected(dto);
    
}

function FileSelected(dto) {

    if (mode) {
        $.ajax({
            method: "POST",
            url: "/Home/LoadFromFile",
            contentType: "application/json",
            data: JSON.stringify(dto)
        }).done(function (msg) {
            alert("Data Saved: " + msg);
        });
    }
    else
    {
        var items = [];
        for (var item in ActiveList) {
            items.push(ActiveList[item]);
        }

        var currentDto = {
            File: dto,
            PageElements: items
        }
        $.ajax({
            method: "POST",
            contentType: "application/json",
            url: "/Home/SaveFile",
            data: JSON.stringify(currentDto)
        }).done(function (msg) {
            alert("Data Saved: " + msg);
        });
    }
   
}