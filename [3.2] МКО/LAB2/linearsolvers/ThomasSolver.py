from typing import Union, Iterable
import numpy as np
from .LinearSolver import LinearSolver


class ThomasSolver(LinearSolver):

    def __init__(self, A: np.ndarray, l: np.ndarray):
        super().__init__(A, l)

    def solve(self, A, l) -> np.ndarray:
        #A = self._A.copy()
        #l = self._l.copy()
        ThomasSolver.direct(A, l)
        ThomasSolver.reverse(A, l)
        return l

    @staticmethod
    def direct(A: np.ndarray, l: np.ndarray) -> None:
        n = A.shape[0]
        for i in range(n - 1):
            if ThomasSolver.check_rows(A, i, i + 1):
                ThomasSolver.swap_rows(A, l, i, i + 1)

            a = A[i, 1]
            b = A[i + 1, 0]

            l[i] /= a
            for j in range(3):
                A[i, j] /= a

            if not np.isclose(b, 0):
                l[i+1] -= l[i] * b
                for j in range(2):
                    A[i + 1, j] -= A[i, j+1] * b

    @staticmethod
    def reverse(A: np.ndarray, l: np.ndarray) -> None:
        n = A.shape[0]
        l[-1] /= A[-1, 1]
        A[-1, 1] = 1
        for i in reversed(range(n - 1)):
            a = A[i, 2]
            if not np.isclose(a, 0):
                l[i] -= a * l[i + 1]
                A[i, 2] = 0

    @staticmethod
    def check_rows(A: np.ndarray, i: int, j: int) -> Union[np.ndarray, Iterable, int, float]:
        a = A[i, 1]
        b = A[j, 0]
        if np.isclose(a, 0) and np.isclose(b, 0):
            raise ValueError("Problem has no solution")
        return np.isclose(a, 0)

    @staticmethod
    def swap_rows(A, l, i, j):
        A[[i, j]] = A[[j, i]]
        l[[i, j]] = l[[j, i]]
