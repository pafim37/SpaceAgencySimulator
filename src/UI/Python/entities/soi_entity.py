from ursina import *
class SoiEntity(Entity):
    def __init__(self, body, **kwargs):
        super().__init__(
            name=body.name,
            model='sphere',
            position=(body.position.x, body.position.y, body.position.z),
            color=(1,1,1,0.1),
            scale=body.sphereOfInfluenceRadius,
            **kwargs
        )
        self.tag = "bodysystemelement"

    def update_position(self, new_position):
        self.position = (new_position.x, new_position.y, new_position.z)