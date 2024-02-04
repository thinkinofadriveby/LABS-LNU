import numpy as np
from abc import ABCMeta, abstractmethod
from pdeproblems import *
from pdeproblems import PDEProblem


class FEMNormCalculator(metaclass=ABCMeta):

    def __init__(self, problem: PDEProblem):
        self._pdeproblem = problem
        self._mu_matrix = np.asarray([[1, -1], [-1, 1]])
        self._beta_matrix = np.asarray([[-1, -1], [1, 1]])
        self._sigma_matrix = np.asarray([[2, 1], [1, 2]])

    @abstractmethod
    def calculate(self, nodes, values) -> np.ndarray:
        pass
