document.addEventListener('DOMContentLoaded', function () {
    const modalButtons = document.getElementsByClassName("modalClose");
    let modal = document.getElementById("modal");

    for (let i = 0; i < modalButtons.length; i++) {
        modalButtons[i].addEventListener('click', function () {
            modal.classList.toggle('is-active');
        });
    }
});