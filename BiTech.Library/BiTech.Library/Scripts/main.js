$(document).ready(function () {
    $(".bt_menu").click(function () {
        if ($(".contain").hasClass("small_menu")) {
            $(".contain").removeClass("small_menu");
        } else {
            $(".contain").addClass("small_menu");
        }
    });
});