from typing import Callable
from .PDEBoundaryCondition import *


class PDEProblem:

    def __init__(self,
               mu: Callable[[float], float],
               beta: Callable[[float], float],
               sigma: Callable[[float], float],
               f: Callable[[float], float],
               leftBoudaryCondition: PDEBoundaryCondition,
               rightBoudnaryCondition: PDEBoundaryCondition):

        self._mu = mu
        self._beta = beta
        self._sigma = sigma
        self._f = f
        self._leftBC = leftBoudaryCondition
        self._rightBC = rightBoudnaryCondition

    def mu(self, x: float) -> float:
        return self._mu(x)

    def beta(self, x: float) -> float:
        return self._beta(x)

    def sigma(self, x: float) -> float:
        return self._sigma(x)

    def f(self, x: float) -> float:
        return self._f(x)

    def leftBC(self) -> PDEBoundaryCondition:
        return self._leftBC

    def rightBC(self) -> PDEBoundaryCondition:
        return self._rightBC

    def isLeftBCFirstType(self) -> bool:
        return isinstance(self._leftBC, PDEDirichletCondition)

    def isRightBCFirstType(self) -> bool:
        return isinstance(self._rightBC, PDEDirichletCondition)
