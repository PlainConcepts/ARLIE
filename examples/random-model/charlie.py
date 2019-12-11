import arlie
from arlie.challenge import evaluate, train, upload
from arlie.models.random import RAND

# CONFIG
name = "DQNDemo"
learn_timesteps = int(1e3)
eval_timesteps = int(1e3)

# Training
env = arlie.make("LunarLander", render_mode=False, port=3000)
model = RAND(env)

path = train(model, name, learn_timesteps)
env.close()

# Evaluation
env = arlie.make("LunarLander", render_mode=False, port=4000)
model = RAND(env)
evaluate(env, model, path, eval_timesteps)
env.close()

# Upload
upload("jcarnero@plainconcepts.com", "****", path)
