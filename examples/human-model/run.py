import arlie
from arlie.models.human import HUMAN
from arlie.envs.lunar_lander.score import LunarLanderScore

env = arlie.make("LunarLander", port=5000, reset_mode="random")
model = HUMAN(env, quit_key="q")
score = LunarLanderScore()

rewards = []
reward_sum = 0
obs = env.reset()
while True:
    action, _states = model.predict(obs)
    if action == -1:
        break
    obs, reward, done, info = env.step(action)
    reward_sum += reward
    score.store_step(obs, action, info)

    env.render()

    if done:
        print("Score: {}, Reward: {}".format(score.get(), reward_sum))
        score.reset()
        reward_sum = 0
        obs = env.reset()

env.close()
