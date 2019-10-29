import time
from stable_baselines.common import set_global_seeds
from stable_baselines.common.vec_env import DummyVecEnv
from stable_baselines.common.vec_env import SubprocVecEnv


def make_env(env_id, wave, *args, seed=None, keyboard=False, **kwargs):
    """
    Utility function for creating environments.

    :param env_id: (str) the environment ID
    :param wave: (int) wether the environment is type wave
    :param seed: (int) the inital seed for RNG
    :param keyboard: (bool) if the environment should enable keyboard management
    :return: (DummyVecEnv) Dummy vector with environment embedded
    """
    # The algorithms require a vectorized environment to run
    envf = [_make_env_func(env_id, wave, seed, *args, **kwargs)]
    vectorized_env = DummyVecEnv(envf)

    if keyboard:
        # add keyboard_map as an attribute of the env wrapper
        vectorized_env.keyboard_map = vectorized_env.envs[0].keyboard_map

    return vectorized_env


def make_multi_env(num_cpu, env_id, wave, *args, seed=None, **kwargs):
    """
    Utility function for multiprocessed env.

    :param env_id: (str) the environment ID
    :param wave: (int) wether the environment is type wave
    :param seed: (int) the inital seed for RNG
    :return: (SubprocVecEnv) Vector with independent environments
    """

    # Vector of independent environments
    envf_list = [
        _make_env_func(env_id, wave, seed, *args, rank=i, port=3000 + i, **kwargs)
        for i in range(num_cpu)  # TODO port is specific for LunarLander
    ]
    return SubprocVecEnv(envf_list)


def _make_env_func(env_id, wave, seed, *args, rank=0, **kwargs):
    if seed is None:
        _seed = int(time.time()) + rank
    else:
        _seed = seed + rank

    def _init():
        if wave:
            import arlie

            env = arlie.make(env_id, *args, seed=_seed, **kwargs)
        else:
            import gym

            env = gym.make(env_id)
        env.seed(_seed)
        return env

    set_global_seeds(seed)
    return _init
