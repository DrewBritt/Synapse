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
                        if (!timeoutReference || changeText.value != "View Grades") return;
                        timeoutReference = null;
                        callback.call(el);
                        onCallback(el);
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
function onCallback(input) {
    const gradeId = input.name;
    const gradeValue = input.value;
    savingBar.classList.remove("is-hidden");
    saved = false;
    saving.textContent = "Saving...";
    connection.invoke("UpdateGrade", gradeId, gradeValue).catch(function (err) {
        console.error(err);
    });
}

connection.on("UpdateGradeFinished", function () {
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
function change() {
    if (changeSection.value == "View Grades") {
        viewGradesChange();
    } else if (changeSection.value == "Manage Assignments") {
        manageAssignmentsChange();
    }
}

function viewGradesChange() {
    input.removeAttribute("hidden");
    assignment.setAttribute("hidden", "true");
    document.cookie = "selectedSection=view";
}

function manageAssignmentsChange() {
    assignment.removeAttribute("hidden");
    input.setAttribute("hidden", "true");
    document.cookie = "selectedSection=assignments";
}

document.addEventListener('DOMContentLoaded', function () {
    let cookieValue = document.cookie.replace(/(?:(?:^|.*;\s*)selectedSection\s*\=\s*([^;]*).*$)|^.*$/, "$1");
    if (cookieValue == "assignments") {
        changeSection.selectedIndex = 1;
        manageAssignmentsChange();
    } else {
        changeSection.selectedIndex = 0;
        viewGradesChange();
    }
});