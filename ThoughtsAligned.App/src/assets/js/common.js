$(document).ready(function () {
    //Navigation
    $('header .menu').click(function () {
        $('header nav').addClass('activeNav');
        $('body').addClass('noScroll');
    });

    $('header nav .closeNav').click(function () {
        $('header nav').removeClass('activeNav');
        $('body').removeClass('noScroll');
    });
});

$(window).scroll(function () {
    var scroll = $(window).scrollTop();

    if (scroll >= 500) {
        $("header").addClass("darkHeader");
        $("footer").addClass("darkFooter");
    } else {
        $("header").removeClass("darkHeader");
        $("footer").removeClass("darkFooter");
    }
}); 