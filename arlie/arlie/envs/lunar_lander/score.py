import numpy as np
from numpy.linalg import norm
from arlie.envs.base_score import BaseScore
from arlie.envs.lunar_lander.env import LunarLander


class LunarLanderScore(BaseScore):
    def __init__(
        self,
        max_crash_score=-100,
        max_landing_score=100,
        main_engine_penalty=0.15,
        side_engine_penalty=0.015,
        max_safe_landing_velocity=0.1,
    ):
        self._CRASH_SCORE = max_crash_score
        self._LANDING_SCORE = max_landing_score
        self._MAIN_ENGINE_PENALTY = main_engine_penalty
        self._SIDE_ENGINE_PENALTY = side_engine_penalty
        self._MAX_SAFE_LANDING_VELOCITY = max_safe_landing_velocity

        self._score = 0.0
        self._done = False
        self.legs_contact = 0

    def store_step(self, observation, action, info):
        state = LunarLander.parse_observation(observation)
        self._last_contact = self.legs_contact
        self.legs_contact = np.sum(state["leg_contact"])

        self._score -= self._engine_penalty(action)

        if not self._done:
            self._done, score = self._get_done_score(state, info["failed_landing"])
            self._score += score

    def get(self):
        return self._score

    def reset(self):
        self._score = 0.0
        self._done = False
        self.legs_contact = 0

    def _engine_penalty(self, action):
        engine_penalty = 0.0
        if action > 0:
            if action < 7:  # side engine
                engine_penalty = self._SIDE_ENGINE_PENALTY
            else:  # main engine
                engine_penalty = self._MAIN_ENGINE_PENALTY
        return engine_penalty

    def _get_done_score(self, state, failed_landing):
        done = False
        score = 0.0

        done = False
        game_over = (
            failed_landing or state["position"][0] >= 1 or state["position"][2] >= 1
        )
        if game_over or self._hit_too_fast(state):
            done = True
            # decrease score with the landing pad distance
            XZ = norm([state["position"][0], state["position"][2]])
            score = self._CRASH_SCORE * XZ
        elif self.legs_contact >= 3:
            done = True
            # decrease score with the landing pad distance
            neg_XZ = norm(
                [1 - abs(state["position"][0]), 1 - abs(state["position"][2])]
            )
            score = self._LANDING_SCORE * neg_XZ

        return done, score

    def _hit_too_fast(self, state):
        if (self.legs_contact - self._last_contact) > 0:
            Vy = abs(state["velocity"][1])
            AVxz = norm([state["angular_velocity"][0], state["angular_velocity"][2]])
            if (
                Vy > self._MAX_SAFE_LANDING_VELOCITY
                or AVxz > self._MAX_SAFE_LANDING_VELOCITY
            ):
                return True
        return False
