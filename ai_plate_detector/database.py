import sqlite3
from classes import Camera
import thread_handler

class CameraDatabase:
    def __init__(self, db_name='camera_database.db', check_same_thread=False):
        self.db_name = db_name
        self.conn = sqlite3.connect(self.db_name)
        self.cursor = self.conn.cursor()
        self.create_table()

    def create_table(self):
        # Create table if it doesn't exist
        self.cursor.execute('''CREATE TABLE IF NOT EXISTS cameras (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                ip TEXT NOT NULL,
                                name TEXT NOT NULL,
                                description TEXT,
                                coordinates TEXT NOT NULL)''')
        self.conn.commit()

    def recreate_table(self):
        # Drop the existing table (if you no longer need it)
        self.cursor.execute('DROP TABLE IF EXISTS cameras')
        self.conn.commit()

        # Recreate the table with AUTOINCREMENT for the 'id'
        self.cursor.execute('''CREATE TABLE cameras (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ip TEXT NOT NULL,
                            name TEXT NOT NULL,
                            description TEXT,
                            coordinates TEXT NOT NULL)''')
        self.conn.commit()

    def add_camera(self, camera):
        # Insert new camera record into the database
        self.cursor.execute('''INSERT INTO cameras (ip, name, description, coordinates) 
                               VALUES (?, ?, ?, ?)''', 
                           (camera.ip, camera.name, camera.description, camera.coordinates))
        self.conn.commit()
        camera.id = self.cursor.lastrowid  # Set the ID of the camera object after insertion
        
        thread = thread_handler.createThread(camera)
        thread_handler.startThread(thread)
        
    def get_camera_by_id(self, camera_id):
        self.cursor.execute('''SELECT * FROM cameras WHERE ID = ?''', (camera_id,))
        camera = self.cursor.fetchone()
        
        if camera:
            cam = Camera(camera[1], camera[2], camera[3], camera[4])
            cam.id = camera[0]
            return cam
        else:
            return None

    def read_all_cameras(self):
        # Read all camera records from the database
        self.cursor.execute('''SELECT * FROM cameras''')
        cameras = self.cursor.fetchall()
        
        # Construct Camera objects for each record
        camera_objects = []
        for camera in cameras:
            cam = Camera(camera[1], camera[2], camera[3], camera[4])  # Pass only the 4 relevant fields
            cam.id = camera[0]  # Set the 'id' field using the property setter
            camera_objects.append(cam)
            
        return camera_objects

    def update_camera(self, camera_id, camera):
        # Update camera information based on camera ID
        self.cursor.execute('''UPDATE cameras SET ip = ?, name = ?, description = ?, coordinates = ? 
                               WHERE id = ?''', 
                           (camera.ip, camera.name, camera.description, camera.coordinates, camera_id))
        self.conn.commit()
        
        thread_handler.stopThread(camera_id)
        thread = thread_handler.createThread(camera)
        thread_handler.startThread(thread)

    def delete_camera(self, camera_id):
        # Delete a camera record from the database based on camera ID
        self.cursor.execute('''DELETE FROM cameras WHERE id = ?''', (camera_id,))
        self.conn.commit()
        
        thread_handler.stopThread(camera_id)

    def connect(self):
        self.conn = sqlite3.connect(self.db_name)
        self.cursor = self.conn.cursor()
        self.create_table()

    def close(self):
        # Close the database connection
        self.conn.close()