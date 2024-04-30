let chatContainer = document.getElementById("chatContainer");

let currentChatId
const chatUl = document.getElementById("chatUl")

const friendsSpinner = document.getElementById("friendsSpinner")
const messagesSpinner = document.getElementById("messagesSpinner")

const friendsError = document.getElementById("friendsError")

const selectChat = document.getElementById("selectChat")

const onChangeUsernameInput = () => {
    friendsError.hidden = true
}

const startChat = () => {
    const username = document.getElementById('usernameInput').value
    const url = "/Home/StartChat/?companionUsername=" + username
    friendsError.hidden = true

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data == false) {
                friendsError.hidden = false
                friendsError.innerHTML = "You have chat with this person"
            } else {
                friendsError.hidden = true
                getUserChats()
            }
        },
        error: function (error) {
            friendsError.hidden = false
            friendsError.innerHTML = "There is no person with this username"
        }
    });
}

const getUserChats = () => {
    const url = "/Home/GetUserChats"

    friendsSpinner.hidden = false

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            friendsSpinner.hidden = true
            let html = ""

            data.forEach(function (companion) {
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
            friendsSpinner.hidden = true
        }
    });
}


const getMessages = (chatId) => {
    selectChat.hidden = true
    const url = "/Home/GetMessagesById/?chatId=" + chatId

    currentChatId = chatId
    messagesSpinner.hidden = false

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            messagesSpinner.hidden = true

            let html = ""

            data.forEach(function (message) {
                html += renderMessage(message)
            })

            chatUl.innerHTML = html
            chatContainer.scrollTop = chatContainer.scrollHeight;
        },
        error: function (error) {
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
        },
        error: function (error) {
        }
    });
}

const sendText = async () => {
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
    chatUl.innerHTML += renderMessage(data)

    chatContainer.scrollTop = chatContainer.scrollHeight;
});

const renderMessage = (message) => {
    let html = ""

    if (message.messageType == 0) {
        if (message.isMyMessage) {
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
        if (message.isMyMessage) {
            html += `
                                <li class="clearfix">
                                    <a href="${message.body}">
                                        <div class="message other-message float-right">
                                            <i class="bi bi-file-arrow-down h2"></i>
                                        </div>
                                    </a>
                                </li>
                            `
        } else {
            html += `
                                <li class="clearfix">
                                    <a href="${message.body}">
                                        <div class="message my-message">
                                            <i class="bi bi-file-arrow-down h2"></i>
                                        </div>
                                    </a>
                                </li>
                            `
        }
    }

    return html
}

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