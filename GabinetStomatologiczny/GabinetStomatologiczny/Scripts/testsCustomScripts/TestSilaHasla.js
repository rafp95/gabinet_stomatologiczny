$(function () {
    let pass = $("input#Haslo");
    let checkInfo = $("span#InfoPassword");
    pass.val("Patryk12345!");

    //test element exist
    //test(pass.length > 0);
    //end test element exist

    //test password is too short
    test(pass, checkInfo);
    //end test password is too short

    //Test sila hasla
    function test(pass, checkInfo) {

        //check if element exist in dom
        //assert(pass,'');
        //end test

        //testPasswordIsToShort(pass, checkInfo);
        //assert(true, 'Hasło poprawne');
        checkPasswordStrength(pass, checkInfo);


    }

    function testPasswordIsToShort(pass, checkInfo) {
        if (pass.val().length < 5) {
            move(0, 'red');
            checkInfo.text("Bardzo słabe");
            checkInfo.css("color", "red");

            assert(false, 'Zbyt mało znaków');
            assert(checkInfo.css('color') === 'red', 'Kolor czerwony');
            assert(checkInfo.text() === 'Bardzo słabe', 'Bardzo słabe');
            return;
        }
    }

    function checkPasswordStrength(pass, checkInfo) {

        let wyrazenie = new Array();
        wyrazenie.push("[A-Z]");
        wyrazenie.push("[a-z]");
        wyrazenie.push("[0-9]");
        wyrazenie.push("[$@$!%*#?&]");
        wyrazenie.push("[ąćęółńżź]");
        wyrazenie.push("[ĄĆĘÓŁŃŻŹ]");

        let poprawnosc = 0;
        for (let i = 0; i < wyrazenie.length; i++) {
            if ((new RegExp(wyrazenie[i])).test(pass.val())) {
                poprawnosc++;
                //console.log(poprawnosc);
            }
        }

        if (poprawnosc > 2 && pass.val().length > 5) {
            poprawnosc++;
            //console.log(poprawnosc);
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
                //console.log(poprawnosc);
                break;
            case 2:
                silaHasla = "Dobre";
                kolor = "darkorange";
                move(40, kolor);
                //console.log(poprawnosc);
                break;
            case 3:
                silaHasla = "Dobre";
                kolor = "darkorange";
                move(60, kolor);
                //console.log(poprawnosc);
                break;
            case 4:
                silaHasla = "Bardzo dobre";
                kolor = "green";
                move(80, kolor);
                //console.log(poprawnosc);
                break;
            case 5:
                silaHasla = "Silne";
                kolor = "darkgreen";
                move(100, kolor);
                break;
        }
        checkInfo.text(silaHasla);
        checkInfo.css("color", kolor);
        assert(kolor === 'darkgreen', 'Ustawiony kolor jest prawidłowy');
        assert(silaHasla === 'Silne', 'Hasło jest silne');
    }

    function move(procent, kolor) {
        let elem = $('#myBar');
        let width = procent || 1;
        elem.css({ 'background': kolor });
        elem.text(width + ' %');
        elem.animate({
            width: width + '%'
        }, 200);
    }

    function assert(value, description) {
        let result = value ? `pass ${description}` : `fail ${description} `;
        if (value) {
            console.log("%c" + result, "color:" + 'green');
        } else {
            console.error(result);
        }
    }
    //end test sila hasla

});
