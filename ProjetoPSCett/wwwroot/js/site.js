// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var elemento = document.getElementById("saudacao");
var saudacao = "", d = new Date().getHours();

if (d <= 12) {
    saudacao = "Bom dia";
}
else if (d <= 18) {
    saudacao = "Boa tarde";
}
else {
    saudacao = "Boa noite";
}
elemento.innerHTML = saudacao + ", seja bem-vindo!";

function abreComentarios() {
    var comentarios = document.getElementById("comentarios");
    if (comentarios.classList.contains("d-none")) {
        comentarios.classList.remove("d-none")
        comentarios.classList.add("d-block")
    }
    else{
        comentarios.classList.add("d-none")
        comentarios.classList.remove("d-block")
    }
}
