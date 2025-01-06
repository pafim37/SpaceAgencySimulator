import ursina as urs
import math
from camera import Camera
from http_client import HttpClient
from entities.sphere_entity import SphereEntity
from entities.player_entity import PlayerEntity

class App:
    def __init__(self):
        self.app = urs.Ursina(info=False)
        print(urs.camera.parent)
        self.camera = Camera(urs.camera)
        self.running = True
        self.bodies = []
        sky = urs.Sky(texture=urs.load_texture("images/stars.jpg"))
        
        
    def run(self):
        self.camera.inject_bodies(self.bodies)
        while self.running:
            self.camera.key_handle()
            self.app.step()

    def get_body_system(self):
        bodies, orbits = HttpClient.get_body_system()
        scale = 1
        for body in bodies:
            if body.name == "Player":
                entity = PlayerEntity(body)
            else:
                entity = SphereEntity(body)
            self.bodies.append(entity)
        for orbit in orbits:
            if orbit.orbitType == 0:
                vertices = [urs.Vec3(point.x, point.y, point.z) for point in orbit.points]
                mesh = urs.Mesh(vertices=vertices, mode='line', thickness=1)
                urs.Entity(model=mesh, name=orbit.name, color=urs.color.blue)
