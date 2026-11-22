import os
from ursina import * 
from coordinates import to_scene


class PlayerEntity(Entity):
    def __init__(self, player, **kwargs):
        super().__init__(
            model='images/shuttle.obj',
            name=player.name,
            position=to_scene(player.position),
            texture="images/shuttle.png",
            scale=player.radius,
            **kwargs
        )
        self.tag = "bodysystemelement"

    def update_position(self, new_position):
        self.position = to_scene(new_position)
