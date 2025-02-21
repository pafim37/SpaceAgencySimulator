import requests
import json
from types import SimpleNamespace

import ursina as urs
class HttpClient:
    @staticmethod
    def get_body_system():
        url = "http://localhost:5000/body-system"
        r = requests.get(url)
        data = json.loads(r.content, object_hook=lambda d: SimpleNamespace(**d))
        return data.bodies, data.orbits