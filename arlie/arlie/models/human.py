import keyboard
import numpy as np


class HUMAN:
    def __init__(self, env, quit_key="q", *args, **kwars):
        self.env = env
        self._quit_key = quit_key

        if not hasattr(env.action_space, "n"):
            raise Exception("Human model only supports discrete action spaces")

        if not hasattr(env, "keyboard_map"):
            raise Exception("Environment does not define a keyboard-actions map")

    def predict(self, *args, **kwargs):
        """
        Get the model's action according to keyboard input
        :return: (int, np.ndarray) the model's
        action and the next state (used in recurrent policies)
        """
        action = 0
        if keyboard.is_pressed("q"):
            action = -1
        else:
            for k, v in self.env.keyboard_map.items():
                if keyboard.is_pressed(k):
                    action = v
                    break
        return action, None

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
