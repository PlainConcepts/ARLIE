def make(id, *args, **kwargs):
    env = env_registration.make(id)
    env.launch(*args, **kwargs)
    return env


def register(id, **kwargs):
    return env_registration.register(id, **kwargs)


def load(name):
    import pkg_resources

    entry_point = pkg_resources.EntryPoint.parse("x={}".format(name))
    if hasattr(entry_point, "resolve"):
        return entry_point.resolve()
    else:
        return entry_point.load(False)


class EnvRegistration:
    def __init__(self):
        self.registered_events = {}

    def make(self, id):
        return self.registered_events[id]

    def register(self, id, entry_point=None):
        cls = load(entry_point)
        env = cls()
        self.registered_events[id] = env


env_registration = EnvRegistration()
