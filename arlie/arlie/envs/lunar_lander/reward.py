import numpy as np
from numpy.linalg import norm
from arlie.envs.base_reward import BaseReward
from arlie.envs.lunar_lander.env import LunarLander


class LunarLanderReward(BaseReward):
    _CRASH_REWARD = -100
    _LANDING_REWARD = 100

    def __init__(self):
        self._steps = 0
        self._shaping = None

    def evaluate(self, observation, action, failed_landing):
        self._steps += 1
        state = LunarLander.parse_observation(observation)

        self._compute_norms(state)

        reward = self._compute_reward(state, action)

        done, state_reward = self._compute_done(state, failed_landing)

        if state_reward is not None:
            reward = state_reward

        return reward, done

    def reset(self):
        self._steps = 0
        self._shaping = None

    def _compute_norms(self, state):
        self.XYZ = norm(state["position"])
        self.V = norm(state["velocity"])
        self.legs_contact = np.sum(state["leg_contact"])

    def _compute_reward(self, state, action):
        reward = self._shaping_reward(state)
        return reward

    def _shaping_reward(self, state):
        # penalty when going away from center
        position_shaping = 100 * self.XYZ  # 100 * |xyz|
        # penalty for going fast
        velocity_shaping = 100 * self.V  # 100 * |V|
        # reward for contacting with a leg
        leg_shaping = 5 * self.legs_contact

        shaping = -position_shaping - velocity_shaping + leg_shaping

        reward = 0.0
        if self._shaping is not None:
            reward = shaping - self._shaping
        self._shaping = shaping

        return reward

    def _compute_done(self, state, failed_landing):
        done = False
        reward = None

        game_over = self._game_over(state, failed_landing)
        if game_over:
            done = True
            reward = self._CRASH_REWARD
        elif self._legs_on_the_ground(state["leg_contact"]):
            done = True
            reward = self._LANDING_REWARD
        # elif not done and self._steps >= self._MAX_STEPS:
        #     done = True
        return done, reward

    def _game_over(self, state, failed_landing):
        return (
            failed_landing or state["position"][0] >= 1.3 or state["position"][2] >= 1.3
        )

    def _legs_on_the_ground(self, leg_contact):
        if self.legs_contact >= 3:
            return True

        # opposite legs are touching the ground
        if leg_contact[0] + leg_contact[1] == 2 or leg_contact[2] + leg_contact[3] == 2:
            return True
