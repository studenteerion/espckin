const io = require('socket.io-client');

// Connect to the Python Socket.IO server
const socket = io('http://localhost:5000');

console.log("turned on");

// Listen for the connection event
socket.on('connect', () => {
    console.log('Connected to the Python API!');
    
    // Send a test message to the server after connecting
    socket.send('Hello from Node.js!');
});

// Listen for the message event from the server
socket.on('message', (data) => {
    console.log('Message from server:', data);
});

// Listen for a custom event like 'detected_plate'
socket.on('detected_plate', (data) => {
    console.log('Detected plate data:', data);
});