document.addEventListener('DOMContentLoaded', function () {
    const editBtn = document.getElementById("editBtn");
    if (editBtn.addEventListener) editBtn.addEventListener('click', editClass, false);
});


function editClass() {
    const teacher = document.getElementById("teacher");
    const teacherLabel = document.getElementById("teacherLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("value", teacher.textContent);
    x.setAttribute("name", "teacher");
    x.setAttribute("id", "teacherInput");
    x.classList.add("input");
    teacherLabel.appendChild(x);
    teacher.setAttribute("hidden", "true");

    const location = document.getElementById("location");
    const locationLabel = document.getElementById("locationLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("name", "location");
    x.setAttribute("value", location.textContent);
    x.setAttribute("id", "locationInput");
    x.classList.add("input");
    locationLabel.appendChild(x);
    location.setAttribute("hidden", "true");

    const period = document.getElementById("period");
    const periodLabel = document.getElementById("periodLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("name", "period");
    x.setAttribute("value", period.textContent);
    x.classList.add("input");
    x.setAttribute("id", "periodInput")
    periodLabel.appendChild(x);
    period.setAttribute("hidden", "true")

    const btns = document.getElementById("btns");
    const editBtn = document.getElementById("editBtn");
    let submitBtn = document.createElement("BUTTON");
    submitBtn.setAttribute("type", "submit");
    submitBtn.setAttribute("id", "submitBtn")
    submitBtn.textContent = "Submit";
    submitBtn.classList.add("button");
    submitBtn.classList.add("is-success");
    btns.appendChild(submitBtn);

    let cancelBtn = document.createElement("BUTTON");
    cancelBtn.setAttribute("type", "cancel");
    cancelBtn.setAttribute("id", "cancelBtn")
    cancelBtn.textContent = "Cancel";
    cancelBtn.classList.add("button");
    cancelBtn.classList.add("is-danger");
    cancelBtn.addEventListener('click', cancel, false)
    btns.appendChild(cancelBtn);

    editBtn.parentNode.removeChild(editBtn);
}

function cancel() {
    let btns = document.getElementById("btns");
    let submitBtn = document.getElementById("submitBtn");
    let cancelBtn = document.getElementById("cancelBtn");
    let teacherInput = document.getElementById("teacherInput");
    let locationInput = document.getElementById("locationInput");
    let periodInput = document.getElementById("periodInput");
    let teacher = document.getElementById("teacher");
    let location = document.getElementById("location");
    let period = document.getElementById("period");

    let editBtn = document.createElement("BUTTON");
    editBtn.classList.add("button")
    editBtn.classList.add("is-primary")
    editBtn.setAttribute("id", "editBtn");
    editBtn.textContent = "Edit";
    btns.appendChild(editBtn);
    editBtn.addEventListener('click', editClass, false);

    teacher.removeAttribute("hidden");
    location.removeAttribute("hidden");
    period.removeAttribute("hidden");

    teacherInput.parentNode.removeChild(teacherInput);
    locationInput.parentNode.removeChild(locationInput);
    periodInput.parentNode.removeChild(periodInput);

    btns.removeChild(cancelBtn);
    btns.removeChild(submitBtn);
}

function insertAfter(el, referenceNode) {
    referenceNode.parentNode.insertBefore(el, referenceNode.nextSibling);
}

function dropdownFunc() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function filterFunction() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    div = document.getElementById("myDropdown");
    a = div.getElementsByTagName("option");
    for (i = 0; i < a.length; i++) {
        txtValue = a[i].textContent || a[i].innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            a[i].style.display = "";
        } else {
            a[i].style.display = "none";
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const modalButtons = document.getElementsByClassName("modalClose");
    let teacherModal = document.getElementById("teacherSearchModal");

    for (let i = 0; i < modalButtons.length; i++) {
        modalButtons[i].addEventListener('click', function () {
            teacherModal.classList.toggle('is-active');
        });
    }

});