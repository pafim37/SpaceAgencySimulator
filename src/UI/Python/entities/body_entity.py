import os
from ursina import *
from coordinates import to_scene

class BodyEntity(Entity):
    def __init__(self, body, **kwargs):
        super().__init__(
            model='sphere',
            name=body.name,
            position=to_scene(body.position),
            rotation=(-90, 0, 0),
            scale=body.radius,
            **kwargs
        )
        self.__set_texture(body.name)
        self.tag = "bodysystemelement"

    def __set_texture(self, name):
        texture=f"images/{name.lower()}.jpg"
        if os.path.exists(texture):
            self.texture = texture
        else:
            self.color = color.red

    def update_position(self, new_position):
        self.position = to_scene(new_position)