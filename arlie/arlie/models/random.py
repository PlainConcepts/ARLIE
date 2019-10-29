import random
import numpy as np


class RAND:
    def __init__(self, env, *args, **kwars):
        self.env = env
        self.observation_space = env.observation_space
        self.action_space = env.action_space

    def predict(self, *args, **kwargs):
        """
        Get the model's action randomly
        :return: (int, np.ndarray) the model's action and the next state
        (used in recurrent policies)
        """
        return random.randint(0, self.action_space.n - 1), None

    def setup_model(self):
        pass

    def get_parameter_list(self):
        return []

    def _get_pretrain_placeholders(self):
        return None

    def learn(
        self,
        total_timesteps,
        callback=None,
        seed=None,
        log_interval=100,
        tb_log_name="run",
        reset_num_timesteps=True,
    ):
        return self

    def action_probability(
        self, observation, state=None, mask=None, actions=None, logp=False
    ):
        return None

    def save(self, save_path, cloudpickle=False):
        pass

    def load(cls, load_path, env=None, custom_objects=None, **kwargs):
        pass
