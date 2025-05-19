const axios = require('axios');

// URL of the Flask API (you can change the address based on your Flask app's running address)
const baseURL = 'http://127.0.0.1:5050/api';  // Adjust to match your Flask app's IP address if necessary

// Camera data to be added
const newCamera = {
    ip: 'http://192.168.103.19:81/stream',
    name: 'Front Door Camera',
    description: 'Camera at the front door',
    coordinates: '40.7128,-74.0060' // Example coordinates (latitude, longitude)
};

// Function to add a camera
async function addCamera() {
    try {
        const response = await axios.post(`${baseURL}/add_camera`, newCamera);
        console.log('Camera added:', response.data);
    } catch (error) {
        console.error('Error adding camera:', error.response ? error.response.data : error.message);
    }
}

// Function to get all cameras
async function getAllCameras() {
    try {
        const response = await axios.get(`${baseURL}/get_all_cameras`);
        console.log('All cameras:', response.data);
    } catch (error) {
        console.error('Error fetching cameras:', error.response ? error.response.data : error.message);
    }
}

// Run the test by adding a camera and then fetching all cameras
async function runTest() {
    await addCamera();
    await getAllCameras();
}

// Execute the test
runTest();