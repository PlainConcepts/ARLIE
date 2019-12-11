import arlie
from model import DQN

from pathlib import Path
from rewards import CustomReward
from arlie.challenge import train, evaluate, upload

# CONFIG
name = "DQNDemo"
learn_timesteps = int(1e3)
eval_timesteps = int(1e3)

# # Training
# env = arlie.make("LunarLander", render_mode=False, reward=CustomReward())
# model = DQN(env)

# path = train(model, name, learn_timesteps)
# env.close()

# Evaluation
path = Path("./training/20191210_174718")
env = arlie.make("LunarLander", render_mode=False)
model = DQN.load(str(path.joinpath()))
evaluate(env, model, path, eval_timesteps)

# # Upload
# upload("jcarnero", "cockburn", path)
