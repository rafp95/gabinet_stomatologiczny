$(function () {

    var tooth = $("polygon,path");
    //Get all tooth using ajax
    var tab = [];
    GetAllTooth();
    function GetAllTooth() {
        tab = [];
        $.ajax({
            type: "GET",
            url: "/Zabiegs/GetAllTooth",
            success: function (data) {
                $.each(data, function (i, v) {
                    $.each(v, function (i, o) {
                        tab.push({
                            idT: o.IdTooth
                        });
                    });
                })
                SetTooth();
            }
        });
    }
    //end get all tooth using ajax

    //set tooth
    //
    function SetTooth() {
        let clicks = 0, timer = null, deley = 300;
        tooth.each(function (index) {
            $(this).on('click', function (e) {
                clicks++;
                if (clicks == 1) {
                    timer = setTimeout(() => {
                        $(this).css('fill', 'red');
                        let tooth = {
                            idTooth: $(this).attr('id')
                        };
                        addTooth(tooth);
                        clicks = 0;
                    }, deley)
                } else {
                    $(this).css('fill', '');
                    let tooth = {
                        idTooth: $(this).attr('id')
                    };
                    deleteTooth(tooth);
                    clearTimeout(timer);
                    clicks = 0;
                }
            });
        });
        toothColor(tab);
    }
    //end set tooth

    //ajax save tooth to database
    function addTooth(data) {
        $.ajax({
            type: 'GET',
            url: "/Zabiegs/SaveTooth",
            data: data,
            success: function (data) {
                console.log(data.status);
            }
        });
    }
    //end ajax save tooth to database

    //ajax delete tooth
    function deleteTooth(data) {
        $.ajax({
            type: 'GET',
            url: "/Zabiegs/DeleteTooth",
            data: data,
            success: function (data) {
                console.log(data.status);
            }
        });
    }
    //end ajax delete tooth

    //set tooth color
    function toothColor(tab) {
        tooth.each(function () {
            let id = $(this).attr('id');
            for (let i = 0; i < tab.length; i++) {
                if (tab[i].idT === id) {
                    $(this).css('fill', 'red');
                }
            }
        });
    }
    //end set tooth color

});