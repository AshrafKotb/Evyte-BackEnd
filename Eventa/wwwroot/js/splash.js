// Splash Screen Handler
(function () {
    var splash = document.getElementById('splashScreen');
    if (!splash) return;

    var alreadyHidden = false;

    function hideSplash() {
        if (alreadyHidden) return;
        alreadyHidden = true;

        splash.classList.add('hidden');
        setTimeout(function () {
            splash.style.display = 'none';
            // Also hide loading-spinner if it exists (belt & suspenders)
            var spinner = document.getElementById('loading-spinner');
            if (spinner) spinner.classList.add('hidden');
        }, 800);
    }

    // Hide splash when page is fully loaded
    window.addEventListener('load', function () {
        setTimeout(hideSplash, 1500);
    });

    // If DOM is already loaded (script loaded late), hide sooner
    if (document.readyState === 'complete') {
        setTimeout(hideSplash, 500);
    }

    document.addEventListener('DOMContentLoaded', function () {
        setTimeout(hideSplash, 2000);
    });

    // Fallback: hide after 5 seconds max regardless
    setTimeout(hideSplash, 5000);
})();
