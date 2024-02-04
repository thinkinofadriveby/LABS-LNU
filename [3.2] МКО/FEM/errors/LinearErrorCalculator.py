from abc import abstractmethod

import numpy as np

from pdeproblems.PDEProblem import PDEProblem


class LinearErrorCalculator(PDEProblem):
    def __init__(self, pdeProblem: PDEProblem):
        self._pdeProblem = pdeProblem
        # self._beta_matrix = np.array([-1, 1]).reshape(1, 2) / (pdeProblem.beta(0) + pdeProblem.beta(1))
        # self._sigma_matrix = np.array([1, -1]).reshape(1, 2) / (pdeProblem.sigma(0) + pdeProblem.sigma(1))

        self._beta_matrix = np.array([-1, 1])
        self._sigma_matrix = np.array([1, 1])

    @abstractmethod
    def calculate(self, nodes: np.ndarray, elements: np.ndarray, values: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        err = np.zeros(n - 1, dtype=np.float64)
        denom = np.zeros(n - 1, dtype=np.float64)

        for i, elem in enumerate(elements):
            el_vals = values[elem]

            mu = self._pdeProblem.mu(nodes[elem].mean())
            denom[i] = (8 * mu) / (15 * h) * (10 + h**2 * self._pdeProblem.sigma(nodes[elem].mean()))

            err[i] += np.dot(self._beta_matrix, el_vals)
            err[i] += 1 / 2 * np.dot(self._sigma_matrix, el_vals)
            err[i] += self._pdeProblem.f(nodes[elem].mean())

        return np.power(err, 2) / denom