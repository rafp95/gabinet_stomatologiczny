$(document).ready(() => {
    var input = $("#inputNumber");

    input.keypress(() => {
        var licznik = input.val().length;
        if ( (licznik === 3 || licznik === 7 || licznik === 11) && licznik < 11) {
            input.val(input.val() + "-");
        }
    });

});