import threading
import time
import socketio
from flask import Flask
from datetime import datetime

# Create a Flask application
app = Flask(__name__)

# Create a Socket.IO server
io = socketio.Server()

# Attach the Socket.IO server to the Flask app
app.wsgi_app = socketio.WSGIApp(io, app.wsgi_app)

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
    
    
def sendMessage():
    i = 0
    while True:
        print("sending message", i)
        
        date = datetime.fromtimestamp(time.time()).strftime('%H:%M:/ %d/%m/%Y')
        
        plate_data = {
            'message': i,
            'camera_alias': 'Camera_1',
            'plate_number': 'ABC123',
            'timestamp': date,
            'bounding_box': {
                'x_min': 100,
                'y_min': 50,
                'x_max': 200,
                'y_max': 150
            }
        }
        
        io.emit('detected_plate', plate_data)
        
        i += 1
        time.sleep(5)

# Start the Flask server
if __name__ == '__main__':
    # Start the Flask app in a separate thread
    thread = threading.Thread(target=sendMessage)
    thread.start()
    
    # Run the Flask app
    app.run(host='0.0.0.0', port=5000)