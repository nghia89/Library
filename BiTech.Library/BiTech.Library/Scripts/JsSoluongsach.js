var SoLuongTTSach = function () {
    this.initialize = function () {
        loadData();
        registerEvevts();

    }
    function registerEvevts() {
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            LoadEdit(that);
            //loadData();
        });
 
    }

    function LoadEdit(that) {
        $.ajax({
            type: "get",
            url: "/Sach/GetById",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                var template = $('#table-template-detail').html();
                var render = "";
                $(response, function (i, item) {
                    render = Mustache.render(template, {
                        IdTrangThai: item.IdTrangThai,
                        SoLuong: item.SoLuong,
                        Id: item.Id
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
                    render += Mustache.render(template, {
                        IdTrangThai: item.IdTrangThai,
                        SoLuong: item.SoLuong,
                        Id: item.Id
                    });
                });
                if (render !== undefined) {
                    $('#tbl-content').html(render);
                }
                else {
                    $('#tbl-content').html('');
                }

            },
            error: function (status) {
                console.log(status);
            }
        });
    };

}