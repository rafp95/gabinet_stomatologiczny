$(document).ready(function () {

    let zapisInfo = $("div#saveDatabaseInfo");
    let ladowanie = $("div.ladowanie");
    let widok = 'month';
    ladowanie.css('display', 'block');
    //Czas

    pokazAktualnyCzas();
    //Czyszczenie

    $("input[type=text],textarea[type=text]").bind('click keyup', function () {
        ErorMessage();
        szukajDane("");
    });

    czyscText();

    function szukajDane(pom) {
        var searchQuery = $("#searchQuery").val();

        if (pom.length > 0) {
            searchQuery = pom;
        }

        if (searchQuery.length > 0) {
            $("span.kasujText").css("display", "block");
        } else {
            $("span.kasujText").css("display", "none");
        }
    }

    function czyscText() {
        var kasuj = $("span.kasujText");

        kasuj.on("click", function () {
            $("#searchQuery").val("");
            szukajDane("");
            kasuj.css("display", "none");
        });

    }
    //Czyszczenie input

    //Dopasownaie do mobilnych urządzeń
    var size = 0;
    $(window).resize(function () {
        if ($(window).width() < 500) {
            size = 0;
            console.log(size);
        } else {
            size = 2;
            console.log(size);
        }
    });

    //Get all events
    var events = [];
    var selectedEvent = null;
    FetchEventAndRenderCalendar();
    function FetchEventAndRenderCalendar() {
        events = [];
        $.ajax({
            type: "GET",
            url: "/Wizytas/GetEvents",
            success: function (data) {
                $("div.loader").html("");
                $.each(data, function (i, v) {
                    events.push({
                        pacjentID: v.p != null ? v.p.PacjentID : null,
                        eventID: v.ID,
                        title: v.Tytul,
                        start: moment(v.Poczatek),
                        end: v.Koniec != null ? moment(v.Koniec) : null,
                        phone: v.p != null ? v.p.NrTelefonu : null,
                        email: v.p != null ? v.p.Email : null,
                        activeUser: v.p != null ? v.p.activeUser : null,
                        role: v.p != null ? v.p.role : null
                    });
                })
                GenerateCalender(events);
                ladowanie.css('display', 'none');
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //End add edit

    //Generate calandar
    function GenerateCalender(events) {
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            theme: true,
            themeSystem: 'bootstrap4',
            weekNumbers: true,
            navLinks: true,
            slotMinutes: 30,
            slotLabelInterval: 30,
            slotEventOverlap: false,
            nowIndicator: true,
            aspectRatio: () => { size },
            hiddenDays: [0, 6],
            timeFormat: 'HH:mm',
            minTime: "08:00:00",
            maxTime: "17:00:00",
            defaultView: widok,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaDay,agendaFourDay'
            },
            views: {
                agendaFourDay: {
                    type: 'agenda',
                    dayCount: 5,
                    buttonText: '5 dni'
                }
            },
            eventLimit: 3,
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                ErorMessage();
                selectedEvent = calEvent;
                //budowanie modal
                let tel = calEvent.phone != null ? calEvent.phone : " *";
                let createZabiegsForPacjents = calEvent.role === "Lekarz" ?
                    "<a href='/Zabiegs/Create?ID=" + calEvent.pacjentID +
                    "' title='Dodaj zabieg'> " + calEvent.title + "  </a>" :
                    calEvent.title;
                $("#myModal #eventTitle").html(createZabiegsForPacjents);
                $('#myModal #pDetails').html("<div> <b>Początek : </b> " + calEvent.start.format('HH:mm') + "</div>").
                               append("<div> <b> Koniec: </b> " + calEvent.end.format("HH:mm") + "</div>").
                               append("<div> <b>Data </b> " + calEvent.start.format("DD-MM-YYYY") + "</div>").
                               append("<div> <b>Tel:</b> " + tel + "</div>");
                if (calEvent.activeUser === calEvent.email ||
                    calEvent.role === "Administrator" ||
                    calEvent.role === "Lekarz") {
                    $('#btnEdit').show();
                    $('#btnDelete').show();
                } else {
                    $('#btnEdit').hide();
                    $('#btnDelete').hide();
                }

                $('#myModal').modal();
                //koniec budowania modal
            },
            selectable: true,
            select: function (start, end) {
                //if user is not autenticated
                let identity = $('#identity-Info');
                identity = identity.length > 0 ? identity.modal() : "";
                //end if user is not autenticated
                ErorMessage();
                selectedEvent = {
                    eventID: 0,
                    description: '',
                    start: start,
                    end: end,
                };
                var pom = new Date(moment(start, "DD-MM-YYYY").format("YYYY-MM-DD"));
                var now = new Date(moment(new Date(), "DD-MM-YYYY").format("YYYY-MM-DD"));
                console.log(pom - now);
                if ((pom - now) >= 0) {
                    openAddEditForm();
                } else {
                    $("#myModalError").modal();
                    return false;
                }
                $('#calendar').fullCalendar('unselect');
            },
            eventDrop: function (event) {
                var data = {
                    EventID: event.eventID,
                    Start: event.start.format('DD-MMYYYY HH:mm'),
                    Description: event.description,
                };
                SaveEvent(data);
            }, eventRender: function (eventObj, el) {
                var dataStart = eventObj.start.format("DD-MMM-YYYY HH:mm");
                var phone = eventObj.phone;
                el.popover({
                    title: eventObj.title,
                    content: phone == null ? "Dla zalogowanych" : "tel. " + phone,
                    effect: 'slide',
                    trigger: 'hover',
                    placement: 'top',
                    container: 'body',
                    animation: true
                });
            }, dayRender: function (date, cell) {
                $("table>thead.fc-head").addClass("modal-info");
                var date = moment(date, "DD-MM-YYYY HH:mm")
                var now = moment(new Date(), "DD-MM-YYYY HH:mm");
                if (date >= now) {
                    cell.addClass("modal-success");
                } else {
                    cell.addClass("modal-warning");
                }
            }
        })
    }
    //End generate calendar

    //Send sms
    $("#btnSms").off().click(() => {
        czyscSmsInput();
        ErorMessage();
        $("#inputDo").val('+48' + getPhoneNumber());
        $("#inputWiadomosc").val("Przypomnienie o wizycie dnia " + selectedEvent.start.format("DD-MMM-YYYY HH:mm"));
        buttonInfo($("#btnSmsSend"), "Wyślij");
        $("#myModalSendSms").modal();
    });


    $("#btnSmsSend").off().click(() => {

        let data = {
            to: $("#inputDo").val(),
            text: $("#inputWiadomosc").val(),
            __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
        };

        if (data.to.length === 0) {
            $("#inputDo").css({ 'border': '1px solid red', 'background': 'pink' });
            $("#errorSmsTo").text("Pole wymagane").css("color", "red");
            return;
        } else if (data.text.length === 0) {
            $("#inputWiadomosc").css({ 'border': '1px solid red', 'background': 'pink' });
            $("#errorSmsWiadom").text("Pole wymagane").css("color", "red");;
            return;
        }

        sendSmsToApi(data);

        buttonInfo($("#btnSmsSend"), "<div class='btnSmsSend' style='float:left; '>Wysyłanie </div> <div class='loader' style='float:left;'></div>");
        $("#myModalSendSms").modal('hide');
    });

    function czyscSmsInput() {
        $("#inputDo").val("");
        $("#inputWiadomosc").val("");
    }

    function sendSmsToApi(data) {
        $.ajax({
            type: "POST",
            url: '/Wizytas/Sms',
            data: data,
            success: function (data) {
                if (data && data.status) {
                    zapisInfo.html("Wysłano pomyślnie ").fadeIn('slow').
                        append('<i class="fa fa-spinner fa-spin"></i>').
                        delay(1000).fadeOut('slow');
                }
            }, error: function () {
                zapisInfo.html("Wystąpił problem z wysłaniem ").fadeIn('slow').
                        append('<i class="fa fa-spinner fa-spin"></i>').
                    css({ "background": "linear-gradient(to top, #800000 0%, #cc3300 100%)", "color": "white" }).
                    fadeIn('slow').delay(1000).fadeOut('slow');
            }
        });
    }

    function getPhoneNumber() {
        let phone = selectedEvent.phone.trim();
        let pomPhone = '';
        for (var i = 0; i < phone.length; i++) {

            if (phone.charAt(i) != '-') {
                pomPhone += phone.charAt(i);
            }
        }
        return pomPhone;
    }

    //End send sms

    //Send email
    $("#btnEmail").off().click(function () {
        czyscEamilInput();
        ErorMessage();

        $("#inputDoEmail").val(selectedEvent.email);
        $("#inputWiadomoscEamil").val("Przypomnienie o wizycie " + selectedEvent.start.format("DD-MMM-YYYY HH:mm"));

        $("#myModalSendEmail").modal();
    });
    function czyscEamilInput() {
        $("#inputDoEmail").val("");
        $("#inputWiadomoscEamil").val("");
    }
    $("#btnEmailSend").click(function () {
        let data = {
            to: $("#inputDoEmail").val(),
            text: $("#inputWiadomoscEamil").val(),
            __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
        };

        if (data.to.length === 0) {
            $("#inputDoEmail").css({ 'border': '1px solid red', 'background': 'pink' });
            $("#errorEmailTo").hide().fadeIn().text("Pole wymagane").css("color", "red");
            return;
        } else if (data.text.length === 0) {
            $("#inputWiadomoscEamil").css({ 'border': '1px solid red', 'background': 'pink' });
            $("#errorEamilWiadom").hide().fadeIn().text("Pole wymagane").css("color", "red");;
            return;
        }
        $("#myModalSendEmail").modal('hide');
        sendEmailToApi(data);
    });

    function sendEmailToApi(data) {
        $.ajax({
            type: 'POST',
            url: 'Wizytas/SendEmail',
            data: data,
            success: function (data) {
                if (data && data.status) {
                    zapisInfo.html("Wysłano pomyślnie ").fadeIn('slow').
                        append('<i class="fa fa-spinner fa-spin"></i>').
                        delay(1000).fadeOut('slow');
                }
            },
            error: function () {
                zapisInfo.html("Wystąpił problem z wysłaniem ").fadeIn('slow').
                        append('<i class="fa fa-spinner fa-spin"></i>').
                    css({ "background": "linear-gradient(to top, #800000 0%, #cc3300 100%)", "color": "white" }).
                    fadeIn('slow').delay(1000).fadeOut('slow');
            }
        });
    }
    //End send email

    $('#btnEdit').off().click(function () {
        //Open modal dialog for edit event
        openAddEditForm();
    })

    //Dete element
    $('#btnDelete').off().click(function () {
        $("#deleteConfirmModal").modal();
        buttonInfo($("#kasujElement"), "Kasuj");
    })

    $("#kasujElement").off().click(function () {
        buttonInfo($(this), "<div clsss='saveText' style='float:left; '>Kasowanie </div> <div class='loader' style='float:left;'></div>");
        if (selectedEvent != null) {
            $.ajax({
                type: "POST",
                url: '/Wizytas/DeleteEvent',
                data: {
                    'ID': selectedEvent.eventID,
                    __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
                },
                success: function (data) {
                    if (data && data.status) {
                        //Refresh the calender
                        FetchEventAndRenderCalendar();
                        zapisInfo.html("Zapisano zmiany ").fadeIn('slow').
                        append('<i class="fa fa-spinner fa-spin"></i>').
                        delay(1000).fadeOut('slow');
                        $('#myModal').modal('hide');
                    }
                },
                error: function () {
                    console.log('error');
                    $('#myModal').modal('hide');
                    zapisInfo.text("Wystąpił problem z zapisem do bazy danych ").
                    append('<i class="fa fa-spinner fa-spin"></i>').
                    css({ "background": "linear-gradient(to top, #800000 0%, #cc3300 100%)", "color": "white" }).
                    fadeIn('slow').delay(1000).fadeOut('slow');
                }
            })
        }
    })
    //End delete element


    //Date time picker
    $('#dtp1').datetimepicker({
        stepping: '30',
        enabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16],
        minDate: new Date(),
        daysOfWeekDisabled: [0, 6],
        sideBySide: false,
        showClose: true,
        widgetPositioning: {
            horizontal: 'right',
            vertical: 'bottom'
        }
    });
    //End datetime picker

    function openAddEditForm() {
        //Save button change text
        buttonInfo($("#btnSave"), "Zapisz");
        if (selectedEvent != null) {
            $('#hdEventID').val(selectedEvent.eventID);
            $('#txtStart').val(selectedEvent.start.format('DD.MM.YYYY HH:mm'));
            $('#tytul').length > 0 ? $('#tytul').val(selectedEvent.title) : "";
        }
        $('#myModal').modal('hide');
        $('#myModalSave').modal();
    }

    $('#btnSave').off().click(function () {
        //Validation/
        ErorMessage();
        var error = $("div.error");
        var errorTime = $("div.errorTime");
        var maxTime = moment("17:00", 'HH:mm').format("HH:mm");
        var clickTime = moment($("#txtStart").val().trim(), 'DD.MM.YYYY HH:mm');
        var minTime = moment("8:00", 'HH:mm').format("HH:mm");

        //check hours is empty
        for (var i = 0; i < events.length; i++) {
            if (clickTime.format('DD.MM.YYYY HH:mm') === moment(events[i].start).format('DD.MM.YYYY HH:mm')) {
                $("#txtStart").css({ 'border': '1px solid red', 'background': 'pink' }).parent().
                after("<div class='errorTime'>Czas zajęty...</div>").next().css("color", "red").hide().fadeIn();
                return;
            }

        }
        //end check hours

        //Check date and time
        var now = moment(new Date(), "DD-MM-YYYY HH:mm");
        var check = (moment(new Date(now), "DD-MM-YYYY HH:mm") <
            moment(new Date(clickTime), "DD-MM-YYYY HH:mm") &&
            (clickTime.format("HH:mm") >= minTime &&
             clickTime.format("HH:mm") < maxTime)) ?
        true : false;

        if (!check) {
            $("#txtStart").css({ 'border': '1px solid red', 'background': 'pink' }).parent().
                  after("<div class='errorTime'>Nietety termin nie dostępny...</div>").next().css("color", "red").hide().fadeIn();
            return;
        }
        //End check date
        //Check title is empty
        var tytul = $("#tytul");
        if (tytul.length > 0) {
            if (tytul.val().length == 0) {
                tytul.css({ 'border': '1px solid red', 'background': 'pink' }).parent().
                after("<div class='errorTime'>Pole nie może być puste</div>").next().css("color", "red").hide().fadeIn();
                return;
            }
        }
        //end check title is empty
        var data = {
            ID: $('#hdEventID').val(),
            Poczatek: $("#txtStart").val().trim(),
            Tytul: tytul.length > 0 ? tytul.val().trim() : null,
            __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
        }
        buttonInfo($(this), "<div clsss='saveText' style='float:left; '>Zapisywanie </div> <div class='loader' style='float:left;'></div>");
        SaveEvent(data);
        // call function for submit data to the server
    })

    function SaveEvent(data) {
        $.ajax({
            type: "POST",
            url: '/Wizytas/SaveEvent',
            data: data,
            success: function (data) {
                if (data && data.status) {
                    console.log(status);
                    //Refresh the calender
                    FetchEventAndRenderCalendar();
                    zapisInfo.html("Zapisano zmiany ").append('<i class="fa fa-spinner fa-spin"></i>').
                    fadeIn('slow').delay(1000).fadeOut('slow');
                    $('#myModalSave').modal('hide');
                } else if (data && !data.status) {
                    //$('#myModalSave').modal('hide');
                    $("#btnSave>div.loader").css({ 'display': 'none' });
                    $("#myModalSaveToDataBaseError").modal();
                }
            },
            error: function (jqXHR, exception) {
                console.log('error ' + jqXHR.status);
                $('#myModalSave').modal('hide');
                zapisInfo.text("Wystąpił problem z zapisem do bazy danych ").
                append('<i class="fa fa-spinner fa-spin"></i>').
                css({ "background": "linear-gradient(to top, #800000 0%, #cc3300 100%)", "color": "white" }).
                fadeIn().delay(1000).fadeOut();
            }
        })
    }

    //Change text in button
    function buttonInfo(button, text) {
        button.html(text);
    }

    //Validate massage
    function ErorMessage() {
        $("#errorSmsTo").html("");
        $("#errorSmsWiadom").html("");
        $("#errorEmailTo").html("");
        $("#errorEamilWiadom").html("");
        $("div.error").html("");
        $("div.errorTime").html("");
        $("div#errorSms").html("");

        $("#inputDo").css({ 'background': '', 'border': '' });
        $("#inputWiadomosc").css({ 'background': '', 'border': '' });
        $("#inputDoEmail").css({ 'background': '', 'border': '' });
        $("#inputWiadomoscEamil").css({ 'background': '', 'border': '' });


        $("#txtStart").css('border', '');
        $("#txtStart").css('background', '');
        $("#tytul").css('border', '');
        $("#tytul").css('background', '');
    }


});

function pokazAktualnyCzas() {
    var aktualny = setInterval(() => {
        var aktualntCzas = moment(new Date());
        $("div#aktualnyCzas").html(aktualntCzas.format("DD MMMM YYYY HH:mm:ss"));
    }, 1000);
}

