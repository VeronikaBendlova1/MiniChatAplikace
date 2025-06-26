let typingTimeout;

const chatInput = document.getElementById("chatInput"); // pole pro zadávání zpráv

chatInput.addEventListener("input", () => {
    if (typingTimeout) clearTimeout(typingTimeout);

    // Řekni serveru, že píšu
    connection.invoke("Typing", username);

    // Po 1 sekundě už nevolej znovu
    typingTimeout = setTimeout(() => {
        // nic – server to smaže sám
    }, 1000);

    connection.on("UserTyping", function (user) {
        document.getElementById("typingIndicator").innerText = `${user} píše...`;
    });

    connection.on("UserStoppedTyping", function (user) {
        document.getElementById("typingIndicator").innerText = "";
    });

    const username = "@Veronika";
    //username je tvoje přihlášené jméno – ujisti se, že ho máš jako JS proměnnou.
});