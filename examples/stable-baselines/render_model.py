import common.shutup as shutup

shutup.future_warnings()
shutup.tensorflow()

import sys  # noqa: E402
import os.path  # noqa: E402
from common.utils import make_env  # noqa: E402
from stable_baselines import A2C  # noqa: E402
from arlie.challenge import render  # noqa: E402


wave = True
render_episodes = 20

if len(sys.argv) < 2:
    print("USAGE: {} PATH-TO-MODEL-FILE".format(sys.argv[0]))
    exit(-1)

model_path = sys.argv[1]
if not os.path.isfile(model_path):
    print("Path '{}' does not exist.".format(model_path))
    exit(-1)

id = "LunarLander" if wave else "LunarLander-v2"
env = make_env(id, wave, port=4000, reset_mode="random")

model = A2C.load(model_path)

render(env, model, render_episodes)

env.close()
