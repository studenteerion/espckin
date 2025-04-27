const io = require('socket.io-client');
const fs = require('fs'); // Import the file system module

// Connect to the Python Socket.IO server
const socket = io('http://localhost:5050');

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
    
    // Extract base64 image data from the received data
    const base64Image = data.image;
    
    // Decode the base64 image and save it as a file
    const buffer = Buffer.from(base64Image, 'base64');
    
    // Specify the file path and name for the image
    const filePath = `./plate_images/plate_${Date.now()}.jpg`;

    // Ensure the directory exists (create if not)
    if (!fs.existsSync('./plate_images')) {
        fs.mkdirSync('./plate_images');
    }

    // Write the image to the file system
    fs.writeFile(filePath, buffer, (err) => {
        if (err) {
            console.error('Error saving the image:', err);
        } else {
            console.log(`Image saved successfully at ${filePath}`);
        }
    });
});