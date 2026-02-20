var ModernAlert = (function () {
    // Private variables
    var confirmCallback = null;
    var toastContainer = null;
    var autoCloseTimer = null;

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
                <div class="modern-modal-footer">
                    <button onclick="ModernAlert.closeAlert()" class="modern-btn modern-btn-ok" id="alertOkBtn">
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
                <div class="modern-modal-footer">
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

    // Public methods
    return {
        showAlert: function (title, message, type, autoClose = false, duration = 3000) {
            initializeModals();

            var modal = document.getElementById('modernAlert');
            var alertTitle = document.getElementById('alertTitle');
            var alertMessage = document.getElementById('alertMessage');
            var alertIcon = document.getElementById('alertIcon');
            var modalContent = modal.querySelector('.modern-modal-content');
            var progressBar = document.getElementById('alertProgress');
            var okBtn = document.getElementById('alertOkBtn');

            // Remove previous type classes
            modalContent.classList.remove('modern-success', 'modern-error', 'modern-warning', 'modern-info', 'modern-confirm');

            // Set icon and wrapper class
            var iconData = getIconAndWrapper(type);
            alertIcon.className = 'fas ' + iconData.icon;
            modalContent.classList.add(iconData.wrapper);

            alertTitle.textContent = title;
            alertMessage.textContent = message;

            // Handle auto-close
            if (autoCloseTimer) {
                clearTimeout(autoCloseTimer);
            }

            if (autoClose) {
                progressBar.style.display = 'block';
                progressBar.style.animation = `modernProgress ${duration / 1000}s linear`;

                autoCloseTimer = setTimeout(() => {
                    this.closeAlert();
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
                    }, duration);
                });
            } else {
                progressBar.style.display = 'none';
            }

            modal.style.display = 'block';
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

        showConfirm: function (message, title, callback) {
            initializeModals();

            var modal = document.getElementById('modernConfirm');
            var confirmTitle = document.getElementById('confirmTitle');
            var confirmMessage = document.getElementById('confirmMessage');

            confirmTitle.textContent = title || 'Confirmation';
            confirmMessage.textContent = message;

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

        // Convenience methods
        showSuccess: function (message, title, autoClose = false) {
            this.showAlert(title || 'Success!', message, 'success', autoClose);
        },

        showError: function (message, title, autoClose = false) {
            this.showAlert(title || 'Error!', message, 'error', autoClose);
        },

        showWarning: function (message, title, autoClose = false) {
            this.showAlert(title || 'Warning!', message, 'warning', autoClose);
        },

        showInfo: function (message, title, autoClose = false) {
            this.showAlert(title || 'Information', message, 'info', autoClose);
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
            var okBtn = document.getElementById('alertOkBtn');

            modalContent.classList.remove('modern-success', 'modern-error', 'modern-warning', 'modern-info', 'modern-confirm');
            modalContent.classList.add('modern-info');

            alertIcon.className = 'fas fa-spinner fa-spin';
            alertTitle.textContent = 'Please wait...';
            alertMessage.textContent = message || 'Processing your request';

            okBtn.style.display = 'none';
            modal.style.display = 'block';

            return {
                close: function () {
                    ModernAlert.closeAlert();
                    okBtn.style.display = 'inline-block';
                },
                update: function (newMessage) {
                    alertMessage.textContent = newMessage;
                }
            };
        }
    };
})();

window.ModernAlert = ModernAlert;

window.onclick = function (event) {
    var alertModal = document.getElementById('modernAlert');
    var confirmModal = document.getElementById('modernConfirm');

    if (event.target === alertModal) {
        ModernAlert.closeAlert();
    }
    if (event.target === confirmModal) {
        ModernAlert.closeConfirm();
    }
};

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        ModernAlert.closeAlert();
        ModernAlert.closeConfirm();
    }
});