$(document).ready(function () {
    powieksznie();
    miniaturka();
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

            let input = event.target;
            let reader = new FileReader();
            reader.onprogress = move;
            reader.onload = function () {
                let dataURL = reader.result;
                let output = $("#miniaturka");
                output.hide().fadeIn('slow').html("<img class='zdjecie' src='" + dataURL + "' />");
            };
            reader.readAsDataURL(input.files[0]);
        });
    }

    function move(e) {
        let elem = $('#myBar');
        if (e.lengthComputable) {
            let width = 0;
            let timer = setInterval(slide, 10);
            function slide() {
                width++;
                if (width > 100) {
                    clearInterval(timer);
                } else {
                    $('#myProgress').fadeIn('slow');
                    elem.css({ 'width': width + '%' });
                    elem.text(width + '%');
                }
            }
        }
    }
});
