$(function () {
    let pass = $("input#Haslo");
    let checkInfo = $("span#InfoPassword");
    pass.val('Patryk12345!');

    //test element exist
    //end test element exist

    //test password is too short
    //end test password is too short

    QUnit.test("Bardzo słabe", function (assert) {
        assert.ok(checkPasswordStrength(pass.val("p"), checkInfo) === "Bardzo słabe red", "passed");
    });

    QUnit.test("Dobre", function (assert) {
        assert.ok(checkPasswordStrength(pass.val("Patryk"), checkInfo) === "Dobre darkorange", "passed");
    });

    QUnit.test("Bardzo dobre", function (assert) {
        assert.ok(checkPasswordStrength(pass.val("Patryk12"), checkInfo) === "Bardzo dobre green", "passed");
    });


    QUnit.test("Silne", function (assert) {
        assert.ok(checkPasswordStrength(pass.val('Patryk12345!'), checkInfo) === "Silne darkgreen", "passed");
    });
    //Test sila hasla

    function checkPasswordStrength(pass, checkInfo) {

        let wyrazenie = [];
        wyrazenie.push("[A-Z]");
        wyrazenie.push("[a-z]");
        wyrazenie.push("[0-9]");
        wyrazenie.push("[$@$!%*#?&]");
        wyrazenie.push("[ąćęółńżź]");
        wyrazenie.push("[ĄĆĘÓŁŃŻŹ]");

        $('#myProgress').css({ 'display': 'block' });
        if (pass.val().length < 5) {
            move(0, 'red');
            checkInfo.text("Bardzo słabe");
            checkInfo.css("color", "red");
            return `Bardzo słabe red`;
        }

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

        console.log(`${silaHasla} ${kolor}`);
        return `${silaHasla} ${kolor}`;
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
