import ursina as urs
from settings import *
class Camera:
    def __init__(self, camera):
        self.camera = camera
        self.camera.position = CAMERA_INITIAL_POSITION
        self.camera.rotation = CAMERA_INITIAL_ROTATION
        self.camera.move_speed = CAMERA_MOVE_SPEED
        self.camera.rotation_speed = CAMERA_ROTATION_SPEED

    def key_handle(self):
        old_camera_position = urs.camera.position
        old_camera_rotation = urs.camera.rotation
        if urs.held_keys['up arrow']:
            urs.camera.position += (0, self.camera.rotation_speed, 0) 
        if urs.held_keys['down arrow']:
            urs.camera.position += (0, -self.camera.rotation_speed, 0) 
        if urs.held_keys['right arrow']:
            urs.camera.position += (self.camera.rotation_speed, 0, 0) 
        if urs.held_keys['left arrow']:
            urs.camera.position += (-self.camera.rotation_speed, 0, 0)
        if urs.held_keys['w']:
            self.camera.position += (0, 0, self.camera.rotation_speed)
        if urs.held_keys['s']:
            self.camera.position += (0, 0, -self.camera.rotation_speed)
        if urs.held_keys['c']:
            self.camera.rotation_x += self.camera.rotation_speed
        if urs.held_keys['v']:
            self.camera.rotation_x -= self.camera.rotation_speed
        if urs.held_keys['e']:
            self.camera.rotation_y += self.camera.rotation_speed
        if urs.held_keys['q']:
            self.camera.rotation_y -= self.camera.rotation_speed
        if urs.held_keys['z']:
            self.camera.rotation_z += self.camera.rotation_speed
        if urs.held_keys['x']:
            self.camera.rotation_z -= self.camera.rotation_speed
        if urs.held_keys['space']:
            self.camera.position = CAMERA_INITIAL_POSITION
            self.camera.rotation = CAMERA_INITIAL_ROTATION
        return old_camera_position != urs.camera.position or old_camera_rotation != urs.camera.rotation
