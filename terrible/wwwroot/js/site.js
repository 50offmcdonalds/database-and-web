// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//dropdown menu
function dropdown(dropMenu)
{
    document.getElementById(dropMenu).classList.toggle("show");
}

//image uploading
function readURL(input) {
    var reader = new FileReader();
    reader.onload = function (e) {
        document.getElementById("profile").setAttribute("src", e.target.result);
    };
    reader.readAsDataURL(input.files[0]);
}

//set image on user index
function setProfile(filepath) {
    document.getElementById("profile").setAttribute("src", filepath);
}