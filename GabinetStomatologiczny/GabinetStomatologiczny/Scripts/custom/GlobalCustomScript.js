$(document).ready(function () {
    $('div.main-loader').fadeOut('slow');
    //carpet menu rotation
    let caret = $(".caret");
    $("ul.nav a.dropdown-toggle").off().on('click', function (event) {
        event.preventDefault();

        if (!caret.hasClass('rotate-180')) {
            caret.toggleClass('rotate-180');
        }
    });
    $("ul.nav li.dropdown").off().on('mouseleave', function (event) {
        event.preventDefault();

        if (caret.hasClass('rotate-180')) {
            caret.toggleClass('rotate-180');
        }
    });

    //end carpet menu rotation

    //tooltip
    $(".tooltipLink").tooltip({
        position: {
            my: "center-2 bottom-10",
            at: "center-2 top",
            using: function (position, feedback) {
                $(this).css(position);
                $("<div>")
                  .addClass("arrow")
                  .addClass(feedback.vertical)
                  .addClass(feedback.horizontal)
                  .appendTo(this);
            }
        }
    });
    //end tooltip
    /*
    //add close alert
    $('div.alert a.close').on('click', () => {
        $(".alert").alert('close');
    });
    //end add close alert
    */
    //back to top page
    let top = $('#back-to-top');
    $(window).scroll(function (event) {
        event.preventDefault();

        if ($(this).scrollTop() > 50) {
            top.fadeIn(100);
        } else {
            top.fadeOut(100);
        }
    });
    // scroll body to 0px on click
    top.off('click').on('click', function (e) {
        e.preventDefault();
        $('html,body').animate({
            scrollTop: 0
        }, 700);
        return false;
    });
    //end back to top page
    //decteded connection is online
    let checkStatus = () => {
        let con = ((navigator.onLine) ?
            "online" : "offline");
        if (con === "online") {
            $("div#status").empty();
        } else if (con === "offline") {
            $("div#status").html('<div class="alert alert-danger text-center" role="alert"><strong>Brak połączenia!</strong> z siecią '
                + 'trwa próba połączenia <i class="fa fa-spinner fa-spin"></i>' + '</div>');
        }
    }
    checkStatus();
    window.addEventListener("online", () => {
        checkStatus();
    });
    console.log(window);
    window.addEventListener("offline", () => {
        checkStatus();
    });

    //end dected connection is online
});