import os
from ultralytics import YOLO
import cv2
import logging
import threading
from paddleocr import PaddleOCR
import asyncio
import socketio
from flask import Flask
import time
from datetime import datetime
import re
import base64
import queue
from database import CameraDatabase
from models import Camera
from routes import app_routes

min_plate_length = 3
max_plate_length = 8

camera_threads = {}

# ASYNC
# DATABASE
#

# ADD LOCATION COORDINATES, USERNAME, PASSW
'''cameras = [
    {"id": 1, "ip": "http://79.10.24.158:80/cgi-bin/faststream.jpg?stream=half&fps=15&rand=COUNTER", "name": "camera 1", "description": "front gate camera"},
    {"id": 2, "ip": "http://86.121.159.16/cgi-bin/faststream.jpg?stream=half&fps=15&rand=COUNTER", "name": "camera 2", "description": "main road"}
]'''

'''cameras = [
    {"id": 1, "ip": "http://192.168.103.53:81/stream", "name": "camera 1", "description": "front gate camera"}
]'''

# Set logging levels to suppress unnecessary output
logging.getLogger('ppocr').setLevel(logging.ERROR)
logging.getLogger('paddle').setLevel(logging.ERROR)

# Initialize model paths and image
model_path = "trained.pt"
# image_path = "./images/1.jpg"

# Initialize PaddleOCR
ocr = PaddleOCR(use_angle_cls=True, lang='en')


# Create a Flask application
app = Flask(__name__)

# Create a Socket.IO server
io = socketio.Server()

# Attach the Socket.IO server to the Flask app
app.wsgi_app = socketio.WSGIApp(io, app.wsgi_app)

app.register_blueprint(app_routes, url_prefix='/api')

# Define an event handler for when a client connects
@io.event
def connect(sid, environ):
    print(f"Client connected: {sid}")
    io.send(sid, "You successfully connected!")

# Define an event handler for receiving messages from clients
@io.event
def message(sid, data):
    print(f"Received message from {sid}: {data}")
    io.send(sid, f"Message received: {data}")

# Define an event handler for when a client disconnects
@io.event
def disconnect(sid):
    print(f"Client disconnected: {sid}")
    
def isPlate(text):
    if min_plate_length <= len(text) <= max_plate_length:
        return text
    
def sendMessage(message_queue):
    while True:
        # Wait for the next plate data to process
        plate_data = message_queue.get()
        
        if plate_data is None:
            # Termination signal, break out of the loop
            break
        
        #print(f"Emitting data: {plate_data}")
        io.emit('detected_plate', plate_data)  # Use actual socket library for emitting
        
        # Indicate that the item has been processed
        message_queue.task_done()

def numpy_to_base64(numpy_img, image_format='jpeg'):
    # Convert numpy array to an image in memory (using OpenCV)
    _, buffer = cv2.imencode(f'.{image_format}', numpy_img)
    
    # Convert the buffer to base64 string
    img_str = base64.b64encode(buffer).decode('utf-8')
    return img_str

def addMessage(plate, camera, original_frame, bbox):
    
    date = datetime.fromtimestamp(time.time()).strftime('%H:%M:/ %d/%m/%Y')
    
    image = numpy_to_base64(original_frame)
    
    #bs64_original_frame = base64.b64decode(original_frame)
    
    plate_data = {
        'camera_name': camera["name"],
        'camera_location': 'no data',
        'camera_description': camera["description"],
        'plate_number': plate,
        'timestamp': date,
        'image': image,
        'bounding_box': {
            'x_min': bbox[0],
            'y_min': bbox[1],
            'x_max': bbox[2],
            'y_max': bbox[3]
        }
    }
        
    message_queue.put(plate_data)


def read(cropped_image, camera, original_frame, bbox):
    """Function to perform OCR using PaddleOCR on cropped image."""
    result = ocr.ocr(cropped_image, cls=True)

    if not result or result[0] is None:
        # print("No text detected.")
        return

    # print(result)

    # Extract detected text
    detected_texts = [line[1][0] for line in result[0]]

    # Print detected texts
    for text in detected_texts:
        text = text.upper()
        text = re.sub(r'[^a-zA-Z0-9]', '', text)
        
        if isPlate(text):
            print("Detected text:", text)
            addMessage(text, camera, original_frame, bbox)
        
    with open('detected_texts.txt', 'a') as file:
        for text in detected_texts:
            file.write(f"{text}\n")  # Write each detected text followed by a new line

    return detected_texts

async def detect(frame, camera, model):  # ASYNC
    """Function to detect objects in the image and perform OCR on cropped images."""
    results = model(frame)

    if not results:
        print(f"No results for camera {camera['id']}")
        return

    # Get bounding boxes and class labels from the detection results
    xyxy = results[0].boxes.xyxy.cpu().numpy()  # Bounding box coordinates
    classes = results[0].boxes.cls.cpu().numpy()  # Class IDs
    image = frame  # Assuming frame is the original image
    cropped_images = []  # List to store cropped license plates

    # Loop over detected objects and crop only license plates
    for box, cls in zip(xyxy, classes):
        if cls == 0:  # Class ID 0 corresponds to "license-plate"
            x_min, y_min, x_max, y_max = map(int, box)
            cropped_image = image[y_min:y_max, x_min:x_max]
            cropped_images.append(cropped_image)

    # Process each cropped license plate
    if cropped_images:
        for cropped_image in cropped_images:
            read(cropped_image, camera, image, [x_min, y_min, x_max, y_max])  # Call the read function only for cropped license plates

def openFeed(camera, model):
    cap = cv2.VideoCapture(camera["ip"])  # AUTHENTICATION

    if not cap.isOpened():
        print("Error: Could not open video stream.")
    else:
        while True:
            ret, frame = cap.read()

            if ret and frame is not None:
                # Ensure asyncio event loop is running before creating task
                loop = asyncio.new_event_loop()  # Create a new event loop for this thread
                asyncio.set_event_loop(loop)  # Set the loop as the current event loop

                # Run the async function in the event loop
                loop.create_task(detect(frame, camera, model))
                loop.run_until_complete(asyncio.gather(*asyncio.all_tasks(loop)))

def startThreads(models):

    for camera in cameras:
        model = models[camera["id"]]

        thread = threading.Thread(target=openFeed, args=(camera, model))
        thread.start()
        
        camera_threads[camera["id"]] = {
        "thread": thread,
        "model": model
        }

def loadModels():
    """Load YOLO models for each camera."""
    models = {}

    for camera in cameras:
        # Load the YOLO model for each camera once
        model = YOLO(model_path)
        model.to("cuda")  # Ensure the model uses GPU
        models[camera["id"]] = model  # Store the model using camera ID as key

    return models

if __name__ == "__main__":
    
    camera_db = CameraDatabase()
    
    #camera_db.recreate_table()
    
    camera_db.create_table()
    
    #camera1 = Camera('http://192.168.103.53:81/stream', 'Camera 1', 'Front gate camera', '40.7128° N, 74.0060° W')
    #camera2 = Camera('http://192.168.103.54:81/stream', 'Camera 2', 'Back gate camera', '40.7128° N, 74.0061° W')

    #for i in range (1, 17):
        #camera_db.delete_camera(i)

    # Add cameras to the database
    #camera_db.add_camera(camera1)
    #camera_db.add_camera(camera2)
    
    cameras = camera_db.read_all_cameras()
    
    #if not cameras:
        #print("database empty")
    
    #for camera in cameras:
        #print(camera)
    
    message_queue = queue.Queue()
    '''consumer_thread = threading.Thread(target=sendMessage, args=(message_queue,))
    #consumer_thread.daemon = True
    consumer_thread.start()
    
    # Detect(image_path)
    models = loadModels()
    startThreads(models)'''
    
    # Run the Flask app
    app.run(host='0.0.0.0', port=5000)