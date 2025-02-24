from ursina import *
class OrbitEntity(Entity):
    def __init__(self, orbit, **kwargs):
        vertices = [Vec3(point.x, point.y, point.z) for point in orbit.points]
        mesh = Mesh(vertices=vertices, mode='line', thickness=1)
        super().__init__(
            model=mesh,
            name=orbit.name,
            color=color.blue,
            **kwargs
        )
        self.tag = "bodysystemelement"