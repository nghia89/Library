var controllerMarc = function () {
    this.initialize = function () {
        var countListSession = 0;
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

                //$('.hidden').addClass('pointer-eventsNone');
                AddList(idCheck);
                LoadListSession();
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
            var idCheck = [];
            if ($('.AddList').is(':checked')) {
                $('.AddList').each(function () {
                    //$('.hidden').addClass('pointer-eventsNone');
                    if ($('.AddList').is(':checked')) {
                        idCheck.push($(this).val());
                    }
                });
                deleteItem(idCheck);
                $(this).prop('checked', false);
                LoadListSession();
            }
            //check cha thì check tất cả con
            $('.AddList').prop('checked', $(this).prop('checked'));
            if ($('#CheckAll').is(':checked')) {
                $('.AddList').each(function () {
                    //$('.hidden').addClass('pointer-eventsNone');
                    if ($('.AddList').is(':checked')) {
                        idCheck.push($(this).val());
                    }
                });
                AddList(idCheck);
                LoadListSession();
            }
            else {
                deleteItem(idCheck);
                LoadListSession();
            }
        });
    }


    //thêm dánh sách
    function AddList(idCheck) {
        $.ajax({

            url: '/ExportMarc/AddList',
            data: {
                Id: idCheck
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status === true) {
                    //$('.pointer-eventsNone').removeClass('pointer-eventsNone');
                    LoadListSession();
                }
                return true;
            }
        });
    }

    //load danh sách trong session
    function LoadListSession() {
        $.ajax({
            url: '/ExportMarc/LoadListSession',
            type: 'post',
            dataType: 'json',
            success: function (response) {
                countListSession = response.data.length;
                if (countListSession > 0)
                    $('.pointer-eventsNone').removeClass('pointer-eventsNone');
                else
                    $('.hidden').addClass('pointer-eventsNone');
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
                    //$('.pointer-eventsNone').removeClass('pointer-eventsNone');
                    LoadListSession();
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