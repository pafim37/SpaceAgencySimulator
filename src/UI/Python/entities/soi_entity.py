from ursina import *
from coordinates import to_scene


class SoiEntity(Entity):
    def __init__(self, body, **kwargs):
        super().__init__(
            name=body.name,
            model='sphere',
            position=to_scene(body.position),
            double_sided=True,
            color=(1,1,1,0.1),
            scale=body.sphereOfInfluenceRadius,
            **kwargs
        )
        self.tag = "bodysystemelement"

    def update_position(self, new_position):
        self.position = to_scene(new_position)