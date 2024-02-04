import numpy as np
from abc import ABCMeta, abstractmethod


class LinearSolver(metaclass=ABCMeta):

    def __init__(self, A: np.ndarray, l: np.ndarray):
        self._A = A
        self._l = l

    @abstractmethod
    def solve(self, A, l) -> np.ndarray:
        pass
