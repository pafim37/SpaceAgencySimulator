from ursina import *
from http_client import HttpClient
from entities.player_entity import PlayerEntity
from entities.body_entity import BodyEntity
from entities.soi_entity import SoiEntity
from entities.orbit_entity import OrbitEntity

class BodySystemController:
    def __init__(self):
        self.dt = 0.0
        return
    
    def scale(self, bodies, orbits):
        # TODO: move out this method to a separate utility class, and find a better way to scale the system to fit the scene
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

    def create_body_system_entities(self, transformSOI = False):
        self.__clear_body_system()
        bodies, orbits = self.__fetch_body_system_data()
        self.scale(bodies, orbits)
        self.orbits = orbits
        entity_bodies = self.transform_bodies_to_entities(bodies, transformSOI)
        self.transform_orbits_to_entities(orbits)
        return entity_bodies
    
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

    def update_body_positions(self):
        bodies, orbits = self.__fetch_body_system_data_at_time(self.dt)
        self.scale(bodies, orbits)
        
        # Update existing body entities
        for body, entity in zip(bodies, [e for e in scene.entities if isinstance(e, BodyEntity)]):
            entity.update_position(body.position)

        # Update existing orbit entities
        for orbit, entity in zip(orbits, [e for e in scene.entities if isinstance(e, OrbitEntity)]):
            entity.update_points(orbit.points)

        self.dt += math.pow(10, 18) # increase time by 1 million seconds (about 11.57 days) each update, to speed up the visualization of the system's evolution over time

    def __clear_body_system(self):
        for e in scene.entities[:]:
            if hasattr(e, 'tag') and e.tag == "bodysystemelement":
                destroy(e)

    def __fetch_body_system_data(self):
        return HttpClient.get_body_system() # bodies, orbits
    
    def __fetch_body_system_data_at_time(self, time):
        return HttpClient.get_body_system_at_time(time) # bodies, orbits
