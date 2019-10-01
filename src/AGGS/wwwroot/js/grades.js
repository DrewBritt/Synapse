"use scrict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gradesHub").build();

//Disable updating of grades until SignalR connection is established