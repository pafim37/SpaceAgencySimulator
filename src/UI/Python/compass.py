import math
import numpy as np
from ursina import *
from entities.arrow_entity import ArrowEntity
class Compass():
    def __init__(self):
        self.arrow_x = ArrowEntity(scale=(0.4,0.3,0.3), position=(3,1.5,0), color=color.blue).e
        self.arrow_y = ArrowEntity(scale=(0.4,0.3,0.3), position=(3,1.5,0), rotation=(0, 0, -90), color=color.green).e
        self.arrow_z = ArrowEntity(scale=(0.4,0.3,0.3), position=(3,1.5,0), rotation=(0, -90, 0), color=color.red).e

    def turn_off(self):
        self.arrow_x.enabled = False
        self.arrow_y.enabled = False
        self.arrow_z.enabled = False

    def turn_on(self):
        self.arrow_x.enabled = True
        self.arrow_y.enabled = True
        self.arrow_z.enabled = True

    def update(self, camera, pivot):
        x = math.radians(pivot.rotation[0])
        y = math.radians(pivot.rotation[1])
        z = math.radians(pivot.rotation[2])
        self.arrow_x.position = self.__rotation_matrix_y(y) @ self.__rotation_matrix_x(x) @ self.__rotation_matrix_z(z) @ (3,1.5,10) + camera.world_position
        self.arrow_y.position = self.__rotation_matrix_y(y) @ self.__rotation_matrix_x(x) @ self.__rotation_matrix_z(z) @ (3,1.5,10) + camera.world_position
        self.arrow_z.position = self.__rotation_matrix_y(y) @ self.__rotation_matrix_x(x) @ self.__rotation_matrix_z(z) @ (3,1.5,10) + camera.world_position
    
    def __rotation_matrix_z(self, yaw):
        cos = np.cos(yaw)
        sin = np.sin(yaw)
        return np.array([
            [cos, sin, 0],
            [-sin, cos, 0],
            [0, 0, 1]
        ])

    def __rotation_matrix_y(self, pitch):
        cos = np.cos(pitch)
        sin = np.sin(pitch)
        return np.array([
            [cos, 0, sin],
            [0, 1, 0],
            [-sin, 0, cos]
        ])

    def __rotation_matrix_x(self, roll):
        cos = np.cos(roll)
        sin = np.sin(roll)
        return np.array([
            [1, 0, 0],
            [0, cos, -sin],
            [0, sin, cos]
        ])
