$(document).ready(function () {

    let ladowanie = $("div.ladowanie");
    let widok = 'month';
    ladowanie.css('display', 'block');

    let date = [];
    let count = [];
    $.ajax({
        type: "GET",
        url: "/Home/Chart",
        success: function (data) {
            $.each(data, function (i, v) {
                var pom = moment(v.DataRejestracji).format("DD-MM-YYYY");
                date.push(pom.toString());
                count.push(v.Pacjentow);
            })
            ladowanie.css('display', 'none');
            var dataT = {
                labels: date,
                datasets: [{
                    label: "Pacjętów",
                    data: count,
                    fill: false,
                    backgroundColor: ["rgba(54, 162, 235, 0.2)", "rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    borderWidth: 1
                }]
            };
            var ctx = $("#myChart").get(0).getContext("2d");
            var myNewChart = new Chart(ctx, {
                theme: "theme2",
                animationEnabled: true,
                type: 'bar',
                data: dataT,
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    title: { display: true, text: 'Statyski rejestracji / edycji pacjętów' },
                    scales: {
                        xAxes: [{ gridLines: { display: false }, display: true, scaleLabel: { display: false, labelString: '' } }],
                        yAxes: [{ gridLines: { display: false }, display: true, scaleLabel: { display: false, labelString: '' }, ticks: { stepSize: 5, beginAtZero: true } }]
                    }, scales: {
                        yAxes: [{
                            ticks: {
                                min: 0,
                                stepSize: 5
                            }
                        }]
                    }
                }
            });
        }
    });
    // Define a plugin to provide data labels

    Chart.plugins.register({
        afterDatasetsDraw: function (chart) {
            var ctx = chart.ctx;
            chart.data.datasets.forEach(function (dataset, i) {
                var meta = chart.getDatasetMeta(i);
                if (!meta.hidden) {
                    meta.data.forEach(function (element, index) {
                        // Draw the text in black, with the specified font
                        ctx.fillStyle = 'rgb(0, 0, 0)';
                        var fontSize = 16;
                        var fontStyle = 'normal';
                        var fontFamily = 'Helvetica Neue';
                        ctx.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);
                        // Just naively convert to string for now
                        var dataString = dataset.data[index].toString();
                        // Make sure alignment settings are correct
                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'middle';
                        var padding = 1;
                        var position = element.tooltipPosition();
                        ctx.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                    });
                }
            });
        }
    });
});