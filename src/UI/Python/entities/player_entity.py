import os
from ursina import * 
class PlayerEntity(Entity):
    def __init__(self, player, **kwargs):
        super().__init__(
            model='images/shuttle.obj',
            name=player.name,
            position=(player.position.x, player.position.y, player.position.z),
            texture="images/shuttle.png",
            scale=player.radius,
            **kwargs
        )
        self.tag = "bodysystemelement"