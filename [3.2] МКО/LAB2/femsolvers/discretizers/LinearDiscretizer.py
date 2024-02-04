import numpy as np
from typing import Union
from tqdm import tqdm
from pdeproblems.ParabolicPDEProblem import PDEProblem
from Discretizer import Discretizer
from sympy import symbols, sympify, lambdify


class LinearDiscretizer(Discretizer):

    def __init__(self, pdeProblem: PDEProblem):
        super().__init__(pdeProblem)
        self._nodCoefs = np.asarray([-1 / 2, 1 / 2])
        self._matCoefs = np.asarray([
            # First Matrix Row: (0, h]
            [[[0, 0, 0], [0, 0, 0]],
             [[0, 0, 0], [1, -1 / 2, 1 / 3]],
             [[0, 0, 0], [-1, 1 / 2, 1 / 6]]],
            [[[-1, -1 / 2, 1 / 6], [0, 0, 0]],
             [[1, 1 / 2, 1 / 3], [1, -1 / 2, 1 / 3]],
             [[0, 0, 0], [-1, 1 / 2, 1 / 6]]],
            [[[-1, -1 / 2, 1 / 6], [0, 0, 0]],
             [[1, 1 / 2, 1 / 3], [0, 0, 0]],
             [[0, 0, 0], [0, 0, 0]]]
        ])
        self._vecCoefs = np.asarray([
            [[[  0], [1/2]]],
            [[[1/2], [1/2]]],
            [[[1/2], [  0]]]
        ])

    def buildMesh(self, n: int) -> np.ndarray:
        nodes = np.linspace(0, 1, n + 1)
        node_indices = np.arange(n + 1)
        elements = np.column_stack((node_indices[:-1], node_indices[1:]))

        return nodes, elements

    def postprocess(self, values: np.ndarray) -> np.ndarray:
        return values

    def buildInitialSolution(self, mesh: Union[np.ndarray,np.ndarray]) -> np.ndarray:
        nodes, _ = mesh
        return self._pdeProblem.initC(nodes)

    def discretize(self, mesh: Union[np.ndarray,np.ndarray], t: float) -> Union[np.ndarray, np.ndarray, np.ndarray]:
        #mesh = self.buildMesh(n)
        M, A = self.buildMatrix(mesh, t)
        l = self.buildVector(mesh, t)
        A, l = self.setBoundaryConditions(A, l, t)

        return M, A, l

    def buildMatrix(self, mesh: Union[np.ndarray, np.ndarray], t: float) -> Union[np.ndarray, np.ndarray]:
        problem = self._pdeProblem
        nodes, _ = mesh
        n = len(nodes)
        h = 1.0 / (n - 1)

        def fill_m_matrix_row(i):
            j = self._getCoefRowIndex(i, n)
            m = self._matCoefs[j][:, :, [2]]
            #print('m coef from m method', m)
            s = np.asarray([h])
            x = nodes[i] + h * self._nodCoefs
            c = np.asarray((problem.rho(x, t),)).T

            print('m method')
            print(problem.rho(x, t))
            #print(c)
            #print((m * s * c))
            #print((m * s * c).sum(axis=(1, 2)))

            return (m * s * c).sum(axis=(1, 2))

        def fill_a_matrix_row(i):
            j = self._getCoefRowIndex(i, n)
            m = self._matCoefs[j]
            #print('m coef from a method', m)
            s = np.asarray([1 / h, 1, h])
            x = nodes[i] + h * self._nodCoefs
            #print(isinstance(problem.sigma(x, t), float))
            #print(isinstance(problem.beta(x, t), float))
            #print(type(problem.beta(x, t)))
            #print(np.isscalar(problem.beta(x, t)))
            #print(np.isscalar(problem.sigma(x, t)))
            if np.isscalar(problem.beta(x, t)):
                array = [problem.beta(x, t), problem.beta(x, t)]
                arr_for_beta = np.asarray(array)
                c = np.asarray((problem.mu(x, t), arr_for_beta, problem.sigma(x, t))).T
            else:
                c = np.asarray((problem.mu(x, t), problem.beta(x, t), problem.sigma(x, t))).T
            #array = [problem.beta(x, t), problem.beta(x, t)]
            #arr_for_beta = np.asarray(array)
            print('a method')
            #print(arr_for_beta)
            #c = np.asarray((problem.mu(x, t), arr_for_beta, problem.sigma(x, t))).T
            #print(np.asarray((problem.mu(x, t), problem.beta(x, t), problem.beta(x, t), problem.sigma(x, t))).T)
            #print(c)
            print(problem.mu(x, t))
            #print(problem.beta(x, t))
            #print(problem.sigma(x, t))
            #print(problem.rho(x, t))
            #print(c)
            #print((m * s * c))
            temp = (m * s * c).sum(axis=(1, 2))
            #print('temp')
            #print(temp)
            temp2 = np.sum(temp, axis=(0))
            temp_list = []
            for item in temp:
                #print('item sum', np.sum(item))
                temp_list.append(np.sum(item))

            #print('temp_list', temp_list)
            #print('temp2')
            #print(temp2)

            #return temp_list
            return (m * s * c).sum(axis=(1, 2))

        M = np.zeros((n, 3), dtype=np.float64)
        A = np.zeros((n, 3), dtype=np.float64)
        for i in tqdm(range(n), desc='Left hand matrix creation'):
            M[i, :] = fill_m_matrix_row(i)
            A[i, :] = fill_a_matrix_row(i)

        return M, A

    def buildVector(self, mesh: Union[np.ndarray, np.ndarray], t: float) -> np.ndarray:
        problem = self._pdeProblem
        nodes, _ = mesh
        n = len(nodes)
        h = 1.0 / (n - 1)

        def fill_vector_row(i):
            j = self._getCoefRowIndex(i, n)
            m = self._vecCoefs[j]
            s = np.asarray([h])
            x = nodes[i] + h * self._nodCoefs
            c = np.asarray((problem.f(x, t), )).T

            return (m*s*c).sum(axis=(1,2))

        l = np.zeros(n, dtype=np.float64)
        for i in tqdm(range(n), desc='Right hand vector creation'):
            l[i] = fill_vector_row(i)

        return l

    def setBoundaryConditions(self, A: np.ndarray, l: np.ndarray, t: float) -> Union[np.ndarray, np.ndarray]:
        problem = self._pdeProblem
        if problem.is_left_bound_cond_dirichlet():
            A[0, :] = 0
            A[0, 1] = 1
            l[0] = problem.left_bound_cond().uvalue(t)
        else:
            A[0, 1] -= problem.left_bound_cond().qvalue()
            l[0] -= problem.left_bound_cond().gvalue(t) #float((sympify(problem.left_bound_cond().gvalue())))
        if problem.is_right_bound_cond_dirichlet():
            A[-1, :] = 0
            A[-1, 1] = 1
            l[-1] = problem.right_bound_cond().uvalue(t)
        else:
            A[-1, 1] = - problem.right_bound_cond().qvalue()
            l[-1] -= problem.right_bound_cond().gvalue(t)

        return A, l

    def _getCoefRowIndex(self, i: int, n: int) -> int:
        if i == 0:
            return 0
        elif i == n-1:
            return 2
        else:
            return 1
