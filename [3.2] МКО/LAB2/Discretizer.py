import numpy as np, pandas as pd
from pdeproblems import *
from pdeproblems.ParabolicPDEProblem import PDEProblem


class Discretizer:
    def __init__(self, problem: PDEProblem):
        self._pdeProblem = problem
        pass

    def discretize(self, n: int):
        nodes = Discretizer.create_nodes(n)
        A = Discretizer.create_and_fill_matrix(self._pdeProblem, nodes)
        l = Discretizer.create_and_fill_vector(self._pdeProblem, nodes)
        return pd.DataFrame(nodes, columns=['x']), (A, l)

    @staticmethod
    def create_nodes(n: int) -> np.ndarray:
        return np.linspace(0, 1, n)


    @staticmethod
    def create_and_fill_first_row_matrix(matrix_A, h, problem, nodes):
        if problem.is_left_bound_cond_dirichlet():
            matrix_A[0, 1] = 1
        else:
            x_right = nodes[0] + h / 2
            matrix_A[0, 1] = problem.mu(x_right) / h - problem.beta(x_right) / 2 + problem.sigma(x_right) * h / 3 - problem.left_bound_cond().qvalue()
            matrix_A[0, 2] = -problem.mu(x_right) / h + problem.beta(x_right) / 2 + problem.sigma(x_right) * h / 6

    @staticmethod
    def create_and_fill_last_row_matrix(matrix_A, h, problem, nodes):
        if problem.is_right_bound_cond_dirichlet():
            matrix_A[-1, 1] = 1
        else:
            x_left = nodes[-1] - h / 2
            matrix_A[-1, 0] = -problem.mu(x_left) / h - problem.beta(x_left) / 2 + problem.sigma(x_left) * h / 6
            matrix_A[-1, 1] = problem.mu(x_left) / h + problem.beta(x_left) / 2 + problem.sigma(x_left) * h / 3 - problem.right_bound_cond().qvalue()

    @staticmethod
    def create_and_fill_first_el_vector(vector_L, h, problem, nodes):
        if problem.is_left_bound_cond_dirichlet():
            vector_L[0] = problem.left_bound_cond().uvalue()
        else:
            x_right = nodes[0] + h / 2
            vector_L[0] = h * problem.f(x_right) / 2 - problem.left_bound_cond().gvalue()

    @staticmethod
    def create_and_fill_last_el_vector(vector_L, h, problem, nodes):
        if problem.is_right_bound_cond_dirichlet():
            vector_L[-1] = problem.right_bound_cond().uvalue()
        else:
            x_left = nodes[-1] - h / 2
            vector_L[-1] = h * problem.f(x_left) / 2 - problem.right_bound_cond().gvalue()

    @staticmethod
    def create_and_fill_matrix(problem: PDEProblem, nodes: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        mu = problem.mu
        beta = problem.beta
        sigma = problem.sigma
        matrix_A = np.zeros((n, 3), dtype=np.float64)

        Discretizer.create_and_fill_first_row_matrix(matrix_A, h, problem, nodes)

        for i in range(1, n - 1):
            x_left = nodes[i] - h / 2
            x_right = nodes[i] + h / 2
            matrix_A[i, 0] = -mu(x_left) / h - beta(x_left) / 2 + sigma(x_left) * h / 6
            matrix_A[i, 1] = (mu(x_left) / h + beta(x_left) / 2 + sigma(x_left) * h / 3) + \
                             (mu(x_right) / h - beta(x_right) / 2 + sigma(x_right) * h / 3)
            matrix_A[i, 2] = -mu(x_right) / h + beta(x_right) / 2 + sigma(x_right) * h / 6


        Discretizer.create_and_fill_last_row_matrix(matrix_A, h, problem, nodes)
        return matrix_A

    @staticmethod
    def create_and_fill_vector(problem: PDEProblem, nodes: np.ndarray) -> np.ndarray:
        n = len(nodes)
        h = 1.0 / (n - 1)
        f = problem.f
        vector_L = np.zeros(n, dtype=np.float64)

        Discretizer.create_and_fill_first_el_vector(vector_L, h, problem, nodes)


        for i in range(1, n - 1):
            x_left = nodes[i] - h / 2
            x_right = nodes[i] + h / 2
            vector_L[i] = h * (f(x_left) + f(x_right)) / 2


        Discretizer.create_and_fill_last_el_vector(vector_L, h, problem, nodes)

        return vector_L
