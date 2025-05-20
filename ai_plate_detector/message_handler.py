import cv2
import base64
from datetime import datetime
import time
import queue

message_queue = queue.Queue()

def sendMessage(io):
    while True:
        
        #time.sleep(5)
        
        if io is None:
            print("io not yet exists")
            continue
        
        try:
            # Wait for the next plate data to process with a timeout
            plate_data = message_queue.get(timeout=1)
        except queue.Empty:
            continue  # If the queue is empty, continue the loop
        
        # Emit the detected plate data
        #print(f"Emitting data: {plate_data}")
        io.emit('detected_plate', plate_data)
        #print("EMITTED sendmessage")
        
        # Indicate that the item has been processed
        message_queue.task_done()

def numpy_to_base64(numpy_img, image_format='jpeg'):
    # Convert numpy array to an image in memory (using OpenCV)
    _, buffer = cv2.imencode(f'.{image_format}', numpy_img)
    
    # Convert the buffer to base64 string
    img_str = base64.b64encode(buffer).decode('utf-8')
    return img_str

def addMessage(plate, camera, original_frame, bbox):
    
    #print("called addMessage")
    
    date = datetime.fromtimestamp(time.time()).strftime('%H:%M:/ %d/%m/%Y')
    
    image = numpy_to_base64(original_frame)
    
    plate_data = {
        'camera_name': camera.name,
        'camera_location': 'no data',
        'camera_description': camera.description,
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
    
    #print("added addMessage")
    
    return