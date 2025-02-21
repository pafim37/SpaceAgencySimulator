from signalrcore.hub_connection_builder import HubConnectionBuilder

class Notification:
    def __init__(self):
        hub_url = "http://localhost:6443/notification"
        self.hub_connection = HubConnectionBuilder()\
            .with_url(hub_url)\
            .build()
        
    def register_listener(self, event, function): 
        self.hub_connection.on(event, function)

    def start(self):
        self.hub_connection.start()

    def stop(self):
        self.hub_connection.stop()