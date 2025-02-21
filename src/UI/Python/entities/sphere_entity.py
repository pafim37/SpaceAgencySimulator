import os
import ursina as urs
class SphereEntity:
    def __init__(self, body):
        self.entity = urs.Entity(model='sphere', name=body.name, position=(body.position.x, body.position.y, body.position.z), scale=body.radius*0.1)
        self.__set_texture()

    def __set_texture(self):
        texture=f"images\{self.entity.name.lower()}.jpg"
        if os.path.exists(texture):
            self.entity.texture = texture