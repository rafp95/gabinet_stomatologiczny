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
                            idT: o.IdTooth,
                            dateT: o.TreatmentDate
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
        let clicks = 0, timer = null, deley = 200;
        tooth.off('click').on('click', function (e) {
            e.preventDefault();
            clicks++;
            if (clicks == 1) {
                ladowanie.css('display', 'block');
                timer = setTimeout(() => {
                    let tooth = {
                        idTooth: $(this).attr('id'),
                    };

                    if ($(this).attr('fill') === '#FFFFFF'
                       || $(this).attr('fill') === '#010101') {
                        let color = $(this).attr('fill');
                        console.log(color);
                        $(this).attr('fill', '#FF0000');
                        addTooth(tooth);
                    }

                    $("#myModalZabDescription").modal();
                    $(".zabDesc").text("Opis zęba " + $(this).attr('id'));

                    sendToothDescr(tooth.idTooth);
                    tratmentHistory(tooth);

                    ladowanie.css('display', 'none');
                    clicks = 0;
                }, deley)
            } else {
                clearTimeout(timer);
                let tooth = {
                    idTooth: $(this).attr('id')
                };
                //if (idToohtExist(tooth.idTooth)) {
                console.log(tooth.idTooth.indexOf('XMLID'));
                if ($(this).attr('fill') === '#FF0000') {

                    $("#deleteConfirmModal").modal();
                    deleteConfirm(tooth, $(this));

                }
                ladowanie.css('display', 'none');
                //}
                clicks = 0;
            }
        });

        toothColor(tab);
    }
    //end set tooth
    //delete tooth confirm 
    function deleteConfirm(tooth, elem) {
        let bDelete = $("#deleteConfirm");
        bDelete.off().on('click', function () {
            deleteTooth(tooth);
            if (tooth.idTooth.indexOf('XMLID') >= 0) {
                console.log(tooth.idTooth)
                elem.attr('fill', '#010101');
            } else {
                elem.attr('fill', '#FFFFFF');
            }

        });

    }
    //end delete tooth confirm 
    //tooltip hover
    function ToothHover() {
        tooth.each(function () {
            let color = $(this).attr('fill');
            let info = color === '#FF0000' ? 'dodano ' + moment(getDateTreatment($(this).attr('id')))
                   .format("DD-MM-YYYY HH:mm") : 'brak informacji';
            $(this).tooltip({
                items: $(this),
                trigger: 'hover',
                placement: 'top',
                container: 'body',
                show: null,
                content: () => {
                    return info;
                },
                track: true,
                open: function (event, ui) {
                    if (typeof (event.originalEvent) === 'undefined') {
                        return false;
                    }
                    let $id = $(ui.tooltip).attr('id');
                    // close any lingering tooltips
                    $('div.ui-tooltip').not('#' + $id).remove();
                }
            });
        });
    }
    //tooltip hover
    //ajax save tooth to database
    function addTooth(data) {
        let bufData = {
            idTooth: data.idTooth,
            __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
        }
        $.ajax({
            type: 'POST',
            url: "/Zabiegs/SaveTooth",
            data: bufData,
            success: function (data) {
                console.log(data.status);
                if (data.status) {
                    console.log('Zapisano');
                    $("#deleteConfirmModal").modal('hide');
                    zapisInfo.text("Zapisano pomyślnie ").
                    append('<i class="fa fa-spinner fa-spin"></i>').
                    css("background", "linear-gradient(to bottom, #33cc33 0%, #66ff33 100%)").css('color', 'black').
                    fadeIn('slow').delay(1000).fadeOut('slow');
                }
                ladowanie.css('display', 'none');
            }, error: function () {
                zapisInfo.text("Wystąpił problem z zapisem ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css({
                    "background": "linear-gradient(to top, #800000 0%, #cc3300 100%)",
                    "color" : "white"
                }).
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
                    zapisInfo.text("Zapisano pomyślnie ").
                    append('<i class="fa fa-spinner fa-spin"></i>').
                    css("background", "linear-gradient(to bottom, #33cc33 0%, #66ff33 100%)").css('color', 'black').
                    fadeIn('slow').delay(1000).fadeOut('slow');
                }
            }, error: function () {
                zapisInfo.text("Wystąpił problem z zapisem ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css({
                    "background": "linear-gradient(to top, #800000 0%, #cc3300 100%)",
                    "color" : "white"
                }).
                fadeIn('slow').delay(1000).fadeOut('slow');
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
                    $(this).attr('fill', '#FF0000');
                }
            }
        });
    }
    //end set tooth color
    //idTooth exist
    function idToohtExist(id) {
        for (let i = 0; i < tab.length; i++) {
            if (tab[i].idT === id) {
                return true;
            }
        }
        return false;
    }
    //end idTooth exist
    function getDateTreatment(id, date) {
        for (let i = 0; i < tab.length; i++) {
            if (tab[i].idT === id && tab[i].dateT !== null) {
                return tab[i].dateT;
            }
        }
        return null;
    }
    //end idTooth exist
    //send description tooth
    function sendToothDescr(idTooth) {
        removeValidateStyle();
        czyascInput($("#zabDescription"));

        let buttonSave = $("#btnDescZabSave");

        buttonSave.off().on('click', function () {
            let data = {
                idTooth: idTooth,
                description: $("#zabDescription").val(),
                __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
            };

            if (data.description.length === 0) {
                $("#zabDescriptionError").hide().fadeIn().text("Pole nie może być puste").css('color', 'red');
                $("#zabDescription").fadeIn().css({ 'border': '1px solid red', 'background': 'pink' });
                return;
            }

            $("#myModalZabDescription").modal('hide');
            sendDescription(data);
        });
    }

    function czyascInput(input) {
        input.val("");
    }

    function sendDescription(data) {
        $.ajax({
            url: '/Zabiegs/SaveTooth',
            cache: false,
            type: 'POST',
            data: data,
            success: function () {
                zapisInfo.text("Zapisano pomyślnie ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css("background", "linear-gradient(to bottom, #33cc33 0%, #66ff33 100%)").css('color', 'black').
                fadeIn('slow').delay(1000).fadeOut('slow');
                GetAllTooth();
            }, error: function () {
                zapisInfo.text("Wystąpił problem z zapisem ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css("background", "linear-gradient(to top, #800000 0%, #cc3300 100%)").
                css('color', 'white').
                fadeIn('slow').delay(1000).fadeOut('slow');
            }
        })
    }
    //end send description tooth
    //removeStyle after click on text area
    $("#zabDescription").off().on('click', function () {
        removeValidateStyle();
    });
    //end
    //validate style remove
    function removeValidateStyle() {
        $("#zabDescriptionError").empty().css('color', '');
        $("#zabDescription").fadeIn().css({ 'border': '1px solid black', 'background': '' });
    }
    //tretment history
    function tratmentHistory(id) {
        $("button#btnInfoHitory").off().on('click', function () {
            $("#myModalHistory").modal();
            $("#zabDescription").val("");
            let history = $("#historiaLeczeniaLista");
            getHistoryFromApi(id, history);
        });
    }
    function getHistoryFromApi(data, setText) {
        $.ajax({
            url: '/Zabiegs/GetHistory',
            type: 'GET',
            data: data,
            success: function (data) {
                $("#laoder-div").css('display', 'none');
                setText.empty();
                if (data.length === 0) {
                    setText.append('<li>' + 'Brak historii leczenia' + '</li>');
                    return;
                }
                $.each(data, function (i, o) {
                    setText.append('<li style="margin-top: 5px;">' + o.description + ' ' + moment(o.dateZabieg).format("DD-MM-YYYY HH:mm")
                        + '<div class="buttonAction">'
                        + '<button id="btnDelDescr" class="btn btn-danger btn-sm" title="Kasuj"> '
                        + '<span class="glyphicon glyphicon-remove"></span> <input type="hidden"  value="' + o.id + '" /> '
                        + '</button>'
                        + ' '
                        + '<button id="btnEditDescr" class="btn btn-warning btn-sm" title="Edytuj">'
                        + '<span class="glyphicon glyphicon-pencil"></span>'
                        + ' <input type="hidden" id="editId"  value="' + o.id + '" /> '
                        + ' <input type="hidden" id="editText" value="' + o.description + '" /> '
                        + '</button></div> '
                        + '</li>');
                })
                //delete zab descr
                $("ol li #btnDelDescr").each(function () {
                    $(this).off().on('click', function () {
                        let element = $(this).parent();
                        let id = $(this).children().closest('input').val();
                        let data = {
                            id: id
                        };

                        let modal = $("#deleteConfirmModal");
                        modal.modal();
                        $("#deleteConfirm").off().on('click', function () {
                            if (id) {
                                modal.modal('hide');
                                deleteDescription(data);
                                element.parent().hide();
                            }
                        })
                    })
                })
                //end delete zab descr

                //edit zab descr
                $("ol li #btnEditDescr").each(function () {
                    $(this).off().on('click', function () {
                        removeValidateStyle();

                        let zabDescrText = $("#zabDescription");
                        let id = $(this).children().closest('#editId').val();
                        let description = $(this).children().closest('#editText').val();
                        zabDescrText.val(description);

                        $("#btnDescZabSave").off().on('click', function () {
                            let data = {
                                id: id,
                                description: $("#zabDescription").val(),
                                __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
                            }
                            if (data.description.length === 0) {
                                $("#zabDescriptionError").hide().fadeIn().text("Pole nie może być puste").css('color', 'red');
                                $("#zabDescription").fadeIn().css({ 'border': '1px solid red', 'background': 'pink' });
                                return;
                            }
                            if (data.id > 0) {
                                editDescription(data);
                            }
                            $("#myModalZabDescription").modal('hide');
                        });

                        $("#myModalHistory").modal('hide');
                    })
                });
                //end edit zab descr
            }
        });
    }
    function deleteDescription(id) {
        $.ajax({
            type: 'GET',
            url: '/Zabiegs/DeleteToothDescr',
            data: id,
            success: function () {
                zapisInfo.text("Zapisano pomyślnie ").
                    append('<i class="fa fa-spinner fa-spin"></i>').
                    css("background", "linear-gradient(to bottom, #33cc33 0%, #66ff33 100%)").css('color', 'black').
                    fadeIn('slow').delay(1000).fadeOut('slow');
            }
        })
    }
    function editDescription(data) {
        $.ajax({
            type: 'POST',
            url: '/Zabiegs/SaveTooth',
            data: data,
            success: function (data) {
                if (data) {
                    zapisInfo.text("Zapisano pomyślnie ").
                    append('<i class="fa fa-spinner fa-spin"></i>').
                    css("background", "linear-gradient(to bottom, #33cc33 0%, #66ff33 100%)").css('color', 'black').
                    fadeIn('slow').delay(1000).fadeOut('slow');
                }
            }, error: function () {
                zapisInfo.text("Wystąpił problem z zapisem ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css("background", "linear-gradient(to top, #800000 0%, #cc3300 100%)").
                css('color', 'white').
                fadeIn('slow').delay(1000).fadeOut('slow');
            }
        })
    }
});