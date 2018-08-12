$(document).ready(function () {
   
    $(".tooltipLink").tooltip();

    $('div.alert a.close').on('click', () => {
        $(".alert").alert('close');
    });

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
});