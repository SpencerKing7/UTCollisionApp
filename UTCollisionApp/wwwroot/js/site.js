// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function removeDiv() {
    document.getElementById("cookie").style.display = "none";
}

$('.close-div').click(function () {
    $(this).parent().parent().remove();
});

function supports_storage() {
    try {
        return 'localStorage' in window && window['localStorage'] !== null;
    } catch (e) {
        return false;
    }
}

if (supports_storage()) localStorage['sitename.nofullscreen'] = true;

if (localStorage['sitename.nofullscreen'] == "true") {
    //go right to #page
}

$(document).ready(function () {
    if (localStorage['nofullscreen'] == 'true') $('.fullscreen').hide();
});