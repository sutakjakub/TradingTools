"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/syncInfoHub").build();

//Disable send button until connection is established
document.getElementById("startSyncButton").disabled = true;
document.getElementById("stopSyncButton").disabled = true;

connection.on("ReceiveMessage", function (message, symbol) {
    document.getElementById("sync-info-text").textContent = message;
    document.getElementById("sync-info-text-symbol").textContent = symbol;
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
});

connection.start().then(function () {
    document.getElementById("startSyncButton").disabled = false;
    document.getElementById("stopSyncButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("startSyncButton").addEventListener("click", function (event) {
    console.log("startSyncButton clicked");
    connection.invoke("StartSync").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("stopSyncButton").addEventListener("click", function (event) {
    connection.invoke("StopSync").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});