

//const delement = document.getElementById("deleteSpan")
//const celement = document.getElementById("confirmDeleteSpan")

//delement.addEventListener("click", confirmDelete)
//celement.addEventListener("click", confirmDelete)

//function confirmDelete() {

//    document.getElementById("deleteSpan");
//    document.getElementById("confirmDeleteSpan");

//    if (isTrue) {
//        $('#' + deleteSpan).hide();
//        $('#' + confirmDeleteSpan).show();
//    } else {
//        $('#' + deleteSpan).show();
//        $('#' + confirmDeleteSpan).hide();
//    }
//}

//function hide() {
//    document.getElementById("confirmDeleteSpan").style.display = 'none';
//    console.log("It worked")
//}

function confirmDelete(e) {
    if (confirm("Confirm deletion of this user?"))
        alert('User Deleted');
    else {
        alert('Cancelled');
        e.preventDefault();
    }
}

function confirmDeleteRole(e) {
    if (confirm("Confirm deletion of this user?"))
        alert('Role Deleted');
    else {
        alert('Cancelled');
        e.preventDefault();
    }
}




