import time
from model import DQN
from reward import CustomReward

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

print("Training...")
_t = time.time()
model.learn(total_timesteps=learn_timesteps)
t = time.time() - _t
str_t = time.strftime("%H h, %M m, %S s", time.gmtime(t))
print("Trained in {} during {} timesteps".format(str_t, learn_timesteps))

model.save("trained-model")

env.close()
