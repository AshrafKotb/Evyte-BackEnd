$(document).ready(function () {
    // Check if SweetAlert2 and confetti are loaded
    if (typeof Swal === 'undefined') {
        console.error('SweetAlert2 is not loaded. Please check _Layout.cshtml.');
        return;
    }
    if (typeof confetti === 'undefined') {
        console.warn('canvas-confetti is not loaded. Confetti effect will be skipped.');
    }

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
            EventTimeTo: { required: true },
            EventPlaceName: { required: true },
            EventAddress: { required: true },
            LocationUrl: { required: true, url: true },
            GroomImage: { required: true, accept: "image/png,image/jpeg,image/jpg" },
            BrideImage: { required: true, accept: "image/png,image/jpeg,image/jpg" },
            EventPlaceImage: { required: true, accept: "image/png,image/jpeg,image/jpg" },
            MainSliderImage: { required: true, accept: "image/png,image/jpeg,image/jpg" },
            GalleryPhotos: { required: true, accept: "image/png,image/jpeg,image/jpg" }
        },
        messages: {
            FullName: "يرجى إدخال الاسم الكامل (على الأقل حرفين)",
            PhoneNumber: "يرجى إدخال رقم هاتف صالح",
            Email: "يرجى إدخال بريد إلكتروني صالح",
            GroomName: "يرجى إدخال اسم العريس (على الأقل حرفين)",
            BrideName: "يرجى إدخال اسم العروس (على الأقل حرفين)",
            EventDate: "يرجى اختيار تاريخ الحدث",
            EventTimeFrom: "يرجى اختيار وقت بدء الحدث",
            EventTimeTo: "يرجى اختيار وقت انتهاء الحدث",
            EventPlaceName: "يرجى إدخال اسم المكان",
            EventAddress: "يرجى إدخال عنوان المكان",
            LocationUrl: "يرجى إدخال رابط موقع صالح",
            GroomImage: "يرجى تحميل صورة العريس (PNG أو JPEG)",
            BrideImage: "يرجى تحميل صورة العروس (PNG أو JPEG)",
            EventPlaceImage: "يرجى تحميل صورة المكان (PNG أو JPEG)",
            MainSliderImage: "يرجى تحميل صورة السلايدر الرئيسية (PNG أو JPEG)",
            GalleryPhotos: "يرجى تحميل صورة واحدة على الأقل للمعرض (PNG أو JPEG)"
        },
        errorElement: "span",
        errorClass: "text-red-500 text-sm mt-1 block",
        errorPlacement: function (error, element) {
            if (element.is(":file")) {
                error.insertAfter(element.next(".image-preview, .image-preview-grid"));
            } else if (element.closest(".input-group").length) {
                error.insertAfter(element.closest(".input-group"));
            } else {
                error.insertAfter(element);
            }
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

    // Image preview for file inputs
    const fileInputs = ['GroomImage', 'BrideImage', 'EventPlaceImage', 'MainSliderImage', 'GalleryPhotos'];
    fileInputs.forEach(id => {
        $(`#${id}`).on('change', function (e) {
            const previewContainer = $(`#${id}Preview`);
            previewContainer.empty();
            const files = e.target.files;
            if (files && files.length > 0) {
                for (let file of files) {
                    if (file.type.match('image.*')) {
                        const reader = new FileReader();
                        reader.onload = function (event) {
                            const img = $('<img>')
                                .attr('src', event.target.result)
                                .addClass('w-24 h-24 object-cover rounded-md m-1 animate-field');
                            previewContainer.append(img);
                        };
                        reader.readAsDataURL(file);
                    }
                }
            }
        });
    });

    // Form submission handler with AJAX
    if ($("#weddingForm").length) {
        $("#weddingForm").on("submit", function (e) {
            if (!$(this).valid()) {
                e.preventDefault();
                return;
            }

            e.preventDefault();
            const submitButton = $(this).find('.btn-primary');
            submitButton.prop('disabled', true).addClass('animate-spin');
            submitButton.html('<i class="fas fa-spinner fa-spin ml-2"></i> جاري الإرسال...');

            $("#loadingOverlay").removeClass("hidden");

            const formData = new FormData(this);

            try {
                $.ajax({
                    url: $(this).attr("action"),
                    type: "POST",
                    data: formData,
                    processData: false,
                    contentType: false,
                    headers: {
                        "X-Requested-With": "XMLHttpRequest"
                    },
                    success: function (response) {
                        console.log("Success response:", response);
                        $("#loadingOverlay").addClass("hidden");
                        submitButton.prop('disabled', false).removeClass('animate-spin');
                        submitButton.html('<i class="fas fa-paper-plane ml-2"></i> إرسال الطلب');

                        if (response.success) {
                            Swal.fire({
                                title: '<i class="fas fa-party-horn mr-2"></i> تم بنجاح!',
                                html: `
                                    <div class="text-right">
                                        <p class="text-lg font-medium text-gray-700 mb-4">تم إنشاء موقع زفافك بنجاح! شاركه مع ضيوفك الآن.</p>
                                        <p class="mb-2"><strong>رابط الدعوة:</strong> 
                                            <a href="${response.invitationUrl}" target="_blank" class="text-purple-600 hover:text-purple-800 transition-colors duration-200">${response.invitationUrl}</a>
                                        </p>
                                        <p class="mb-2"><strong>رمز الاستجابة السريعة:</strong></p>
                                        <div class="flex justify-center">
                                            <img src="${response.qrCodeUrl}" alt="QR Code" class="w-40 h-40 object-contain bg-white p-2 rounded-lg shadow-md" onerror="this.src='/images/default-qr.png'" />
                                        </div>
                                    </div>
                                `,
                                icon: "success",
                                confirmButtonText: "الصفحة الرئيسية",
                                customClass: {
                                    popup: 'swal-custom-popup',
                                    title: 'text-xl font-bold text-gray-800',
                                    htmlContainer: 'text-base',
                                    confirmButton: 'btn btn-primary px-5 py-2 text-white bg-gradient-to-r from-purple-600 to-pink-500 hover:from-purple-700 hover:to-pink-600 rounded-md shadow-md'
                                },
                                buttonsStyling: false,
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                width: '28rem',
                                padding: '1.5rem',
                                backdrop: 'rgba(0,0,0,0.6)',
                                showClass: { popup: 'animate__animated animate__fadeIn' },
                                hideClass: { popup: 'animate__animated animate__fadeOut' }
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    console.log("Redirecting to Home/Index");
                                    window.location.href = response.redirectUrl || "/Home/Index";
                                }
                            });

                            if (typeof confetti !== 'undefined') {
                                confetti({
                                    particleCount: 150,
                                    spread: 70,
                                    origin: { y: 0.6 },
                                    colors: ['#7c3aed', '#f472b6', '#facc15', '#a78bfa']
                                });
                            }
                        } else {
                            Swal.fire({
                                title: "حدث خطأ",
                                html: `<p class="text-lg font-medium text-gray-700">${response.message || "حدث خطأ غير متوقع."}</p>`,
                                icon: "error",
                                confirmButtonText: "حسنًا",
                                customClass: {
                                    popup: 'swal-custom-popup',
                                    title: 'text-xl font-bold text-gray-800',
                                    htmlContainer: 'text-base',
                                    confirmButton: 'btn btn-primary px-5 py-2 text-white bg-gradient-to-r from-purple-600 to-pink-500 hover:from-purple-700 hover:to-pink-600 rounded-md shadow-md'
                                },
                                buttonsStyling: false,
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                width: '28rem',
                                padding: '1.5rem',
                                backdrop: 'rgba(0,0,0,0.6)',
                                showClass: { popup: 'animate__animated animate__fadeIn' },
                                hideClass: { popup: 'animate__animated animate__fadeOut' }
                            });
                        }
                    },
                    error: function (xhr) {
                        console.log("Error response:", xhr);
                        $("#loadingOverlay").addClass("hidden");
                        submitButton.prop('disabled', false).removeClass('animate-spin');
                        submitButton.html('<i class="fas fa-paper-plane ml-2"></i> إرسال الطلب');

                        let errorMessage = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا.";
                        if (xhr.responseJSON) {
                            if (xhr.responseJSON.message) {
                                errorMessage = xhr.responseJSON.message;
                            } else if (xhr.responseJSON.errors) {
                                errorMessage = Object.values(xhr.responseJSON.errors)
                                    .flat()
                                    .join("<br>");
                            }
                        }

                        Swal.fire({
                            title: "حدث خطأ",
                            html: `<p class="text-lg font-medium text-gray-700">${errorMessage}</p>`,
                            icon: "error",
                            confirmButtonText: "حسنًا",
                            customClass: {
                                popup: 'swal-custom-popup',
                                title: 'text-xl font-bold text-gray-800',
                                htmlContainer: 'text-base',
                                confirmButton: 'btn btn-primary px-5 py-2 text-white bg-gradient-to-r from-purple-600 to-pink-500 hover:from-purple-700 hover:to-pink-600 rounded-md shadow-md'
                            },
                            buttonsStyling: false,
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            width: '28rem',
                            padding: '1.5rem',
                            backdrop: 'rgba(0,0,0,0.6)',
                            showClass: { popup: 'animate__animated animate__fadeIn' },
                            hideClass: { popup: 'animate__animated animate__fadeOut' }
                        });
                    },
                    complete: function () {
                        console.log("AJAX request completed, ensuring loadingOverlay is hidden");
                        $("#loadingOverlay").addClass("hidden");
                    }
                });
            } catch (err) {
                console.error("AJAX error:", err);
                $("#loadingOverlay").addClass("hidden");
                submitButton.prop('disabled', false).removeClass('animate-spin');
                submitButton.html('<i class="fas fa-paper-plane ml-2"></i> إرسال الطلب');
                Swal.fire({
                    title: "خطأ غير متوقع",
                    html: "<p class='text-lg font-medium text-gray-700'>حدث خطأ أثناء معالجة الطلب. يرجى المحاولة لاحقًا.</p>",
                    icon: "error",
                    confirmButtonText: "حسنًا",
                    customClass: {
                        popup: 'swal-custom-popup',
                        title: 'text-xl font-bold text-gray-800',
                        htmlContainer: 'text-base',
                        confirmButton: 'btn btn-primary px-5 py-2 text-white bg-gradient-to-r from-purple-600 to-pink-500 hover:from-purple-700 hover:to-pink-600 rounded-md shadow-md'
                    },
                    buttonsStyling: false,
                    width: '28rem',
                    padding: '1.5rem',
                    backdrop: 'rgba(0,0,0,0.6)'
                });
            }
        });
    }

    // Add animation to form fields on focus
    $('.form-control, .form-select').on('focus', function () {
        $(this).addClass('animate-glow');
    }).on('blur', function () {
        $(this).removeClass('animate-glow');
    });
});