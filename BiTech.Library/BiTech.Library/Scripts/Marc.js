var controllerMarc = function () {
    this.initialize = function () {
        registerEvents();
        if ($('.AddList:checked').length === $('#tblFunction tbody tr .AddList').length) {
            $('#CheckAll').prop('checked', true);
        }
    }

    function registerEvents() {
        //thêm xóa từng item trong session
        $('.AddList').change(function () {
            if ($(this).is(":checked")) {
                var idCheck = $(this).val();

                $('.hidden').addClass('pointer-eventsNone');
                AddList(idCheck, true);

            }
            else {
                var removeIdCheck = $(this).val();
                deleteItem(removeIdCheck);
            }

            //check tổng số con == cha thì check cha
            if ($('.AddList:checked').length === $('#tblFunction tbody tr .AddList').length) {
                $('#CheckAll').prop('checked', true);
            } else {
                $('#CheckAll').prop('checked', false);
            }
        });

        $('#CheckAll').on('click', function () {
            if ($('.AddList').is(':checked')) {
                DeleteAll();
                $(this).prop('checked', false);
            }
            //check cha thì check tất cả con
            $('.AddList').prop('checked', $(this).prop('checked'));
            if ($('#CheckAll').is(':checked')) {

                $('.hidden').addClass('pointer-eventsNone');
                var count = $('.AddList').length;
                var stt = 0;
                $('.AddList').each(function () {
                    stt++;
                    if ($('.AddList').is(':checked')) {
                        var idCheck = $(this).val();
                        AddList(idCheck, (stt === count));
                    }
                });

            }
            else {
                DeleteAll();
            }
        });
    }


    //thêm dánh sách
    function AddList(idCheck, removeHidden) {
        $.ajax({

            url: '/ExportMarc/AddList',
            data: {
                Id: idCheck
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (removeHidden) {
                    $('.pointer-eventsNone').removeClass('pointer-eventsNone');
                    $('.hidden').addClass('pointer-eventsAuto');
                }
                return true;
            }
        });
    }

    //xóa từng danh sách
    function deleteItem(removeIdCheck) {
        $.ajax({
            url: '/ExportMarc/DeleteItem',
            data: {
                Id: removeIdCheck
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    return true;
                }
            }
        });
    }

    //xóa từng danh sách
    function DeleteAll() {
        $.ajax({
            url: '/ExportMarc/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    return true;    
                }
            }
        });
    }
};