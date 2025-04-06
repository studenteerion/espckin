import socketio

def setup_socketio(app):
    
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
    
    return io  
    