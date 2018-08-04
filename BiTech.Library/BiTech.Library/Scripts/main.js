/*====TÌnh trạng sách====*/
function laber_check() {
    $(".tinhtrangsach #TrangThai").next().next().html("Không cho phép mượn sách");
    $(".tinhtrangsach #TrangThai:checked").next().next().html("Cho phép mượn sách");
}

$(document).ready(function () {
    laber_check();
    $(".tinhtrangsach input[type=checkbox]").on("click", laber_check);
});

/*====Main====*/
$(document).ready(function () {
    $(".bt_menu").click(function () {
        turn_on_off_menu();
    });

    $(document).on("click", "#accordion .card .card-header ", function () {
        //Trường hợp chọn chức năng nào thì active ngay chức năng đó (khi ở trạng thái small_menu)
        if ($("#contain").hasClass("small_menu")) {
            //Nếu menu đang ở trạng thái small_menu
            if ($(this).hasClass("active")) {

            } else {
                $("#accordion .card").removeClass("active");
                $(this).parent().addClass("active");
            }
        } else {
            if ($(this).parent().hasClass("active")) {
                $("#accordion .card").removeClass("active");
            } else {
                $("#accordion .card").removeClass("active");
                $(this).parent().addClass("active");
            }
        }
        turn_on_menu(this);
        //removeActiveMenuLeftAccordion(this);
    });

    //Nếu width trình duyệt lớn hơn 1200 thì hiện menu left sau khi load
    var width_screen = $(window).width();
    if (width_screen < 992) {
        turn_off_menu();
    }
    $(window).resize(function () {
        var width_screen = $(window).width();
        if (width_screen < 992) {
            turn_off_menu();
        }
    });

    /*$("#ListTacGia .tags").focus(function () {
        console.log("abc");
    });*/

    /*Thêm focus cho ngTagsInput (trong thêm sách)*/
    $("#ListTacGia").on("focus", ".tags", function () {
        $(this).parent().parent().addClass("focused");
    });

    $("#ListTacGia").on("focusout", ".tags", function () {
        $(this).parent().parent().removeClass("focused");
    });
});



function turn_on_off_menu() {
    if ($(".contain").hasClass("small_menu")) {
        $(".contain").removeClass("small_menu");
        setCookie("menu", " ", "1");
    } else {
        $(".contain").addClass("small_menu");
        setCookie("menu", "small_menu", "1");
        small_menu_function_active();
    }
}
function turn_off_menu() {
    if ($(".contain").hasClass("small_menu")) {

    } else {
        $(".contain").addClass("small_menu");
        setCookie("menu", "small_menu", "1");
        small_menu_function_active();
    }
}
function turn_on_menu(obj) {
    if ($(".contain").hasClass("small_menu")) {
        $(".contain").removeClass("small_menu");
        setCookie("menu", " ", "1");
    } else {

    }
}

function small_menu_function_active() {
    removeActiveMenuLeftAccordion(".card");
}

function removeActiveMenuLeftAccordion(obj) {
    //đống menu left
    $(obj).children().children().addClass("collapsed");
    $(obj).children(".collapse").removeClass("show");
    $(obj).removeClass(".active");

    $(obj + ".function_active").children().children().removeClass("collapsed");
    $(obj + ".function_active").children(".collapse").addClass("show");
    $(obj + ".function_active").addClass("active");
}

function goBack() {
    window.history.go(-1);
}

/*==Cookie==*/
/*Tạo*/
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
/*View*/
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
/*Xoá*/
var delete_cookie = function (name) {
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};