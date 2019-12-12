import arlie
from arlie.models.random import RAND
from arlie.challenge import render

render_episodes = 7

env = arlie.make("LunarLander", port=5000, reset_mode="random")
model = RAND(env)

render(env, model, render_episodes)

env.close()
