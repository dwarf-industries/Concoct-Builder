﻿const { get } = require("jquery");

var isFullScreen = false;
var fileName = "Untiteled";
var IsOpen = 0;

var editObj = new ej.inplaceeditor.InPlaceEditor({
    mode: 'popup',
    type: 'Text',
    value: 'Untiteled',
    change: FileNameChanged,
    submitOnEnter: true,
    model: {
        placeholder: 'Enter Layout Name'
    },
    popupSettings: {
        title: 'Enter Layout Name'
    }
});
editObj.appendTo('#inplace_editor');

function FileNameChanged(args) {
    fileName = args.value;
}

function GetActiveFileName() {
    return fileName;
}

function ToggleSetting(val) {
    if (IsOpen === 1) {
        $("#SlidingElement").slideToggle();

        $("#PlatformElement").slideToggle();
    }
    else {
        $("#PlatformElement").slideToggle();
        $("#SlidingElement").slideToggle();
    }
    IsOpen = val;
} 

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
    FileSelected({
        Path: fileName,
        Name: fileName
    });
}

function LoadLayout() {
    
    mode = true;

    FileSelected({
        Path: fileName,
        Name: fileName
    });
}

 

function FileSelected(dto) {

    if (mode) {
        $.ajax({
            method: "POST",
            url: "/Home/LoadFromFile",
            contentType: "application/json",
            data: JSON.stringify(dto)
        }).done(function (args) {
            AddLayoutElementsAt(args);
        });
    }
    else
    {
        var items = [];
        for (var item in nodes) {
            items.push({
                ElementName: nodes[item].id === undefined ? "" : nodes[item].id,
                Width: nodes[item].width.toString(),
                Height: nodes[item].height.toString(),
                OffsetX: nodes[item].offsetX.toString(),
                OffsetY: nodes[item].offsetY.toString(),
                ComponentName: nodes[item].ComponentName === undefined ? "" : nodes[item].ComponentName
            });
        }

        var currentDto = {
            File: dto,
            PageElements: items,
            LayoutDetail : ""
        }
        var container = document.getElementById("outer-dropzone");
        html2canvas(container).then(function (canvas) {
             
            var base64 = canvas.toDataURL();
            currentDto.LayoutDetail = base64;

            $.ajax({
                method: "POST",
                contentType: "application/json",
                url: "/Home/SaveFile",
                data: JSON.stringify(currentDto)
            }).done(function (msg) {
                 ShowInfo("Layout saved!");
            });
        });

      
    }
   
}

function InitScreen() {
    RunScreen(fileName);
}