import common.shutup as shutup

shutup.future_warnings()
shutup.tensorflow()

import os  # noqa: E402
import time  # noqa: E402
from pathlib import Path  # noqa: E402
from common.utils import make_env, make_multi_env  # noqa: E402
from common.callbacks import save_callback  # noqa: E402
from stable_baselines.common.policies import MlpPolicy  # noqa: E402
from stable_baselines import A2C  # noqa: E402


# CONFIG
wave = True
label = "a2c_example"
order = int(1e3)
order_str = "K"
learn_timesteps = 24 * order
save_interval = 2 * order
num_cpu = 12
models_dir = "./models"
log_dir = "./logs"

if __name__ == "__main__":
    # e.g.: ./models/wave_a2c_example/
    model_path = Path(models_dir).joinpath(
        "{}_{}".format("wave" if wave else "gym", label)
    )
    # e.g.: ./logs
    log_path = Path(log_dir)

    # create folders
    model_path.mkdir(parents=True, exist_ok=True)
    log_path.mkdir(exist_ok=True)

    # create the wave or gym environment, with or without multiprocessing
    id = "LunarLander" if wave else "LunarLander-v2"
    if num_cpu > 1:
        env = make_multi_env(num_cpu, id, wave, render_mode=False, reset_mode="random")
    else:
        env = make_env(id, wave, render_mode=False, reset_mode="random")

    # create A2C with Mlp policy, and the callback to save snapshots
    model = A2C(MlpPolicy, env, ent_coef=0.1, verbose=0, tensorboard_log=log_dir)
    callback = save_callback(
        model_path,
        "snapshot-",
        save_interval,
        call_interval=model.n_steps * num_cpu,
        order=order,
        order_str=order_str,
    )

    # save final model
    print("Training...")
    _t = time.time()
    model.learn(total_timesteps=learn_timesteps, callback=callback)
    t = time.time() - _t
    str_t = time.strftime("%H h, %M m, %S s", time.gmtime(t))
    print("Trained in {} during {} timesteps".format(str_t, learn_timesteps))

    final_model = model_path.joinpath(
        "{}{}-final".format(int(learn_timesteps / order), order_str)
    )
    model.save(str(final_model))

    env.close()
