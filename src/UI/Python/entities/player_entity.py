import os
import ursina as urs
class PlayerEntity:
    def __init__(self, body):
        self.entity = urs.Entity(model="images/shuttle.obj", texture="images/shuttle.png", name=body.name, position=(body.position.x, body.position.y, body.position.z), scale=0.001)