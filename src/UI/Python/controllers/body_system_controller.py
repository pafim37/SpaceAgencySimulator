from ursina import *
from http_client import HttpClient
from entities.player_entity import PlayerEntity
from entities.body_entity import BodyEntity
from entities.soi_entity import SoiEntity
from entities.orbit_entity import OrbitEntity

class BodySystemController:
    def __init__(self):
        return

    def create_body_system_entities(self, transformSOI = False):
        self.clear_body_system()
        bodies, orbits = self.fetch_body_system_data()
        entity_bodies = self.transform_bodies_to_entities(bodies, transformSOI)
        self.transform_orbits_to_entities(orbits)
        return entity_bodies

    def clear_body_system(self):
        for e in scene.entities[:]:
            if hasattr(e, 'tag') and e.tag == "bodysystemelement":
                destroy(e)
    
    def fetch_body_system_data(self):
        return HttpClient.get_body_system() # bodies, orbits

    def transform_bodies_to_entities(self, bodies, transformSOI = False):
        entity_bodies = []
        for body in bodies:
            if body.name.lower() == "player":
                PlayerEntity(body)
            else:
                entity_bodies.append(BodyEntity(body))
                if transformSOI and body.sphereOfInfluenceRadius is not None:
                    SoiEntity(body)
        return entity_bodies

    def transform_orbits_to_entities(self, orbits):
        for orbit in orbits:
            OrbitEntity(orbit)
