import numpy as np
from pdeproblems import PDEProblem
from .LinearErrorCalculator import LinearErrorCalculator

class ExplicitGradientJumpCalculator(LinearErrorCalculator):
    def init(self, pdeProblem: PDEProblem):
        super().init(pdeProblem)

    def calculate(self, nodes: np.ndarray, elements: np.ndarray, values: np.ndarray):
        n = len(nodes)
        h = 1.0 / (n - 1)
        err = np.zeros(n - 1, dtype=np.float64)
        denom = np.zeros(n - 1, dtype=np.float64)

        for i, elem in enumerate(elements):
            el_vals = values[elem]

            mu_l = self._pdeProblem.mu(nodes[elem[0]])
            mu_r = self._pdeProblem.mu(nodes[elem[1]])
            beta_l = self._pdeProblem.beta(nodes[elem[0]])
            beta_r = self._pdeProblem.beta(nodes[elem[1]])

            denom[i] = (mu_l + mu_r) / h
            err[i] = (beta_l * (el_vals[1] - el_vals[0]) + beta_r * (el_vals[2] - el_vals[1])) / (mu_l + mu_r)

        return np.power(err, 2) / denom
