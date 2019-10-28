document.addEventListener('DOMContentLoaded', function () {
    const editBtn = document.getElementById("editBtn");
    if (editBtn.addEventListener) editBtn.addEventListener('click', editClass, false);

    const modalButtons = document.getElementsByClassName("modalClose");
    let deleteClassModal = document.getElementById("deleteClassVerificationModal");
    let teacherModal = document.getElementById("teacherSearchModal");

    for (let i = 0; i < modalButtons.length; i++) {
        if (modalButtons[i].name == "classVerificationModalClose") {
            modalButtons[i].addEventListener('click', function () {
                deleteClassModal.classList.toggle("is-active");
            });
        } else {
            modalButtons[i].addEventListener('click', function () {
                teacherModal.classList.toggle("is-active");
            });
        }
    }
});


function editClass() {
    const modalBtn = document.getElementById("modalButton");
    const teacher = document.getElementById("teacher");
    const teacherLabel = document.getElementById("teacherLabel");
    const modalButtonDiv = document.getElementById("modalButtonDiv");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("value", teacher.textContent);
    x.setAttribute("name", "teacher");
    x.setAttribute("id", "teacherInput");
    x.classList.add("input");
    //teacherLabel.appendChild(x);
    //teacher.setAttribute("hidden", "true");
    modalButtonDiv.removeAttribute("hidden");

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
    let editBtn = document.getElementById("editBtn");
    editBtn.classList.toggle("is-hidden");

    let submitBtn = document.createElement("INPUT");
    submitBtn.setAttribute("type", "submit");
    submitBtn.setAttribute("id", "submitBtn")
    submitBtn.value = "Submit";
    submitBtn.classList.add("button");
    submitBtn.classList.add("is-success");
    btns.appendChild(submitBtn);

    let deleteBtn = document.getElementById("deleteBtn");
    deleteBtn.classList.toggle("is-hidden", "true");

    let cancelBtn = document.createElement("BUTTON");
    cancelBtn.setAttribute("type", "button");
    cancelBtn.setAttribute("id", "cancelBtn")
    cancelBtn.textContent = "Cancel";
    cancelBtn.classList.add("button");
    cancelBtn.classList.add("is-danger");
    btns.appendChild(cancelBtn);

    cancelBtn.addEventListener('click', cancel, false)
    modalBtn.removeAttribute("hidden");
}

function cancel() {
    const modalBtn = document.getElementById("modalButtonDiv");
    let btns = document.getElementById("btns");
    let submitBtn = document.getElementById("submitBtn");
    let cancelBtn = document.getElementById("cancelBtn");
    let teacherInput = document.getElementById("teacherInput");
    let locationInput = document.getElementById("locationInput");
    let periodInput = document.getElementById("periodInput");
    let teacher = document.getElementById("teacher");
    let location = document.getElementById("location");
    let period = document.getElementById("period");
    let newTeacher = document.getElementById("newTeacher");
    if (newTeacher !== null) {
        teacher.removeAttribute("hidden");
        newTeacher.parentNode.removeChild(newTeacher);
    }

    let editBtn = document.getElementById("editBtn");
    editBtn.classList.toggle("is-hidden");

    let deleteBtn = document.getElementById("deleteBtn");
    deleteBtn.classList.toggle("is-hidden");

    location.removeAttribute("hidden");
    period.removeAttribute("hidden");
    modalBtn.setAttribute("hidden", "true");

    //teacherInput.parentNode.removeChild(teacherInput);
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


function newTeacher(name, id) {
    let teacherId = document.getElementById("teacherid");
    if (document.getElementById("newTeacher")) {
        let newTeacher = document.getElementById("newTeacher");
        newTeacher.parentNode.removeChild(newTeacher);
    }
    const teacherLabel = document.getElementById("teacherLabel");
    let teacher = document.getElementById("teacher");
    teacher.setAttribute("hidden", "true");
    let newSelecetedTeacher = document.createElement("DIV");
    newSelecetedTeacher.setAttribute("id", "newTeacher");
    newSelecetedTeacher.textContent = name;
    insertAfter(newSelecetedTeacher, teacherLabel);
    teacherId.setAttribute("value", id)
}

function deleteClassVerification() {
    let modal = document.getElementById("deleteClassVerificationModal");
    modal.classList.add("is-active");
}