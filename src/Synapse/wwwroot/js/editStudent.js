document.addEventListener('DOMContentLoaded', function () {
    const editBtn = document.getElementById("editBtn");
    if (editBtn.addEventListener) editBtn.addEventListener('click', editStudent, false);

    const modalButtons = document.getElementsByName("modalClose");
    let modal = document.getElementById("deleteStudentVerificationModal");

    for (let i = 0; i < modalButtons.length; i++) {
        modalButtons[i].addEventListener('click', function () {
            modal.classList.toggle("is-active");
        });
    }
});

function editStudent() {
    const name = document.getElementById("name");
    const nameLabel = document.getElementById("nameLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("value", name.textContent);
    x.setAttribute("name", "name");
    x.setAttribute("id", "nameInput");
    x.classList.add("input");
    nameLabel.appendChild(x);
    name.setAttribute("hidden", "true");

    const email = document.getElementById("email");
    const emailLabel = document.getElementById("emailLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("name", "email");
    x.setAttribute("value", email.textContent);
    x.setAttribute("id", "emailInput");
    x.classList.add("input");
    emailLabel.appendChild(x);
    email.setAttribute("hidden", "true");

    const grade = document.getElementById("gradelevel");
    const gradeLabel = document.getElementById("gradeLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "number");
    x.setAttribute("name", "gradelevel");
    x.setAttribute("value", grade.textContent);
    x.classList.add("input");
    x.setAttribute("id", "gradeInput")
    gradeLabel.appendChild(x);
    grade.setAttribute("hidden", "true")

    const btns = document.getElementById("btns");
    const editBtn = document.getElementById("editBtn");
    let submitBtn = document.createElement("INPUT");
    submitBtn.setAttribute("type", "submit");
    submitBtn.setAttribute("id", "submitBtn")
    submitBtn.textContent = "Submit";
    submitBtn.classList.add("button");
    submitBtn.classList.add("is-success");
    btns.appendChild(submitBtn);

    let deleteBtn = document.getElementById("deleteBtn");
    deleteBtn.setAttribute("hidden","true");


    let cancelBtn = document.createElement("BUTTON");
    cancelBtn.setAttribute("type", "button");
    cancelBtn.setAttribute("id", "cancelBtn")
    cancelBtn.textContent = "Cancel";
    cancelBtn.classList.add("button");
    cancelBtn.classList.add("is-danger");
    btns.appendChild(cancelBtn);

    cancelBtn.addEventListener('click', cancel, false)
    editBtn.parentNode.removeChild(editBtn);
    deleteBtn.parentNode.removeChild(deleteBtn);
}

function cancel() {
    let btns = document.getElementById("btns");
    let submitBtn = document.getElementById("submitBtn");
    let cancelBtn = document.getElementById("cancelBtn");
    let nameInput = document.getElementById("nameInput");
    let emailInput = document.getElementById("emailInput");
    let gradeInput = document.getElementById("gradeInput");
    let name = document.getElementById("name");
    let email = document.getElementById("email");
    let grade = document.getElementById("gradelevel");

    let editBtn = document.createElement("BUTTON");
    editBtn.setAttribute("type", "button");
    editBtn.classList.add("button");
    editBtn.classList.add("is-primary");
    editBtn.setAttribute("id", "editBtn");
    editBtn.textContent = "Edit";
    btns.appendChild(editBtn);
    editBtn.addEventListener('click', editStudent, false);

    let deleteBtn = document.getElementById("deleteBtn");
    deleteBtn.setAttribute("hidden", "false");

    name.removeAttribute("hidden");
    email.removeAttribute("hidden");
    grade.removeAttribute("hidden");

    nameInput.parentNode.removeChild(nameInput);
    emailInput.parentNode.removeChild(emailInput);
    gradeInput.parentNode.removeChild(gradeInput);

    btns.removeChild(cancelBtn);
    btns.removeChild(submitBtn);
}

function deleteStudentVerification() {
    let modal = document.getElementById("deleteStudentVerificationModal");
    modal.classList.add("is-active");
}