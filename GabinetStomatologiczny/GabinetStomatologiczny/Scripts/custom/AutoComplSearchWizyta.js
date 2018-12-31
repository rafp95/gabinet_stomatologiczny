(function ($) {

    let setupAutoComplete = function () {
        let input = $("#tytul");

        let options = {
            source: input.attr("data-autocomplete-source"),
            select: function (event, ui) {
                input.val(ui.item.item);
                let form = input.parents("form:first");
                form.submit();
            }
        };

        input.autocomplete(options);
    };

    setupAutoComplete();
})(jQuery);

