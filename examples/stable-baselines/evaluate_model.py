import common.shutup as shutup

shutup.future_warnings()
shutup.tensorflow()

import sys  # noqa: E402
import time  # noqa: E402
import os.path  # noqa: E402
import numpy as np  # noqa: E402
from common.utils import make_env, make_multi_env  # noqa: E402
from stable_baselines import A2C  # noqa: E402
from arlie.envs.lunar_lander.score import LunarLanderScore  # noqa: E402

eval_timesteps = 1e5
multi = True
num_cpu = 12


def evaluate(env, model, num_steps=1000):
    """
    Evaluate a RL model
    :param model: (BaseRLModel object) the RL model
    :param num_steps: (int) number of timesteps to evaluate it
    :return: (float) Mean reward, (int) Number of episodes performed
    """
    scores = [LunarLanderScore() for _ in range(env.num_envs)]
    episode_scores = [[0.0] for _ in range(env.num_envs)]
    episode_rewards = [[0.0] for _ in range(env.num_envs)]
    obs = env.reset()
    steps = (int)(num_steps // env.num_envs)
    for i in range(steps):
        # _states are only useful when using LSTM policies
        actions, _states = model.predict(obs)
        # here, action, rewards and dones are arrays
        # because we are using vectorized env
        obs, rewards, dones, info = env.step(actions)

        # Stats
        for i in range(env.num_envs):
            scores[i].store_step(obs[i], actions[i], info[i])
            episode_scores[i][-1] = scores[i].get()
            episode_rewards[i][-1] += rewards[i]
            if dones[i]:
                episode_scores[i].append(0.0)
                episode_rewards[i].append(0.0)
                scores[i].reset()

    mean_scores = [0.0 for _ in range(env.num_envs)]
    mean_rewards = [0.0 for _ in range(env.num_envs)]
    n_episodes = 0
    for i in range(env.num_envs):
        mean_scores[i] = np.mean(episode_scores[i][:-1])
        mean_rewards[i] = np.mean(episode_rewards[i][:-1])
        n_episodes += len(episode_rewards[i]) - 1

    # Compute mean reward
    mean_score = round(np.mean(mean_scores), 1)
    mean_reward = round(np.mean(mean_rewards), 1)

    return mean_score, mean_reward, n_episodes


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("USAGE: {} PATH-TO-MODEL-FILE".format(sys.argv[0]))
        exit(-1)

    model_path = sys.argv[1]
    if not os.path.isfile(model_path):
        print("Path '{}' does not exist.".format(model_path))
        exit(-1)

    if num_cpu > 1:
        env = make_multi_env(num_cpu, "LunarLander", True, render_mode=False)
    else:
        env = make_env("LunarLander", True, render_mode=False, reset_mode="random")

    if len(sys.argv) == 1:
        print("No model provided")
        exit(-1)

    model_path = sys.argv[1]
    if not os.path.isfile(model_path):
        print("Path '{}' does not exist.".format(model_path))
        exit(-1)

    model = A2C.load(model_path)

    print("Evaluating...")
    _t = time.time()
    mean_score, mean_reward, n_episodes = evaluate(env, model, num_steps=eval_timesteps)
    t = time.time() - _t
    str_t = time.strftime("%H h, %M m, %S s", time.gmtime(t))
    print(
        "Trained mean score: {}, reward {}, in {} episodes during {}".format(
            mean_score, mean_reward, n_episodes, str_t
        )
    )

    env.close()
