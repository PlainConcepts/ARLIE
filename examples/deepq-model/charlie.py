import arlie
from arlie.challenge import evaluate, train, upload
from model import DQN
from rewards import CustomReward

# CONFIG
name = "DQNDemo"
learn_timesteps = int(1e3)
eval_timesteps = int(1e3)

# Training
env = arlie.make("LunarLander", render_mode=False, reward=CustomReward(), port=3000)
model = DQN(env)

path = train(model, name, learn_timesteps)
env.close()

# Evaluation
env = arlie.make("LunarLander", render_mode=False, port=4000)
model = DQN.load(str(path.joinpath(name)))
evaluate(env, model, path, eval_timesteps)
env.close()

# Upload
upload("jcarnero@plainconcepts.com", "****", path)
