import numpy as np, pandas as pd
from typing import Union
from tqdm import tqdm
from pdeproblems import PDEProblem


class Discretizer:
    def __init__(self, problem: PDEProblem):
        self._pdeProblem = problem
        pass

    def discretize(self, n: int):
        nodes = Discretizer.buildFiniteElementNodes(n)
        A = Discretizer.buildRightHandMatrix(self._pdeProblem, nodes)
        print(A is None)
        l = Discretizer.buildLeftHandVector(self._pdeProblem, nodes)
        return pd.DataFrame(nodes, columns=['x']), (A, l)

    @staticmethod
    def buildFiniteElementNodes(n: int) -> np.ndarray:
        return np.linspace(0, 1, n)

    @staticmethod
    def buildRightHandMatrix(problem: PDEProblem, nodes: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        mu = problem.mu
        beta = problem.beta
        sigma = problem.sigma
        A = np.zeros((n, 3), dtype=np.float64)
        # First row
        if problem.isLeftBCFirstType():
            A[0, 1] = 1
        else:
            xr = nodes[0] + h / 2
            A[0, 1] = mu(xr) / h - beta(xr) / 2 + sigma(xr) * h / 3 - problem.leftBC().qvalue()
            A[0, 2] = -mu(xr) / h + beta(xr) / 2 + sigma(xr) * h / 6
            # From 2nd to n-1
        for i in tqdm(range(1, n - 1), desc='Right hand matrix creation'):
            xl = nodes[i] - h / 2
            xr = nodes[i] + h / 2
            A[i, 0] = -mu(xl) / h - beta(xl) / 2 + sigma(xl) * h / 6
            A[i, 1] = (mu(xl) / h + beta(xl) / 2 + sigma(xl) * h / 3) + \
                      (mu(xr) / h - beta(xr) / 2 + sigma(xr) * h / 3)
            A[i, 2] = -mu(xr) / h + beta(xr) / 2 + sigma(xr) * h / 6
        # Last row
        if problem.isRightBCFirstType():
            A[-1, 1] = 1
        else:
            xl = nodes[-1] - h / 2
            A[-1, 0] = -mu(xl) / h - beta(xl) / 2 + sigma(xl) * h / 6
            A[-1, 1] = mu(xl) / h + beta(xl) / 2 + sigma(xl) * h / 3 - problem.rightBC().qvalue()
        return A

    @staticmethod
    def buildLeftHandVector(problem: PDEProblem, nodes: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        f = problem.f
        l = np.zeros(n, dtype=np.float64)
        # First row
        if problem.isLeftBCFirstType():
            l[0] = problem.leftBC().uvalue()
        else:
            xr = nodes[0] + h / 2
            l[0] = h * f(xr) / 2 - problem.leftBC().gvalue()
        # From 2nd to n-1
        for i in tqdm(range(1, n - 1), desc='Left hand vector creation'):
            xl = nodes[i] - h / 2
            xr = nodes[i] + h / 2
            l[i] = h * (f(xl) + f(xr)) / 2
        # Last row
        if problem.isRightBCFirstType():
            l[-1] = problem.rightBC().uvalue()
        else:
            xl = nodes[-1] - h / 2
            l[-1] = h * f(xl) / 2 - problem.rightBC().gvalue()

        return l
