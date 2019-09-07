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

        if (firstName || lastName) {
            firstNameValue = firstName.textContent || firstName.innerText;
            lastNameValue = lastName.textContent || lastName.innerText;
            if ((firstNameValue.toUpperCase().indexOf(filter) && lastNameValue.toUpperCase().indexOf(filter)) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}