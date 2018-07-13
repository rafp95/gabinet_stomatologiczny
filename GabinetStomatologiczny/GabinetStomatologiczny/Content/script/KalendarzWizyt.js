$(document).ready(function () {

    //Czas

    pokazAktualnyCzas();

    //Mechanizm autopodpowiedzi
    var setupAutoComplete = function () {
        var input = $("#searchQuery");

        var options = {
            source: input.attr("data-autocomplete-source"),
            select: function (event, ui) {
                input.val(ui.item.label);
                var form = input.parents("form:first");
                form.submit();
            }
        };

        input.autocomplete(options);
    };
    setupAutoComplete();
    //Koniec autopodpowiedzi

    //Czyszczenie

    $("input[type=text]").bind('click keyup', function (event) {
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

        event.preventDefault();
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
    //Akttuana data do sprawdzania czy wprowadzona data nie jest wcześniejsza
    var now = moment(new Date(), "DD-MM-YYYY HH:mm");
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
                $("h2#ladowanie").html("");
                $.each(data, function (i, v) {
                    events.push({
                        pacjentID: v.PacjentID,
                        eventID: v.ID,
                        title: v.Tytul,
                        start: moment(v.Poczatek),
                        end: v.Koniec != null ? moment(v.Koniec) : null
                    });
                })

                GenerateCalender(events);
            },
            error: function (error) {
                alert('failed');
            }
        })
    }
    //End add edit

    //Generate calandar
    function GenerateCalender(events) {
        let widok = 'month';
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            theme: true,
            slotMinutes: 30,
            slotEventOverlap: false,
            nowIndicator: true,
            aspectRatio: () => { size },
            hiddenDays: [0, 6],
            timeFormat: 'HH:mm',
            minTime: "08:00:00",
            maxTime: "17:00:00",
            defaultView: widok,
            selectHelper: true,
            success: function(events)
            {
                $('#calendar').fullCalendar('refetchEvents');
            },
            header: {
                left: 'prev,next,today',
                center: 'title',
                right: 'month,agenda,list'
            },
            eventLimit: 3,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                selectedEvent = calEvent;
                //budowanie modal
                $("#myModal #eventTitle").text(calEvent.title);
                $('#myModal #pDetails').html("<div> <b>Początek : </b> " + calEvent.start.format('HH:mm') + "</div>").
                               append("<div> <b> Koniec: </b> " + calEvent.end.format("HH:mm") + "</div>").
                               append("<div> <b>Data </b> " + calEvent.start.format("DD-MM-YYYY") + "</div>");
                $('#myModal').modal();
                //koniec budowania modal
            },
            selectable: true,
            select: function (start, end) {
                ErorMessage();
                selectedEvent = {
                    eventID: 0,
                    description: '',
                    start: start,
                    end: end,
                };
                var dataStart = new Date(moment(start, "DD-MM-YYYY HH:mm"));
                if (dataStart >= now
                    || start.format("DD-MM-YYYY")
                    === now.format("DD-MM-YYYY")) {
                    openAddEditForm();
                } else {
                    $("#myModalError").modal();
                    return false;
                }

                openAddEditForm();
                $('#calendar').fullCalendar('unselect');
            },
            eventDrop: function (event) {
                var data = {
                    EventID: event.eventID,
                    Start: event.start.format('DD/MM/YYYY HH:mm'),
                    Description: event.description,
                };
                SaveEvent(data);
            }, eventRender: function (eventObj, el) {
                var dataStart = eventObj.start.format("DD-MMM-YYYY HH:mm");
                el.popover({
                    title: eventObj.title,
                    content: dataStart,
                    trigger: 'hover',
                    placement: 'top',
                    container: 'body'
                });
            }
        })
    }
    //End generate calendar

    $('#btnEdit').click(function () {
        //Open modal dialog for edit event
        openAddEditForm();
    })

    //Dete element
    $('#btnDelete').click(function () {
        $("#deleteConfirmModal").modal();
        buttonInfo($("#kasujElement"), "Kasuj");
    })

    $("#kasujElement").click(function () {
        buttonInfo($(this), "Kasowanie......");

        if (selectedEvent != null) {
            $.ajax({
                type: "POST",
                url: '/Wizytas/DeleteEvent',
                data: { 'ID': selectedEvent.eventID },
                success: function (data) {
                    if (data.status) {
                        //Refresh the calender
                        FetchEventAndRenderCalendar();
                        $('#myModal').modal('hide');
                        $("h2#ladowanie").html("Kasowanie ......");
                    }
                },
                error: function () {
                    alert('Failed');
                }
            })
        }
    })
    //End delete element


    //Date time picker
    $('#dtp1,#dtp2').datetimepicker({
        stepping: '30',
        enabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16],
        minDate: new Date(),
        daysOfWeekDisabled: [0, 6]
    });
    //End datetime picker

    function openAddEditForm() {
        //Save button change text
        buttonInfo($("#btnSave"), "Zapisz");
        if (selectedEvent != null) {
            $('#hdEventID').val(selectedEvent.eventID);
            $('#txtStart').val(selectedEvent.start.format('DD.MM.YYYY HH:mm'));
        }
        $('#myModal').modal('hide');
        $('#myModalSave').modal();
    }

    $('#btnSave').click(function () {
        //Validation/
        ErorMessage();
        var error = $("div.error");
        var errorTime = $("div.errorTime");
        var maxTime = moment("17:00", 'HH:mm').format("HH:mm");
        var clickTime = moment($("#txtStart").val().trim(), 'DD.MM.YYYY HH:mm');
        var minTime = moment("8:00", 'HH:mm').format("HH:mm");

        for (var i = 0; i < events.length; i++) {
            if (clickTime.format('DD.MM.YYYY HH:mm') === moment(events[i].start).format('DD.MM.YYYY HH:mm')) {
                $("#txtStart").css('border', '1px solid red').parent().
               after("<div class='errorTime'>Czas zajęty...</div>").next().css("color", "red");
                return;
            }
            
        }

        //Check date and time
        if (new Date(now) <= new Date(clickTime)
            && clickTime.format("HH:mm") >= minTime
            && clickTime.format("HH:mm") < maxTime) {
            $("#txtStart").css('border', '');
            errorTime.html("");
        } else {
            $("#txtStart").css('border', '1px solid red').parent().
               after("<div class='errorTime'>Nietety termin nie dostępny...</div>").next().css("color", "red");
            return;
        }

        var data = {
            ID: $('#hdEventID').val(),
            Poczatek: $("#txtStart").val().trim(),
        }
        buttonInfo($(this), "Zapisywanie.......");
        SaveEvent(data);
        // call function for submit data to the server
    })

    function SaveEvent(data) {
        $.ajax({
            type: "POST",
            url: '/Wizytas/SaveEvent',
            data: data,
            success: function (data) {
                if (data.status) {
                    //Refresh the calender
                    FetchEventAndRenderCalendar();
                    $('#myModalSave').modal('hide');
                    $("h2#ladowanie").html("Dodawanie ......");
                }
            },
            error: function () {
                alert('Failed');
            }
        })
    }

    //Change text in button
    function buttonInfo(button, text) {
        button.text(text);
    }

    //Validate massage
    function ErorMessage() {
        $("div.error").html("");
        $("div.errorTime").html("");
        $("#txtStart").css('border', '');
    }
});

function pokazAktualnyCzas() {
    var aktualny = setInterval(() => {
        var aktualntCzas = moment(new Date());
        $("div#aktualnyCzas").html(aktualntCzas.format("HH:mm:ss"));
    }, 1000);
}

