function isNumber(evt) {
    evt = evt || window.event;
    let charCode = evt.which || evt.keyCode;

    if (charCode >= 48 && charCode <= 57) {
        return true;
    }

    return false;
}

function ValidatePAN(res) {
    if ($('#' + res.id).val() != "") {
        var pan = $('#' + res.id).val().trim().toUpperCase();
        var panPattern = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
        if (!panPattern.test(pan)) {
            ModernAlert.toast.info('Invalid PAN format (example: ABCDE1234F)');
            $('#' + res.id).val("");
            return false;
        }
    }
}
function ValidateAadhaar(res) {
    var aadhaar = document.getElementById(res.id);
    var value = aadhaar.value.trim();
    if (value === "") return true; 
    if (!/^[0-9]{12}$/.test(value)) {
        ModernAlert.toast.info('Aadhaar must be 12 digits');
        $('#' + res.id).val("");
        return false;
    }
}
function toProperCase(str) {
    return str
        .toLowerCase()
        .trim()
        .split(/\s+/)
        .map(word => word.charAt(0).toUpperCase() + word.slice(1))
        .join(' ');
}
function ValidatePIN(res) {
    var aadhaar = document.getElementById(res.id);
    var value = aadhaar.value.trim();
    if (value === "") return true;
    if (value.length != 6) {
        ModernAlert.toast.info('Invalid PIN Code');
        $('#' + res.id).val("");
    }
}