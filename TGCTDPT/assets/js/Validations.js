function isNumber(evt) {
    evt = evt || window.event;
    let charCode = evt.which || evt.keyCode;

    if (charCode >= 48 && charCode <= 57) {
        return true;
    }

    return false;
}