let chatContainer = document.getElementById("chatContainer");

let currentChatId
const chatUl = document.getElementById("chatUl")

const startChat = () => {
    const username = document.getElementById('usernameInput').value
    const url = "/Home/StartChat/?companionUsername=" + username

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            getUserChats()
        },
        error: function (error) {
            console.log("erre")
            console.log(error)
        }
    });
}

const getUserChats = () => {
    const url = "/Home/GetUserChats"

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            let html = ""

            data.forEach(function (companion) {
                console.log("a")
                console.log(companion)

                html += `
                                <li onclick="getMessages(${companion.id})" class="clearfix">
                                    <div class="about">
                                        <div class="name">${companion.companionName}</div>
                                        <div class="status">${companion.companionUserName}</div>
                                    </div>
                            `
            })

            $("#friends")[0].innerHTML = html;
        },
        error: function (error) {
            console.log(error)
        }
    });
}

const getMessages = (chatId) => {
    const url = "/Home/GetMessagesById/?chatId=" + chatId

    currentChatId = chatId

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log(data)

            let html = ""

            data.forEach(function (message) {
                if (message.messageType == 0) {
                    if (message.isMyMessage == true) {
                        html += `
                                <li class="clearfix">
                                    <div class="message other-message float-right"> ${message.body} </div>
                                </li>
                            `
                    } else {
                        html += `
                                <li>
                                    <div class="message my-message"> ${message.body} </div>
                                </li>
                            `
                    }
                } else {
                    if (message.isMyMessage == true) {
                        html += `
                                <li class="clearfix">
                                    <div class="message other-message float-right"><a href="${message.body}">file</a></div>
                                </li>
                            `
                    } else {
                        html += `
                                <li>
                                    <div class="message my-message"><a href="${message.body}">file</a></div>
                                </li>
                            `
                    }
                }
            })

            chatUl.innerHTML = html
            chatContainer.scrollTop = chatContainer.scrollHeight;
        },
        error: function (error) {
            console.log(error)
        }
    });
}

window.addEventListener('load', () => {
    getUserChats()
});

let file
let isFile = false

const dropHandler = (ev) => {
    ev.preventDefault()

    if (ev.dataTransfer.items) {
        [...ev.dataTransfer.items].forEach((item, i) => {
            if (item.kind === "file") {
                file = item.getAsFile();
                document.getElementById("message").value = file.name
                isFile = true
            }
        });
    } else {
        [...ev.dataTransfer.files].forEach((file, i) => {
            file = file
            document.getElementById("message").value = file.name
            isFile = true
        });
    }
}

const dragOverHandler = (ev) => {
    ev.preventDefault();
}

const sendFile = async () => {
    isFile = false

    const formData = new FormData()
    formData.append('file', file)

    $.ajax({
        url: "/Home/SendFile?chatId=" + currentChatId.toString(),
        method: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        method: 'POST',
        data: formData,
        success: function (data) {
            console.log(data)
        },
        error: function (error) {
            console.log(error)
        }
    });
}

const sendText = async () => {
    console.log("was")
    await hubConnection.invoke("SendMessage",
        currentChatId.toString(),
        document.getElementById("message").value)
}

const sendMessage = async (e) => {
    e.preventDefault()
    
    if (isFile) {
        await sendFile()
    } else {
        await sendText()
    }
}

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .withAutomaticReconnect()
    .build();

hubConnection.on("Chat", (data) => {
    if (data.messageType == 0) {
        if (data.isMyMessage) {
            chatUl.innerHTML += `
                                <li class="clearfix">
                                    <div class="message other-message float-right"> ${data.body} </div>
                                </li>
                            `
        } else {
            chatUl.innerHTML += `
                                <li>
                                    <div class="message my-message"> ${data.body} </div>
                                </li>
                            `
        }
    } else {
        if (data.isMyMessage) {
            chatUl.innerHTML += `
                                <li class="clearfix">
                                    <div class="message other-message float-right"><a href="${data.body}">file</a></div>
                                </li>
                            `
        } else {
            chatUl.innerHTML += `
                                <li>
                                    <div class="message my-message"><a href="${data.body}">file</a></div>
                                </li>
                            `
        }
    }

    chatContainer.scrollTop = chatContainer.scrollHeight;
});

const videocall = async () => {
    await hubConnection.invoke("SendInvite", currentChatId.toString())
}

const avideocall = document.getElementById("avideocall")

avideocall.addEventListener("click", () => {
    avideocall.hidden = true
})

hubConnection.on("Invite", data => {
    avideocall.hidden = false
    avideocall.href = "/Video/Index?roomId=" + data
    avideocall.target = "_blank"
})

hubConnection.start();