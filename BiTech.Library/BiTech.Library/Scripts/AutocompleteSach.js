var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Sach/ListName",
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.TenSach);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.TenSach);
                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<a>")
                    .append("<li style='width:100%; border-bottom-style: dashed; border-width: thin; border-color:#d3d3d3;'>" + item.TenSach + "</li>")
                    .appendTo(ul);
            };
    }
}

common.init();