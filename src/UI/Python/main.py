from ursina import *
from notification import Notification
import queue
from controllers.body_system_controller import BodySystemController
from controllers.camera_controller import CameraController
from controllers.world_controller import WorldController

"""
add_get_body_system_function_to_task_queue runs on Thread-1 (SignalR thread) while destroy() involve UI changes and runs on MainThread.
It causes break get_body_system function so that function should be processes on MainThread
"""
def add_get_body_system_function_to_task_queue():
    task_queue.put(lambda: body_system_controller.create_body_system_entities(transformSOI = True))

def run_tasks_from_queue():
    while not task_queue.empty():
        task = task_queue.get()
        try:
            task()
        except Exception as e:
            print(f"Task from queue failed: {e}")

def update():
    run_tasks_from_queue()
    camera_controller.handle_input()
    compass.update_directions(camera_controller.pivot)

def input(key):
    global camera_controller
    if key == "v":
        camera_controller.change_body_view()
    if key == "scroll down":
        camera_controller.zoom(True)
    if key == "scroll up":
        camera_controller.zoom(False)
    if key == 'r':
        entity_bodies = body_system_controller.create_body_system_entities(transformSOI = True)
        camera_controller = CameraController(entity_bodies)

if __name__ == '__main__':

    app = Ursina()
    Sky(texture=load_texture("images/stars.jpg"))
    
    # notification
    notification = Notification()
    notification.start()
    task_queue = queue.Queue()
    notification.register_listener("BodyDatabaseChanged", lambda *args: add_get_body_system_function_to_task_queue())

    
    # body sytem
    body_system_controller = BodySystemController()
    entity_bodies = body_system_controller.create_body_system_entities(transformSOI = True)

    # camera
    camera_controller = CameraController(entity_bodies)

    # world
    wc = WorldController()
    wc.create_world_axis()
    compass = wc.create_compass()

    # run
    app.run()


