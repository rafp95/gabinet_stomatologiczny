(function ($) {


    var setupAutoComplete = function () {
        var input = $("#searchQuery");

        var options = {
            source: input.attr("data-autocomplete-source"),
            select: function (event, ui) {
                input.val(ui.item.item);
                var form = input.parents("form:first");
                form.submit();
            }
        };

        input.autocomplete(options);
    };

    setupAutoComplete();
})(jQuery);

