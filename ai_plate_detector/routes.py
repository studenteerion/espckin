from flask import Blueprint, request, jsonify
from database import CameraDatabase
from classes import Camera

app_routes = Blueprint('app_routes', __name__)

camera_db = CameraDatabase()  # Initialize the CameraDatabase

# Route to add a new camera (POST request)
@app_routes.route('/add_camera', methods=['POST'])
def add_camera():
    camera_db.connect()
    try:
        camera_data = request.json  # Get camera data from the request body
        # Create Camera instance
        camera = Camera(
            ip=camera_data['ip'],
            name=camera_data['name'],
            description=camera_data['description'],
            coordinates=camera_data['coordinates']
        )
        # Add the camera to the database
        camera_db.add_camera(camera)
        return jsonify({"message": "Camera added successfully!"}), 201
    except Exception as e:
        return jsonify({"error": str(e)}), 400
    finally:
        camera_db.close()

# Route to update a camera (PUT request)
@app_routes.route('/update_camera', methods=['PUT'])
def update_camera():
    camera_db.connect()
    try:
        camera_data = request.json  # Get camera data from the request body
        camera_id = camera_data.get('id')  # Camera ID should be included in the request body
        
        if not camera_id:
            return jsonify({"error": "Camera ID is required"}), 400
        
        camera = Camera(
            ip=camera_data['ip'],
            name=camera_data['name'],
            description=camera_data['description'],
            coordinates=camera_data['coordinates']
        )
        
        camera.id = camera_id
        
        camera_db.update_camera(camera_id, camera)
        return jsonify({"message": f"Camera {camera_id} updated successfully!"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400
    finally:
        camera_db.close()

# Route to delete a camera (DELETE request)
@app_routes.route('/delete_camera', methods=['DELETE'])
def delete_camera():
    camera_db.connect()
    try:
        camera_data = request.json  # Get camera data from the request body
        camera_id = camera_data.get('id')  # Camera ID should be included in the request body
        
        if not camera_id:
            return jsonify({"error": "Camera ID is required"}), 400
        
        camera_db.delete_camera(camera_id)
        return jsonify({"message": f"Camera {camera_id} deleted successfully!"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400
    finally:
        camera_db.close()

# Route to get all cameras (GET request)
@app_routes.route('/get_all_cameras', methods=['GET'])
def get_all_cameras():
    camera_db.connect()
    try:
        cameras = camera_db.read_all_cameras()
        camera_list = [{"id": camera.id, "ip": camera.ip, "name": camera.name, "description": camera.description, "coordinates": camera.coordinates} for camera in cameras]
        return jsonify(camera_list), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400
    finally:
        camera_db.close()

# Route to get a camera by ID (GET request)
@app_routes.route('/get_camera', methods=['GET'])
def get_camera():
    camera_db.connect()
    try:
        camera_data = request.json  # Get camera data from the request body
        camera_id = camera_data.get('id')  # Camera ID should be included in the request body
        
        if not camera_id:
            return jsonify({"error": "Camera ID is required"}), 400
        
        camera = camera_db.get_camera_by_id(camera_id)
        if camera:
            camera_data = {"id": camera.id, "ip": camera.ip, "name": camera.name, "description": camera.description, "coordinates": camera.coordinates}
            return jsonify(camera_data), 200
        else:
            return jsonify({"error": "Camera not found"}), 404
    except Exception as e:
        return jsonify({"error": str(e)}), 400
    finally:
        camera_db.close()