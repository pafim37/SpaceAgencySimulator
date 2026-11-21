from ursina import *
from http_client import HttpClient
from entities.player_entity import PlayerEntity
from entities.body_entity import BodyEntity
from entities.soi_entity import SoiEntity
from entities.orbit_entity import OrbitEntity

class BodySystemController:
    def __init__(self):
        self.dt = 0.0

    def create_body_system_entities(self, transformSOI = False):
        self.__clear_body_system()
        self.sois = []
        bodies, orbits = self.__fetch_body_system_data()
        self.orbits = orbits
        entity_bodies = self.__transform_bodies_to_entities(bodies, transformSOI)
        self.__transform_orbits_to_entities(orbits)
        return entity_bodies
    
    def update_body_positions(self):
        self.dt += 365.25 * 24 * 3600 / 1000 # math.pow(10, -14) # Increment time for the next update
        
        bodies, orbits = self.__fetch_body_system_data_at_time(self.dt)
        
        # Update existing body entities
        for body, entity in zip(bodies, [e for e in scene.entities if isinstance(e, BodyEntity)]):
            entity.update_position(body.position)

        # Update existing orbit entities
        for orbit, entity in zip(orbits, [e for e in scene.entities if isinstance(e, OrbitEntity)]):
            entity.update_points(orbit.points)

        # Update SOI TODO: fix it
        for soi, entity in zip(self.sois, [e for e in scene.entities if isinstance(e, SoiEntity)]):
            body = next((b for b in bodies if b.name == entity.name), None)
            entity.update_position(body.position)
        

    def __transform_orbits_to_entities(self, orbits):
        for orbit in orbits:
            OrbitEntity(orbit)
    
    def __transform_bodies_to_entities(self, bodies, transformSOI = False):
        entity_bodies = []
        for body in bodies:
            if body.name.lower() == "player":
                PlayerEntity(body)
            else:
                entity_bodies.append(BodyEntity(body))
                if transformSOI and body.sphereOfInfluenceRadius is not None:
                    soi = SoiEntity(body)
                    self.sois.append(soi)
        return entity_bodies

    def __clear_body_system(self):
        for e in scene.entities[:]:
            if hasattr(e, 'tag') and e.tag == "bodysystemelement":
                destroy(e)

    def __fetch_body_system_data(self):
        return HttpClient.get_body_system() # bodies, orbits
    
    def __fetch_body_system_data_at_time(self, time):
        print(f"Fetching body system data at time: {time}")
        return HttpClient.get_body_system_at_time(time) # bodies, orbits
