import arlie
from arlie.models.human import HUMAN
from arlie.challenge import render

env = arlie.make("LunarLander", port=5000, reset_mode="random")
model = HUMAN(env, quit_key="q")

render(env, model, 0)  # use 0 to render forever

env.close()
