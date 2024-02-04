import numpy as np
import pandas as pd
from typing import Union
from tqdm import tqdm
from pdeproblems import PDEProblem

from .Discretizer import Discretizer

class LinearFEMDiscretizer(Discretizer):
    def __init__(self, pdeProblem: PDEProblem):
        super.__init__(pdeProblem)

    def discretize(self, n:int) -> Union[Union[np.ndarray, np.ndarray], Union[np.ndarray, np.ndarray]]:
        nodes, elements = LinearFEMDiscretizer.buildFiniteElementsMesh(n)
        A = LinearFEMDiscretizer.buildRightHandMatrix(self._pdeProblem, nodes)
        l = LinearFEMDiscretizer.buildLeftHandVector(self._pdeProblem, nodes)
        return (nodes, elements), (A, l)

    @staticmethod
    def buildFiniteElementsMesh(n : int) -> np.ndarray:
        nodes = np.linspace(0, 1, n)
        node_indices = np.arange(n)
        elements = np.column_stack((node_indices[:-1], node_indices[1:]))
        return (nodes, elements)

    @staticmethod
    def buildRightHandMatrix(problem: PDEProblem, nodes: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n-1)
        mu = problem.mu
        beta = problem.beta
        sigma = problem.sigma
        A = np.zeros((n, 3), dtype=np.float64)

        # First row
        if problem.leftBC().isFirstType:
            A[0, 1] = 1
        else:
            xr = nodes[0] + h/2
            A[0, 1] = mu(xr) / h - beta(xr) / 2 + sigma(xr) * h / 3 - problem.leftBC().qvalue
            A[0, 2] = -mu(xr) / h + beta(xr) / 2 + sigma(xr) * h / 6
        # From 2nd to n-1
        for i in tqdm(range(1, n-1), desc='Lefthand matrix creation:'):
            xl = nodes[i] - h / 2
            xr = nodes[i] + h / 2
            A[i, 0] = -mu(xl) / h - beta(xl) / 2 + sigma(xl) * h / 6
            A[i, 1] = (mu(xl) / h + beta(xl) / 2 + sigma(xl) * h / 3) + \
                      (mu(xr) / h - beta(xr) / 2 + sigma(xr) * h / 3)
            A[i, 2] = -mu(xr) / h + beta(xr) / 2 + sigma(xr) * h / 6
        # Last row
        if problem.rightBC().isFirstType:
            A[-1, 1] = 1
        else:
            xl = nodes[-1] - h / 2
            A[-1, 0] = -mu(xl) / h - beta(xl) / 2 + sigma(xl) * h / 6
            A[-1, 1] = mu(xl) / h + beta(xl) / 2 + sigma(xl) * h / 3 - problem.rightBC().qvalue

        return A

    @staticmethod
    def buildLeftHandVector(problem: PDEProblem, nodes: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        f = problem.leftBC
        l = np.zeros(n, dtype=np.float64)

        # First row
        if problem.leftBC().isFirstType:
            l[0] = problem.leftBC().gvalue
        else:
            xr = nodes[0] + h / 2
            l[0] = h * f(xr) / 2 - problem.leftBC().gvalue
        # From 2nd to n-1
        for i in tqdm(range(1, n - 1), desc='Righthand vector creation:'):
            xl = nodes[i] - h / 2
            xr = nodes[i] + h / 2
            l[i] = h * (f(xl) + f(xr)) / 2
        # Last row
        if problem.rightBC().isFirstType:
            l[-1] = problem.rightBC().gvalue
        else:
            xl = nodes[-1] - h / 2
            l[-1] = h * f(xl) / 2 - problem.rightBC().gvalue

