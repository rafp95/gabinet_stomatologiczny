$(document).ready(function () {
    powieksznie();
    miniaturka();

});

function powieksznie() {
    var modal = $("#myModal");
    var zdjecie = $("img#myImg");
    var modalImg = $("#img01");
    var text = $("caption");
    var close = $(".close");

    var modalImg = $("#img01");
    zdjecie.on('click', function () {
        modal.css("display", "block");
        modalImg.attr('src', $(this).attr("src"));
    });

    close.on('click', function () {
        modal.css("display", "none");
    });
}

function miniaturka() {
    var miniaturka = $(".editOraz");
    var reader = new FileReader();
    miniaturka.change(function (event) {
        var input = event.target;
        var reader = new FileReader();
        $("#miniaturka").html("<div class='loader'></div>");
        reader.onload = function () {
            var dataURL = reader.result;
            var output = $("#miniaturka");
            output.html("<img class='zdjecie' src='" + dataURL + "' />");
        };
        reader.readAsDataURL(input.files[0]);
    });
}