$(document).ready(function () {
    // Hide splash screen after 5 seconds
    setTimeout(function () {
        $('body').addClass('loaded');
        $('.content-wrapper').removeClass('hidden');

        // Remove splash screen from DOM after animation completes
        setTimeout(function () {
            $('#splash-screen').remove();
        }, 500);
    }, 5000);

    // Toggle menu for mobile
    $('.layout-menu-toggle').click(function () {
        $('body').toggleClass('menu-open');
    });
});