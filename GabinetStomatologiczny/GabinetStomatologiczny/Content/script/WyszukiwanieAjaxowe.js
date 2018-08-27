$(document).ready(function wyslij() {
    
    let ladowanie = $("div.ladowanie");

    //Pacjent
    var szukajL = $(".szukajPacjentAjax");
    szukajL.on('click', function () {
        var pom = $("#searchQuery").val();
        SzukajDane(pom, "Pacjents/"); 
    });

    
    $(".pacjent-query").bind('click keyup', function (event) {
        var pom = $("#searchQuery").val();
        showAndHideClearText(pom);
    });
    

    //End pacjent

    //Lekarz
    var szukaj = $(".szukajLekarzaAjax").on('click', function () {
        var pom = $("#searchQuery").val();
        SzukajDane(pom,"Lekarzs/Index/");
    });

    
    $(".lekarz-query").bind('click keyup', function (event) {
        var pom = $("#searchQuery").val();
        showAndHideClearText(pom);
    });
    

    //end lekarz

    //zabieg

    var szukaj = $(".szukajZabiegAjax").on('click', function () {
        var pom = $("#searchQuery").val();
        SzukajDane(pom, "Zabiegs/Index/");
    });

    
    $(".zabieg-query").bind('click keyup', function (event) {
        var pom = $("#searchQuery").val();
        showAndHideClearText(pom);
    });
    

    //end zabieg

    //clear text with input
    czyscText();

    //end clear text with input

    //method using ajax send data to controller
    function SzukajDane(pom,akcja) {
        ladowanie.css("display", "block");
        var searchQuery = $("#searchQuery").val();
        var setData = $(".data-select");
        var param = getUrlParams("sortOrder");
        var page = getUrlParams("page");
        
        showAndHideClearText(pom);

        $.ajax({
            type: "GET",
            url: akcja,
            data: ({ "searchString": searchQuery, "sortOrder": param, "page": page }),
            success: function (data) {
                setData.html(data);
                console.log(data);
                ladowanie.css("display", "none");
                history.pushState({}, "", "");
                history.pushState({}, "", "?sortOrder=" + param + "&" + "currentFilter=" + searchQuery);
            }, statusCode: {
                404: function () {
                    alert("Nie ma takiej strony");
                }
            }
        });
    }
    //end method using ajax send data to controller

    //icon disable text
    function showAndHideClearText(pom) {
            if (pom.length > 0) {
                $("span.kasujText").css("display", "block");
            } else {
                $("span.kasujText").css("display", "none");
            }
    }
    //end disable icon clear text

    //to param with url
    function getUrlParams(sParam) {
        var pageUrl = window.location.search.substring(1);
        var urlVar = pageUrl.split("&");
        for (var i = 0; i < urlVar.length; i++) {
            var params = urlVar[i].split('=');
            if (params[0] == sParam) {
                return params[1];
            }
        }
    }
    //end param with url

    //clear input
    function czyscText() {
        var kasuj = $("span.kasujTextPacjent");
        kasuj.on("click", function () {
            $("#searchQuery").val("");
            SzukajDane("", "Pacjents/Index/");
            kasuj.css("display", "none");
        });

        var kasuj = $("span.kasujTextLekarz");
        kasuj.on("click", function () {
            $("#searchQuery").val("");
            SzukajDane("", "Lekarzs/Index/");
            kasuj.css("display", "none");
        });

        var kasuj = $("span.kasujTextZabieg");
        kasuj.on("click", function () {
            $("#searchQuery").val("");
            SzukajDane("", "Zabiegs/Index/");
            kasuj.css("display", "none");
        });

    }
    //end clear input

});
