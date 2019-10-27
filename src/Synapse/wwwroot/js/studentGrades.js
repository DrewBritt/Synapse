function openGrades(period, periodDropdown) {
    let dropdown = document.getElementById(periodDropdown);
    if (dropdown.classList.contains("fa-chevron-up")) {
        let table = document.getElementById(`${period}`);
        table.parentNode.removeChild(table);
    } else {
        const url = $(`#${period}gradesTable`).data('url');
        console.log(url);
        $.get(url, function (data) {
            $(`#${period}container`).html(data);
        });
    }
    
    dropdown.classList.toggle("fa-chevron-down");
    dropdown.classList.toggle("fa-chevron-up");
}
