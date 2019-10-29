import numpy as np
from numpy.linalg import norm
from arlie.envs.base_reward import BaseReward
from arlie.envs.lunar_lander.reward import LunarLanderReward
from arlie.envs.lunar_lander.env import LunarLander


class CustomReward(BaseReward):
    """Reward for going to the center as far as possible"""

    _FINAL_REWARD = 100.0
    _STEP_PENALTY = 0.05

    def __init__(self):
        self._steps = 0
        self._shaping = None

    def evaluate(self, observation, action, failed_landing):
        self._steps += 1
        state = LunarLander.parse_observation(observation)

        self.XYZ = norm(state["position"])
        self.neg_XZ = norm(
            [1 - abs(state["position"][0]), 1 - abs(state["position"][2])]
        )
        legs_contact = np.sum(state["leg_contact"])

        # penalty when going away from center
        position_shaping = 100 * self.XYZ  # 100 * |xyz|

        # reward for contacting with a leg
        leg_shaping = 5 * legs_contact

        reward = 0.0
        shaping = -position_shaping + leg_shaping
        if self._shaping is not None:
            reward = shaping - self._shaping
        self._shaping = shaping

        reward -= self._STEP_PENALTY

        done = False
        if failed_landing or legs_contact >= 3:
            done = True
            reward = self._FINAL_REWARD * self.neg_XZ

        return reward, done

    def reset(self):
        self._steps = 0
        self._shaping = None


class LunarCustomReward(LunarLanderReward):
    """ Custom reward that applies no penalty for engine usage (infinite fuel)
    and allows more velocity for touching ground without crashing
    (the lander is more resistant to hits) """

    _MAX_SAFE_LANDING_VELOCITY = 0.2

    def _compute_reward(self, state, action):
        reward = self._shaping_reward(state)
        reward -= self._engine_penalty(action)
        return reward

    def _engine_penalty(self, action):
        engine_penalty = 0.0
        if action > 0:
            if action < 7:  # side engine
                engine_penalty = self._SIDE_ENGINE_PENALTY
            else:  # main engine
                engine_penalty = self._MAIN_ENGINE_PENALTY
        return engine_penalty
