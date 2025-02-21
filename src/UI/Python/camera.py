import ursina as urs
from settings import *
from compass import Compass

class Camera:
    def __init__(self, camera):
        self.camera = camera
        self.camera.position = CAMERA_INITIAL_POSITION
        self.camera.rotation = CAMERA_INITIAL_ROTATION
        self.camera.move_speed = CAMERA_MOVE_SPEED
        self.camera.rotation_speed = CAMERA_ROTATION_SPEED
        self.pivot = None
        self.compass = Compass()
        self.state = 1
        self.pressed_keys = []

    def inject_bodies(self, bodies):
        self.bodies = bodies

    def key_handle(self):
        for key in self.pressed_keys:
            if not urs.held_keys[key]:
                self.pressed_keys.remove(key)
        old_camera_position = self.camera.position
        old_camera_rotation = self.camera.rotation
        look_direction = urs.Vec3(self.camera.forward.normalized())
        normal_look_direction = urs.Vec3(self.camera.right.normalized())
        mouse_sensitivity = 10000
        if urs.held_keys['up arrow']:
            self.camera.position += self.camera.move_speed * look_direction
        if urs.held_keys['down arrow']:
            self.camera.position -= self.camera.move_speed * look_direction 
        if urs.held_keys['right arrow']:
            self.camera.rotation_y += self.camera.rotation_speed
        if urs.held_keys['left arrow']:
            self.camera.rotation_y -= self.camera.rotation_speed
        if urs.held_keys['w']:
            self.camera.rotation_x -= self.camera.rotation_speed
        if urs.held_keys['s']:
            self.camera.rotation_x += self.camera.rotation_speed
        if urs.held_keys['d']:
            self.camera.position += self.camera.move_speed * normal_look_direction 
        if urs.held_keys['a']:
            self.camera.position -= self.camera.move_speed * normal_look_direction
        if urs.held_keys['space'] and "space" not in self.pressed_keys:
            self.pressed_keys.append('space')
            self.camera.position = CAMERA_INITIAL_POSITION
            self.camera.rotation = CAMERA_INITIAL_ROTATION
            self.state -= 1
            self._set_view()
        if urs.held_keys['v'] and "v" not in self.pressed_keys:
            self._set_view()
        if urs.held_keys['1']:
            self.__set_pivot()
        if urs.held_keys['-']:
            self.__reset_pivot()

        if urs.mouse.left:
            self.camera.rotation_y -= urs.mouse.velocity[0] * mouse_sensitivity * urs.time.dt
            self.camera.rotation_x += urs.mouse.velocity[1] * mouse_sensitivity * urs.time.dt

        if urs.mouse.right and self.pivot is not urs.scene and self.pivot is not None:
            self.pivot.rotation_y += urs.mouse.velocity[0] * mouse_sensitivity * urs.time.dt
            self.pivot.rotation_x -= urs.mouse.velocity[1] * mouse_sensitivity * urs.time.dt
            self.compass.update(urs.camera)

        # print(self.camera.position)
        if old_camera_position != self.camera.position or old_camera_rotation != self.camera.rotation:
            self.compass.update(urs.camera)
            print(self.camera.position)
    
    def __set_pivot(self):
        my_entity = self.bodies[1] # TODO: remove hardcoded
        pivot = urs.Entity(position=my_entity.entity.position)

        self.camera.parent = pivot
        self.camera.position = (0, 0, -10)
        self.camera.look_at(my_entity.entity)
        self.pivot=pivot
        # self.compass.turn_off()

    def __reset_pivot(self):
        self.camera.parent = urs.scene
        # self.compass.turn_on()

    def _set_view(self):
        self.pressed_keys.append('v')
        if self.state > 1:
            self.state = 0
        if self.state == 0:
            self.camera.position = (0, 0, -200)
            self.camera.rotation = (0, 0, 0)
        elif self.state == 1:
            self.camera.position = (0, 0, 200)
            self.camera.rotation = (180, 0, 180)
        # elif self.state == 2:
        #     self.camera.position = (0, 200, 0)
        #     self.camera.rotation = (180, 0, 0)
        # elif self.state == 3:
        #     self.camera.position = (0, -200, 0)
        #     self.camera.rotation = (-90, 0, 0)
        # elif self.state == 4:
        #     self.camera.position = (-200, 0, 0)
        #     self.camera.rotation = (0, 0, 0)
        # elif self.state == 5:
        #     self.camera.position = (0, -200, 0)
        #     self.camera.rotation = (-90, 0, 0)
        self.state += 1
        
