const { get } = require("jquery");

var isFullScreen = false;
var fileName = "Untiteled.cf";
var IsOpen = 0;

var editObj = new ej.inplaceeditor.InPlaceEditor({
    mode: 'popup',
    type: 'Text',
    value: 'Untiteled.cf',
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
            var getArea = document.getElementById("outer-dropzone");
            getArea.innerHTML = "";
            var draggableValue = 0;
            ClearActiveList();
            for(var element in args)
            {
                GenerateWidgetAt(args[element], draggableValue);
                draggableValue++;
            }
            draggableValue++;

            SetDraggableStartingIndex(draggableValue);
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