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
                toothColor(tab);
            }
        });
    }
    //end get all tooth using ajax

    //set tooth
    //
    function SetTooth() {
        tooth.each(function (index) {
            $(this).on('click', () => {
                   $(this).css('fill', 'red');
                   let tooth = {
                      idTooth: $(this).attr('id')
                   };
                 addTooth(tooth);
            }).on('dblclick', () => {
               $(this).css('fill', '');
               let tooth = {
                  idTooth: $(this).attr('id')
               };
               deleteTooth(tooth);
            });
        });
    }
    //end set tooth

    //ajax save tooth to database
    function addTooth(data) {
        $.ajax({
            type: 'GET',
            url: "/Zabiegs/SaveTooth",
            data: data
        });
    }
    //end ajax save tooth to database
    
    //ajax delete tooth
    function deleteTooth(data) {
        $.ajax({
            type: 'GET',
            url: "/Zabiegs/DeleteTooth",
            data: data
        });
    }
    //end ajax delete tooth

    //set tooth color
    function toothColor(tab) {
        tooth.each(function () {
        let id = $(this).attr('id');
        for (let i = 0; i < tab.length; i++) {
            console.log(tab[i].idT);
            if (tab[i].idT === id) {
                $(this).css('fill', 'red');
            }
        }
    });
    }
    //end set tooth color
    
});