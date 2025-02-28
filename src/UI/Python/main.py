from ursina import *
from settings import *
from notification import Notification
from http_client import HttpClient
from entities.body_entity import BodyEntity
from entities.player_entity import PlayerEntity
from entities.orbit_entity import OrbitEntity
from entities.soi_entity import SoiEntity
import queue
from entities.compass_entity import CompassEntity

"""
add_get_body_system_function_to_task_queue runs on Thread-1 (SignalR thread) while destroy() involve UI changes and runs on MainThread.
It causes break get_body_system function so that function should be processes on MainThread
"""
def add_get_body_system_function_to_task_queue(self):
    self.task_queue.put(lambda: self.get_body_system())

def run_tasks_from_queue(self):
    while not self.task_queue.empty():
        task = self.task_queue.get()
        try:
            task()
        except Exception as e:
            print(f"Task from queue failed: {e}")

def get_body_system():
    global scene_body
    scene_body = []
    for e in scene.entities[:]:
        if hasattr(e, 'tag') and e.tag == "bodysystemelement":
            destroy(e)
    bodies, orbits = HttpClient.get_body_system()
    global body_number
    body_number = len(bodies)
    for body in bodies:
        if body.name.lower() == "player":
            PlayerEntity(body)
        else:
            scene_body.append(BodyEntity(body))
            if body.sphereOfInfluenceRadius is not None:
                SoiEntity(body)
    for orbit in orbits:
        OrbitEntity(orbit)

def update():
    global pivot
    
    pivot.rotation_x -= mouse.velocity[1] * mouse.right * 100
    pivot.rotation_y += mouse.velocity[0] * mouse.right * 100
        
    rotation_resetter.rotation_x += 100 * (held_keys['i'] - held_keys['o']) * time.dt
    rotation_resetter.rotation_z += 100 * (held_keys['p'] - held_keys['l']) * time.dt
    pivot.rotation = pivot.world_rotation
    rotation_resetter.rotation = (0,0,0)

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
        pivot.rotation_x += 1
    if held_keys['g']:
        pivot.rotation_x -= 1
    if held_keys['y']:
        pivot.rotation_y += 1
    if held_keys['h']:
        pivot.rotation_y -= 1
    if held_keys['u']:
        pivot.rotation_z += 1
    if held_keys['j']:
        pivot.rotation_z -= 1

    compass.update_directions(pivot)


def input(key):
    if key == "v": # change body watch
        change_body_view()
    if key == "scroll down":
        zoom(True)
    if key == "scroll up":
        zoom(False)
    if key == 'r':
        get_body_system()
        set_camera()

def zoom(positive):
    factor = 1.0 if positive else -1.0
    distance_vector = camera.world_position - pivot.world_position
    direction = distance_vector.normalized()
    camera.world_position += direction * (1 + math.pow(4, math.log10(distance_vector.length()) - 1)) * factor

def set_camera():
    global pivot, body_watcher_index
    body_watcher_index = 0
    pivot.rotation = (0, 0, 0)
    camera.z = scene_body[body_watcher_index].position.z - 20 # central body watch (Sun)
    camera.rotation = CAMERA_INITIAL_ROTATION
    body_watch = scene_body[body_watcher_index]
    pivot.position = body_watch.position
    camera.position = CAMERA_INITIAL_POSITION
    camera.parent = pivot
    compass.update_directions(pivot)

def change_body_view():
    global body_watcher_index, body_number
    body_watcher_index += 1
    if body_watcher_index > body_number - 1:
        body_watcher_index = 0
    body_watch = scene_body[body_watcher_index]
    pivot.position = body_watch.position
    camera.position = CAMERA_INITIAL_POSITION
    camera.parent = pivot

def create_world_axis():
    ox = Entity(model="cube", scale=(10000,0.2,0.2), position=(0,0,0), color=color.blue)
    oy = Entity(model="cube", scale=(10000,0.2,0.2), position=(0,0,0), rotation=(0, 0, -90), color=color.green)
    oz = Entity(model="cube", scale=(10000,0.2,0.2), position=(0,0,0), rotation=(0, -90, 0), color=color.red)

if __name__ == '__main__':
    body_watcher_index = 0
    body_number = 0
    scene_body = []

    app = Ursina()
    Sky(texture=load_texture("images/stars.jpg"))
    
    # fetch data
    get_body_system()

    # notification
    notification = Notification()
    notification.start()
    notification.register_listener("BodyDatabaseChanged", lambda *args: self.add_get_body_system_function_to_task_queue())

    # camera setup
    rotation_resetter = Entity()
    pivot = Entity(parent=rotation_resetter)

    # create helpers
    compass = CompassEntity()
    create_world_axis()
    set_camera()

    # run
    app.run()


