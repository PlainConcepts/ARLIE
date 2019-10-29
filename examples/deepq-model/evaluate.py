import time
import numpy as np

import arlie
from arlie.envs.lunar_lander.score import LunarLanderScore

from model import DQN
from rewards import CustomReward

# CONFIG
use_custom_reward = True
eval_episodes = 10
seed = int(time.time())


def evaluate(env, model, score, num_episodes=10):
    """
    Evaluate a RL model
    :param model: (object) the RL model
    :param num_episodes: (int) number of episodes to evaluate
    :return: (float) Mean reward, (int) Number of episodes performed
    """
    episode_score = []
    episode_reward = []
    for e in range(num_episodes):
        obs = env.reset()
        episode_score.append(0.0)
        episode_reward.append(0.0)
        while True:
            action, _ = model.predict(np.reshape(obs, (1, model.obs_size)))
            obs, reward, done, info = env.step(action)
            score.store_step(obs, action, info)

            # Stats
            episode_reward[-1] += reward
            if done:
                episode_score[-1] = score.get()
                score.reset()
                break

    mean_score = np.mean(episode_score)
    mean_reward = np.mean(episode_reward)
    n_episodes = len(episode_reward)

    return mean_score, mean_reward, n_episodes


score = LunarLanderScore()
reward = CustomReward() if use_custom_reward else None
env = arlie.make("LunarLander", port=4000, seed=seed, render_mode=False, reward=reward)
model = DQN.load("trained-model")

print("Evaluating...")
mean_score, mean_reward, n_episodes = evaluate(
    env, model, score, num_episodes=eval_episodes
)
print(
    "Mean score: {}, reward: {}, in {} episodes".format(
        mean_score, mean_reward, n_episodes
    )
)

env.close()
