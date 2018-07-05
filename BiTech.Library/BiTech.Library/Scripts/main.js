$(document).ready(function () {
    $(".bt_menu").click(function () {
        turn_on_off_menu();
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
});


function turn_on_off_menu() {
    if ($(".contain").hasClass("small_menu") ) {
        $(".contain").removeClass("small_menu");
        setCookie("menu", " ", "1");
    } else {
        $(".contain").addClass("small_menu");
        setCookie("menu", "small_menu", "1");
    }
}
function turn_off_menu(){
    if ($(".contain").hasClass("small_menu")) {
        
    }else{
        $(".contain").addClass("small_menu");
        setCookie("menu", "small_menu", "1");
    }
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