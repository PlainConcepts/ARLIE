""" Evaluates Wave Lunar Lander model """

import common.shutup as shutup

shutup.future_warnings()
shutup.tensorflow()

import sys  # noqa: E402
import os.path  # noqa: E402
from common.utils import make_env, make_multi_env  # noqa: E402
from stable_baselines import A2C  # noqa: E402
from arlie.challenge import evaluate  # noqa: E402

wave = True
eval_timesteps = 1e5
num_cpu = 12

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("USAGE: {} PATH-TO-MODEL-FILE".format(sys.argv[0]))
        exit(-1)

    model_path = sys.argv[1]
    if not os.path.isfile(model_path):
        print("Path '{}' does not exist.".format(model_path))
        exit(-1)

    id = "LunarLander" if wave else "LunarLander-v2"
    if num_cpu > 1:
        env = make_multi_env(num_cpu, id, wave, render_mode=False, reset_mode="random")
    else:
        env = make_env(id, wave, render_mode=False, reset_mode="random")

    if len(sys.argv) == 1:
        print("No model provided")
        exit(-1)

    model_path = sys.argv[1]
    if not os.path.isfile(model_path):
        print("Path '{}' does not exist.".format(model_path))
        exit(-1)

    model = A2C.load(model_path)

    evaluate(env, model, train_path=model_path, num_steps=eval_timesteps)

    env.close()
