function ConfirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
    if (isDeleteClicked) {
        $('#' + deleteSpan_.hide());
        $('#' + confirmDeleteSpan.show());
    }
    else {
        $('#' + deleteSpan.show());
        $('#' + confirmDeleteSpan.hide());
    }
}