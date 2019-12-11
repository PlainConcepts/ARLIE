import time
import json
import numpy as np
from pathlib import Path
from datetime import datetime


def train(model, name, total_timesteps, *args, **kwargs):
    now = datetime.now()  # current date and time

    train_path = Path("./training").joinpath(now.strftime("%Y%m%d_%H%M%S"))

    # create folders
    train_path.mkdir(parents=True, exist_ok=True)

    print("Training...")
    _t = time.time()
    model.learn(total_timesteps=total_timesteps, *args, **kwargs)
    t = time.time() - _t
    str_t = time.strftime("%d d, %H h, %M m, %S s", time.gmtime(t))
    print("Trained in {} during {} timesteps".format(str_t, total_timesteps))

    # save model
    model.save(str(train_path.joinpath(name)))

    # save metadata
    meta = {
        "name": name,
        "training": {"total_timesteps": total_timesteps, "Time": str_t},
    }
    with open(train_path.joinpath("train-metadata.json"), "w") as fp:
        json.dump(meta, fp)

    return train_path


def evaluate(env, model, train_path, num_steps=0):
    print("Evaluating...")
    _t = time.time()

    min_num_steps = env.get_min_evaluation_steps()
    min_num_steps_per_env = env.get_min_evaluation_steps_per_env()
    score = env.get_score_function()

    if num_steps < min_num_steps:
        num_steps = min_num_steps
    if (num_steps // env.num_envs) < min_num_steps_per_env:
        num_steps = min_num_steps_per_env * env.num_envs

    scores = [score() for _ in range(env.num_envs)]
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

    # Step metrics
    mean_steps = 0
    best_score = 0
    worst_score = 0
    mean_score = 0
    best_reward = 0
    worst_reward = 0
    mean_reward = 0
    if n_episodes > 0:
        mean_steps = num_steps / n_episodes

        # Score metrics
        best_score = np.max(episode_scores[i][:-1])
        worst_score = np.min(episode_scores[i][:-1])
        mean_score = round(np.mean(mean_scores), 2)

        # Reward metrics
        best_reward = np.max(episode_rewards[i][:-1])
        worst_reward = np.min(episode_rewards[i][:-1])
        mean_reward = round(np.mean(mean_rewards), 2)

    t = time.time() - _t
    str_t = time.strftime("%d d, %H h, %M m, %S s", time.gmtime(t))

    # save metadata
    metadata = {
        "steps": num_steps,
        "episodes": n_episodes,
        "mean_steps": mean_steps,
        "mean_score": mean_score,
        "best_score": best_score,
        "worst_score": worst_score,
        "mean_reward": mean_reward,
        "best_reward": best_reward,
        "worst_reward": worst_reward,
        "time": str_t,
    }
    with open(train_path.joinpath("eval-metadata.json"), "w") as fp:
        json.dump(metadata, fp)

    print(
        """
        Steps: {}
        Episodes: {}
        Steps avg: {}
        Score:
            avg: {}
            best: {}
            worst: {}
        Reward:
            avg: {}
            best: {}
            worst:{}
        Evaluated in {}""".format(
            num_steps,
            n_episodes,
            mean_steps,
            mean_score,
            best_score,
            worst_score,
            mean_reward,
            best_reward,
            worst_reward,
            str_t,
        )
    )

    return metadata


def upload(user, passwd, train_path):
    pass
