from abc import ABC, abstractmethod


class BaseScore(ABC):
    """Abstract class to define score functions"""

    @abstractmethod
    def store_step(self, observation, action, info):
        pass

    @abstractmethod
    def get(self):
        pass

    @abstractmethod
    def reset(self):
        pass
