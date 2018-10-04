$(document).ready(() => {
    let input = $("#inputNumber");

    input.on("keypress", (event) => {
        let licznik = input.val().length;
        if ( ((licznik === 3 || licznik === 7) && event.key !== '-' ) && licznik <= 7) {
            input.val(input.val() + "-");
        }
    });

});