$(document).ready(function () {
   
    $('#txtSearch').keypress(function (event) {       
        var validator = $('#btnSearch').attr('data-validation');
        if (validator != null) {
            $('#' + validator).css({ 'display': 'none' });
        }
        if (event.keyCode == 13) {
            debugger;
            $('#btnSearch').click();
            $('#txtSearch').focus();
        }
    });
    /////
    $('#btnSearch').on('click', function (event) {

        debugger;
        var id = event.target.id;
        var url = $(this).attr('data-url');
        var container = $(this).attr('data-container');
        var validator = $(this).attr('data-validation');
        var searchText = $('#txtSearch').val();
        if (searchText.length > 0)
        {
            searchText = searchText.trim();
        }
        // custom paramter
        var customParm = $(this).attr('data-customparm');
        var customParmValues = getCustomParms(customParm);
        //
        var pageSize = 10;
        if (document.getElementById("SelectPageSize") != null) {
            pageSize = document.getElementById('SelectPageSize').value;
        }
        if (validator != null && searchText.length < 2 && searchText.length != 0) {
            $('#' + validator).css({ 'display': 'block' });
            return false;
        }
        $.ajax({
           
            url: url + "?searchText=" + encodeURIComponent(searchText) + "&pageSize=" + pageSize + '&pageNumber=1' + customParmValues,
            type: 'Get',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#' + container).html(data);
                $('#txtSearch').focus();
            },
            error: function () {
            }
        });
    });
    /////
    $('#SelectPageSize').change(function (event) {
        var id = event.target.id;
        var url = $(this).attr('data-url');
        var container = $(this).attr('data-container');
        var searchText = $(this).attr('data-search');
        if (searchText.length > 0)
        {
            searchText = searchText.trim();
        }
        // custom paramter
        var customParm = $(this).attr('data-customparm');
        var customParmValues = getCustomParms(customParm);
        //
        var sortOrder = $(this).attr('data-sort');
        var pageSize = 10;
        if (document.getElementById("SelectPageSize") != null) {
            pageSize = document.getElementById('SelectPageSize').value;
        }
        $.ajax({
            url: url + "?searchText=" + encodeURIComponent(searchText) + "&pageSize=" + pageSize + '&pageNumber=1' + '&sortOrder=' + sortOrder + customParmValues,
            type: 'Get',
            async: false,
            cache: false,
            //data: { 'SearchText': searchText, 'pageSize': pageSize,'sortOrder': sortOrder,'pageNumber':1 },
            success: function (data) {
                $('#' + container).html(data);
            },
            error: function () {
               
            }
        });
    });
});
/////
function gotoPage(pageNumber,pagesize) {
    var pager = $('#pager');
    if (pager == null) return;
    var url = pager.attr('data-url');
    var container = pager.attr('data-container');
    var searchText = pager.attr('data-search');
    var sortOrder = pager.attr('data-sort');
    // custom paramter
    var customParm = pager.attr('data-customparm');
    var customParmValues = getCustomParms(customParm);
    //
    var pageSize = pager.attr('data-page-size');
    if (pageSize == '') {
        pageSize = pagesize;
    }
    
    $.ajax({
        url: url + "?pageSize=" + pageSize + "&PageNumber=" + pageNumber + "&searchText=" + encodeURIComponent(searchText) + "&sortOrder=" + sortOrder + customParmValues,
        type: 'Get',
        async: false,
        cache: false,
        success: function (data) {
            $('#' + container).html(data);
        },
        error: function () {

        }
    });
}
/////
function getCustomParms(customParm)
{
    if (customParm == null) return "";
    var keys = customParm.split(",");
    var keysWithValues
    for (var i = 0; i < keys.length; i++) {
        keysWithValues = "&" + keys[i].replace("|", "=");
    }
    return keysWithValues;
}

