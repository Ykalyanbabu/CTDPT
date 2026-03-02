// Modern Custom Alerts with Redirect Support for ASP.NET MVC 5
var ModernAlert = (function () {
    // Private variables
    var confirmCallback = null;
    var toastContainer = null;
    var autoCloseTimer = null;
    var redirectUrl = null;
    var redirectTarget = '_self'; // _self, _blank, or custom

    // Define functions first before using them
    function createAlertModal() {
        var modal = document.createElement('div');
        modal.id = 'modernAlert';
        modal.className = 'modern-modal';
        modal.innerHTML = `
            <div class="modern-modal-content">
                <div class="modern-close" onclick="ModernAlert.closeAlert()">
                    <i class="fas fa-times"></i>
                </div>
                <div class="modern-modal-header">
                    <div class="modern-icon-wrapper" id="alertIconWrapper">
                        <i class="fas" id="alertIcon"></i>
                    </div>
                    <h2 id="alertTitle"></h2>
                </div>
                <div class="modern-modal-body">
                    <p id="alertMessage"></p>
                </div>
                <div class="modern-modal-footer" id="alertFooter">
                    <button onclick="ModernAlert.handleAlertOk()" class="modern-btn modern-btn-ok" id="alertOkBtn">
                        <i class="fas fa-check"></i> OK
                    </button>
                </div>
                <div class="modern-progress-bar" id="alertProgress"></div>
            </div>
        `;
        document.body.appendChild(modal);
    }

    function createConfirmModal() {
        var modal = document.createElement('div');
        modal.id = 'modernConfirm';
        modal.className = 'modern-modal';
        modal.innerHTML = `
            <div class="modern-modal-content">
                <div class="modern-close" onclick="ModernAlert.closeConfirm()">
                    <i class="fas fa-times"></i>
                </div>
                <div class="modern-modal-header">
                    <div class="modern-icon-wrapper">
                        <i class="fas fa-question-circle"></i>
                    </div>
                    <h2 id="confirmTitle">Confirmation</h2>
                </div>
                <div class="modern-modal-body">
                    <p id="confirmMessage"></p>
                </div>
                <div class="modern-modal-footer" id="confirmFooter">
                    <button onclick="ModernAlert.confirmAction(true)" class="modern-btn modern-btn-confirm">
                        <i class="fas fa-check"></i> Yes
                    </button>
                    <button onclick="ModernAlert.confirmAction(false)" class="modern-btn modern-btn-cancel">
                        <i class="fas fa-times"></i> No
                    </button>
                </div>
            </div>
        `;
        document.body.appendChild(modal);
    }

    function createToastContainer() {
        toastContainer = document.createElement('div');
        toastContainer.id = 'toastContainer';
        toastContainer.className = 'modern-toast-container';
        document.body.appendChild(toastContainer);
    }

    // Initialize modals
    function initializeModals() {
        if (!document.getElementById('modernAlert')) {
            createAlertModal();
        }
        if (!document.getElementById('modernConfirm')) {
            createConfirmModal();
        }
        if (!document.getElementById('toastContainer')) {
            createToastContainer();
        }
    }

    // Get icon based on type
    function getIconAndWrapper(type) {
        var icons = {
            success: { icon: 'fa-check-circle', wrapper: 'modern-success' },
            error: { icon: 'fa-exclamation-circle', wrapper: 'modern-error' },
            warning: { icon: 'fa-exclamation-triangle', wrapper: 'modern-warning' },
            info: { icon: 'fa-info-circle', wrapper: 'modern-info' },
            confirm: { icon: 'fa-question-circle', wrapper: 'modern-confirm' }
        };
        return icons[type] || icons.info;
    }

    // Handle redirect based on configuration
    function executeRedirect() {
        if (redirectUrl) {
            if (redirectTarget === '_blank') {
                window.open(redirectUrl, '_blank');
            } else if (redirectTarget === '_self') {
                window.location.href = redirectUrl;
            } else if (typeof redirectTarget === 'function') {
                redirectTarget(); // Custom callback
            }
            redirectUrl = null;
            redirectTarget = '_self';
        }
    }

    // Public methods
    return {
        showAlert: function (title, message, type, options = {}) {
            initializeModals();

            var modal = document.getElementById('modernAlert');
            var alertTitle = document.getElementById('alertTitle');
            var alertMessage = document.getElementById('alertMessage');
            var alertIcon = document.getElementById('alertIcon');
            var modalContent = modal.querySelector('.modern-modal-content');
            var progressBar = document.getElementById('alertProgress');
            var okBtn = document.getElementById('alertOkBtn');
            var footer = document.getElementById('alertFooter');

            // Set redirect options
            redirectUrl = options.redirectUrl || null;
            redirectTarget = options.redirectTarget || '_self';

            // Remove previous type classes
            modalContent.classList.remove('modern-success', 'modern-error', 'modern-warning', 'modern-info', 'modern-confirm');

            // Set icon and wrapper class
            var iconData = getIconAndWrapper(type);
            alertIcon.className = 'fas ' + iconData.icon;
            modalContent.classList.add(iconData.wrapper);

            alertTitle.textContent = title;
            alertMessage.textContent = message;

            // Handle custom buttons
            if (options.buttons && options.buttons.length > 0) {
                footer.innerHTML = '';
                options.buttons.forEach(btn => {
                    var button = document.createElement('button');
                    button.className = `modern-btn ${btn.className || 'modern-btn-ok'}`;
                    button.innerHTML = btn.icon ? `<i class="fas ${btn.icon}"></i> ${btn.text}` : btn.text;
                    button.onclick = function () {
                        if (btn.redirectUrl) {
                            if (btn.redirectTarget === '_blank') {
                                window.open(btn.redirectUrl, '_blank');
                            } else {
                                window.location.href = btn.redirectUrl;
                            }
                        }
                        if (btn.callback) {
                            btn.callback();
                        }
                        ModernAlert.closeAlert();
                    };
                    footer.appendChild(button);
                });
            } else {
                // Reset to default OK button
                footer.innerHTML = `
                    <button onclick="ModernAlert.handleAlertOk()" class="modern-btn modern-btn-ok" id="alertOkBtn">
                        <i class="fas fa-check"></i> OK
                    </button>
                `;
            }

            // Handle auto-close
            if (autoCloseTimer) {
                clearTimeout(autoCloseTimer);
            }

            var autoClose = options.autoClose || false;
            var duration = options.duration || 3000;

            if (autoClose) {
                progressBar.style.display = 'block';
                progressBar.style.animation = `modernProgress ${duration / 1000}s linear`;

                autoCloseTimer = setTimeout(() => {
                    this.closeAlert();
                    if (options.onAutoClose) {
                        options.onAutoClose();
                    }
                    if (redirectUrl && !options.buttons) {
                        executeRedirect();
                    }
                }, duration);

                // Allow user to cancel auto-close by hovering
                modal.addEventListener('mouseenter', () => {
                    clearTimeout(autoCloseTimer);
                    progressBar.style.animation = 'none';
                });

                modal.addEventListener('mouseleave', () => {
                    progressBar.style.animation = `modernProgress ${duration / 1000}s linear`;
                    autoCloseTimer = setTimeout(() => {
                        this.closeAlert();
                        if (redirectUrl && !options.buttons) {
                            executeRedirect();
                        }
                    }, duration);
                });
            } else {
                progressBar.style.display = 'none';
            }

            modal.style.display = 'block';
        },

        handleAlertOk: function () {
            ModernAlert.closeAlert();
            if (redirectUrl) {
                executeRedirect();
            }
        },

        closeAlert: function () {
            var modal = document.getElementById('modernAlert');
            if (modal) {
                modal.style.display = 'none';
                if (autoCloseTimer) {
                    clearTimeout(autoCloseTimer);
                }
            }
        },

        showConfirm: function (message, title, callback, options = {}) {
            initializeModals();

            var modal = document.getElementById('modernConfirm');
            var confirmTitle = document.getElementById('confirmTitle');
            var confirmMessage = document.getElementById('confirmMessage');
            var footer = document.getElementById('confirmFooter');

            confirmTitle.textContent = title || 'Confirmation';
            confirmMessage.textContent = message;

            // Customize confirm buttons
            if (options.buttons) {
                footer.innerHTML = '';
                options.buttons.forEach(btn => {
                    var button = document.createElement('button');
                    button.className = `modern-btn ${btn.className || 'modern-btn-confirm'}`;
                    button.innerHTML = btn.icon ? `<i class="fas ${btn.icon}"></i> ${btn.text}` : btn.text;
                    button.onclick = function () {
                        if (btn.redirectUrl) {
                            if (btn.redirectTarget === '_blank') {
                                window.open(btn.redirectUrl, '_blank');
                            } else {
                                window.location.href = btn.redirectUrl;
                            }
                        }
                        if (btn.callback) {
                            btn.callback();
                        }
                        ModernAlert.closeConfirm();
                        if (callback) {
                            callback(btn.value === 'yes' || btn.value === true);
                        }
                    };
                    footer.appendChild(button);
                });
            }

            modal.style.display = 'block';
            confirmCallback = callback;
        },

        confirmAction: function (confirmed) {
            this.closeConfirm();
            if (confirmCallback) {
                confirmCallback(confirmed);
                confirmCallback = null;
            }
        },

        closeConfirm: function () {
            var modal = document.getElementById('modernConfirm');
            if (modal) {
                modal.style.display = 'none';
            }
        },

        // Convenience methods with redirect
        showSuccess: function (message, title, redirectUrl = null, redirectTarget = '_self') {
            this.showAlert(title || 'Success!', message, 'success', {
                redirectUrl: redirectUrl,
                redirectTarget: redirectTarget
            });
        },

        showError: function (message, title, redirectUrl = null, redirectTarget = '_self') {
            this.showAlert(title || 'Error!', message, 'error', {
                redirectUrl: redirectUrl,
                redirectTarget: redirectTarget
            });
        },

        showWarning: function (message, title, redirectUrl = null, redirectTarget = '_self') {
            this.showAlert(title || 'Warning!', message, 'warning', {
                redirectUrl: redirectUrl,
                redirectTarget: redirectTarget
            });
        },

        showInfo: function (message, title, redirectUrl = null, redirectTarget = '_self') {
            this.showAlert(title || 'Information', message, 'info', {
                redirectUrl: redirectUrl,
                redirectTarget: redirectTarget
            });
        },

        // Toast notifications
        showToast: function (message, type = 'info', duration = 3000) {
            initializeModals();

            var toast = document.createElement('div');
            toast.className = `modern-toast ${type}`;

            var icon = getIconAndWrapper(type).icon;

            toast.innerHTML = `
                <i class="fas ${icon}"></i>
                <span class="modern-toast-message">${message}</span>
                <span class="modern-toast-close" onclick="this.parentElement.remove()">
                    <i class="fas fa-times"></i>
                </span>
            `;

            toastContainer.appendChild(toast);

            setTimeout(() => {
                if (toast.parentNode) {
                    toast.remove();
                }
            }, duration);
        },

        toast: {
            success: function (message) {
                ModernAlert.showToast(message, 'success');
            },
            error: function (message) {
                ModernAlert.showToast(message, 'error');
            },
            warning: function (message) {
                ModernAlert.showToast(message, 'warning');
            },
            info: function (message) {
                ModernAlert.showToast(message, 'info');
            }
        },

        // Loading/Progress alerts
        showLoading: function (message) {
            initializeModals();

            var modal = document.getElementById('modernAlert');
            var alertTitle = document.getElementById('alertTitle');
            var alertMessage = document.getElementById('alertMessage');
            var alertIcon = document.getElementById('alertIcon');
            var modalContent = modal.querySelector('.modern-modal-content');
            var footer = document.getElementById('alertFooter');

            modalContent.classList.remove('modern-success', 'modern-error', 'modern-warning', 'modern-info', 'modern-confirm');
            modalContent.classList.add('modern-info');

            alertIcon.className = 'fas fa-spinner fa-spin';
            alertTitle.textContent = 'Please wait...';
            alertMessage.textContent = message || 'Processing your request';

            footer.innerHTML = ''; // Remove buttons during loading
            modal.style.display = 'block';

            return {
                close: function () {
                    ModernAlert.closeAlert();
                },
                update: function (newMessage) {
                    alertMessage.textContent = newMessage;
                }
            };
        }
    };
})();

// Make it globally available
window.ModernAlert = ModernAlert;

// Close modals when clicking outside
/*window.onclick = function (event) {
    var alertModal = document.getElementById('modernAlert');
    var confirmModal = document.getElementById('modernConfirm');

    if (event.target === alertModal) {
        ModernAlert.closeAlert();
    }
    if (event.target === confirmModal) {
        ModernAlert.closeConfirm();
    }
};*/

// Handle escape key
document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        ModernAlert.closeAlert();
        ModernAlert.closeConfirm();
    }
});