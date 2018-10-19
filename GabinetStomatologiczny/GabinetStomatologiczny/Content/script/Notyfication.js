$(function () {
    // Click on notification icon for show notification
    $('button.noti').click(function (e) {
        e.stopPropagation();
        $('.card-notification-desktop').fadeIn();
        let count = 0;
        count = parseInt($('span.count').html()) || 0;
        //only load notification if not already loaded
        if (count > 0) {
            updateNotification();
        }
        $('span.count', this).html('0');
    })
    // hide notifications
    $('html').click(function () {
        $('.card-notification-desktop').fadeOut();
    })
    // update notification
    function updateNotification() {
        $('#notiContent').empty();
        $('#notiContent').append($('<li>Ladownaie...</li>'));

        $.ajax({
            type: 'GET',
            url: '/Home/GetNotyficationWizytas',
            success: function (response) {
                $('#notiContent').empty();
                if (response.length == 0) {
                    $('#notiContent').append($('<li> Odśwież listę <a href="/Wizytas"> ' + 'wizyt' + '</a>' + '</li>'));
                }
                $.each(response, function (index, value) {
                    $('#notiContent').append($('<li>Zapisał(a) się na ' + "<a href='/Wizytas'>" + ' wizytę : ' + '</a>' + ' ' + value.Tytul + ' '
                        + moment(value.Poczatek).format('DD-MMM-YYYY HH:mm') + '</li>'));
                });
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    // update notification count
    function updateNotificationCount() {
        let count = 0;
        count = parseInt($('span.count').html()) || 0;
        count++;
        $('span.count').html(count);
    }
    // signalr js code for start hub and send receive notification
    var notificationHub = $.connection.notyficationsHub;
    $.connection.hub.start().done(function () {
        console.log('Notification hub started');
    });

    //signalr method for push server message to client
    var lastTime = new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds();
    notificationHub.client.notify = function (message) {
        if (message && message.toLowerCase() === "added") {
            console.log(lastTime != (new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds()));
            if (lastTime != (new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds())) {
                lastTime = new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds();
                updateNotificationCount();
            }
        }
    }

});