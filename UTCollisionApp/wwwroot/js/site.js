
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




