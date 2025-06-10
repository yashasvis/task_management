$(".nav-link").on("click", function () {
    $(".nav-link").removeClass("active");
    var anchor = this.id;
    sessionStorage.setItem("anchor", anchor);
    $("#" + anchor).addClass("active");
});

$(document).ready(function () {
    var anchor = sessionStorage.getItem("anchor");
    if (anchor != null && anchor != "undefined") {
        $(".nav-link").removeClass("active");
        $("#" + anchor).addClass("active");
    }
});