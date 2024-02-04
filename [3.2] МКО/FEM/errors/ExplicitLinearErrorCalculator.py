import numpy as np
from pdeproblems import PDEProblem
from .LinearErrorCalculator import LinearErrorCalculator

class ExplicitNeumannErrorCalculator(LinearErrorCalculator):
    def init(self, pdeProblem: PDEProblem):
        super().init(pdeProblem)

    def calculate(self, nodes: np.ndarray, elements: np.ndarray, values: np.ndarray):
        n = len(nodes)
        h = 1.0 / (n - 1)
        err = np.zeros(n - 1, dtype=np.float64)
        denom = np.zeros(n - 1, dtype=np.float64)

        for i, elem in enumerate(elements):
            el_vals = values[elem]

            mu = self._pdeProblem.mu(nodes[i])
            beta = self._pdeProblem.beta(nodes[i])
            sigma = self._pdeProblem.sigma(nodes[i])
            f = self._pdeProblem.f(nodes[i])

            denom[i] = mu / h
            err[i] = beta * (el_vals[1] - el_vals[0])
            err[i] += sigma * (el_vals[1] - el_vals[0]) / 2
            err[i] += f * h / 2

        return np.power(err, 2) / denom
