import numpy as np
from pdeproblems.ParabolicPDEProblem import PDEProblem
from .NormCalculator import NormCalculator


class SobolevNormCalculator(NormCalculator):

    def __init__(self, problem: PDEProblem):
        super().__init__(problem)

    def calculate(self, nodes, values, t) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        norm = np.zeros(n-1, dtype=np.float64)

        for i in range(n-1):
            el_vals = values[i:i+2]

            norm[i] += 1/h * np.dot(np.dot(el_vals, self._mu_matrix), el_vals)
            norm[i] += h/6 * np.dot(np.dot(el_vals, self._sigma_matrix), el_vals)

        return norm
