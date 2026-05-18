// Splash Screen Handler - يحترم window.SPLASH_DURATION_MS لو متعين من _SplashLoader
(function () {
    var splash = document.getElementById('splashScreen');
    if (!splash) return;

    // لو السبلاش بيحتاج تفاعل (RequiresInteraction)، خلّيه - الـ _SplashLoader هيتعامل معاه
    if (splash.dataset.requiresInteraction === 'true') return;

    var alreadyHidden = false;
    var durationMs = (typeof window.SPLASH_DURATION_MS === 'number' && window.SPLASH_DURATION_MS > 0)
        ? window.SPLASH_DURATION_MS
        : 3000;

    function hideSplash() {
        if (alreadyHidden) return;
        alreadyHidden = true;
        splash.style.transition = 'opacity 0.6s ease';
        splash.style.opacity = '0';
        splash.classList.add('hidden');
        setTimeout(function () {
            splash.style.display = 'none';
            splash.style.pointerEvents = 'none';
            splash.style.zIndex = '-1';
        }, 700);
    }

    // اخفي بعد المدة المحددة من السبلاش تيمبلت (مش 3 ثواني hardcoded)
    setTimeout(hideSplash, durationMs);
})();
