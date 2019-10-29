import numpy as np
import tensorflow as tf


def save_callback(
    path, prefix, save_interval, call_interval=1, order_str="M", order=int(1e6)
):
    """
    :param step_interval: Number of steps between saves
    :param path: Path where the model will be saved
    :param prefix: Prefix filename of the saved models
    :return: callback to be called after n steps
    """

    file_path = path
    file_prefix = prefix
    N = save_interval
    C = call_interval
    n_calls = 0
    next_index = 1

    def callback(locals_, globals_):
        """
        Callback called after n steps to save current training model
        (see ACER or PPO2)
        :param _locals: (dict)
        :param _globals: (dict)
        """
        nonlocal file_path, file_prefix, N, n_calls, next_index
        n_calls += 1
        steps = n_calls * C
        if steps // N >= next_index:
            print(
                "Saving model {}{} at step {} ...".format(
                    int(steps / order), order_str, steps
                )
            )
            locals_["self"].save(
                "{}{}{}{}".format(file_path, file_prefix, int(steps / order), order_str)
            )
            next_index = steps // N + 1
        return True

    return callback


def custom_log_callback():
    """
    :return: callback to be called at each at step (for DQN an others)
    or after n steps (see ACER or PPO2)
    """

    is_tb_set = False

    def callback(locals_, globals_):
        """
        Callback called at each step (for DQN an others) or after n steps
        (see ACER or PPO2)
        :param _locals: (dict)
        :param _globals: (dict)
        """
        nonlocal is_tb_set
        self_ = locals_["self"]
        # Log additional tensor
        if not is_tb_set:
            with self_.graph.as_default():
                tf.summary.scalar(
                    "entropy_jc", tf.reduce_mean(self_.entropy)
                )  # TODO select algorithm parameters
                self_.summary = tf.summary.merge_all()
            is_tb_set = True
        # Log scalar value (here a random variable)
        value = np.random.random()
        summary = tf.Summary(
            value=[tf.Summary.Value(tag="random_value", simple_value=value)]
        )
        locals_["writer"].add_summary(summary, self_.num_timesteps)
        return True

    return callback
