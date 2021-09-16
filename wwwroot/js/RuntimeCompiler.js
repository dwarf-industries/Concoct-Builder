function RunScreen(screeName) {
    var dto = {
       Path: screeName
    }
    $.ajax({
        method: "POST",
        contentType: "application/json",
        url: "/Home/InitScreen",
        data: JSON.stringify(dto)
    }).done(function (msg) {
       
    });
}