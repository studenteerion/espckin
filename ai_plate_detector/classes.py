import threading


class Camera:
    def __init__(self, ip, name, description, coordinates):
        self._id = None  # We use a private variable to store the id
        self.ip = ip
        self.name = name
        self.description = description
        self.coordinates = coordinates
        self.latest_frame = None  # Store the latest frame
        self.lock = threading.Lock()

    @property
    def id(self):
        #Allow read access to the id, but not write.
        return self._id

    @id.setter
    def id(self, value):
        #Prevent setting the id directly after it's been assigned.
        if self._id is None:
            self._id = value
        else:
            raise ValueError("Cannot modify the id after it has been set.")

    def update_frame(self, frame):
        with self.lock:
            self.latest_frame = frame

    def get_frame(self):
        with self.lock:
            return self.latest_frame

    def __str__(self):
        return f"Camera {self.name} (ID: {self.id}): {self.ip}, {self.coordinates}, {self.description}"


class CameraThread:
    def __init__(self, camera, model, thread, stop_signal):
        self.camera = camera
        self.model = model
        self.thread = thread
        self.stop_signal = stop_signal

    # Start the thread
    def start_thread(self):
        """Starts the thread."""
        if not self.thread.is_alive():
            self.thread.start()

    # Stop the thread
    def stop_thread(self):
        """Stops the thread."""
        if self.thread.is_alive():
            self.stop_signal.set()  # Signal the thread to stop
            self.thread.join()  # Wait for the thread to finish

    # Check if the thread is running
    def is_thread_running(self):
        """Check if the thread is running."""
        return self.thread.is_alive()

    # Getter for camera
    @property
    def camera(self):
        return self._camera

    # Setter for camera
    @camera.setter
    def camera(self, value):
        self._camera = value

    # Getter for model
    @property
    def model(self):
        return self._model

    # Setter for model
    @model.setter
    def model(self, value):
        self._model = value

    # Getter for thread
    @property
    def thread(self):
        return self._thread

    # Setter for thread
    @thread.setter
    def thread(self, value):
        self._thread = value

    # Getter for stop_signal
    @property
    def stop_signal(self):
        return self._stop_signal

    # Setter for stop_signal
    @stop_signal.setter
    def stop_signal(self, value):
        self._stop_signal = value

    
'''import threading

class CameraThread:
    def __init__(self, camera, model, thread, stop_signal=None):
        """Initialize the CameraThread with camera, model, thread, and stop_signal."""
        self.camera = camera
        self.model = model
        self.thread = thread
        self.stop_signal = stop_signal or threading.Event()  # Default to a new stop signal if none provided

    # Getter for camera
    def get_camera(self):
        return self.camera

    # Setter for camera
    def set_camera(self, camera):
        self.camera = camera

    # Getter for model
    def get_model(self):
        return self.model

    # Setter for model
    def set_model(self, model):
        self.model = model

    # Getter for thread
    def get_thread(self):
        return self.thread

    # Setter for thread
    def set_thread(self, thread):
        self.thread = thread

    # Getter for stop_signal
    def get_stop_signal(self):
        return self.stop_signal

    # Setter for stop_signal
    def set_stop_signal(self, stop_signal):
        self.stop_signal = stop_signal

    # Method to check if the thread is running
    def is_thread_running(self):
        return self.thread.is_alive()

    # Method to stop the thread gracefully
    def stop_thread(self):
        self.stop_signal.set()  # Signal the thread to stop
        self.thread.join()  # Wait for the thread to finish
        print(f"Thread for camera {self.camera.id} has been stopped.")

    # Method to restart the thread
    def restart_thread(self, target_function, args):
        """Restart the thread by creating a new one with the provided function and args."""
        # Stop the old thread gracefully
        self.stop_thread()

        # Create a new stop signal
        new_stop_signal = threading.Event()

        # Create a new thread
        new_thread = threading.Thread(target=target_function, args=args)

        # Update the CameraThread object with the new thread and stop signal
        self.set_thread(new_thread)
        self.set_stop_signal(new_stop_signal)

        # Start the new thread
        new_thread.start()
        print(f"Camera {self.camera.id} feed has been restarted.")

'''