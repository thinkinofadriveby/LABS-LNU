from typing import Callable
from pdeproblems import *


class BoundaryCondition:

    def __init__(self):
        pass

    def uvalue(self, t: float) -> float:
        return 0

    def gvalue(self, t: float) -> float:
        return 0

    def qvalue(self) -> float:
        return 0


class DirichletCondition(BoundaryCondition):

    def __init__(self, uvalue: Callable[[float], float]):
        self._uvalue = uvalue

    def uvalue(self, t: float) -> float:
        return self._uvalue(t)


class RobinCondition(BoundaryCondition):

    def __init__(self, g: Callable[[float], float], q: float):
        self._g = g
        self._q = q

    def gvalue(self, t: float) -> float:
        return self._g(t)

    def qvalue(self) -> float:
        return self._q
