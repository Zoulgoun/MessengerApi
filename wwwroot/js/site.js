document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('loginForm');
    const authContainer = document.getElementById('auth-form-container');
    const chatWindow = document.getElementById('chat-window');
    const messagesSection = document.getElementById('messages-section');
    const messageInput = document.getElementById('messageInput');
    let currentUser = null;

    
    loginForm.addEventListener('submit', async event => {
        event.preventDefault();
        const email = loginForm.elements['email'].value.trim();
        const password = loginForm.elements['password'].value.trim();

        if (!email || !password) {
            alert('Необходимо заполнить оба поля.');
            return;
        }

        try {
            const UserCredentials = {
                username: email,
                password: password
            };
            const jsonObject = JSON.stringify(UserCredentials);
            console.log(jsonObject);

            const response = await fetch('https://localhost:7068/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: jsonObject
            });

            if (!response.ok) {
                throw new Error(`Ошибка сервера: ${response.status}`);
            }

            const res = await response.text();
            console.log(res); 

            /
            localStorage.setItem('jwt-token', res.token);

            
            window.location.href = "C:/Users/oa666/OneDrive/Desktop/project/MessengerApi/MessengerApi/wwwroot/chat.html"; 
        } catch (err) {
            handleError(err.message || 'Ошибка сервера');
        }
    });

    function logout() {
        fetch('/api/auth/logout', {
            method: 'POST'
        })
            .then(() => {
                location.reload(); 
            })
            .catch(handleError);
    }

    function sendMessage() {
        const content = messageInput.value.trim();
        if (!content) return;

        fetch('/api/messages/send', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ user: currentUser, content })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Ошибка сервера: ${response.status}`);
                }
                return response.json();
            })
            .then(res => {
                renderMessage(res);
                messageInput.value = '';
            })
            .catch(handleError);
    }

    function fetchMessages() {
        fetch('/api/messages')
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Ошибка сервера: ${response.status}`);
                }
                return response.json();
            })
            .then(response => {
                response.forEach(renderMessage);
            })
            .catch(handleError);
    }

    function renderMessage(message) {
        const div = document.createElement('div');
        div.classList.add('message-bubble');
        div.innerHTML = `<p><strong class="message-author">${message.user}:</strong> ${message.content}</p>`;
        messagesSection.appendChild(div);
        scrollToBottom(messagesSection);
    }

    function scrollToBottom(element) {
        element.scrollTop = element.scrollHeight;
    }

    function handleError(errorMsg) {
        const errorDiv = document.querySelector('.error-message') || document.createElement('div');
        errorDiv.textContent = errorMsg;
        errorDiv.className = 'error-message';
        authContainer.appendChild(errorDiv);
    }

    function showChat(username) {
        authContainer.style.display = 'none';
        chatWindow.style.display = 'block';
        document.getElementById('username').innerText = username;
        fetchMessages();
    }
});