$(document).ready(function wyslij() {
    
    let ladowanie = $("div.ladowanie");

    tooltip();

    //Pacjent
    var szukajL = $(".szukajPacjentAjax");
    szukajL.on('click', function () {
        var pom = $("#searchQuery").val();
        SzukajDane(pom, "Pacjents/Index/"); 
    });


    $(".pacjent-query").bind('click keyup', function (event) {
        SzukajDane("", "Pacjents/Index/");
    });

    //End pacjent

    //Lekarz
    var szukaj = $(".szukajLekarzaAjax").on('click', function () {
        var pom = $("#searchQuery").val();
        SzukajDane(pom,"Lekarzs/Index/");
    });

    $(".lekarz-query").bind('click keyup', function (event) {
        SzukajDane("", "Lekarzs/Index/");
    });

    //end lekarz



    czyscText();


    function tooltip() {
        $("[data-toggle=tooltip]").tooltip({
            placment: 'top'
        });
    }

    function SzukajDane(pom,akcja) {
        ladowanie.css("display", "block");
        var searchQuery = $("#searchQuery").val();
        var setData = $(".data-select");
        var param = getUrlParams("sortOrder");
        var page = getUrlParams("page");

        if (pom.length > 0) {
            searchQuery = pom;
        }

        if (searchQuery.length > 0) {
            $("span.kasujText").css("display", "block");
        } else {
            $("span.kasujText").css("display", "none");
        }

        $.ajax({
            type: "GET",
            url: akcja,
            data: ({ "searchString": searchQuery, "sortOrder": param, "page": page }),
            success: function (data) {
                setData.html(data);
                console.log(data);
                ladowanie.css("display", "none");
            }, statusCode: {
                404: function () {
                    alert("Nie ma takiej strony");
                }
            }
        });
        event.preventDefault();
    }

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

    }

});
