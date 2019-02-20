

$(document).ready(function () {
    
   
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
        $('a.delete').click(OnDeleteClick);
});

//var replaceDigits = function () {
//    var map =
//                [
//                "&\#1632;", "&\#1633;", "&\#1634;", "&\#1635;", "&\#1636;",
//                "&\#1637;", "&\#1638;", "&\#1639;", "&\#1640;", "&\#1641;"
//                ]

//    document.body.innerHTML =
//        document.body.innerHTML.replace(
//            /\d(?=[^<>]*(<|$))/g,
//            function ($0) { return map[$0] }
//        );
//}

function getUrlVars(url) {
    var vars = {};
    var parts = url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}



function OnDeleteClick(e) {
   
    var Id = e.target.id;
    var url = document.getElementById(Id).href;
    var SpURL ="";
    var Message = "";
    var Contname = "";
    var Mthodname = "";
    var page = 0;
   
    Message = decodeURIComponent(getUrlVars(url)["message"]);
    page = decodeURIComponent(getUrlVars(url)["page"]);
    Contname = getUrlVars(url)["ControllarName"];
    Mthodname = getUrlVars(url)["Methodname"];

    var flag = confirm(Message);
    if (flag) {
        var _url = '/' + Contname + '/' + Mthodname + '/id/' + Id
        window.location.href = _url;
        return;
        $.ajax({

            url: '/'+Contname +'/'+ Mthodname,

            type: 'POST',

            data: { id: Id },

            dataType: 'json',
         
            success: function (result) {
                toastr.success(result);
                //check for the count of rows including the header
                if ((result.toString().indexOf("Deleted successfully") > 0 || result.toString().indexOf("بنجاح") > 0) && $("#" + Id).parent().parent().siblings().length <= 1) {

                    $("#" + Id).parent().parent().parent().remove();
                }
                else if(result.toString().indexOf("Deleted successfully") > 0 || result.toString().indexOf("بنجاح") > 0)
                    $("#" + Id).parent().parent().remove();
                window.location.reload();
            },

            error: function () { toastr.error('Error!'); }

        });

    }




    return false;

}
function alertMsg(msg)
{
    toastr.info(msg);
}

