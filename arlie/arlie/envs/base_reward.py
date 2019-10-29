from abc import ABC, abstractmethod


class BaseReward(ABC):
    """Abstract class to define reward functions"""

    @abstractmethod
    def evaluate(self, observation, action, failed_landing):
        pass

    @abstractmethod
    def reset(self):
        pass
