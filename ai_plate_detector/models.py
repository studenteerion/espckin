class Camera:
    def __init__(self, ip, name, description, coordinates):
        self._id = None  # We use a private variable to store the id
        self.ip = ip
        self.name = name
        self.description = description
        self.coordinates = coordinates

    @property
    def id(self):
        """Allow read access to the id, but not write."""
        return self._id

    @id.setter
    def id(self, value):
        """Prevent setting the id directly after it's been assigned."""
        if self._id is None:
            self._id = value
        else:
            raise ValueError("Cannot modify the id after it has been set.")

    def __str__(self):
        return f"Camera {self.name} (ID: {self.id}): {self.ip}, {self.coordinates}, {self.description}"
