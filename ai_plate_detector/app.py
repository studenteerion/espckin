import threading
from flask import Flask
from database import CameraDatabase
from routes import app_routes
from message_handler import sendMessage
from socket_manager import setup_socketio
import thread_handler

import signal
import sys

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


if __name__ == "__main__":

    # Create a Flask application
    app = Flask(__name__)
    app.register_blueprint(app_routes, url_prefix='/api')
    
    
    camera_db = CameraDatabase()
    camera_db.create_table()
    startup_cameras = camera_db.read_all_cameras()
    
    thread_handler.createThreads(startup_cameras)
    thread_handler.startThreads()
    
    io = setup_socketio(app)
    
    consumer_thread = threading.Thread(target=sendMessage, args=(io,), daemon=True)
    consumer_thread.start()


    def shutdown_handler(sig, frame):
        print("Signal received, shutting down...")
        thread_handler.stopThreads()
        sys.exit(0)


    signal.signal(signal.SIGINT, shutdown_handler)
    signal.signal(signal.SIGTERM, shutdown_handler)

    app.run(debug=True, host='0.0.0.0', port=5050, use_reloader=False)