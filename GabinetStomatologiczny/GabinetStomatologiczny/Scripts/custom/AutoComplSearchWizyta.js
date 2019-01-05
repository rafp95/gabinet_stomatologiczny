(function ($) {

    let setupAutoComplete = function () {
        let input = $("#tytul");

        let options = {
            source: input.attr("data-autocomplete-source"),
            select: function (event, ui) {
                $('#hdPacjentID').val(ui.item.ID);
                input.val(ui.item.item);
                let form = input.parents("form:first");
                form.submit();
            },
            appendTo: "#dtp2"
        };

        input.autocomplete(options);
    };

    setupAutoComplete();
})(jQuery);

