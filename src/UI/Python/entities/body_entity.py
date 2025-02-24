import os
from ursina import *
class BodyEntity(Entity):
    def __init__(self, body, **kwargs):
        super().__init__(
            model='sphere',
            name=body.name,
            position=(body.position.x, body.position.y, body.position.z),
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