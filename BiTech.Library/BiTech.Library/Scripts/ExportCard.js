var controllerExportCard = function () {
    this.initialize = function () {
        if ($('.AddList_HS:checked').length === $('#tblFunction tbody tr .AddList_HS').length) {
            $('#CheckAll').prop('checked', true);
        }
        registerEvents();
    }

    function registerEvents() {

        $('.AddList_HS').change(function () {
            if ($(this).is(":checked")) {
                var idCheck = $(this).val();
                AddList(idCheck);
            }
            else {
                var rmIdCheck = $(this).val();
                DeleteItem(rmIdCheck);
            }

            if ($('.AddList_HS:checked').length === $('#tblFunction tbody tr .AddList_HS').length) {
                $('#CheckAll').prop('checked', true);
            } else {
                $('#CheckAll').prop('checked', false);
            }

        });

        $("#CheckAll").click(function () {
            var status = this.checked;
            $("input[name='chon']").each(function () { this.checked = status; })
            if (status == true) {
                $('.AddList_HS').each(function () {
                    if ($('.AddList_HS').is(':checked')) {
                        var idCheck = $(this).val();
                        AddList(idCheck);
                    }
                })
            }
            else {
                DeleteAll();
            }
        });
    }

    //thêm dánh sách
    function AddList(idCheck) {
        $.ajax({
            url: '/HocSinh/AddList',
            data: {
                Id: idCheck
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
    function DeleteItem(rmIdCheck) {
        $.ajax({
            url: '/HocSinh/DeleteItem',
            data: {
                Id: rmIdCheck
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

    //xóa tat ca danh sách
    function DeleteAll() {
        $.ajax({
            url: '/HocSinh/DeleteAll',
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