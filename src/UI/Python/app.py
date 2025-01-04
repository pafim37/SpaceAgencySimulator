import ursina as urs
import math
from camera import Camera
from http_client import HttpClient
from compass import Compass
from entities.sphere_entity import SphereEntity 

class App:
    def __init__(self):
        self.app = urs.Ursina(info=False)
        self.camera = Camera(urs.camera)
        self.compass = Compass()
        self.running = True
        self.entities = []

    def run(self):
        while self.running:
            is_camera_move = self.camera.key_handle()
            if is_camera_move:
                self.compass.update(urs.camera)
            self.app.step()

    def get_body_system(self):
        bodies, orbits = HttpClient.get_body_system()
        scale = 1
        for body in bodies:
            entity = SphereEntity(body)
            self.entities.append(entity)
        for orbit in orbits:
            vertices = [urs.Vec3(point.x, point.y, point.z) for point in orbit.points]
            mesh = urs.Mesh(vertices=vertices, mode='line', thickness=1)
            urs.Entity(model=mesh, name=orbit.name, color=urs.color.blue)
