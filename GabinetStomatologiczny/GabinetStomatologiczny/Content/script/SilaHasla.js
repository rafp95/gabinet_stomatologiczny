$(document).ready(function () {
    //Zmienne pobrania wartości z inputa plus informacja o sile hasła
    var pass = $("input#Haslo");
    var checkInfo = $("span#InfoPassword");
    //Sprawdzenie poprawności hasła
    silaHasla(pass, checkInfo);

});

//Funkcja sprawdzająca siłę hasła
function silaHasla(pass, checkInfo) {

    pass.off().on('keyup', function () {
        $('#myProgress').css({ 'display': 'block' });
        if ($(this).val().length <= 5) {
            move(0, 'red');
            checkInfo.text("Bardzo słabe");
            checkInfo.css("color", "red");
            return;
        }

        let wyrazenie = new Array();
        wyrazenie.push("[A-Z]");
        wyrazenie.push("[a-z]");
        wyrazenie.push("[0-9]");
        wyrazenie.push("[$@$!%*#?&]");
        wyrazenie.push("[ąćźęółńĄĆŻĘÓŁŃ]");

        let poprawnosc = 0;
        for (let i = 0; i < wyrazenie.length; i++) {
            if ((new RegExp(wyrazenie[i])).test($(this).val())) {
                poprawnosc++;
                console.log(poprawnosc);
            }
        }

        if (poprawnosc > 2 && pass.val().length > 8) {
            poprawnosc++;
            console.log(poprawnosc);
        }

        let kolor = "";
        let silaHasla = "";
        switch (poprawnosc) {
            case 0:
                move(0, 'red');
                break;
            case 1:
                silaHasla = "Bardzo słabe";
                kolor = "red";
                move(0, kolor);
                console.log(poprawnosc);
                break;
            case 2:
                silaHasla = "Dobre";
                kolor = "darkorange";
                move(40,kolor);
                console.log(poprawnosc);
                break;
            case 3:
                silaHasla = "Dobre";
                kolor = "darkorange";
                move(60, kolor);
                console.log(poprawnosc);
                break;
            case 4:
                silaHasla = "Bardzo dobre";
                kolor = "green";
                move(80, kolor);
                console.log(poprawnosc);
                break;
            case 5:
                silaHasla = "Silne";
                kolor = "darkgreen";
                move(100, kolor);
                console.log(poprawnosc);
                break;
        }

        checkInfo.text(silaHasla);
        checkInfo.css("color", kolor);

    });
}
//Koniec metody sprawdzania hasła

function move(procent, kolor) {
    console.log('zmiana koloru '+kolor);
    let elem = $('#myBar');
    let width = procent || 1;
    elem.css({'background':kolor});
    elem.animate({
        width: width + '%'
    },200);
}