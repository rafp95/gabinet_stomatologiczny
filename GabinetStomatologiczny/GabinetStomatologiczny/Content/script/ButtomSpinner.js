$(function () {
    //login page spinner
    let loginButton = $("button.btnLogin");
    let spiner = $("i.loginSpinner");
    loginButton.on('click', function () {
        spiner.css({ "opacity": "1" });
        var time = setInterval(() => {
            spiner.css({ "opacity": "0" });
        }, 2000);
    });
    //end login page spinner
});