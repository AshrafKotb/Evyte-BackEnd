$(document).ready(function () {
    // Preloader
    $(window).on('load', function () {
        $('#preloader').fadeOut('slow', function () {
            $(this).remove();
        });
    });

    // Initialize Swiper Slider
    const heroSlider = new Swiper('.hero-slider', {
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        effect: 'fade',
        direction: 'horizontal',
    });

    // GSAP Animations
    gsap.from('.navbar-brand', {
        y: -50,
        opacity: 0,
        duration: 1,
        ease: 'bounce.out',
    });

    gsap.from('.nav-link', {
        y: -50,
        opacity: 0,
        duration: 1,
        stagger: 0.2,
        delay: 0.5,
        ease: 'power2.out',
    });

    // Confetti Animation Enhancement
    function createConfetti() {
        const confettiContainer = $('.confetti-background');
        for (let i = 0; i < 50; i++) {
            const confetti = $('<div class="confetti"></div>')
                .css({
                    right: Math.random() * 100 + 'vw',
                    animationDuration: Math.random() * 3 + 2 + 's',
                    animationDelay: Math.random() * 2 + 's',
                });
            confettiContainer.append(confetti);
        }
    }
    createConfetti();

    // Sparkle Effect for Header
    function createSparkles() {
        const sparkleContainer = $('.sparkle-effect');
        for (let i = 0; i < 20; i++) {
            const sparkle = $('<div class="sparkle"></div>')
                .css({
                    right: Math.random() * 100 + '%',
                    top: Math.random() * 100 + '%',
                    animationDelay: Math.random() * 1.5 + 's',
                });
            sparkleContainer.append(sparkle);
        }
    }
    createSparkles();
});