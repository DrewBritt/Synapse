function newTeacher(name, id) {
    let teacherid = document.getElementById("teacherid");
    teacherid.setAttribute("value", id);

    let currentlySelectedTeacherText = document.getElementById("currentlySelectedTeacherText");
    currentlySelectedTeacherText.textContent = "Currently Selected: " + name;

    closeModal();
}

$(document).ready(function () {
    $('#selectTeacherBtn').click(function () {
        var url = $('#teachersModal').data('url');

        $.get(url, function (data) {
            $('#teachersModalContainer').html(data);

            var teacherSearchModal = document.getElementById('selectTeacherModal');
            teacherSearchModal.classList.toggle('is-active');
        });
    });
});

function closeModal() {
    var modal = document.getElementById('selectTeacherModal');

    modal.parentNode.removeChild(modal);
}