import common.shutup as shutup

shutup.future_warnings()
shutup.tensorflow()

import arlie  # noqa: E402
from pathlib import Path  # noqa: E402
from common.utils import make_env, make_multi_env  # noqa: E402
from common.callbacks import save_callback  # noqa: E402
from stable_baselines.common.policies import MlpPolicy  # noqa: E402
from stable_baselines import A2C  # noqa: E402
from arlie.challenge import train, evaluate, upload  # noqa: E402

# CONFIG
label = "a2c_example"
order = int(1e3)
order_str = "K"
learn_timesteps = 24 * order
eval_timesteps = 1 * order
save_interval = 2 * order
num_cpu = 12
log_dir = "./logs"
models_dir = "./training/wave_a2c_example"

if __name__ == "__main__":
    train_path = Path(models_dir).joinpath("{}_{}".format("wave", label))
    # e.g.: ./logs
    log_path = Path(log_dir)

    # create folders
    log_path.mkdir(exist_ok=True)

    # create the wave or gym environment, with or without multiprocessing
    id = "LunarLander"
    if num_cpu > 1:
        env = make_multi_env(num_cpu, id, True, render_mode=False, reset_mode="random")
    else:
        env = make_env(id, True, render_mode=False, reset_mode="random")

    # create A2C with Mlp policy, and the callback to save snapshots
    model = A2C(MlpPolicy, env, ent_coef=0.1, verbose=0, tensorboard_log=log_dir)
    callback = save_callback(
        train_path,
        "snapshot-",
        save_interval,
        call_interval=model.n_steps * num_cpu,
        order=order,
        order_str=order_str,
    )

    name = "{}{}-final".format(int(learn_timesteps / order), order_str)

    # Training
    path = train(
        model,
        name,
        total_timesteps=learn_timesteps,
        train_path=train_path,
        callback=callback,
    )
    env.close()

    # Evaluation
    if num_cpu > 1:
        env = make_multi_env(
            num_cpu, id, True, render_mode=False, reset_mode="random", port=4000
        )
    else:
        env = make_env(id, True, render_mode=False, reset_mode="random", port=4000)
    model = A2C.load(str(path.joinpath(name)))
    evaluate(env, model, path, eval_timesteps)
    env.close()

    # Upload
    upload("jcarnero@plainconcepts.com", "****", path)
