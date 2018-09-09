$(document).ready(function () {
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

    //add close alert
    $('div.alert a.close').on('click', () => {
        $(".alert").alert('close');
    });
    //end add close alert

    //back to top page
    let top = $('#back-to-top');
    $(window).scroll(() => {
        if ($(this).scrollTop() > 50) {
            top.fadeIn(300);
        } else {
            top.fadeOut(300);
        }
    });
    // scroll body to 0px on click
    top.click((e) => {
        e.preventDefault();
        $('body,html').animate({
            scrollTop: 0
        }, 700);
        return false;
    });
    //end back to top page
});