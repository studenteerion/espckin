from ultralytics import YOLO
import cv2
import asyncio
import threading
import torch
from ai import detect
from classes import CameraThread

# Initialize model path
model_path = "trained.pt"

camera_threads = []

def openFeed(camera, model, stop_signal):

    cap = cv2.VideoCapture(camera.ip)  # ADD AUTHENTICATION?

    if not cap.isOpened():
        print(f"Error: Could not open video stream for {camera.ip}")
        return
    else:
        while not stop_signal.is_set():  # Check the stop signal before reading frames
            ret, frame = cap.read()

            if ret and frame is not None:
                camera.update_frame(frame.copy())  # Store the latest frame
                # Ensure asyncio event loop is running before creating task
                loop = asyncio.new_event_loop()  # Create a new event loop for this thread
                asyncio.set_event_loop(loop)  # Set the loop as the current event loop

                # Run the async function in the event loop
                loop.create_task(detect(frame, camera, model))
                loop.run_until_complete(asyncio.gather(*asyncio.all_tasks(loop)))

        cap.release()  # Release the capture object
        print(f"Stream from {camera.ip} has been stopped.")


def createThread(camera, returnValue = True):
    model = YOLO(model_path)
    if torch.cuda.is_available(): # Check for CUDA availability
        device = torch.device("cuda")
        #print("using cuda GPU")
    elif torch.backends.mps.is_available(): # Check for MPS availability (for macOS)
        device = torch.device("mps")
        #print("using mps GPU")
    else:
        device = torch.device("cpu")
        #print("using CPU")
    model.to(device) # Move your model to the selected device
    stop_signal = threading.Event()
    thread = threading.Thread(target=openFeed, args=(camera, model, stop_signal))
    cameraThread = CameraThread(camera, model, thread, stop_signal)
    camera_threads.append(cameraThread)
    
    if returnValue:
        return cameraThread

def createThreads(cameras):
    
    for camera in cameras:
        createThread(camera, False)
        
    return camera_threads

def getCameraThread(cameraId):
    for camera_thread in camera_threads:
        if camera_thread.camera.id == cameraId:
            return camera_thread
    return None

def startThreads():
    for cameraThread in camera_threads:
        startThread(cameraThread)
    return

def startThread(cameraThread):
    if not cameraThread.is_thread_running():
        cameraThread.start_thread()
        print(f"Started thread for Camera {cameraThread.camera.id}")
    return

def stopThread(cameraId):
    camera_thread = getCameraThread(cameraId)
    if camera_thread:
        camera_thread.stop_signal.set()  # Signal the thread to stop
        camera_thread.thread.join()  # Wait for the thread to finish
        print(f"Stopped thread for Camera {cameraId}")
        camera_threads.remove(camera_thread)  # Remove from the list
        return True
    return False

def stopThreads():
    print("Stopping all threads and cleaning up...")
    for cameraThread in camera_threads:
        cameraThread.stop_signal.set()  # Stop the thread
        cameraThread.thread.join()  # Wait for the thread to finish
    print("All threads have been stopped.")