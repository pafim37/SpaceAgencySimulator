from ursina import *
from coordinates import to_scene


class OrbitEntity(Entity):
    def __init__(self, orbit, **kwargs):
        vertices = [to_scene(point) for point in orbit.points]
        mesh = Mesh(vertices=vertices, mode='line', thickness=1)
        super().__init__(
            model=mesh,
            name=orbit.name,
            color=color.blue,
            **kwargs
        )
        self.tag = "bodysystemelement"

    def update_points(self, new_points):
        vertices = [to_scene(point) for point in new_points]
        self.model.vertices = vertices
        self.model.generate()