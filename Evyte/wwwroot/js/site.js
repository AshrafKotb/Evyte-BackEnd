$(document).ready(function () {
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
            // وضع رسالة الخطأ بعد الحقل أو المجموعة
            if (element.is(":file")) {
                // بالنسبة للحقول من نوع file، حط الخطأ بعد div المعاينة
                error.insertAfter(element.next(".image-preview, .image-preview-grid"));
            } else if (element.closest(".input-group").length) {
                // بالنسبة للحقول داخل input-group، حط الخطأ بعد المجموعة
                error.insertAfter(element.closest(".input-group"));
            } else {
                // الحقول العادية، حط الخطأ بعد الحقل مباشرة
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

    // URL validation for social media and location fields (optional but must be valid if provided)
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
            previewContainer.empty(); // إفراغ المعاينة القديمة
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

            e.preventDefault(); // Prevent default form submission
            const submitButton = $(this).find('.btn-primary');
            submitButton.prop('disabled', true).addClass('animate-spin');
            submitButton.html('<i class="fas fa-spinner fa-spin ml-2"></i> جاري الإرسال...');

            // Show loading overlay
            $("#loadingOverlay").removeClass("hidden");

            // Prepare form data for AJAX
            const formData = new FormData(this);

            // Send AJAX request
            $.ajax({
                url: $(this).attr("action"),
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                headers: {
                    "X-Requested-With": "XMLHttpRequest" // Ensure controller detects AJAX
                },
                success: function (response) {
                    // Hide loading overlay
                    $("#loadingOverlay").addClass("hidden");
                    submitButton.prop('disabled', false).removeClass('animate-spin');
                    submitButton.html('<i class="fas fa-paper-plane ml-2"></i> إرسال الطلب');

                    if (response.success) {
                        // Populate success modal with URLs
                        $("#invitationUrl").attr("href", response.invitationUrl).text(response.invitationUrl);
                        $("#qrCodeUrl").attr("href", response.qrCodeUrl).text(response.qrCodeUrl);

                        // Show success modal
                        $("#successModal").modal("show");

                        // Trigger confetti animation
                        confetti({
                            particleCount: 100,
                            spread: 70,
                            origin: { y: 0.6 },
                            colors: ['#7c3aed', '#f472b6', '#facc15']
                        });
                    }
                },
                error: function (xhr) {
                    // Hide loading overlay
                    $("#loadingOverlay").addClass("hidden");
                    submitButton.prop('disabled', false).removeClass('animate-spin');
                    submitButton.html('<i class="fas fa-paper-plane ml-2"></i> إرسال الطلب');

                    // Show error modal
                    const errorMessage = xhr.responseJSON && xhr.responseJSON.message
                        ? xhr.responseJSON.message
                        : "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا.";
                    $("#errorMessage").text(errorMessage);
                    $("#errorModal").modal("show");
                }
            });
        });
    }

    // Add animation to form fields on focus
    $('.form-control, .form-select').on('focus', function () {
        $(this).addClass('animate-glow');
    }).on('blur', function () {
        $(this).removeClass('animate-glow');
    });
});