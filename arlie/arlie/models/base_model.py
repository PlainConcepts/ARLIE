from abc import ABC, abstractmethod


class BaseModel(ABC):
    @abstractmethod
    def predict(self, *args, **kwargs):
        """
        Get the model's action randomly
        :return: (int, np.ndarray) the model's action and the next state
        (used in recurrent policies)
        """
        pass

    @abstractmethod
    def learn(
        self,
        total_timesteps,
        callback=None,
        seed=None,
        log_interval=100,
        tb_log_name="run",
        reset_num_timesteps=True,
    ):
        pass

    @abstractmethod
    def save(self, save_path, cloudpickle=False):
        pass

    @abstractmethod
    def load(cls, load_path, env=None, custom_objects=None, **kwargs):
        pass
