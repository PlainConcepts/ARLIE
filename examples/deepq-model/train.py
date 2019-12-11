from model import DQN
from rewards import CustomReward
from arlie.challenge import train

# CONFIG
wave = True
learn_timesteps = 1 * int(1e3)

if wave:
    import arlie

    env = arlie.make("LunarLander", render_mode=False, reward=CustomReward())
else:
    import gym

    env = gym.make("LunarLander-v2")

model = DQN(env)

name = "{}-dqnmodel".format("wave" if wave else "gym")

path = train(model, name, learn_timesteps)
print("Trained model at {}".format(path))
env.close()
