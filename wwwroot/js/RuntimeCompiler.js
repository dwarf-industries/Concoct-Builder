function RunScreen(screeName) {

   
    $.ajax({
        method: "POST",
        contentType: "application/json",
        url: "/Home/InitScreen",
        data: screeName
    }).done(function (msg) {
       
    });
}