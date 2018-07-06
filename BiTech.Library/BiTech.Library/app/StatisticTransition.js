$('#slide-two').on('click', function () {
    if ($(this).prop('checked'))
        $('#disableYear').css('display', 'block')
    else
        $('#disableYear').css('display', 'none')
});


$('#slide-two1').on('click', function () {
    if ($(this).prop('checked'))
        $('#disableMonth').css('display', 'block')
    else
        $('#disableMonth').css('display', 'none')
});

