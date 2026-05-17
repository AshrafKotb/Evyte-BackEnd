/**
 * Eventa Live Editor Engine
 * Handles: element detection, live preview, image upload, section management, data collection & submission
 */
(function () {
    'use strict';

    var config = window.EDITOR_CONFIG || {};
    // Uploaded files tracking: { fieldName: { url, id } }
    var uploadedFiles = {};
    var galleryPhotos = []; // [{ url, id }]
    var memories = [];      // [{ title, date, imageUrl, imageId }]
    var dedications = [];   // [{ personName, relationship, message }]
    var audioData = null;   // { url, id, name }
    var isPreviewMode = false;
    var templateDoc = null; // iframe document reference

    // ==========================================
    // TEMPLATE IFRAME ACCESS
    // ==========================================
    /**
     * Returns the template document (inside the iframe).
     * Falls back to inline templateContainer if iframe is not available.
     */
    function getTemplateDoc() {
        if (templateDoc) return templateDoc;
        var iframe = document.getElementById('templateFrame');
        if (iframe && iframe.contentDocument && iframe.contentDocument.body) {
            templateDoc = iframe.contentDocument;
            return templateDoc;
        }
        // Fallback: inline template container
        return document;
    }

    /**
     * Returns the root element that contains all template elements.
     * In iframe mode: the iframe body
     * In inline mode: the #templateContainer div
     */
    function getTemplateContainer() {
        var doc = getTemplateDoc();
        if (doc !== document) {
            return doc.body;
        }
        return document.getElementById('templateContainer');
    }

    // ==========================================
    // INITIALIZATION
    // ==========================================
    document.addEventListener('DOMContentLoaded', function () {
        var iframe = document.getElementById('templateFrame');

        function initAll() {
            // Cache the iframe document
            if (iframe && iframe.contentDocument && iframe.contentDocument.body) {
                templateDoc = iframe.contentDocument;
            }

            initPanelSections();
            initTextInputs();
            initImageUploads();
            initGallerySection();
            initMemoriesSection();
            initDedicationsSection();
            initAudioSection();
            initCustomContent();
            initSectionToggles();
            initMobilePanel();
            initToolbarActions();
            initClickableElements();

            // Setup iframe: remove splash, inject editor highlight CSS
            var tDoc = getTemplateDoc();
            if (tDoc && tDoc !== document) {
                // Remove splash screen inside template iframe
                var splash = tDoc.getElementById('splashScreen');
                if (splash) splash.remove();

                // Inject editor highlight styles into iframe
                var style = tDoc.createElement('style');
                style.textContent =
                    '[data-editor-field]{position:relative;cursor:pointer;transition:outline 0.2s,box-shadow 0.2s;}' +
                    '[data-editor-field]:hover{outline:2px dashed #4a6bdf;outline-offset:4px;box-shadow:0 0 0 4px rgba(74,107,223,0.1);}' +
                    '[data-editor-field].editor-active{outline:2px solid #4a6bdf;outline-offset:4px;box-shadow:0 0 0 4px rgba(74,107,223,0.15);}' +
                    '[data-editor-section]{position:relative;}';
                tDoc.head.appendChild(style);
            }
        }

        // Wait for iframe to load before initializing
        if (iframe && iframe.src) {
            iframe.addEventListener('load', function () {
                console.log('[Editor] Template iframe loaded');
                initAll();
            });
        } else {
            // Fallback for inline template
            initAll();
        }
    });

    // ==========================================
    // PANEL SECTION TOGGLE (Accordion)
    // ==========================================
    function initPanelSections() {
        document.querySelectorAll('.panel-section-header[data-toggle]').forEach(function (header) {
            header.addEventListener('click', function (e) {
                // Don't toggle if clicking the toggle switch
                if (e.target.closest('.section-toggle-switch')) return;

                var sectionId = 'section-' + this.getAttribute('data-toggle');
                var body = document.getElementById(sectionId);
                if (body) {
                    body.classList.toggle('collapsed');
                    this.classList.toggle('collapsed');
                }
            });
        });
    }

    // ==========================================
    // TEXT INPUTS → Live Update on Template
    // ==========================================
    function initTextInputs() {
        document.querySelectorAll('.editor-input[data-target]').forEach(function (input) {
            var target = input.getAttribute('data-target');

            input.addEventListener('input', function () {
                updateTemplateText(target, this.value);
            });
        });
    }

    function updateTemplateText(fieldName, value) {
        var container = getTemplateContainer();
        if (!container) return;

        var elements = container.querySelectorAll('[data-editor-field="' + fieldName + '"]');
        elements.forEach(function (el) {
            if (el.tagName === 'INPUT') {
                el.value = value;
            } else {
                el.textContent = value || el.getAttribute('data-editor-placeholder') || '';
            }
        });
    }

    // ==========================================
    // IMAGE UPLOADS
    // ==========================================
    function initImageUploads() {
        document.querySelectorAll('.image-upload-area[data-target]').forEach(function (area) {
            var fileInput = area.querySelector('.file-input');
            var preview = area.querySelector('.upload-preview');
            var placeholder = area.querySelector('.upload-placeholder');
            var target = area.getAttribute('data-target');
            var isUploading = false;

            // Skip if no file input found in this area
            if (!fileInput) {
                console.warn('[ImageUpload] No .file-input found in area for', target, '- creating one');
                fileInput = document.createElement('input');
                fileInput.type = 'file';
                fileInput.accept = 'image/*';
                fileInput.className = 'file-input';
                fileInput.style.display = 'none';
            }

            // Move file input OUTSIDE the click area to prevent
            // any event bubbling or double-trigger issues
            if (fileInput.parentNode) {
                fileInput.parentNode.removeChild(fileInput);
            }
            document.body.appendChild(fileInput);

            // Give unique ID for this file input
            var inputId = 'fileInput_' + target + '_' + Date.now();
            fileInput.id = inputId;

            // Create preview img if not found
            if (!preview) {
                preview = document.createElement('img');
                preview.className = 'upload-preview';
                preview.style.display = 'none';
                area.insertBefore(preview, area.firstChild);
            }

            // Single click handler - opens file picker only once
            area.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                if (isUploading) return;
                fileInput.click();
            });

            fileInput.addEventListener('change', function () {
                if (!this.files || !this.files[0]) return;
                if (isUploading) return;

                var file = this.files[0];
                var input = this;
                isUploading = true;

                console.log('[ImageUpload] File selected:', file.name, file.size, 'for', target);

                // Show loading state
                area.classList.add('uploading');
                var loadingEl = document.createElement('div');
                loadingEl.className = 'upload-loading-indicator';
                loadingEl.innerHTML = '<i class="fas fa-spinner fa-spin"></i><span>جاري الرفع...</span>';
                area.appendChild(loadingEl);

                // Show local preview immediately
                var reader = new FileReader();
                reader.onload = function (ev) {
                    preview.src = ev.target.result;
                    preview.style.display = 'block';
                    if (placeholder) placeholder.style.display = 'none';
                    area.classList.add('has-image');
                };
                reader.readAsDataURL(file);

                // Upload with auto-retry
                var retryCount = 0;
                var maxRetries = 2;

                function doUpload() {
                    var isFinalAttempt = (retryCount >= maxRetries);
                    var silentMode = !isFinalAttempt;

                    uploadImage(file, getFolderForField(target), function (data) {
                        // SUCCESS
                        console.log('[ImageUpload] Upload success for', target);
                        uploadedFiles[target] = { url: data.url, id: data.id };
                        updateTemplateImage(target, data.url);
                        preview.src = data.url;
                        input.value = '';
                        isUploading = false;
                        area.classList.remove('uploading');
                        var indicator = area.querySelector('.upload-loading-indicator');
                        if (indicator) indicator.remove();
                    }, function () {
                        // FAILURE
                        retryCount++;
                        if (retryCount <= maxRetries) {
                            console.log('[ImageUpload] Retrying... attempt', retryCount + 1);
                            var indicatorSpan = area.querySelector('.upload-loading-indicator span');
                            if (indicatorSpan) indicatorSpan.textContent = 'جاري إعادة المحاولة (' + retryCount + ')...';
                            setTimeout(doUpload, 1500 * retryCount);
                        } else {
                            // All retries exhausted - keep local preview
                            console.error('[ImageUpload] All retries failed for', target);
                            input.value = '';
                            isUploading = false;
                            area.classList.remove('uploading');
                            var loadingIndicator = area.querySelector('.upload-loading-indicator');
                            if (loadingIndicator) loadingIndicator.remove();
                        }
                    }, silentMode);
                }

                doUpload();
            });
        });
    }

    function getFolderForField(fieldName) {
        var folders = {
            'GroomImage': 'groom',
            'BrideImage': 'bride',
            'MainSliderImage': 'slider',
            'EventPlaceImage': 'eventplace'
        };
        return folders[fieldName] || 'editor';
    }

    function updateTemplateImage(fieldName, url) {
        var container = getTemplateContainer();
        if (!container) return;

        var images = container.querySelectorAll('[data-editor-field="' + fieldName + '"]');
        images.forEach(function (img) {
            if (img.tagName === 'IMG') {
                img.src = url;
            }
        });
    }

    function uploadImage(file, folder, callback, onError, silent) {
        // Build absolute upload URL to avoid any relative path issues
        var uploadUrl = config.uploadImageUrl;
        if (!uploadUrl || uploadUrl === '' || uploadUrl === 'undefined') {
            uploadUrl = '/Editor/UploadImage';
        }
        // Ensure absolute URL (prevents relative path issues when Layout=null)
        if (uploadUrl.indexOf('http') !== 0 && uploadUrl.indexOf('/') !== 0) {
            uploadUrl = '/' + uploadUrl;
        }

        var formData = new FormData();
        formData.append('file', file);
        formData.append('folder', folder);

        console.log('[Upload] URL:', uploadUrl, '| File:', file.name, '| Size:', file.size, '| Type:', file.type);

        // Use XMLHttpRequest instead of fetch for better compatibility
        var xhr = new XMLHttpRequest();
        xhr.open('POST', uploadUrl, true);
        xhr.withCredentials = true; // Include cookies for same-origin requests
        xhr.setRequestHeader('Accept', 'application/json');
        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
        // Do NOT set Content-Type - browser sets it automatically with correct boundary for FormData

        xhr.onload = function () {
            console.log('[Upload] Response status:', xhr.status);
            if (xhr.status >= 200 && xhr.status < 300) {
                try {
                    var data = JSON.parse(xhr.responseText);
                    if (data.success) {
                        console.log('[Upload] Success:', data.url);
                        callback(data);
                    } else {
                        console.warn('[Upload] Server returned error:', data.message);
                        if (!silent) alert(data.message || 'فشل رفع الصورة');
                        if (onError) onError();
                    }
                } catch (e) {
                    console.error('[Upload] Failed to parse response:', xhr.responseText.substring(0, 500));
                    if (!silent) alert('حدث خطأ في الخادم (كود: ' + xhr.status + '). أعد المحاولة.');
                    if (onError) onError();
                }
            } else {
                console.error('[Upload] HTTP error:', xhr.status, xhr.statusText);
                if (!silent) {
                    try {
                        var errData = JSON.parse(xhr.responseText);
                        alert(errData.message || 'فشل رفع الصورة (كود: ' + xhr.status + ')');
                    } catch (e) {
                        alert('حدث خطأ في الخادم (كود: ' + xhr.status + '). أعد المحاولة.');
                    }
                }
                if (onError) onError();
            }
        };

        xhr.onerror = function () {
            console.error('[Upload] Network error - XMLHttpRequest failed');
            if (!silent) alert('فشل الاتصال بالخادم. تحقق من اتصال الإنترنت وأعد المحاولة.');
            if (onError) onError();
        };

        xhr.ontimeout = function () {
            console.error('[Upload] Request timed out');
            if (!silent) alert('انتهت مهلة الرفع. حاول مرة أخرى أو استخدم صورة أصغر.');
            if (onError) onError();
        };

        xhr.timeout = 60000; // 60 second timeout
        xhr.send(formData);
    }

    // ==========================================
    // GALLERY SECTION
    // ==========================================
    function initGallerySection() {
        var btnAdd = document.getElementById('btnAddGalleryPhoto');
        var fileInput = document.getElementById('galleryFileInput');
        var grid = document.getElementById('galleryGrid');

        if (!btnAdd || !fileInput) return;

        btnAdd.addEventListener('click', function () {
            if (galleryPhotos.length >= 10) {
                alert('الحد الأقصى 10 صور');
                return;
            }
            fileInput.click();
        });

            fileInput.addEventListener('change', function () {
                var files = Array.from(this.files);
                var remaining = 10 - galleryPhotos.length;
                files = files.slice(0, remaining);

                files.forEach(function (file) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var tempIndex = galleryPhotos.length;
                        galleryPhotos.push({ url: e.target.result, id: '', uploading: true });
                        renderGalleryGrid();

                        uploadImage(file, 'gallery', function (data) {
                            galleryPhotos[tempIndex] = { url: data.url, id: data.id, uploading: false };
                            renderGalleryGrid();
                            updateTemplateGallery();
                        }, function () {
                            // On error: remove failed item so user can add again without "uploading twice"
                            galleryPhotos.splice(tempIndex, 1);
                            renderGalleryGrid();
                            updateTemplateGallery();
                        });
                    };
                    reader.readAsDataURL(file);
                });

                fileInput.value = '';
            });
    }

    function renderGalleryGrid() {
        var grid = document.getElementById('galleryGrid');
        if (!grid) return;

        grid.innerHTML = '';
        galleryPhotos.forEach(function (photo, index) {
            var div = document.createElement('div');
            div.className = 'gallery-item';
            div.innerHTML =
                '<img src="' + photo.url + '" alt="">' +
                '<button class="remove-btn" data-index="' + index + '"><i class="fas fa-times"></i></button>' +
                (photo.uploading ? '<div style="position:absolute;top:0;left:0;right:0;bottom:0;background:rgba(255,255,255,0.6);display:flex;align-items:center;justify-content:center;"><i class="fas fa-spinner fa-spin"></i></div>' : '');
            grid.appendChild(div);
        });

        // Remove buttons
        grid.querySelectorAll('.remove-btn').forEach(function (btn) {
            btn.addEventListener('click', function (e) {
                e.stopPropagation();
                var idx = parseInt(this.getAttribute('data-index'));
                galleryPhotos.splice(idx, 1);
                renderGalleryGrid();
                updateTemplateGallery();
            });
        });
    }

    function updateTemplateGallery() {
        var container = getTemplateContainer();
        if (!container) return;

        var galleryContainer = container.querySelector('.portfolio-grids.gallery-container');
        if (!galleryContainer) return;

        galleryContainer.innerHTML = '';
        galleryPhotos.forEach(function (photo) {
            if (photo.uploading) return;
            var div = document.createElement('div');
            div.className = 'grid';
            div.innerHTML =
                '<div class="img-holder">' +
                '<a href="' + photo.url + '" class="fancybox" data-fancybox-group="gall-1">' +
                '<img src="' + photo.url + '" alt="" class="img img-responsive">' +
                '<div class="hover-content"><i class="ti-plus"></i></div>' +
                '</a></div>';
            galleryContainer.appendChild(div);
        });
    }

    // ==========================================
    // MEMORIES SECTION
    // ==========================================
    function initMemoriesSection() {
        var btnAdd = document.getElementById('btnAddMemory');
        if (!btnAdd) return;

        btnAdd.addEventListener('click', function () {
            if (memories.length >= 5) {
                alert('الحد الأقصى 5 ذكريات');
                return;
            }
            memories.push({ title: '', date: '', imageUrl: '', imageId: '' });
            renderMemoriesList();
        });
    }

    function renderMemoriesList() {
        var list = document.getElementById('memoriesList');
        if (!list) return;

        list.innerHTML = '';
        memories.forEach(function (memory, index) {
            var div = document.createElement('div');
            div.className = 'memory-item';
            div.innerHTML =
                '<div class="item-header"><span>ذكرى ' + (index + 1) + '</span>' +
                '<button class="btn-remove-item" data-index="' + index + '"><i class="fas fa-trash"></i></button></div>' +
                '<div class="form-group"><label>العنوان</label>' +
                '<input type="text" class="editor-input memory-title" data-index="' + index + '" placeholder="مثال: الخطوبة" value="' + (memory.title || '') + '"></div>' +
                '<div class="form-group"><label>التاريخ</label>' +
                '<input type="date" class="editor-input memory-date" data-index="' + index + '" value="' + (memory.date || '') + '"></div>' +
                '<div class="form-group"><label>صورة (اختياري)</label>' +
                '<div class="image-upload-area memory-image-upload" data-index="' + index + '">' +
                (memory.imageUrl ? '<img src="' + memory.imageUrl + '" class="upload-preview" style="display:block;max-width:100%;max-height:100px;border-radius:8px;">' : '<div class="upload-placeholder"><i class="fas fa-cloud-upload-alt"></i><span>اضغط لرفع صورة</span></div>') +
                '<input type="file" accept="image/*" class="file-input" style="display:none;"></div></div>';
            list.appendChild(div);
        });

        // Bind events
        list.querySelectorAll('.btn-remove-item').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var idx = parseInt(this.getAttribute('data-index'));
                memories.splice(idx, 1);
                renderMemoriesList();
                updateTemplateMemories();
            });
        });

        list.querySelectorAll('.memory-title').forEach(function (input) {
            input.addEventListener('input', function () {
                var idx = parseInt(this.getAttribute('data-index'));
                memories[idx].title = this.value;
                updateTemplateMemories();
            });
        });

        list.querySelectorAll('.memory-date').forEach(function (input) {
            input.addEventListener('change', function () {
                var idx = parseInt(this.getAttribute('data-index'));
                memories[idx].date = this.value;
                updateTemplateMemories();
            });
        });

        // Clean up any orphaned memory file inputs from previous renders
        document.querySelectorAll('.memory-file-input-detached').forEach(function (el) {
            el.parentNode.removeChild(el);
        });

        list.querySelectorAll('.memory-image-upload').forEach(function (area) {
            var fileInput = area.querySelector('.file-input');
            var idx = parseInt(area.getAttribute('data-index'));
            var isUploading = false;

            // Move file input OUTSIDE the click area to document.body
            // to prevent event bubbling / double-trigger issues
            fileInput.parentNode.removeChild(fileInput);
            document.body.appendChild(fileInput);
            fileInput.className = 'file-input memory-file-input-detached';
            var inputId = 'memoryFileInput_' + idx + '_' + Date.now();
            fileInput.id = inputId;

            area.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                if (isUploading) return;
                fileInput.click();
            });

            fileInput.addEventListener('change', function () {
                if (!this.files || !this.files[0]) return;
                if (isUploading) return;

                var file = this.files[0];
                var prevUrl = memories[idx].imageUrl;
                isUploading = true;
                area.classList.add('uploading');

                // Show loading indicator
                var loadingEl = document.createElement('div');
                loadingEl.className = 'upload-loading-indicator';
                loadingEl.innerHTML = '<i class="fas fa-spinner fa-spin"></i><span>جاري الرفع...</span>';
                area.appendChild(loadingEl);

                var reader = new FileReader();
                reader.onload = function (e) {
                    memories[idx].imageUrl = e.target.result;
                    // Don't re-render list during upload to avoid losing state
                    var previewImg = area.querySelector('.upload-preview');
                    if (previewImg) {
                        previewImg.src = e.target.result;
                        previewImg.style.display = 'block';
                    } else {
                        var placeholder = area.querySelector('.upload-placeholder');
                        if (placeholder) placeholder.style.display = 'none';
                        var img = document.createElement('img');
                        img.src = e.target.result;
                        img.className = 'upload-preview';
                        img.style.cssText = 'display:block;max-width:100%;max-height:100px;border-radius:8px;';
                        area.insertBefore(img, area.firstChild);
                    }
                };
                reader.readAsDataURL(file);

                // Upload with auto-retry
                var retryCount = 0;
                var maxRetries = 2;

                function doUpload() {
                    var isFinalAttempt = (retryCount >= maxRetries);
                    var silentMode = !isFinalAttempt;

                    uploadImage(file, 'memories', function (data) {
                        memories[idx].imageUrl = data.url;
                        memories[idx].imageId = data.id;
                        isUploading = false;
                        area.classList.remove('uploading');
                        var indicator = area.querySelector('.upload-loading-indicator');
                        if (indicator) indicator.remove();
                        renderMemoriesList();
                        updateTemplateMemories();
                    }, function () {
                        retryCount++;
                        if (retryCount <= maxRetries) {
                            console.log('[MemoryUpload] Retrying... attempt', retryCount + 1);
                            var indicatorSpan = area.querySelector('.upload-loading-indicator span');
                            if (indicatorSpan) indicatorSpan.textContent = 'جاري إعادة المحاولة (' + retryCount + ')...';
                            setTimeout(doUpload, 1500 * retryCount);
                        } else {
                            memories[idx].imageUrl = prevUrl || '';
                            memories[idx].imageId = '';
                            isUploading = false;
                            area.classList.remove('uploading');
                            var loadingIndicator = area.querySelector('.upload-loading-indicator');
                            if (loadingIndicator) loadingIndicator.remove();
                            renderMemoriesList();
                            updateTemplateMemories();
                        }
                    }, silentMode);
                }

                doUpload();
            });
        });
    }

    function updateTemplateMemories() {
        var container = getTemplateContainer();
        if (!container) return;

        var section = container.querySelector('[data-editor-section="memories"]');
        if (!section) return;

        var storyWrap = section.querySelector('.wpo-story-wrap .row');
        if (!storyWrap) return;

        storyWrap.innerHTML = '';
        memories.forEach(function (memory, index) {
            if (!memory.title) return;
            var dateStr = memory.date ? new Date(memory.date).toLocaleDateString('en-US', { day: '2-digit', month: 'long', year: 'numeric' }) : '';
            var imgSrc = memory.imageUrl || 'https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg';

            var div = document.createElement('div');
            div.className = 'col col-lg-12 col-12';
            div.innerHTML =
                '<div class="wpo-story-item ' + (index % 2 === 1 ? 'right-item' : '') + '">' +
                '<div class="wpo-story-img"><img src="' + imgSrc + '" alt="' + memory.title + '"></div>' +
                '<div class="wpo-story-content"><span class="date">' + dateStr + '</span><h2>' + memory.title + '</h2></div></div>';
            storyWrap.appendChild(div);
        });
    }

    // ==========================================
    // DEDICATIONS SECTION
    // ==========================================
    function initDedicationsSection() {
        var btnAdd = document.getElementById('btnAddDedication');
        if (!btnAdd) return;

        btnAdd.addEventListener('click', function () {
            dedications.push({ personName: '', relationship: '', message: '' });
            renderDedicationsList();
        });
    }

    function renderDedicationsList() {
        var list = document.getElementById('dedicationsList');
        if (!list) return;

        list.innerHTML = '';
        dedications.forEach(function (ded, index) {
            var div = document.createElement('div');
            div.className = 'dedication-item';
            div.innerHTML =
                '<div class="item-header"><span>إهداء ' + (index + 1) + '</span>' +
                '<button class="btn-remove-item" data-index="' + index + '"><i class="fas fa-trash"></i></button></div>' +
                '<div class="form-group"><label>اسم الشخص</label>' +
                '<input type="text" class="editor-input ded-name" data-index="' + index + '" placeholder="الاسم" value="' + (ded.personName || '') + '"></div>' +
                '<div class="form-group"><label>صلة القرابة</label>' +
                '<input type="text" class="editor-input ded-rel" data-index="' + index + '" placeholder="مثال: أخو العريس" value="' + (ded.relationship || '') + '"></div>' +
                '<div class="form-group"><label>الرسالة</label>' +
                '<textarea class="editor-input ded-msg" data-index="' + index + '" placeholder="نص الإهداء" rows="3">' + (ded.message || '') + '</textarea></div>';
            list.appendChild(div);
        });

        // Bind events
        list.querySelectorAll('.btn-remove-item').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var idx = parseInt(this.getAttribute('data-index'));
                dedications.splice(idx, 1);
                renderDedicationsList();
                updateTemplateDedications();
            });
        });

        list.querySelectorAll('.ded-name').forEach(function (input) {
            input.addEventListener('input', function () {
                dedications[parseInt(this.getAttribute('data-index'))].personName = this.value;
                updateTemplateDedications();
            });
        });

        list.querySelectorAll('.ded-rel').forEach(function (input) {
            input.addEventListener('input', function () {
                dedications[parseInt(this.getAttribute('data-index'))].relationship = this.value;
                updateTemplateDedications();
            });
        });

        list.querySelectorAll('.ded-msg').forEach(function (input) {
            input.addEventListener('input', function () {
                dedications[parseInt(this.getAttribute('data-index'))].message = this.value;
                updateTemplateDedications();
            });
        });
    }

    function updateTemplateDedications() {
        var container = getTemplateContainer();
        if (!container) return;

        var section = container.querySelector('[data-editor-section="dedications"]');
        if (!section) return;

        var row = section.querySelector('.wpo-testimonial-wrap .row');
        if (!row) return;

        row.innerHTML = '';
        dedications.forEach(function (ded) {
            if (!ded.personName && !ded.message) return;
            var div = document.createElement('div');
            div.className = 'col-lg-4 col-md-6 col-12';
            div.innerHTML =
                '<div class="wpo-testimonial-item">' +
                '<div class="wpo-testimonial-content"><p>"' + (ded.message || '') + '"</p></div>' +
                '<div class="wpo-testimonial-info"><h3>' + (ded.personName || '') + '</h3>' +
                (ded.relationship ? '<span>' + ded.relationship + '</span>' : '') +
                '</div><div class="quote-icon"><i class="fa fa-quote-right"></i></div></div>';
            row.appendChild(div);
        });
    }

    // ==========================================
    // AUDIO SECTION
    // ==========================================
    function initAudioSection() {
        var uploadArea = document.getElementById('audioUploadArea');
        var fileInput = document.getElementById('audioFileInput');
        var preview = document.getElementById('audioPreview');
        var fileName = document.getElementById('audioFileName');
        var btnRemove = document.getElementById('btnRemoveAudio');

        if (!uploadArea || !fileInput) return;

        // Move file input OUTSIDE the click area to prevent double-trigger
        fileInput.parentNode.removeChild(fileInput);
        document.body.appendChild(fileInput);

        uploadArea.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            fileInput.click();
        });

        fileInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                var file = this.files[0];
                fileName.textContent = file.name;
                preview.style.display = 'flex';
                uploadArea.style.display = 'none';

                // Upload audio using XHR (consistent with image upload approach)
                var uploadUrl = config.uploadAudioUrl;
                if (!uploadUrl || uploadUrl === '' || uploadUrl === 'undefined') {
                    uploadUrl = '/Editor/UploadAudio';
                }
                if (uploadUrl.indexOf('http') !== 0 && uploadUrl.indexOf('/') !== 0) {
                    uploadUrl = '/' + uploadUrl;
                }

                var formData = new FormData();
                formData.append('file', file);

                var xhr = new XMLHttpRequest();
                xhr.open('POST', uploadUrl, true);
                xhr.withCredentials = true;
                xhr.setRequestHeader('Accept', 'application/json');
                xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');

                xhr.onload = function () {
                    if (xhr.status >= 200 && xhr.status < 300) {
                        try {
                            var data = JSON.parse(xhr.responseText);
                            if (data.success) {
                                audioData = { url: data.url, id: data.id, name: file.name };
                            } else {
                                alert('فشل رفع الملف الصوتي');
                            }
                        } catch (e) {
                            alert('حدث خطأ أثناء رفع الملف الصوتي');
                        }
                    } else {
                        alert('فشل رفع الملف الصوتي (كود: ' + xhr.status + ')');
                    }
                };

                xhr.onerror = function () {
                    alert('فشل الاتصال بالخادم أثناء رفع الملف الصوتي');
                };

                xhr.timeout = 60000;
                xhr.ontimeout = function () {
                    alert('انتهت مهلة رفع الملف الصوتي. حاول مرة أخرى.');
                };

                xhr.send(formData);
            }
        });

        if (btnRemove) {
            btnRemove.addEventListener('click', function () {
                audioData = null;
                preview.style.display = 'none';
                uploadArea.style.display = 'flex';
                fileInput.value = '';
            });
        }
    }

    // ==========================================
    // CUSTOM CONTENT (Predefined Texts + BG Images)
    // ==========================================
    function initCustomContent() {
        // Predefined text selects
        document.querySelectorAll('.predefined-select').forEach(function (select) {
            var targetInput = select.getAttribute('data-target-input');
            var input = document.querySelector('.editor-input[data-target="' + targetInput + '"]');

            select.addEventListener('change', function () {
                if (this.value && this.value !== '__custom__' && input) {
                    input.value = this.value;
                    input.dispatchEvent(new Event('input'));
                } else if (this.value === '__custom__' && input) {
                    input.value = '';
                    input.focus();
                }
            });
        });

        // Background image library
        document.querySelectorAll('.bg-lib-item').forEach(function (item) {
            item.addEventListener('click', function () {
                document.querySelectorAll('.bg-lib-item').forEach(function (i) {
                    i.classList.remove('selected');
                });
                this.classList.add('selected');
            });
        });
    }

    // ==========================================
    // SECTION TOGGLES (show/hide sections in template)
    // ==========================================
    function initSectionToggles() {
        var toggles = {
            'toggleGallery': 'gallery',
            'toggleMemories': 'memories',
            'toggleDedications': 'dedications',
            'toggleAudio': 'audio'
        };

        Object.keys(toggles).forEach(function (toggleId) {
            var toggle = document.getElementById(toggleId);
            if (!toggle) return;

            toggle.addEventListener('change', function () {
                var sectionName = toggles[toggleId];
                var container = getTemplateContainer();
                if (!container) return;

                var section = container.querySelector('[data-editor-section="' + sectionName + '"]');
                if (section) {
                    section.style.display = this.checked ? '' : 'none';
                }
            });
        });
    }

    // ==========================================
    // MOBILE PANEL
    // ==========================================
    function initMobilePanel() {
        // Legacy toggle (kept for backwards compatibility)
        var toggle = document.getElementById('mobilePanelToggle');
        var panel = document.getElementById('editorPanel');

        if (toggle && panel) {
            toggle.addEventListener('click', function () {
                panel.classList.toggle('open');
                toggle.classList.toggle('open');
            });
        }

        // New mobile preview FAB - toggles between form and preview
        var mobilePreviewFab = document.getElementById('mobilePreviewFab');
        if (mobilePreviewFab) {
            var isMobilePreview = false;

            mobilePreviewFab.addEventListener('click', function () {
                isMobilePreview = !isMobilePreview;
                document.body.classList.toggle('mobile-preview', isMobilePreview);

                if (isMobilePreview) {
                    mobilePreviewFab.innerHTML = '<i class="fas fa-edit"></i><span>تعديل الدعوة</span>';
                    // Scroll canvas to top
                    var canvas = document.getElementById('editorCanvas');
                    if (canvas) canvas.scrollTop = 0;
                } else {
                    mobilePreviewFab.innerHTML = '<i class="fas fa-eye"></i><span>معاينة الدعوة</span>';
                }
            });
        }
    }

    // ==========================================
    // TOOLBAR ACTIONS
    // ==========================================
    function initToolbarActions() {
        // Preview toggle
        var btnPreview = document.getElementById('btnPreview');
        if (btnPreview) {
            btnPreview.addEventListener('click', function () {
                isPreviewMode = !isPreviewMode;
                document.body.classList.toggle('preview-mode', isPreviewMode);
                this.innerHTML = isPreviewMode
                    ? '<i class="fas fa-edit"></i><span>تعديل</span>'
                    : '<i class="fas fa-eye"></i><span>معاينة</span>';
            });
        }

        // Submit
        var btnSubmit = document.getElementById('btnSubmit');
        if (btnSubmit) {
            btnSubmit.addEventListener('click', function () {
                openContactModal();
            });
        }
    }

    // ==========================================
    // CLICKABLE ELEMENTS (click on template to focus panel input)
    // ==========================================
    function initClickableElements() {
        var container = getTemplateContainer();
        if (!container) return;

        container.addEventListener('click', function (e) {
            var el = e.target.closest('[data-editor-field]');
            if (!el) return;

            e.preventDefault();
            e.stopPropagation();

            var fieldName = el.getAttribute('data-editor-field');
            var fieldType = el.getAttribute('data-editor-type');

            // Remove previous active
            container.querySelectorAll('.editor-active').forEach(function (a) {
                a.classList.remove('editor-active');
            });
            el.classList.add('editor-active');

            // Focus corresponding panel input
            var panelInput = document.querySelector('.editor-input[data-target="' + fieldName + '"]');
            if (panelInput) {
                panelInput.focus();
                panelInput.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }

            // For image fields, trigger file upload via the area click
            // (uses area.click() so the upload-lock guard in initImageUploads applies)
            if (fieldType === 'image') {
                var uploadArea = document.querySelector('.image-upload-area[data-target="' + fieldName + '"]');
                if (uploadArea) {
                    uploadArea.click();
                }
            }

            // Open mobile panel if on mobile
            if (window.innerWidth <= 768) {
                var panel = document.getElementById('editorPanel');
                var toggle = document.getElementById('mobilePanelToggle');
                if (panel) panel.classList.add('open');
                if (toggle) toggle.classList.add('open');
            }
        });
    }

    // ==========================================
    // CONTACT MODAL & SUBMISSION
    // ==========================================
    function openContactModal() {
        document.getElementById('contactModal').style.display = 'flex';
    }

    // Close contact modal
    var closeBtn = document.getElementById('closeContactModal');
    if (closeBtn) {
        closeBtn.addEventListener('click', function () {
            document.getElementById('contactModal').style.display = 'none';
        });
    }

    // Confirm submit
    var confirmBtn = document.getElementById('btnConfirmSubmit');
    if (confirmBtn) {
        confirmBtn.addEventListener('click', function () {
            submitEditorData();
        });
    }

    function submitEditorData() {
        var fullName = document.getElementById('contactFullName').value.trim();
        var phone = document.getElementById('contactPhone').value.trim();
        var whatsApp = document.getElementById('contactWhatsApp').value.trim();
        var email = document.getElementById('contactEmail').value.trim();

        if (!fullName || !phone || !email) {
            alert('يرجى ملء جميع البيانات المطلوبة');
            return;
        }

        // Collect all data
        var groomName = getInputValue('GroomName');
        var brideName = getInputValue('BrideName');

        if (!groomName || !brideName) {
            alert('يرجى إدخال اسم العريس والعروسة');
            return;
        }

        // Build submit DTO
        var dto = {
            fullName: fullName,
            phoneNumber: phone,
            whatsAppNumber: whatsApp || phone,
            email: email,
            designId: config.designId,

            groomName: groomName,
            brideName: brideName,
            groomImageUrl: uploadedFiles['GroomImage'] ? uploadedFiles['GroomImage'].url : null,
            groomImageId: uploadedFiles['GroomImage'] ? uploadedFiles['GroomImage'].id : null,
            brideImageUrl: uploadedFiles['BrideImage'] ? uploadedFiles['BrideImage'].url : null,
            brideImageId: uploadedFiles['BrideImage'] ? uploadedFiles['BrideImage'].id : null,
            groomFacebook: getInputValue('GroomFacebook') || null,
            groomInstagram: getInputValue('GroomInstagram') || null,
            groomX: null,
            brideFacebook: getInputValue('BrideFacebook') || null,
            brideInstagram: getInputValue('BrideInstagram') || null,
            brideX: null,

            mainSliderImageUrl: uploadedFiles['MainSliderImage'] ? uploadedFiles['MainSliderImage'].url : null,
            mainSliderImageId: uploadedFiles['MainSliderImage'] ? uploadedFiles['MainSliderImage'].id : null,

            eventDate: getInputValue('EventDate') || new Date().toISOString(),
            eventTimeFrom: getInputValue('EventTimeFrom') || '18:00',
            eventTimeTo: getInputValue('EventTimeTo') || '23:00',
            eventPlaceName: getInputValue('EventPlaceName') || '',
            eventAddress: getInputValue('EventAddress') || '',
            locationUrl: getInputValue('LocationUrl') || null,
            eventPlaceImageUrl: uploadedFiles['EventPlaceImage'] ? uploadedFiles['EventPlaceImage'].url : null,
            eventPlaceImageId: uploadedFiles['EventPlaceImage'] ? uploadedFiles['EventPlaceImage'].id : null,

            galleryPhotos: galleryPhotos.filter(function (p) { return !p.uploading && p.id; }).map(function (p) {
                return { photoUrl: p.url, photoId: p.id };
            }),

            memories: memories.filter(function (m) { return m.title; }).map(function (m, i) {
                return {
                    title: m.title,
                    eventDate: m.date || new Date().toISOString(),
                    imageUrl: m.imageUrl || null,
                    imageId: m.imageId || null,
                    displayOrder: i
                };
            }),

            dedications: dedications.filter(function (d) { return d.personName && d.message; }).map(function (d, i) {
                return {
                    personName: d.personName,
                    relationship: d.relationship || null,
                    message: d.message,
                    displayOrder: i
                };
            }),

            audioUrl: audioData ? audioData.url : null,
            audioId: audioData ? audioData.id : null,
            audioName: getInputValue('AudioName') || (audioData ? audioData.name : null),
            audioAutoPlay: document.getElementById('audioAutoPlay') ? document.getElementById('audioAutoPlay').checked : false,

            customSentence1: getInputValue('CustomSentence1') || null,
            customSentence2: getInputValue('CustomSentence2') || null,
            backgroundImageUrl: getSelectedBgUrl(),
            backgroundImageId: null,
            backgroundImageLibraryId: getSelectedBgId()
        };

        // Show loading
        document.getElementById('contactModal').style.display = 'none';
        document.getElementById('loadingOverlay').style.display = 'flex';

        // Submit
        fetch(config.submitUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        })
            .then(function (res) { return res.json(); })
            .then(function (data) {
                document.getElementById('loadingOverlay').style.display = 'none';
                if (data.success) {
                    showSuccessModal(data.invitationUrl);
                } else {
                    alert(data.message || 'حدث خطأ');
                }
            })
            .catch(function () {
                document.getElementById('loadingOverlay').style.display = 'none';
                alert('حدث خطأ أثناء الإرسال. يرجى المحاولة مرة أخرى.');
            });
    }

    function getInputValue(target) {
        var input = document.querySelector('.editor-input[data-target="' + target + '"]');
        return input ? input.value.trim() : '';
    }

    function getSelectedBgUrl() {
        var selected = document.querySelector('.bg-lib-item.selected');
        return selected ? selected.getAttribute('data-url') : null;
    }

    function getSelectedBgId() {
        var selected = document.querySelector('.bg-lib-item.selected');
        return selected ? selected.getAttribute('data-id') : null;
    }

    function showSuccessModal(invitationUrl) {
        var modal = document.getElementById('successModal');
        if (modal) {
            modal.style.display = 'flex';
            if (invitationUrl) {
                var details = document.getElementById('successDetails');
                var link = document.getElementById('successInvitationUrl');
                if (details && link) {
                    link.href = invitationUrl;
                    link.textContent = invitationUrl;
                    details.style.display = 'block';
                }
            }
        }
    }

    // ==========================================
    // TEMPLATE SCALING (fit template to canvas width)
    // ==========================================
    function initTemplateScaling() {
        // With iframe approach, template is self-contained and doesn't need scaling
        var iframe = document.getElementById('templateFrame');
        if (iframe && iframe.src) return;

        var canvas = document.getElementById('editorCanvas');
        var container = getTemplateContainer();
        if (!canvas || !container) return;

        var templateWidth = 1400; // Fixed template render width (matches full viewport)

        function scaleTemplate() {
            // On mobile, don't scale - let it be full width
            if (window.innerWidth <= 768) {
                container.style.width = '100%';
                container.style.transform = 'none';
                container.style.marginBottom = '';
                return;
            }

            var canvasWidth = canvas.clientWidth;
            var scale = canvasWidth / templateWidth;

            // Don't scale up, only down
            if (scale >= 1) {
                container.style.width = '100%';
                container.style.transform = 'none';
                container.style.marginBottom = '';
                return;
            }

            // First set the width so the content reflows correctly
            container.style.transform = 'none';
            container.style.width = templateWidth + 'px';

            // Let the browser reflow, then measure and scale
            requestAnimationFrame(function () {
                var naturalHeight = container.scrollHeight;
                container.style.transform = 'scale(' + scale + ')';
                // Use negative margin to compensate for the space the unscaled element takes
                var scaledHeight = naturalHeight * scale;
                var diff = naturalHeight - scaledHeight;
                container.style.marginBottom = '-' + diff + 'px';
            });
        }

        // Scale on load (with delay for images)
        setTimeout(scaleTemplate, 300);

        // Re-scale on window resize
        var resizeTimer;
        window.addEventListener('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(scaleTemplate, 100);
        });

        // Re-scale when images load
        container.querySelectorAll('img').forEach(function (img) {
            img.addEventListener('load', function () {
                setTimeout(scaleTemplate, 100);
            });
        });

        // Re-scale when content changes (dynamic sections)
        var observerTimer;
        var observer = new MutationObserver(function () {
            clearTimeout(observerTimer);
            observerTimer = setTimeout(scaleTemplate, 300);
        });
        observer.observe(container, { childList: true, subtree: true });
    }

})();
