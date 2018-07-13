$('#IDTacGia').on('click', function () {
    $.ajax({
        type: "post",
        url: "/Tacgia/GetAll",
        dataType: "json",
        success: function (response) {
            $('#LoadTG').html("");
            $.each(response,function (i, item) {
                 $('#LoadTG').append("<option value='" + item.Id + "'>" + item.TenTacGia + "</option>")
            });
        }
    });
});