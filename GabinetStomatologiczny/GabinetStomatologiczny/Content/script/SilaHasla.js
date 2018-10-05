$(document).ready(function () {
    //Zmienne pobrania wartości z inputa plus informacja o sile hasła
    var pass = $("input#Haslo");
    var checkInfo = $("span#InfoPassword");
    //Sprawdzenie poprawności hasła
    silaHasla(pass, checkInfo);

});

//Funkcja sprawdzająca siłę hasła
function silaHasla(pass, checkInfo) {

    pass.on('keyup', function () {

        if ($(this).val().length <= 5) {
            checkInfo.text("Bardzo słabe");
            checkInfo.css("color", "red");
            return;
        }

        var wyrazenie = new Array();
        wyrazenie.push("[A-Z]");
        wyrazenie.push("[a-z]");
        wyrazenie.push("[0-9]");
        wyrazenie.push("[$@$!%*#?&]");
        wyrazenie.push("[ąćźęółńĄĆŻĘÓŁŃ]");

        var poprawnosc = 0;
        for (var i = 0; i < wyrazenie.length; i++) {
            if ((new RegExp(wyrazenie[i])).test($(this).val())) {
                poprawnosc++;
                console.log(poprawnosc);
            }
        }

        if (poprawnosc > 2 && pass.val().length > 8) {
            poprawnosc++;
            console.log(poprawnosc);
        }

        var kolor = "";
        var silaHasla = "";
        switch (poprawnosc) {
            case 0:
                break;
            case 1:
                silaHasla = "Bardzo słabe";
                kolor = "red";
                console.log(poprawnosc);
                break;
            case 2:
                silaHasla = "Dobre";
                kolor = "darkorange";
                console.log(poprawnosc);
                break;
            case 3:
                silaHasla = "Dobre";
                kolor = "darkorange";
                console.log(poprawnosc);
                break;
            case 4:
                silaHasla = "Bardzo dobre";
                kolor = "green";
                console.log(poprawnosc);
                break;
             case 5:
                silaHasla = "Silne";
                kolor = "darkgreen";
                console.log(poprawnosc);
                break;
        }

        checkInfo.text(silaHasla);
        checkInfo.css("color", kolor);

    });
}
//Koniec metody sprawdzania hasła
