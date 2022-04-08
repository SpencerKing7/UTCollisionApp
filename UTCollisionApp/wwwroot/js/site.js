const delement = document.getElementById("deleteSpan")
const celement = document.getElementById("confirmDeleteSpan")

delement.addEventListener("click", confirmDelete)
celement.addEventListener("click", confirmDelete)

function confirmDelete(uniqueId, isTrue) {

    document.getElementById("deleteSpan" = 'deleteSpan_' + uniqueId);
    document.getElementById("confirmDeleteSpan" = 'confirmDeleteSpan_' + uniqueId);

    if (isTrue) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}




