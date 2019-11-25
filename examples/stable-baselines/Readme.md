# Integration with stable-baselines algorithms example

This example shows how to use stable-baselines, a compilation of state of the art RL algorithms made for OpenAI Gym, in ARLIE.

## Additional requirements

See detailed information on installing stable-baselines [here](https://github.com/hill-a/stable-baselines#installation).

Stabe-Baselines supports Tensorflow versions from 1.8.0 to 1.14.0. Support for Tensorflow 2 API is planned.

```console
pip install tensorflow==1.14
```

Or with GPU support:

```console
pip install tensorflow-gpu==1.14
```

Obviously, [Stable baselines](https://github.com/hill-a/stable-baselines) is required to run this example

```console
pip install stable-baselines
```

You can include an optional dependency on MPI if you use it, enabling algorithms DDPG, GAIL, PPO1 and TRPO.

```console
pip install stable-baselines[mpi]
```

Optionally, the example allows to run the same model using the LunarLander-v2 environment from gym. Additional requirements need to be installed to run the gym version:

In windows, you will first need to download and install [Build Tools for Visual Studio](https://www.scivision.dev/python-windows-visual-c-14-required/). Then restart and:

```console
conda install swig
pip install gym[box2d]
```
