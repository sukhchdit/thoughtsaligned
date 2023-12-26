$(document).ready(function () {
    //Navigation
    $('header .menu').click(function () {
        $('header nav').addClass('activeNav');
    });

    $('header nav .closeNav').click(function () {
        $('header nav').removeClass('activeNav');
    });
});