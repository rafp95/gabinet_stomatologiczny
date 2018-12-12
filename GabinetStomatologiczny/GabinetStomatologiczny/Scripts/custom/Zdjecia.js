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
    let miniaturka = $(".editOraz");
    let reader = new FileReader();
   
    miniaturka.change(function (event) {
        move();
        let input = event.target;
        let reader = new FileReader();
        reader.onload = function () {
            let dataURL = reader.result;
            let output = $("#miniaturka");
            output.html("<img class='zdjecie' src='" + dataURL + "' />");
        };
        reader.readAsDataURL(input.files[0]);
    });
}

function move() {
    let elem = $('#myBar');
    $('#myProgress').css({ 'display': 'block' });
    let width = 1;
    let id = setInterval(frame, 10);
    function frame() {
        if (width >= 100) {
            clearInterval(id);
        } else {
            width++;
            elem.css({ 'width': width + '%' });
            elem.text(width + '%');
        }
    }
}