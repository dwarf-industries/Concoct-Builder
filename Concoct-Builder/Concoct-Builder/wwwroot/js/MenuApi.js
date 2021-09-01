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

 