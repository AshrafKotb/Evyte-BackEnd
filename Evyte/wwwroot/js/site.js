$(document).ready(function () {
    // Initialize Particles.js
    particlesJS('particles-js', {
        particles: {
            number: { value: 80, density: { enable: true, value_area: 800 } },
            color: { value: ['#f472b6', '#facc15', '#ffffff'] },
            shape: {
                type: ['circle', 'heart'],
                stroke: { width: 0, color: '#000000' },
                polygon: { nb_sides: 5 }
            },
            opacity: { value: 0.5, random: true, anim: { enable: false } },
            size: { value: 10, random: true, anim: { enable: false } },
            line_linked: { enable: false },
            move: {
                enable: true,
                speed: 2,
                direction: 'bottom',
                random: true,
                straight: false,
                out_mode: 'out',
                bounce: false
            }
        },
        interactivity: {
            detect_on: 'canvas',
            events: {
                onhover: { enable: true, mode: 'grab' },
                onclick: { enable: true, mode: 'push' },
                resize: true
            },
            modes: {
                grab: { distance: 140, line_linked: { opacity: 0.7 } },
                push: { particles_nb: 4 }
            }
        },
        retina_detect: true
    });

    // Initialize form validation
    $("#weddingForm").validate({
        rules: {
            FullName: { required: true, minlength: 2 },
            PhoneNumber: { required: true },
            Email: { required: true, email: true },
            GroomName: { required: true, minlength: 2 },
            BrideName: { required: true, minlength: 2 },
            EventDate: { required: true },
            EventTimeFrom: { required: true },
            EventTimeTo: { required: true }
        },
        messages: {
            FullName: "يرجى إدخال الاسم الكامل (على الأقل حرفين)",
            PhoneNumber: "يرجى إدخال رقم هاتف صالح",
            Email: "يرجى إدخال بريد إلكتروني صالح",
            GroomName: "يرجى إدخال اسم العريس (على الأقل حرفين)",
            BrideName: "يرجى إدخال اسم العروس (على الأقل حرفين)",
            EventDate: "يرجى اختيار تاريخ الحدث",
            EventTimeFrom: "يرجى اختيار وقت بدء الحدث",
            EventTimeTo: "يرجى اختيار وقت انتهاء الحدث"
        },
        errorElement: "span",
        errorClass: "text-red-500 text-sm mt-1 block",
        errorPlacement: function (error, element) {
            error.insertAfter(element.closest(".input-group") || element);
        },
        highlight: function (element) {
            $(element).addClass("border-red-500").removeClass("border-gray-300");
            $(element).closest('.form-control').addClass('animate-shake');
            setTimeout(() => {
                $(element).closest('.form-control').removeClass('animate-shake');
            }, 500);
        },
        unhighlight: function (element) {
            $(element).removeClass("border-red-500").addClass("border-gray-300");
        }
    });

    // URL validation for social media and location fields
    const urlInputs = document.querySelectorAll('input[type="url"]');
    urlInputs.forEach(input => {
        input.addEventListener('input', function () {
            if (this.value && !this.value.match(/^https?:\/\/.+/)) {
                this.setCustomValidity('يرجى إدخال رابط صالح يبدأ بـ http:// أو https://');
            } else {
                this.setCustomValidity('');
            }
        });
    });

    // Social media link click handler
    $('.social-link').on('click', function (e) {
        e.preventDefault();
        const inputId = $(this).data('input');
        const url = $(`#${inputId}`).val();
        if (url && url.match(/^https?:\/\/.+/)) {
            window.open(url, '_blank');
        }
    });

    // Image preview for file inputs
    const fileInputs = ['groomeImage', 'brideImage', 'eventPlaceImage', 'mainSliderImage', 'galleryPhotos'];
    fileInputs.forEach(id => {
        $(`#${id}`).on('change', function (e) {
            const previewContainer = $(`#${id}Preview`);
            previewContainer.empty();
            const files = e.target.files;
            for (let file of files) {
                if (file.type.match('image.*')) {
                    const reader = new FileReader();
                    reader.onload = function (event) {
                        const img = $('<img>').attr('src', event.target.result).addClass('animate-field');
                        previewContainer.append(img);
                    };
                    reader.readAsDataURL(file);
                }
            }
        });
    });

    // Form submission handler with loading animation
    $("#weddingForm").on("submit", async function (e) {
        if (!$(this).valid()) {
            e.preventDefault();
            return;
        }
        const submitButton = $(this).find('.btn-primary');
        submitButton.prop('disabled', true).addClass('animate-spin');
        submitButton.html('<i class="fas fa-spinner fa-spin mr-2"></i> جاري الإرسال...');
    });

    // Add animation to form fields on focus
    $('.form-control, .form-select').on('focus', function () {
        $(this).addClass('animate-glow');
    }).on('blur', function () {
        $(this).removeClass('animate-glow');
    });
});