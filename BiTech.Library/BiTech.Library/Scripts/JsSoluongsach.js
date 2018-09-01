
var SoLuongTTSach = function () {

    this.initialize = function () {
       
        loadData();
        registerEvents();
    }
    function registerEvents() {
        var sl;
        $("#frmMaintainance").validate({
            rules: {
                name: {
                    required: true,
                    max: sl
                },
                IdTrangThai: {
                    required: true
                }
            }, messages: {
                name: {
                    required: "Bạn cần nhập số lượng",
                    max: "số lượng nhập phải nhỏ hơn số lượng hiện tại"
                },
                IdTrangThai: {
                    required: "Bạn cần chọn trạng thái"
                }
            }
        });

        $('body').on('click', '.btn-edit', function (e) {   
       
            e.preventDefault();
            var that = $(this).data('id');
            var idtt = $(this).next().val();
            LoadEdit(that);
            //loadData();
            $.ajax({
                type: 'get',
                url: '/Sach/GetAllTT',
                data: {
                    Id: idtt
              
                },
                dataType: 'json',
                success: function (response) {
                    var render = "<option value=''>--Chọn trạng thái--</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.TenTT + "</option>"
                    });
                    $('#tbl-bill-TTDetail').html(render);
                }
            });        
        });

        $('#txtSave').on('click', function () {
            SaveChange();
            loadData();
        });
    }

   
    function SaveChange(e) {
        if ($('#frmMaintainance').valid()) {
            var txtid = $('#txtId').val();
            var txtSoLuong = $('#txtSoLuong').val();
            var txtIdTT = $('#txtEditIdtt').val();
            var txtIdSach = $('#txtEditIdSach').val();
            var txtIdttCategory = $('.txtIdttCategory').val();
            $.ajax({
                type: "post",
                url: "/Sach/EditSaveChange",
                data: {
                    Id: txtid,
                    SoLuong: txtSoLuong,
                    IdTrangThai: txtIdTT,
                    IdSach: txtIdSach,
                    txtIdttCategory: txtIdttCategory
                },
                dataType: "json",
                success: function (res) {
                    $('#modal-add-edit').modal('hide');
                    loadData();
                }

            });
            return false;
        }
    }

    function LoadEdit(that) {
        $.ajax({
            type: "get",
            url: "/Sach/GetById",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                sl = parseInt(response.SoLuong);     
                var template = $('#template').html();
                var render = "";
                Mustache.parse(template);
                $.each(response, function () {
                    render = Mustache.render(template, {
                        TrangThai: response.TrangThai,
                        SoLuong: response.SoLuong,
                        Id: response.Id,
                        IdSach: response.IdSach,
                        IdTrangThaiSach: response.IdTrangThai
                    });
                });
                if (render !== undefined) {
                    $('#tbl-content-detail').html(render);              
                }
                else {
                    $('#tbl-content-detail').html('');
                }
                $('#modal-add-edit').modal('show');
            }
        });
    }

    function loadData() {
        var that = $('#Data-IDsach').val();
        $.ajax({
            type: "GET",
            url: "/Sach/GetByFindId",
            data: {
                Id: that
            },
            dataType: "json",
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                Mustache.parse(template);
                $.each(response, function (i, item) {
                    if (item.SoLuong !== 0) {
                        render += Mustache.render(template, {
                            TrangThai: item.TrangThai,
                            SoLuong: item.SoLuong,
                            Id: item.Id,
                            IdTrangThai: item.IdTrangThai,
                            IdSach: item.IdSach
                        });
                    }
                 
                });
                if (render !== undefined) {
                    $('#tbl-content').html(render);
                }
                else {
                    $('#tbl-content').html('');
                }

            },
        });
    };

}