"use scrict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gradesHub").build();

function disableInputs() {
    let inputs = document.getElementsByTagName("input");
    for (let i = 0; i < inputs.length; i++) {
        inputs[i].setAttribute("disabled", "true");
    }
}

function enableInputs() {
    let inputs = document.getElementsByTagName("input");
    for (let i = 0; i < inputs.length; i++) {
        inputs[i].removeAttribute("disabled");
    }
}

//Disable updating of grades until SignalR connection is established
disableInputs();

//Enable grades input when SignalR connection is established
connection.start().then(function () {
    enableInputs();
}).catch(function (errMessage) {
    return console.error(errMessage.toString());
})

    // $('#element').donetyping(callback[, timeout=1000])
    // Fires callback when a user has finished typing. This is determined by the time elapsed
    // since the last keystroke and timeout parameter or the blur event--whichever comes first.
    //   @callback: function to be called when even triggers
    //   @timeout:  (default=1000) timeout, in ms, to to wait before triggering event if not
    //              caused by blur.
    // Requires jQuery 1.7+
    ; (function ($) {
        $.fn.extend({
            donetyping: function (callback, timeout) {
                timeout = timeout || 50; // 50ms timeout
                var timeoutReference,
                    doneTyping = function (el) {
                        if (timeoutReference || changeSection.value == "View Grades" || changeSection.value == "Set Categories") {
                            timeoutReference = null;
                            callback.call(el);
                            if (changeSection.value == "View Grades") {
                                onCallback(el);
                            } else if (changeSection.value == "Set Categories") {
                                tabulateWeights(el);
                            }
                        } else {
                            return;
                        }
                    };
                return this.each(function (i, el) {
                    var $el = $(el);
                    
                    $el.is(':input') && $el.on('keyup keypress paste', function (e) {
                        // This catches the backspace button in chrome, but also prevents
                        // the event from triggering too preemptively. Without this line,
                        // using tab/shift+tab will make the focused element fire the callback.
                        if (e.type == 'keyup' && e.keyCode != 8) return;

                        // Check if timeout has been set. If it has, "reset" the clock and
                        // start over again.
                        if (timeoutReference) clearTimeout(timeoutReference);
                        timeoutReference = setTimeout(function () {
                            // if we made it here, timeout has elapsed. Fire the
                            // callback
                            doneTyping(el);
                        }, timeout);
                    }).on('focusout', function () {
                        // If we can, fire event since we're leaving the field
                        doneTyping(el);
                    });
                });
            }
        });
    })(jQuery);

let count = 0;
$('input').donetyping(function () {
    
});

let savingBar = document.getElementById("savingbar");
let saved = true;
let saving = document.getElementById("saving");

var tr;

function onCallback(input) {
    const gradeId = input.name;
    const gradeValue = input.value;
    let filteredGradeValue = checkIfProperValue(gradeValue, input);
    savingBar.classList.remove("is-hidden");
    saved = false;
    saving.textContent = "Saving...";
    tr = input.parentNode.parentNode;
    var trChildren = tr.childNodes;
    var studentid;
    for (let i = 0; i < trChildren.length; i++) {
        if (trChildren[i].id == "studentid") {
            studentid = trChildren[i].innerHTML;
        }
    }

    connection.invoke("UpdateGrade", gradeId, filteredGradeValue, studentid, classid).catch(function (err) {
        console.error(err);
    });
}

connection.on("UpdateGradeFinished", function (gradevalue) {
    for (let i = 0; i < tr.childNodes.length; i++) {
        if (tr.childNodes[i].id == "gradeaverage") {
            tr.childNodes[i].innerHTML = gradevalue + "%";
        }
    }

    saved = true;
    savingBar.classList.add("is-hidden");
    saving.textContent = "Changes Saved";
});


window.addEventListener("beforeunload", function (e) {
    if (saved === false) {
        var confirmationMessage = 'It looks like you have been editing something. '
            + 'If you leave before saving, your changes will be lost.';

        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
        return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
    }
});

const changeSection = document.getElementById("changeSection");
let input = document.getElementById("view");
let assignment = document.getElementById("assignment");
let category = document.getElementById("category");
function change() {
    if (changeSection.value == "View Grades") {
        viewGradesChange();
    } else if (changeSection.value == "Manage Assignments") {
        manageAssignmentsChange();
    } else if (changeSection.value == "Set Categories") {
        setCategoriesChange();
    } else {
        viewGradesChange();
    }
}

function viewGradesChange() {
    input.removeAttribute("hidden");
    assignment.setAttribute("hidden", "true");
    category.setAttribute("hidden", "true");
    document.cookie = "selectedSection=viewGrades";
}

function manageAssignmentsChange() {
    assignment.removeAttribute("hidden");
    input.setAttribute("hidden", "true");
    category.setAttribute("hidden", "true");
    document.cookie = "selectedSection=assignments";
}

function setCategoriesChange() {
    category.removeAttribute("hidden");
    input.setAttribute("hidden", "true");
    assignment.setAttribute("hidden", "true");
    document.cookie = "selectedSection=categories";
}

document.addEventListener('DOMContentLoaded', function () {
    let cookieValue = document.cookie.replace(/(?:(?:^|.*;\s*)selectedSection\s*\=\s*([^;]*).*$)|^.*$/, "$1");
    if (cookieValue == "assignments") {
        changeSection.selectedIndex = 1;
        manageAssignmentsChange();
    } else if (cookieValue == "viewGrades") {
        changeSection.selectedIndex = 0;
        viewGradesChange();
    } else if (cookieValue == "categories") {
        changeSection.selectedIndex = 2;
        setCategoriesChange();
    } else {
        changeSection.selectedIndex = 0;
        viewGradesChange();
    }
});


function checkIfProperValue(gradeValue, input) {
    gradeValue = gradeValue.toLowerCase();
    let parsedGrade = parseInt(gradeValue);
    if (gradeValue === 'm' || gradeValue === "x" || gradeValue === "") {
        return gradeValue;
    } else if (isNaN(parsedGrade)) {
        input.value = "";
        return "";
    } else if (typeof parsedGrade == "number") {
        if(parsedGrade > 100) parsedGrade = 100;
        input.value = parsedGrade;
        return parsedGrade;
    } else {
        input.value = "";
        return "";
    }
}

function tabulateWeights(input) {
    if (!input.classList.contains("categoryWeight")) return;
    console.log(input.value);
    const categoryWeights = document.getElementsByClassName("categoryWeight");
    let categoryWarning = document.getElementById("categoryWarning");
    let sumWeights = 0;
    let parsedValue = parseInt(input.value);
    if (isNaN(parsedValue)) {
        input.value = "";
        return;
    } else if (typeof parsedGrade == "number") {
        input.value = parsedValue;
    }
    sumWeights += parsedValue;
    for (let i = 0; i < categoryWeights.length; i++) {
        if (categoryWeights[i] == input || categoryWeights[i].value == "") {
            continue;
        }
        sumWeights += parseInt(categoryWeights[i].value);
    }
    if (sumWeights > 100 || sumWeights < 0) {
        input.value = "";
        categoryWarning.classList.remove("is-hidden");
    }

}

function closeMessage(id) {
    let message = document.getElementById(id);
    message.classList.add("is-hidden");
}