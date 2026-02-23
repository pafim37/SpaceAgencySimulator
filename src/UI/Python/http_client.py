import requests
import json
from types import SimpleNamespace

class HttpClient:
    @staticmethod
    def get_scaled_body_system():
        url = "http://localhost:5000/body-system/scaled"
        r = requests.get(url)
        data = json.loads(r.content, object_hook=lambda d: SimpleNamespace(**d))
        return data.bodies, data.orbits