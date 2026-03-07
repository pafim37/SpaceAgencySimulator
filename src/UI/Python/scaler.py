import math

class Scaler:
    dt = math.pow(10, 18)
    
    @staticmethod
    def scale(bodies, orbits):
        scale = 100 / 147098291000
        for body in bodies:
            body.position.x *= scale
            body.position.y *= scale
            body.position.z *= scale
        for orbit in orbits:
            for point in orbit.points:
                point.x *= scale
                point.y *= scale
                point.z *= scale