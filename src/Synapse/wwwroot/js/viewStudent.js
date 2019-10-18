$(document).ready(function () {
    $('#showModalBtn').click(function () {
        var url = $('#classesModal').data('url');

        $.get(url, function (data) {
            $('#classesModalContainer').html(data);

            var classSearchModal = document.getElementById('classSearchModal');
            classSearchModal.classList.toggle('is-active');
        });
    });
});