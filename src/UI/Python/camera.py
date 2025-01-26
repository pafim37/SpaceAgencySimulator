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


    def inject_bodies(self, bodies):
        self.bodies = bodies

    def key_handle(self):
        old_camera_position = urs.camera.position
        old_camera_rotation = urs.camera.rotation
        mouse_sensitivity = 10000
        if urs.held_keys['up arrow']:
            urs.camera.position += (0, self.camera.move_speed , 0) 
        if urs.held_keys['down arrow']:
            urs.camera.position += (0, -self.camera.move_speed , 0) 
        if urs.held_keys['right arrow']:
            urs.camera.position += (self.camera.move_speed , 0, 0) 
        if urs.held_keys['left arrow']:
            urs.camera.position += (-self.camera.move_speed , 0, 0)
        if urs.held_keys['w']:
            self.camera.position += (0, 0, self.camera.move_speed )
        if urs.held_keys['s']:
            self.camera.position += (0, 0, -self.camera.move_speed )
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
        if urs.held_keys['l']:
            if self.state == 0:
                self.camera.position = (0, -200, 0)
                self.camera.rotation = (-90, 0, 0)
            elif self.state == 1:
                self.camera.position = (0, 0, -200)
                self.camera.rotation = (0, 0, 0)
            elif self.state == 2:
                self.camera.position = (-200, 0, 0)
                self.camera.rotation = (0, 0, 0)
            self.state += 1
            if self.state > 1:
                self.state = 0
        if urs.held_keys['1']:
            self.__set_pivot()
        if urs.held_keys['-']:
            self.__reset_pivot()

        if urs.mouse.left:
            urs.camera.rotation_y -= urs.mouse.velocity[0] * mouse_sensitivity * urs.time.dt
            urs.camera.rotation_x += urs.mouse.velocity[1] * mouse_sensitivity * urs.time.dt

        if urs.mouse.right and self.pivot is not urs.scene and self.pivot is not None:
            self.pivot.rotation_y += urs.mouse.velocity[0] * mouse_sensitivity * urs.time.dt
            self.pivot.rotation_x -= urs.mouse.velocity[1] * mouse_sensitivity * urs.time.dt
            self.compass.update(urs.camera)

        # print(urs.camera.position)
        if old_camera_position != urs.camera.position or old_camera_rotation != urs.camera.rotation:
            self.compass.update(urs.camera)
            print(urs.camera.position)
    
    def __set_pivot(self):
        my_entity = self.bodies[1] # TODO: remove hardcoded
        pivot = urs.Entity(position=my_entity.entity.position)

        urs.camera.parent = pivot
        urs.camera.position = (0, 0, -10)
        urs.camera.look_at(my_entity.entity)
        self.pivot=pivot
        # self.compass.turn_off()

    def __reset_pivot(self):
        urs.camera.parent = urs.scene
        # self.compass.turn_on()
