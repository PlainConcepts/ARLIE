# DQN model with Keras example

## Additional requirements

This model uses tensorflow:

```console
pip install tensorflow==2.0
```

Or with GPU support:

```console
pip install tensorflow-gpu==2.0
```

Optionally, the example allows to run the same model using the LunarLander-v2 environment from gym. Additional requirements need to be installed to run the gym version:

In windows, you will first need to download and install [Build Tools for Visual Studio](https://www.scivision.dev/python-windows-visual-c-14-required/). Then restart and:

```console
conda install swig
pip install gym[box2d]
```
