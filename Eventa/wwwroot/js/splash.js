// Splash Screen Handler - RADICAL: fast, aggressive, guaranteed to disappear
(function () {
    var splash = document.getElementById('splashScreen');
    if (!splash) return;

    var alreadyHidden = false;

    function hideSplash() {
        if (alreadyHidden) return;
        alreadyHidden = true;

        // Hide splash with transition
        splash.style.transition = 'opacity 0.5s ease';
        splash.style.opacity = '0';
        splash.classList.add('hidden');

        setTimeout(function () {
            splash.style.display = 'none';
            splash.style.pointerEvents = 'none';
            splash.style.zIndex = '-1';
        }, 600);
    }

    // Hide splash when page is fully loaded (fastest trigger)
    window.addEventListener('load', function () {
        setTimeout(hideSplash, 800);
    });

    // If DOM is already loaded (script loaded late), hide immediately
    if (document.readyState === 'complete') {
        setTimeout(hideSplash, 200);
    }

    document.addEventListener('DOMContentLoaded', function () {
        setTimeout(hideSplash, 1500);
    });

    // Fallback: hide after 3 seconds max regardless of anything
    setTimeout(hideSplash, 3000);
})();
