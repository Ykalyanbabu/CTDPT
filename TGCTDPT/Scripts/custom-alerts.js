var CustomAlert = (function () {
    var confirmCallback = null;

    function initializeModals() {
        if (!document.getElementById('customAlert')) {
            createAlertModal();
        }
        if (!document.getElementById('customConfirm')) {
            createConfirmModal();
        }
    }

    function createAlertModal() {
        var modal = document.createElement('div');
        modal.id = 'customAlert';
        modal.className = 'modal';
        modal.innerHTML = `
            <div class="modal-content">
                <div class="modal-header" id="alertHeader">
                    <span class="close" onclick="CustomAlert.closeAlert()">&times;</span>
                    <h2 id="alertTitle"></h2>
                </div>
                <div class="modal-body">
                    <p id="alertMessage"></p>
                </div>
                <div class="modal-footer">
                    <button onclick="CustomAlert.closeAlert()" class="btn-ok">OK</button>
                </div>
            </div>
        `;
        document.body.appendChild(modal);
    }

    function createConfirmModal() {
        var modal = document.createElement('div');
        modal.id = 'customConfirm';
        modal.className = 'modal';
        modal.innerHTML = `
            <div class="modal-content">
                <div class="modal-header">
                    <span class="close" onclick="CustomAlert.closeConfirm()">&times;</span>
                    <h2 id="confirmTitle">Confirmation</h2>
                </div>
                <div class="modal-body">
                    <p id="confirmMessage"></p>
                </div>
                <div class="modal-footer">
                    <button onclick="CustomAlert.confirmAction(true)" class="btn-confirm">Yes</button>
                    <button onclick="CustomAlert.confirmAction(false)" class="btn-cancel">No</button>
                </div>
            </div>
        `;
        document.body.appendChild(modal);
    }

    return {
        showAlert: function (title, message, type) {
            initializeModals();
            var modal = document.getElementById('customAlert');
            var alertTitle = document.getElementById('alertTitle');
            var alertMessage = document.getElementById('alertMessage');
            var modalContent = modal.querySelector('.modal-content');

            modalContent.classList.remove('success', 'error', 'warning', 'info');
            modalContent.classList.add(type || 'info');

            alertTitle.textContent = title;
            alertMessage.textContent = message;

            modal.style.display = 'block';
        },

        closeAlert: function () {
            var modal = document.getElementById('customAlert');
            if (modal) {
                modal.style.display = 'none';
            }
        },

        showConfirm: function (message, title, callback) {
            initializeModals();
            var modal = document.getElementById('customConfirm');
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
            var modal = document.getElementById('customConfirm');
            if (modal) {
                modal.style.display = 'none';
            }
        },

        showSuccess: function (message, title) {
            this.showAlert(title || 'Success', message, 'success');
        },

        showError: function (message, title) {
            this.showAlert(title || 'Error', message, 'error');
        },

        showWarning: function (message, title) {
            this.showAlert(title || 'Warning', message, 'warning');
        },

        showInfo: function (message, title) {
            this.showAlert(title || 'Information', message, 'info');
        }
    };
})();

window.CustomAlert = CustomAlert;

window.onclick = function (event) {
    var alertModal = document.getElementById('customAlert');
    var confirmModal = document.getElementById('customConfirm');

    if (event.target === alertModal) {
        CustomAlert.closeAlert();
    }
    if (event.target === confirmModal) {
        CustomAlert.closeConfirm();
    }
};