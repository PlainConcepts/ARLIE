import os
import random
import zipfile
from collections import deque

import numpy as np
from tensorflow.keras import Sequential
from tensorflow.keras.activations import linear, relu
from tensorflow.keras.layers import Dense
from tensorflow.keras.models import load_model
from tensorflow.keras.optimizers import Adam
from tensorflow.python.framework.ops import disable_eager_execution

from common.save_util import data_to_json, json_to_data


class DQN:
    """Agent using deep q learn algorithm"""

    def __init__(
        self,
        env,
        *args,
        epsilon=1.0,
        gamma=0.99,
        batch_size=64,
        epsilon_min=0.01,
        lr=0.001,
        epsilon_decay=0.996,
        memory_length=int(1e6),
        seed=None,
        **kwars
    ):
        disable_eager_execution()

        self.env = env
        self.epsilon = epsilon
        self.gamma = gamma
        self.batch_size = batch_size
        self.epsilon_min = epsilon_min
        self.lr = lr
        self.epsilon_decay = epsilon_decay
        self.memory = deque(maxlen=memory_length)

        self.observation_space = None
        self.action_space = None
        self.obs_size = 0
        self.action_size = 0
        self.policy = None
        if self.env is not None:
            self.observation_space = env.observation_space
            self.action_space = env.action_space
            self.obs_size = self.observation_space.shape[0]
            self.action_size = self.action_space.n
            self.policy = self._build_policy()

        self.seed(seed)

    def predict(self, observation, *args, **kwargs):
        """
        Get the model's action from an observation
        :param observation: (np.ndarray) the input observation
        :return: (np.ndarray, np.ndarray) the model's action and the next state
        (used in recurrent policies)
        """
        if self.env is not None and np.random.rand() <= self.epsilon:
            action = random.randrange(self.action_size)
        else:
            act_values = self.policy.predict(observation)
            action = np.argmax(act_values[0])
        return action, None

    def learn(
        self,
        total_timesteps,
        callback=None,
        seed=None,
        log_interval=100,
        tb_log_name="run",
        reset_num_timesteps=True,
    ):

        obs = np.reshape(self.env.reset(), (1, self.obs_size))
        episode_rewards = [0.0]
        for step in range(total_timesteps):
            action, _ = self.predict(obs)
            obs_, reward, done, _ = self.env.step(action)
            obs_ = np.reshape(obs_, (1, self.obs_size))

            self._store_transition(obs, action, reward, obs_, done)
            self._replay()

            episode_rewards[-1] += reward
            obs = obs_

            if done:
                episode_rewards.append(0.0)
                obs = np.reshape(self.env.reset(), (1, self.obs_size))

    def save(self, save_path):
        """
        Save the current parameters to file
        :param save_path: (str or file-like) The save location
        """
        # params
        model_params = {
            "batch_size": self.batch_size,
            "lr": self.lr,
            "epsilon": self.epsilon,
            "gamma": self.gamma,
            "epsilon_min": self.epsilon_min,
            "epsilon_decay": self.epsilon_decay,
            "memory": self.memory,
            "observation_space": self.observation_space,
            "action_space": self.action_space,
            "_seed": self._seed,
        }

        serialized_params = data_to_json(model_params)
        self.policy.save(save_path + ".h5")

        # Check postfix if save_path is a string
        if isinstance(save_path, str):
            _, ext = os.path.splitext(save_path)
            if ext == "":
                save_path += ".zip"

        # Create a zip-archive and write our params
        # there. This works when save_path
        # is either str or a file-like
        with zipfile.ZipFile(save_path, "w") as file_:
            # Do not try to save "None" elements
            file_.writestr("parameters", serialized_params)

    @classmethod
    def load(cls, load_path, load_data=True, env=None, custom_objects=None, **kwargs):
        """
        Load the model from file
        :param load_path: (str or file-like) the saved parameter location
        :param load_data: (bool) whether parameters should be loaded in the model
        :param env: (Gym Envrionment) the new environment to run the loaded model on
            (can be None if you only need prediction from a trained model)
        :param custom_objects: (dict) Dictionary of objects to replace
            upon loading. If a variable is present in this dictionary as a
            key, it will not be deserialized and the corresponding item
            will be used instead. Similar to custom_objects in
            `keras.models.load_model`. Useful when you have an object in
            file that can not be deserialized.
        :param kwargs: extra arguments to change the model when loading
        """
        # Check if file exists if load_path is
        # a string
        if isinstance(load_path, str):
            if not os.path.exists(load_path):
                if not os.path.exists(load_path + ".zip") or not os.path.exists(
                    load_path + ".h5"
                ):
                    raise ValueError(
                        "Error: the file {} could not be found".format(load_path)
                    )

        # Open the zip archive and load data.
        try:
            with zipfile.ZipFile(load_path + ".zip", "r") as file_:
                namelist = file_.namelist()
                # If data or parameters is not in the
                # zip archive, assume they were stored
                # as None (_save_to_file allows this).
                params = None
                if "parameters" in namelist and load_data:
                    # Load class parameters and convert to string
                    # (Required for json library in Python 3.5)
                    json_data = file_.read("parameters").decode()
                    params = json_to_data(json_data, custom_objects=custom_objects)

        except zipfile.BadZipFile:
            print("ERROR: model could not be loaded")
            return None

        model = cls(env=env)
        model.__dict__.update(params)
        model.__dict__.update(kwargs)

        model.obs_size = model.observation_space.shape[0]
        model.action_size = model.action_space.n
        model.policy = load_model(load_path + ".h5")

        return model

    def seed(self, seed=None):
        self._seed = seed
        np.random.seed(seed)

    def setup_model(self):
        pass

    def get_parameter_list(self):
        return []

    def _get_pretrain_placeholders(self):
        return None

    def _store_transition(self, observation, action, reward, next_observation, done):
        self.memory.append((observation, action, reward, next_observation, done))

    def _replay(self):

        if len(self.memory) < self.batch_size:
            return

        minibatch = random.sample(self.memory, self.batch_size)

        observations = np.array([i[0] for i in minibatch])
        actions = np.array([i[1] for i in minibatch])
        rewards = np.array([i[2] for i in minibatch])
        next_observations = np.array([i[3] for i in minibatch])
        dones = np.array([i[4] for i in minibatch])

        observations = np.squeeze(observations)
        next_observations = np.squeeze(next_observations)

        targets = rewards + self.gamma * (
            np.amax(self.policy.predict_on_batch(next_observations), axis=1)
        ) * (1 - dones)

        targets_full = self.policy.predict_on_batch(observations)
        ind = np.array([i for i in range(self.batch_size)])
        targets_full[[ind], [actions]] = targets

        self.policy.fit(observations, targets_full, epochs=1, verbose=0)
        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay

    def _build_policy(self):
        policy = Sequential()
        policy.add(Dense(150, input_dim=self.obs_size, activation=relu))
        policy.add(Dense(120, activation=relu))
        policy.add(Dense(self.action_size, activation=linear))
        policy.compile(loss="mse", optimizer=Adam(lr=self.lr))
        return policy
