from ursina import *
class ArrowEntity(Entity):
    def __init__(self, scale_factor=2, color_shaft=color.gray, color_head=color.gray, **kwargs):
        super().__init__(**kwargs)
        self.shaft = Entity(parent=self, model='cube', position=(-0.2, 0, 0), scale=(scale_factor, 0.2, 0.2), color=color_shaft)
        self.head = Entity(parent=self, model=Cone(), scale=(0.5, 0.5, 0.5), color=color_head, position=(scale_factor / 2-0.2, 0, 0), rotation=(0,0,90))