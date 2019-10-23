function newStudent(name, id) {
    let studentid = document.getElementById("studentid");
    studentid.setAttribute("value", id);

    let currentlySelectedStudentText = document.getElementById("currentlySelectedStudentText");
    currentlySelectedStudentText.textContent = "Currently Selected: " + name;

    closeModal();
}

$(document).ready(function () {
    $('#selectStudentBtn').click(function () {
        var url = $('#studentsModal').data('url');

        $.get(url, function (data) {
            $('#studentsModalContainer').html(data);

            var studentSearchModal = document.getElementById('selectStudentModal');
            studentSearchModal.classList.toggle('is-active');
        });
    });
});

function closeModal() {
    var modal = document.getElementById('selectStudentModal');

    modal.parentNode.removeChild(modal);
}