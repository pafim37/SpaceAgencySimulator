from ursina import Vec3


def to_scene(vector):
    """Convert right-handed orbital coordinates to Ursina scene coordinates."""
    return Vec3(vector.x, vector.y, -vector.z)


ORBITAL_X = to_scene(Vec3(1, 0, 0))
ORBITAL_Y = to_scene(Vec3(0, 1, 0))
ORBITAL_Z = to_scene(Vec3(0, 0, 1))
