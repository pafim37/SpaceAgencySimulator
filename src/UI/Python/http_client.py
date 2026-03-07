import requests
import json
from types import SimpleNamespace

class HttpClient:
    @staticmethod
    def get_body_system():
        url = "http://localhost:5000/body-system/"
        r = requests.get(url)
        data = json.loads(r.content, object_hook=lambda d: SimpleNamespace(**d))
        return data.bodies, data.orbits
    
    @staticmethod
    def get_body_system_at_time(time):
        url = f"http://localhost:5000/body-system/{time}"
        r = requests.get(url)
        data = json.loads(r.content, object_hook=lambda d: SimpleNamespace(**d))
        return data.bodies, data.orbits