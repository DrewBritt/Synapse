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