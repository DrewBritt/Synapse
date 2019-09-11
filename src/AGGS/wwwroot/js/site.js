//Provides functionality for filtering through table of students
function studentsTableSearchBar(tableId, searchBarId) {
    var input, filter, table, tr, firstName, lastName, i, firstNameValue, lastNameValue;
    input = document.getElementById(searchBarId);
    filter = input.value.toUpperCase();
    table = document.getElementById(tableId);
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        firstName = tr[i].getElementsByTagName("td")[0];
        lastName = tr[i].getElementsByTagName("td")[1];

        if (firstName && lastName) {
            firstNameValue = firstName.textContent.toUpperCase();
            lastNameValue = lastName.textContent.toUpperCase();
            if (firstNameValue.indexOf(filter) > -1 || lastNameValue.indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

//Provides functionality for filtering through table of classes
function classesTableSearchBar(tableId, searchBarId) {
    var input, filter, table, tr, className, i, classNameValue;
    input = document.getElementById(searchBarId);
    filter = input.value.toUpperCase();
    table = document.getElementById(tableId);
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        className = tr[i].getElementsByTagName("td")[0];

        if (className) {
            classNameValue = className.textContent.toUpperCase();
            if ((classNameValue.indexOf(filter)) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

//Bulma Functionality for navbar
document.addEventListener('DOMContentLoaded', function () {
    var navbarBurgers = document.getElementsByClassName('navbar-burger')
    navbarBurgers = navbarBurgers[0]
    var menu = document.getElementById('navbarMain')

    navbarBurgers.addEventListener('click', function () {
        navbarBurgers.classList.toggle('is-active');
        menu.classList.toggle('is-active');
    })
});


function pageInit() {
    const editBtn = document.getElementById("editBtn");
    if (editBtn.addEventListener) editBtn.addEventListener('click', editClass, false);
}

function editClass() {
    const teacher = document.getElementById("teacher");
    const teacherLabel = document.getElementById("teacherLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("value", teacher.textContent);
    x.setAttribute("name", "teacher");
    x.classList.add("input");
    teacherLabel.appendChild(x);
    teacher.parentNode.removeChild(teacher);

    const location = document.getElementById("location");
    const locationLabel = document.getElementById("locationLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("name", "location");
    x.setAttribute("value", location.textContent);
    x.classList.add("input");
    locationLabel.appendChild(x);
    location.parentNode.removeChild(location);

    const period = document.getElementById("period");
    const periodLabel = document.getElementById("periodLabel");
    var x = document.createElement("INPUT");
    x.setAttribute("type", "text");
    x.setAttribute("name", "period");
    x.setAttribute("value", period.textContent);
    x.classList.add("input");
    periodLabel.appendChild(x);
    period.parentNode.removeChild(period);

    const btns = document.getElementById("btns");
    const editBtn = document.getElementById("editBtn");
    let submitBtn = document.createElement("BUTTON");
    submitBtn.setAttribute("type", "submit");
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
    btns.appendChild(cancelBtn);

    editBtn.parentNode.removeChild(editBtn);
}