import math

import numpy as np
from pdeproblems import PDEProblem
from .FEMNormCalculator import FEMNormCalculator


class FEMEnergyNormCalculator(FEMNormCalculator):

    def __init__(self, problem: PDEProblem):
        super().__init__(problem)

    def calculate(self, nodes, values) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        norm = np.zeros(n - 1, dtype=np.float64)

        for i in range(n - 1):
            el_cent = nodes[i:i + 2].mean()
            el_vals = values[i:i + 2]

            mu = self._pdeproblem.mu(el_cent)
            beta = self._pdeproblem.beta(el_cent)
            sigma = self._pdeproblem.sigma(el_cent)
            norm[i] += mu * 1 / h * np.dot(np.squeeze(np.dot(el_vals, np.squeeze(self._mu_matrix))), el_vals)
            norm[i] += beta * 1 / 2 * np.dot(np.squeeze(np.dot(el_vals, np.squeeze(self._beta_matrix))), el_vals)
            norm[i] += sigma * h / 6 * np.dot(np.squeeze(np.dot(el_vals, np.squeeze(self._sigma_matrix))), el_vals)
        return norm

