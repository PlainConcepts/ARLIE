import os
import time
import json
import shutil
import requests
import numpy as np
from pathlib import Path
from datetime import datetime


def train(model, name, total_timesteps, *args, train_path=None, **kwargs):
    now = datetime.now()  # current date and time

    if train_path is None:
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


def render(env, model, num_episodes):
    score = env.get_score_function()()
    episode = num_episodes if num_episodes > 0 else 1
    reward_sum = 0
    obs = env.reset()
    while episode > 0:
        action, _states = model.predict(obs)
        obs, reward, done, info = env.step(action)
        reward_sum += reward
        score.store_step(obs, action, info)

        env.render()
        if done:
            print("Score: {}, Reward: {}".format(score.get(), reward_sum))
            score.reset()
            if num_episodes > 0:
                episode -= 1
            reward_sum = 0
            obs = env.reset()


def evaluate(env, model, train_path=None, num_steps=0):
    print("Evaluating...")
    _t = time.time()

    # Check if env is vectorial (multicpu)
    vectorial = False
    num_envs = 1
    if hasattr(env, "num_envs"):
        num_envs = env.num_envs
        vectorial = True

    min_num_steps = env.get_min_evaluation_steps()
    min_num_steps_per_env = env.get_min_evaluation_steps_per_env()
    score = env.get_score_function()

    if num_steps < min_num_steps:
        num_steps = min_num_steps
    if (num_steps // num_envs) < min_num_steps_per_env:
        num_steps = min_num_steps_per_env * num_envs

    scores = [score() for _ in range(num_envs)]
    episode_scores = [[0.0] for _ in range(num_envs)]
    episode_rewards = [[0.0] for _ in range(num_envs)]
    obs = env.reset()
    steps = (int)(num_steps // num_envs)
    for i in range(steps):
        # _states are only useful when using LSTM policies
        actions, _states = model.predict(obs)
        # here, action, rewards and dones are arrays
        # because we are using vectorized env
        obs, rewards, dones, info = env.step(actions)

        # Stats
        if vectorial:
            for i in range(num_envs):
                scores[i].store_step(obs[i], actions[i], info[i])
                episode_scores[i][-1] = scores[i].get()
                episode_rewards[i][-1] += rewards[i]
                if dones[i]:
                    episode_scores[i].append(0.0)
                    episode_rewards[i].append(0.0)
                    scores[i].reset()
        else:
            scores[0].store_step(obs, actions, info)
            episode_scores[0][-1] = scores[0].get()
            episode_rewards[0][-1] += rewards
            if dones:
                episode_scores[0].append(0.0)
                episode_rewards[0].append(0.0)
                scores[0].reset()
                obs = env.reset()

    mean_scores = [0.0 for _ in range(num_envs)]
    mean_rewards = [0.0 for _ in range(num_envs)]
    n_episodes = 0
    for i in range(num_envs):
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

    if train_path is not None:
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


def upload(username, passwd, train_path):
    base_url = "5cf7386d-18e1-4cb8-ba62-6bc1bbe23241.mock.pstmn.io"
    # base_url = "charlie.plainconcepts.com"

    # Read data
    train_data = {}
    with open(train_path.joinpath("train-metadata.json")) as fp:
        train_data = json.load(fp)
    eval_data = {}
    with open(train_path.joinpath("eval-metadata.json")) as fp:
        eval_data = json.load(fp)

    # Get token
    token_url = "/api/oauth/token"

    payload = {
        "grant_type": "password",
        "client_id": 2,
        "client_secret": "****",
        "username": username,
        "password": passwd,
        "scope": "*",
    }

    resp = requests.post("https://" + base_url + token_url, data=payload)
    if resp.status_code != 200:
        # This means something went wrong.
        raise Exception

    data = resp.json()

    access_token = data["access_token"]
    # refresh_token = data["refresh_token"]

    print("Access token: {}".format(access_token))

    # Upload experiment
    experiment_url = "/api/experiment"

    headers = {"Authorization": "Bearer " + access_token}

    payload = {
        "name": "PlainConcepts",
        "agent": train_data["name"],
        "runs": train_data["training"]["total_timesteps"],
        "reward_avg": eval_data["mean_score"],
        "steps_avg": eval_data["mean_steps"],
        "reward_best": eval_data["best_score"],
        "points": eval_data["mean_score"],
        "challenge_id": 1,
    }

    shutil.make_archive("experiment", "zip", os.getcwd())
    files = {"experiments": open("experiment.zip", "rb")}

    resp = requests.post(
        "https://" + base_url + experiment_url,
        headers=headers,
        data=payload,
        files=files,
    )
    if resp.status_code != 200:
        # This means something went wrong.
        # raise Exception
        print("status: "+str(resp.status_code))

    data = resp.json()
    print(data)
