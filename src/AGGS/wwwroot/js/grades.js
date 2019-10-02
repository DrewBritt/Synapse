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

