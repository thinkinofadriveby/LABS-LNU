from typing import Callable
from .BoundaryCondition import *


class PDEProblem:

    def __init__(self,
                 rho: Callable[[float, float], float],
                 mu: Callable[[float, float], float],
                 beta: Callable[[float, float], float],
                 sigma: Callable[[float, float], float],
                 f: Callable[[float, float], float],
                 left_boundary_condition: BoundaryCondition,
                 right_boundary_condition: BoundaryCondition,
                 initC: Callable[[float], float]):

        self._rho = rho
        self._mu = mu
        self._beta = beta
        self._sigma = sigma
        self._f = f
        self._leftBC = left_boundary_condition
        self._rightBC = right_boundary_condition
        self._initC = initC

    def rho(self, x: float, t: float) -> float:
        return self._rho(x, t)

    def mu(self, x: float, t: float) -> float:
        return self._mu(x, t)

    def beta(self, x: float, t: float) -> float:
        return self._beta(x, t)

    def sigma(self, x: float, t: float) -> float:
        return self._sigma(x, t)

    def f(self, x: float, t: float) -> float:
        return self._f(x, t)

    def initC(self, x: float) -> float:
        return self._initC(x)

    def left_bound_cond(self) -> BoundaryCondition:
        return self._leftBC

    def right_bound_cond(self) -> BoundaryCondition:
        return self._rightBC

    def is_left_bound_cond_dirichlet(self) -> bool:
        return isinstance(self._leftBC, DirichletCondition)

    def is_right_bound_cond_dirichlet(self) -> bool:
        return isinstance(self._rightBC, DirichletCondition)
