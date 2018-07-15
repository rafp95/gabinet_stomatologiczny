$(document).ready(function wyslij() {

    let ladowanie = $("div.ladowanie");

    tooltip();
    var szukaj = $(".szukajLekarzaAjax").on('click', function () {
        var pom = $("#searchQuery").val();
        szukajDane(pom);
    });

    $("input[type=text]").bind('click keyup', function (event) {
        szukajDane("");
    });

    czyscText();

    function tooltip() {
        $("[data-toggle=tooltip]").tooltip();
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

    function szukajDane(pom) {
        ladowanie.css("display", "block");
        var searchQuery = $("#searchQuery").val();
        var setData = $(".lekarzs-select");
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
            url: "Lekarzs/Index/",
            data: ({ "searchString": searchQuery, "sortOrder": param, "page": page }),
            success: function (data) {
                setData.html(data);
                ladowanie.css("display", "none");
            }, statusCode: {
                404: function () {
                    console.log("Nie ma takiej strony");
                }
            }

        });
        event.preventDefault();
    }


    function czyscText() {
        var kasuj = $("span.kasujText");

        kasuj.on("click", function () {
            $("#searchQuery").val("");
            szukajDane("");
            kasuj.css("display", "none");
        });


    }

});

