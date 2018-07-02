$(document).ready(function wyslij() {
    
    tooltip();

    var szukajL = $(".szukajPacjentAjax");
    szukajL.on('click', function () {
        var pom = $("#searchQuery").val();
        szukajPacjenta(pom);
    });

    $("input[type=text]").bind('click keyup', function (event) {
        szukajPacjenta("");
    });

    czyscText();

});

function tooltip() {
    $("[data-toggle=tooltip]").tooltip();
}

function szukajPacjenta(pom) {
    var searchQuery = $("#searchQuery").val();
    var setData = $(".select-patients");
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
        url: "Pacjents/Index/",
        data: ({ "searchString": searchQuery, "sortOrder": param, "page": page }),
        success: function (data) {
            setData.html(data);
            console.log(data);
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
        if(params[0] == sParam)
        {
            return params[1];
        }
    }
}

function czyscText() {
    var kasuj = $("span.kasujText");
    kasuj.on("click", function () {
        $("#searchQuery").val("");
        szukajPacjenta("");
        kasuj.css("display", "none");
    });

}