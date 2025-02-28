from ursina import *
from settings import *

class CameraController:
    def __init__(self, entity_bodies):
        self.rotation_resetter = Entity()
        self.pivot = Entity(parent=self.rotation_resetter)
        self.set_camera(entity_bodies)

    def set_camera(self, entity_bodies):
        self.entity_bodies = entity_bodies
        self.body_index = 0
        self.current_body_view = entity_bodies[0]
        self.pivot.position = self.current_body_view.position
        self.pivot.rotation = (0, 0, 0)
        camera.rotation = CAMERA_INITIAL_ROTATION
        camera.position = CAMERA_INITIAL_POSITION
        camera.parent = self.pivot

    def change_body_view(self):
        self.body_index += 1
        if self.body_index > len(self.entity_bodies) - 1:
            self.body_index = 0
        self.current_body_view = self.entity_bodies[self.body_index]
        self.pivot.position = self.current_body_view.position
        camera.position = CAMERA_INITIAL_POSITION
        camera.parent = self.pivot

    def zoom(self, positive):
        factor = 1.0 if positive else -1.0
        distance_vector = camera.world_position - self.pivot.world_position
        direction = distance_vector.normalized()
        camera.world_position += direction * (1 + math.pow(4, math.log10(distance_vector.length()) - 1)) * factor

    def handle_input(self):
        self.pivot.rotation_x -= mouse.velocity[1] * mouse.right * 100
        self.pivot.rotation_y += mouse.velocity[0] * mouse.right * 100
            
        self.rotation_resetter.rotation_x += 100 * (held_keys['i'] - held_keys['o']) * time.dt
        self.rotation_resetter.rotation_z += 100 * (held_keys['p'] - held_keys['l']) * time.dt
        self.pivot.rotation = self.pivot.world_rotation
        self.rotation_resetter.rotation = (0,0,0)

        if held_keys['q']:
            camera.rotation_x += 1
        if held_keys['a']:
            camera.rotation_x -= 1
        if held_keys['w']:
            camera.rotation_y += 1
        if held_keys['s']:
            camera.rotation_y -= 1
        if held_keys['e']:
            camera.rotation_z += 1
        if held_keys['d']:
            camera.rotation_z -= 1

        if held_keys['t']:
            self.pivot.rotation_x += 1
        if held_keys['g']:
            self.pivot.rotation_x -= 1
        if held_keys['y']:
            self.pivot.rotation_y += 1
        if held_keys['h']:
            self.pivot.rotation_y -= 1
        if held_keys['u']:
            self.pivot.rotation_z += 1
        if held_keys['j']:
            self.pivot.rotation_z -= 1
