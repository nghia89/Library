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
    CreateHeightHeaderTop()
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
        CreateHeightHeaderTop()
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

    /*sticky*/
    $(document).ready(function () {
        //$(".left_sticky").stick_in_parent();
        tinhvitri_in_parent_v2(".left_sticky_menu_main");
        $(window).resize(function () {
            tinhvitri_in_parent_v2(".left_sticky_menu_main");
        });
    });

    function tinhvitri_in_parent_v2(div) {
        var w = $(div).width(); //width menu left
        var h = $(div).height(); //height menu left
        var temp_scroll = 0;
        $(window).scroll(function () {
            var h_s = $(window).height(); //height cửa sổ trình duyệt
            var h_parent_of_top = $(div).parent().offset().top; //parent left menu cách top
            var h_of_top = $(div).offset().top; //left menu cách top
            var h_parent = $(div).parent().height(); //height parent left menu
            var top = $(window).scrollTop();
            
            var h_left = $(div).height();
            var kq = top - h_parent_of_top; //cach top khi scroll
            var kq_dung = h_parent_of_top + h_parent - h_left;
            //console.log((top + h_s) + "-" + (h_of_top + h_left));
            
            //màng hình nhỏ hơn menu
            if (h_s < h_left) {
                //console.log($(div).offset().top);
                //kiểm tra hướng scroll
                if (top > temp_scroll) {
                    //Scroll từ trên xuống
                    if (top + h_s > h_of_top + h_left) {                  
                        $(div).stop().css({ "position": "absolute", "top": kq - h_left + h_s, "bottom": "auto", "width": "100%" });
                    } 
                } else {
                    //Scroll từ dưới lên
                    if (top < h_of_top ) {
                        $(div).stop().css({ "position": "absolute", "top": kq + h_parent_of_top, "bottom": "auto", "width": "100%" });
                    }
                    if (kq < 0) {
                        $(div).stop().css({ "position": "relative", "top": 0, "bottom": "auto" });
                    }
                }
            } else {
                //console.log((h_s) + "-" + (h_left));
                if (top > 0 /*h_parent_of_top*/) {
                    $(div).stop().css({ "position": "absolute", "top": kq + h_parent_of_top, "bottom": "auto", "width": "100%" });
                } else {
                    $(div).stop().css({ "position": "relative", "top": 0, "bottom": "auto" });
                }

                if (top > kq_dung) {
                    $(div).stop().css({ "position": "absolute", "bottom": 0, "top": "auto", "width": "100%" });
                }
            }
            //
            temp_scroll = top;
        })
    }

    function CreateHeightHeaderTop() {
        var h = $(".contain>.top>.top_in").height();
        $(".contain>.top").css({ "height": h });
    }

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