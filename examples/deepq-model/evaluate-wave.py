import time
from pathlib import Path

import arlie
from arlie.challenge import evaluate
from model import DQN
from rewards import CustomReward

# CONFIG
use_custom_reward = True
eval_timesteps = 0  # min evaluation
seed = int(time.time())
path = Path("./training/20191211_174703")

name = "wave-dqnmodel"

reward = CustomReward() if use_custom_reward else None
env = arlie.make("LunarLander", port=4000, seed=seed, render_mode=False, reward=reward)
model = DQN.load(str(path.joinpath(name)))

evaluate(env, model, path, eval_timesteps)
env.close()

env.close()
