import numpy as np
from model import DQN
from reward import CustomReward

wave = True
render_episodes = 7

if wave:
    import arlie

    env = arlie.make("LunarLander", reward=CustomReward())
else:
    import gym

    env = gym.make("LunarLander-v2")

model = DQN.load("trained-model")

episode = render_episodes
reward_sum = 0
obs = np.reshape(env.reset(), (1, model.obs_size))
while episode > 0:
    action, _states = model.predict(obs)
    obs, reward, done, _ = env.step(action)
    obs = np.reshape(obs, (1, model.obs_size))
    reward_sum += reward
    env.render()
    if done:
        print("Points: {}".format(reward_sum))
        episode -= 1
        reward_sum = 0
        obs = np.reshape(env.reset(), (1, model.obs_size))

env.close()
