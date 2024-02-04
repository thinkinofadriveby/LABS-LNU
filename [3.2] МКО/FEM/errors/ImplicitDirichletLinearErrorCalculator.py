import numpy as np
from pdeproblems import PDEProblem
from .LinearErrorCalculator import LinearErrorCalculator


class ImplicitDirichletErrorCalculator(LinearErrorCalculator):

    def __init__(self, pdeProblem: PDEProblem):
        super().__init__(pdeProblem)

    def calculate(self, nodes, elements, values) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        err = np.zeros(n - 1, dtype=np.float64)
        denom = np.zeros(n - 1, dtype=np.float64)

        for i, elem in enumerate(elements):
            el_cent = nodes[elem].mean()
            el_vals = values[elem]

            mu = self._pdeProblem.mu(el_cent)
            beta = self._pdeProblem.beta(el_cent)
            sigma = self._pdeProblem.sigma(el_cent)
            f = self._pdeProblem.f(el_cent)

            denom[i] = (8 * mu) / (15 * h) * (10 + h**2 * sigma / mu)
            err[i] += beta * 1/h * np.dot(self._beta_matrix, el_vals)
            err[i] += sigma * 1/2 * np.dot(self._sigma_matrix, el_vals)
            err[i] = (f - err[i]) * h * 2/3

        return np.power(err, 2) / denom