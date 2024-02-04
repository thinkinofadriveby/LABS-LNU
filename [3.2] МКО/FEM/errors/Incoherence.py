import numpy as np
from pdeproblems import PDEProblem

class Incoherence:
    def __init__(self, problem: PDEProblem):
        self._pdeproblem = problem
        self._beta_matrix = np.asarray([-1, 1])
        self._sigma_matrix = np.asarray([1, 1])

    def calculate(self, nodes, values) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n-1)
        incoherence = np.zeros(n-1, dtype=np.float64)

        for i in range (n-1):
            el_cent = nodes[i:i+2].mean()
            el_vals = values[i:i+2]

            beta = self._pdeproblem.beta(el_cent)
            sigma = self._pdeproblem.sigma(el_cent)
            f = self._pdeproblem.f(el_cent)

            incoherence[i] += beta * 1 / h * np.dot(self._beta_matrix, el_vals)
            incoherence[i] += sigma * 1 / 2 * np.dot(self._sigma_matrix, el_vals)
            incoherence[i] += f - incoherence[i]

            incoherence[i] = np.power(incoherence[i], 2)
            incoherence[i] = incoherence[i] * (h * h)

        return incoherence