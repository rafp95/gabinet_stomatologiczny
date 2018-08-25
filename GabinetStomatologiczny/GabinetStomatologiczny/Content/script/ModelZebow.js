$(function () {

    var tooth = $("polygon,path");

    var tab = ["Tooth5", "Tooth16", "Tooth27", "Tooth31", "Tooth17"];

    tooth.each(function () {
        var id = $(this).attr('id');
        for (let i = 0; i < tab.length; i++) {
            if (tab[i] === id) {
                $(this).css('fill', 'red');
            }
        }
    });

    tooth.each(function (index) {
        $(this).on('click', () => {
            $(this).css('fill', 'red');
        }).on('dblclick', () => {
            $(this).css('fill', '');
        });
    });
});