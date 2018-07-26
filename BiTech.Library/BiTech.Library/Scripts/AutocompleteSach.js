﻿var common = {
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
                return $("<li>")
                    .append("<a style='width:100%;position: absolute'>" + item.TenSach + "</a>")
                    .appendTo(ul);
            };
    }
}

common.init();