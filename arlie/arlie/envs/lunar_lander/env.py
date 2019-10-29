import os
import subprocess
import time

import grpc
import gym
import numpy as np
from arlie.envs.base_reward import BaseReward
from arlie.envs.lunar_lander.python_protos import Lunar3D_pb2, Lunar3D_pb2_grpc
from gym import spaces


class LunarLander(gym.Env):

    metadata = {"render.modes": ["human"]}
    # reward_range = (-100.0, 100.0)
    # spec = None

    # keys map for Human model
    keyboard_map = {
        "space": 7,
        "down": 4,
        "up": 3,
        "left": 6,
        "right": 5,
        "z": 2,
        "x": 1,
    }

    def __init__(self):
        self.channel = None
        self.stub = None

        self._seed = 0

    # elapsed_t = 0.0
    # count = 0.0

    def step(self, action):
        # self.count += 1.0
        # _t = time.time()
        request = Lunar3D_pb2.Action(EngineAction=action)
        result = self.stub.PerformAction(request)
        # self.elapsed_t += time.time() - _t

        # if self.count % 1000 == 0:
        #     print("Elapsed time: {} ms".format(self.elapsed_t / self.count))

        observation = [
            getattr(result.observation, field.name)
            for field in result.observation.DESCRIPTOR.fields
        ]
        reward, done = self.reward.evaluate(observation, action, result.done)
        return (
            np.array(observation, dtype=np.float32),
            reward,
            done,
            {"failed_landing": result.done},
        )

    def reset(self):
        result = self.stub.Reset(Lunar3D_pb2.ServiceMessage())
        state = [getattr(result, field.name) for field in result.DESCRIPTOR.fields]
        self.reward.reset()
        return np.array(state, dtype=np.float32)

    def render(self, mode="human"):
        self.stub.Render(Lunar3D_pb2.ServiceMessage())

    def close(self):
        self.process.terminate()

    def seed(self, seed):
        self._seed = seed

    def launch(
        self,
        reward=None,
        seed=0,
        host="localhost",
        port=3000,
        render_mode=True,
        reset_mode="center",
    ):
        if reward is None:
            from arlie.envs.lunar_lander.reward import LunarLanderReward

            self.reward = LunarLanderReward()
        elif not isinstance(reward, BaseReward):
            print("Reward must inheritate from 'BaseReward'")
            exit(-1)
        else:
            self.reward = reward

        self.host = host
        self.port = port
        self.render_mode = render_mode
        self.reset_mode = reset_mode
        self.seed(seed)

        self.__init_connection()
        self.channel = grpc.insecure_channel("{}:{}".format(self.host, self.port))
        self.stub = Lunar3D_pb2_grpc.Lunar3DServiceStub(self.channel)

        act_size = self.stub.GetActionDim(Lunar3D_pb2.ServiceMessage()).value  # 8
        obs_size = self.stub.GetObservationDim(Lunar3D_pb2.ServiceMessage()).value  # 16

        self.action_space = spaces.Discrete(act_size)
        # useful range is -1 .. +1, but spikes can be higher
        self.observation_space = spaces.Box(
            -np.inf, np.inf, shape=(obs_size,), dtype=np.float32
        )

    def __init_connection(self):
        path = os.path.dirname(os.path.abspath(__file__))
        cmd = os.path.join(path, "LunarLander", "RLEnvs.exe")
        self.process = subprocess.Popen(
            [
                cmd,
                "server",
                "{}".format(self.port),
                "{}".format(self.render_mode),
                "{}".format(self.reset_mode),
                "{}".format(self._seed),
            ],
            stdout=subprocess.PIPE,
        )
        time.sleep(5)

    @staticmethod
    def parse_observation(observation):
        return {
            "position": observation[0:3],  # [X, Y, Z]
            "velocity": observation[3:6],  # [X, Y, Z]
            "angle": observation[6:9],  # [X, Y, Z]
            "angular_velocity": observation[9:12],  # [X, Y, Z]
            "leg_contact": observation[12:16],  # [Front, Back, Left, Right]
        }
