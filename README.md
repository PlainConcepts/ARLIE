# ARLIE

Agent of Reinforcement Learning for Intelligent Environments

ARLIE makes easier to work on complex reinforcement learning problems by providing different 3D environments and a common API to work with them.

For each environment, users can develop and/or test multiple training algorithms, policies, or rewards to get to an optimal solution in as less training steps as possible. To measure the optimality of a solution, each environment provides a _score_ function.

## [CHARLIE](http://charlie.plainconcepts.com/)

[CHallenging with ARLIE](http://charlie.plainconcepts.com/) is a platform to encourage developers to come up with novel RL techniques and solve the different ARLIE environments.

Best scores are published in the [Learderboard](http://charlie.plainconcepts.com/leaderboard). Do you have what it takes to beat the leader? Submit your solution and compete with the others for the first place!

## Playing with ARLIE

### Installation

ARLIE is _python_ based and it is published in [Pypi](https://pypi.org/project/arlie/).

`pip install arlie`

There is also a conda file `min-environment.yml` that creates a conda environment named _arlie_ by default:

`conda create -f min-environment.yml`

As the 3D environments can be much heavy than regular python packages, they are downloaded separately the first time the package is executed.

Check this PlainTV on how to start with ARLIE (Spanish):

<iframe width="560" height="315" src="https://www.youtube.com/embed/EH1dywTdybQ" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

### Environments

Environments simulate 3D scenarios using [Wave Engine](https://waveengine.net/), in which a certain goal must be achieve. That goal is modeled by a _score_ function which evaluate according to how good the goal was achieved.

ARLIE users are meant to provide, for a certain environment, a training algorithm, an agent (a neural network to be trained, **the policy**), and optionally a reward function. After the training process, the goodness of the trained agent (the policy optimality) can be evaluated using the score function.

The pair of the training algorithm and the agent is often called __a model__. ARLIE provides two simple models by default: _Human_, in which agent actions are taken by a person (using the keyboard), and _Random_, in which actions are taken randomly. As you probably notice, training these models is futile.

When no reward function is provided, a default one is used. This implementation can be seen as an example on how rewards functions can be modeled for a particular environment.

To open ARLIE to well stablished RL implementations, their API is 100% compatible with [OpenAI Gym](https://gym.openai.com/), which means that all open source code available for that platform can work on ARLIE as well.

---
__Available environments:__

* [LunarLander](https://github.com/PlainConcepts/ARLIE/blob/master/wave/lunar_lander/README.md)

---

### Examples

Four examples illustrates how to work with ARLIE.

* [Random Model](https://github.com/PlainConcepts/ARLIE/tree/master/examples/random-model/Readme.md)
* [Human Model](https://github.com/PlainConcepts/ARLIE/tree/master/examples/human-model/Readme.md)
* [DeepQ Model](https://github.com/PlainConcepts/ARLIE/tree/master/examples/deepq-model/Readme.md)
* [Stable Baselines](https://github.com/PlainConcepts/ARLIE/blob/master/examples/stable-baselines/Readme.md)

In this PlainTV we present the Random, Human and DeepQ examples (Spanish):

<iframe width="560" height="315" src="https://www.youtube.com/embed/w4h0nTHOyMg" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

In this PlainTV we present how ARLIE can be integrated with Stable Baselines (Spanish):

<iframe width="560" height="315" src="https://www.youtube.com/embed/y-wJFa_3WME" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
