$(document).ready(function () {

   // alert('inside');
    // the below code to show place holder in browser not support it
    if (!Modernizr.input.placeholder) {
        $("input").each(function () {
            if ($(this).val() == "" && $(this).attr("placeholder") != "") {
                $(this).val($(this).attr("placeholder"));
                $(this).focus(function () {
                    if ($(this).val() == $(this).attr("placeholder")) $(this).val("");
                });
                $(this).blur(function () {
                    if ($(this).val() == "") $(this).val($(this).attr("placeholder"));
                });
            }
        });
    }
    //
});