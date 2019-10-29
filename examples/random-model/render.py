import arlie
from arlie.models.random import RAND
from arlie.envs.lunar_lander.score import LunarLanderScore

render_episodes = 7

env = arlie.make("LunarLander", port=5000, reset_mode="random")
model = RAND(env)
score = LunarLanderScore()

episode = render_episodes
reward_sum = 0
obs = env.reset()
while episode > 0:
    action, _states = model.predict(obs)
    obs, reward, done, info = env.step(action)
    reward_sum += reward
    score.store_step(obs, action, info)

    env.render()
    if done:
        print("Score: {}, Reward: {}".format(score.get(), reward_sum))
        score.reset()
        episode -= 1
        reward_sum = 0
        obs = env.reset()

env.close()
