import time
import arlie
from arlie.models.random import RAND
from arlie.challenge import evaluate

eval_timesteps = 0  # min evaluation
seed = int(time.time())

env = arlie.make(
    "LunarLander", port=4000, seed=seed, render_mode=False, reset_mode="random"
)
model = RAND(env)

evaluate(env, model)

env.close()
