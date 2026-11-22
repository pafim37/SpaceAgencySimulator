from panda3d.core import Quat, lookAt
from ursina import EditorCamera, camera


from coordinates import ORBITAL_X, ORBITAL_Y, ORBITAL_Z


class CameraController(EditorCamera):
    def __init__(self, entity_bodies):
        super().__init__()
        self.entity_bodies = entity_bodies
        self.body_index = 0
        self.axis_index = 1
        self.axis_frame = Quat()
        self.change_axis_rotation()

    def update_entities(self, entity_bodies):
        self.entity_bodies = entity_bodies
        self.body_index = 0

    def change_body_view(self):
        if self.entity_bodies:
            self.body_index = (self.body_index + 1) % len(self.entity_bodies)
            self.world_position = self.entity_bodies[self.body_index].world_position

    def follow_body(self):
        if self.entity_bodies:
            self.world_position = self.entity_bodies[self.body_index].world_position

    def change_axis_rotation(self):
        # Each pair gives the forward direction and the positive orbital yaw axis.
        frames = ((-ORBITAL_Z, ORBITAL_Y), (ORBITAL_Y, ORBITAL_X), (ORBITAL_X, ORBITAL_Z))
        self.axis_index = (self.axis_index + 1) % len(frames)
        forward, up = frames[self.axis_index]
        lookAt(self.axis_frame, forward, up)
        self._apply_axis_rotation()

    def update(self):
        super().update()
        self._apply_axis_rotation()

    def _apply_axis_rotation(self):
        # Keep the native mouse angles local and rotate the whole camera rig.
        self.setQuat(self.smoothing_helper.getQuat() * self.axis_frame)
        camera.rotation = (0, 0, 0)

    def reset_camera_configuration(self):
        self.axis_index = 0
        self.axis_frame = Quat()
        self.rotation = (0, 0, 0)
        self._apply_axis_rotation()
