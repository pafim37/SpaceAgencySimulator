from ursina import *
from entities.compass_entity import CompassEntity

class WorldController:
    def __init__(self):
        return

    def create_world_axis(self):
        self.ox = Entity(model="cube", scale=(10000,0.2,0.2), position=(0,0,0), color=color.blue)
        self.oy = Entity(model="cube", scale=(10000,0.2,0.2), position=(0,0,0), rotation=(0, 0, -90), color=color.green)
        self.oz = Entity(model="cube", scale=(10000,0.2,0.2), position=(0,0,0), rotation=(0, -90, 0), color=color.red)
    
    def turn_off_world_axis(self):
        self.ox.enabled = False
        self.oy.enabled = False
        self.oz.enabled = False

    def turn_on_world_axis(self):
        self.ox.enabled = True
        self.oy.enabled = True
        self.oz.enabled = True
        
    def create_compass(self):
        return CompassEntity()