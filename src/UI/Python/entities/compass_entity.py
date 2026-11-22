from math import atan2, degrees, hypot
from ursina import Entity, Mesh, camera, color, scene, window
from coordinates import ORBITAL_X, ORBITAL_Y, ORBITAL_Z


class CompassEntity(Entity):
    def __init__(self, **kwargs):
        super().__init__(parent=camera.ui, **kwargs)
        vertices = [
            (0, -0.002, 0), (0.033, -0.002, 0),
            (0.033, 0.002, 0), (0, 0.002, 0),
            (0.031, -0.006, 0), (0.045, 0, 0), (0.031, 0.006, 0),
        ]
        triangles = [(0, 1, 2), (0, 2, 3), (4, 5, 6)]
        self.arrow_x, self.arrow_y, self.arrow_z = (
            Entity(
                parent=self,
                model=Mesh(vertices=vertices, triangles=triangles),
                color=axis_color,
                double_sided=True,
            )
            for axis_color in (color.blue, color.green, color.red)
        )
        self.axes = (
            (self.arrow_x, ORBITAL_X),
            (self.arrow_y, ORBITAL_Y),
            (self.arrow_z, ORBITAL_Z),
        )
        self.update_directions()

    def turn_off(self):
        self.enabled = False

    def turn_on(self):
        self.enabled = True

    def update_directions(self):
        top_right = window.top_right
        self.position = (top_right.x - 1.5 * 0.065, top_right.y - 0.065, 0)
        for arrow, axis in self.axes:
            direction = camera.getRelativeVector(scene, axis)
            arrow.rotation_z = -degrees(atan2(direction.y, direction.x))
            arrow.scale_x = hypot(direction.x, direction.y)
