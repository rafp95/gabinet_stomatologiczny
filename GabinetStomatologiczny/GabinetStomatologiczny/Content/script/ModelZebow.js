$(function () {
    let ladowanie = $("div.ladowanie");
    let zapisInfo = $("div#saveDatabaseInfo");

    ladowanie.css('display', 'block');

    let tooth = $("polygon,path");
    //Get all tooth using ajax
    let tab = [];
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
                console.log('pobrano tab ' + tab.length);
                SetTooth();
                ToothHover();
                ladowanie.css('display', 'none');
            }
        });
    }
    //end get all tooth using ajax

    //set tooth
    //
    function SetTooth() {
        let clicks = 0, timer = null, deley = 500;
        tooth.each(function (index) {
            $(this).on('click', function (e) {
                clicks++;
                console.log('kliknięcia ' + clicks);
                ladowanie.css('display', 'block');
                if (clicks == 1) {
                    timer = setTimeout(() => {
                        let tooth = {
                            idTooth: $(this).attr('id')
                        };
                        $(this).attr('fill', 'red');
                        addTooth(tooth);
                        ladowanie.css('display', 'none');
                        clicks = 0;
                        e.stopPropagation();
                    }, deley)
                } else {
                    let tooth = {
                        idTooth: $(this).attr('id')
                    };
                    //if (idToohtExist(tooth.idTooth)) {
                    console.log(tooth.idTooth.indexOf('XMLID'));
                    if (tooth.idTooth.indexOf('XMLID') >= 0) {
                        console.log(tooth.idTooth)
                        $(this).attr('fill', 'black');
                    } else {
                        $(this).attr('fill', 'white');
                    }
                    deleteTooth(tooth);
                    ladowanie.css('display', 'none');
                    //}
                    clearTimeout(timer);
                    clicks = 0;
                    e.stopPropagation();
                }
            });
        });
        toothColor(tab);
    }
    //end set tooth
    //tooltip hover
    function ToothHover() {
        tooth.each(function (e) {
            event.preventDefault();
            let color = $(this).attr('fill');
            let info = color === 'red' ? 'Ząb leczony' : 'ząb zdrowy';
            $(this).tooltip({ 
                items: $(this),
                content: () => { return info },
                track: true,
                my: "left top+20 center",
                at: "right center"
            });
        });
    }
    //tooltip hover
    //ajax save tooth to database
    function addTooth(data) {
        $.ajax({
            type: 'GET',
            url: "/Zabiegs/SaveTooth",
            data: data,
            success: function (data) {
                if (data.status) {
                    console.log('Zapisano');
                    GetAllTooth();
                }
                ladowanie.css('display', 'none');
            }, error: function () {
                zapisInfo.text("Wystąpił problem z zapisem do bazy danych ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css("background", "linear-gradient(to top, #800000 0%, #cc3300 100%)").
                fadeIn('slow').delay(1000).fadeOut('slow');
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
                if (data.status) {
                    console.log('Zapisano');
                    GetAllTooth();
                }
            }, error: function () {
                zapisInfo.text("Wystąpił problem z zapisem do bazy danych ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css("background", "linear-gradient(to top, #800000 0%, #cc3300 100%)").
                fadeIn('slow').delay(1000).fadeOut('slow');
            }
        });
    }
    //end ajax delete tooth

    //set tooth color
    function toothColor(tab) {
        tooth.each(function () {
            $(this).append('<div class="tooltip"><span class="tooltiptext">Tooltip text</span></div>');
            let id = $(this).attr('id');
            for (let i = 0; i < tab.length; i++) {
                if (tab[i].idT === id) {
                    $(this).attr('fill', 'red');
                }
            }
        });
    }
    //end set tooth color
    //idTooth exist
    function idToohtExist(id) {
        for (var i = 0; i < tab.length; i++) {
            if (tab[i].idT === id) {
                return true;
            }
        }
        return false;
    }
    //end idTooth exist
});