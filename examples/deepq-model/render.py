from model import DQN
from rewards import CustomReward
from pathlib import Path
from arlie.challenge import render

wave = True
render_episodes = 5
path = Path("./training/20191211_174703")

if wave:
    import arlie

    env = arlie.make("LunarLander", reward=CustomReward())
else:
    import gym

    env = gym.make("LunarLander-v2")

name = "{}-dqnmodel".format("wave" if wave else "gym")

model = DQN.load(str(path.joinpath(name)))

render(env, model, render_episodes)

env.close()
