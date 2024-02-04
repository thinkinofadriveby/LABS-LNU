class PDEBoundaryCondition:

    def __init__(self):
        pass

    def uvalue(self) -> float:
        return 0

    def gvalue(self) -> float:
        return 0

    def qvalue(self) -> float:
        return 0


class PDEDirichletCondition(PDEBoundaryCondition):

    def __init__(self, uvalue: float):
        self._uvalue = uvalue

    def uvalue(self) -> float:
        return self._uvalue


class PDERobinCondition(PDEBoundaryCondition):

    def __init__(self, g: float, q: float):
        self._g = g
        self._q = q

    def gvalue(self) -> float:
        return self._g

    def qvalue(self) -> float:
        return self._q