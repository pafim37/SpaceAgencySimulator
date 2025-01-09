import ursina as urs
from camera import Camera
from http_client import HttpClient
from entities.sphere_entity import SphereEntity
from entities.player_entity import PlayerEntity
from notification import Notification
import queue

class App:
    def __init__(self):
        self._init_ursina()
        self._init_notification()
        self._init_fields()

    def _init_ursina(self):
        self.app = urs.Ursina()
        self.camera = Camera(urs.camera)
        urs.Sky(texture=urs.load_texture("images/stars.jpg"))

    def _init_notification(self):
        self.notification = Notification()
        self.notification.start()
        self.notification.register_listener("BodyDatabaseChanged", lambda *args: self._process_body_system())

    def _init_fields(self):
        self.bodies = []
        self.orbits = []
        self.task_queue = queue.Queue()
        self.running = True
        
    def run(self):
        self.camera.inject_bodies(self.bodies)
        while self.running:
            self.process_task_queue()
            self.camera.key_handle()
            self.app.step()
        self.notification.stop()

    def _process_body_system(self):
        # _process_body_system runs on Thread-1 (SignalR thread) while _clean_body_system() involve UI changes and runs on MainThread.
        # It causes break get_body_system function so that function should be processes on MainThread
        self.task_queue.put(lambda: self.get_body_system())

    def process_task_queue(self):
        while not self.task_queue.empty():
            task = self.task_queue.get()
            task()

    def get_body_system(self):
        self._clean_body_system()
        bodies, orbits = HttpClient.get_body_system()
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
                entity = urs.Entity(model=mesh, name=orbit.name, color=urs.color.blue)
                self.orbits.append(entity)

    def _clean_body_system(self):
        for body in self.bodies:
            urs.destroy(body.entity)
        self.bodies = []
        for orbit in self.orbits:
            urs.destroy(orbit)
        self.orbits = []