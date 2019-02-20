$('.full-overlay .message').css('margin-top',-($('.full-overlay .message').height()/2))
var data = [
    {
        label: 'Africa', id: 1,
        children: [
            {label: 'child1', id: 2, children:[
                {label:'child1.1', id:2.1},
                {label:'child1.2', id:2.2}
            ] },
            { label: 'child2', id: 3 },
            {label:'child3', id:4}
        ]
    },
    {
        label: 'Europe', id: 5,
        children: [
            { label: 'child3', id: 5.1 }
        ]
    },
    {
        label: 'Asia', id: 6,
        children: [
            { label: 'child3', id: 6.1 }
        ]
    },
    {
        label: 'North America', id: 7,
        children: [
            { label: 'child3', id: 7.1 }
        ]
    },
    {
        label: 'South America', id: 8,
        children: [
            { label: 'child3', id: 8.1 }
        ]
    }

];
if($('.tree1').length>0){
    drawTree()
   }
$('.linkBtn').click(function () {
    uploadPhoto()
})
$('.Zone-list .fa-times').click(function(){
    $(this).parent().remove();
})

$(".fileStyle input[type=file]").change(function(){
    readURL(this);
});

if($('select').length>0){
   $("select").selecter() 
}
if ($('.checkbox input').length > 0) { 
    $('.checkbox input').iCheck();
}


//$('.remove').click(showAlertMessage())

function showAlertMessage(){
    $('.full-overlay.warn').show()
}
function hideAlertMessage(){
    $('.full-overlay.warn').hide()
}
function showAConfirmationMessage(){
    $('.full-overlay.confirm').show()
}
function hideAConfirmationMessage(){
    $('.full-overlay.confirm').hide()
}
function drawTree(){
     $('.tree1').tree({
    data: data,
    autoOpen: false,
    dragAndDrop: true,
    closedIcon: '[+]',
    openedIcon: '[+]'
});

}
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.companyLogo img').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
function uploadPhoto(){
    var value = $('.Zone-list input').val();
    var element = '<span class="label col-md-7 label-default">' + value + '<i class="fa fa-times"></i></span>'
    $('.Zone-list').append(element);
}

$('.fa-search').click(function () {
    $('.absoluteInput').css('display','table');
})
$(document).mouseup(function (e)
{
    var container = $(".absoluteInput");

    if (!container.is(e.target) // if the target of the click isn't the container...
        && container.has(e.target).length === 0) // ... nor a descendant of the container
    {
        container.hide();
    }
});
$(document).ready(function () {
    // Create jqxExpander
    if ($('#jqxTree').length > 0) {
        $('#jqxExpander').jqxExpander({ showArrow: false, toggleMode: 'none', width: '100%', height: '350px' });
        // Create jqxTree
        var source = [
            {
                label: "Mail", items: [
                  { label: "Calendar", items:[
                  {label:"item1"},
                  {label:"item2"}
                  ] },
                  { label: "Contacts" }
                ]
            },
            {
                label: "Inbox", items: [
                 { label: "Admin" },
                 { label: "Corporate" },
                 { label: "Finance" },
                 { label: "Other" },
                ]
            },
            { label: "Deleted Items" },
            { label: "Notes" },
            { label: "Settings" },
            { label: "Favorites" }
            ];


        $('#jqxTree').jqxTree({ source: source, height: '100%', width: '100%' });
        // $('#jqxTree').jqxTree('selectItem', null);
        $('#Event').jqxPanel({ height: '30px', width: '200px' });
        $('#jqxTree').on('select', function (event) {

            var args = event.args;
            var item = $('#jqxTree').jqxTree('getItem', args.element);
            //$('#Event').children().html('');
            $('#Event').show();
            $(document).mouseup(function (e)
            {
                var container = $("#Event");

                if (!container.is(e.target) // if the target of the click isn't the container...
                    && container.has(e.target).length === 0) // ... nor a descendant of the container
                {
                    container.hide();
                }
            });

            //<div style="margin-top: 5px;">Selected: ' + item.label + '</div>
        });

    }

    //if($('li.jqx-tree-item-li').children('ul').length>0){
    //     $('.jqx-tree-item').after('<a><i class="fa fa-plus-square"></i></a>')
    // }
    // else{
    //    $('.jqx-tree-item').after('<a><i class="fa fa-plus-square"></i></a>')
    //  $('.jqx-tree-item').after('<a><i class="fa remove fa-times"></i></a>')
    //$('.jqx-tree-item').after('<a><i class="fa fa-pencil"></i></a>')
    // }

})


          $('#coderequired').blur(blurInput);
          $('#descrequired').blur(blurInput)
          function blurInput(){
                if (!$(this).val()) {
                    $(this).parent().addClass('has-error');
                    $(this).next('span').show()
                }
                else{
                    $(this).parent().removeClass('has-error');
                    $(this).next('span').hide()
                }
          }
