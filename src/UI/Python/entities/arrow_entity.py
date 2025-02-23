from ursina import *
class ArrowEntity():
    def __init__(self, position=(0,0,0), rotation=(0,0,0), scale_factor=2, color=color.gray, scale=(1,1,1), start_origin = False):
        self.e = Entity(position=position, rotation=rotation, scale=scale)
        self.shaft = Entity(parent=self.e, model='cube', position=(-0.2, 0, 0), scale=(scale_factor, 0.2, 0.2), color=color)
        self.head = Entity(parent=self.e, model=Cone(), scale=(0.5, 0.5, 0.5), color=color, position=(scale_factor / 2-0.2, 0, 0), rotation=(0,0,90))
        self.tag = "bodysystemelement"

    def point_to(self, direction):
        direction = direction.normalized()

        yaw = math.degrees(math.atan2(direction.x, direction.z))
        pitch = -math.degrees(math.asin(direction.y))

        self.e.rotation = (pitch, yaw, 0)