from pdeproblems.ParabolicPDEProblem import *
from femsolvers.discretizers.LinearDiscretizer import LinearDiscretizer
from linearsolvers.ThomasSolver import ThomasSolver
import pandas as pd
from plotly import express as px
from pdeproblems.BoundaryCondition import BoundaryCondition
from pdeproblems.ParabolicPDEProblem import PDEProblem
import numpy as np
from sympy import symbols, sympify, lambdify

from pdeproblems.ParabolicPDEProblem import PDEProblem


def mmul(a, b):
    r = np.zeros_like(b)
    r[0] = a[0, 1] * b[0] + a[0, 2] * b[1]
    for i in range(1, len(a) - 1):
        r[i] = a[i, 0] * b[i - 1] + a[i, 1] * b[i] + a[i, 2] * b[i + 1]
    r[-1] = a[-1, 0] * b[-2] + a[-1, 1] * b[-1]

    return r


rho = '1/4'
mu = '3*x'
beta ='-1'
sigma = 'pi*pi*x/4'
f = 'pi*pi*x*cos(pi*(x+4*t)/2) + pi*3/2*sin(pi/2*(x+4*t))'


leftBC = {'type': 'Dirichlet', 'gvalue': 'cos(2*pi*t)', 'qvalue': '0'}
rightBC = {'type': 'Dirichlet', 'gvalue': '-sin(2*pi*t)', 'qvalue': '0'}

initC = 'cos(pi*x/2)'

# new code
x = symbols('x')
t = symbols('t')
rho = lambdify([x, t], sympify(rho))
mu = lambdify([x, t], sympify(mu))
beta = lambdify([x, t], sympify(beta))
sigma = lambdify([x, t], sympify(sigma))
f = lambdify([x, t], sympify(f))
initC = lambdify(x, sympify(initC))
gvalueLeft = lambdify(t, sympify('cos(2*pi*t)'))
qvalueLeft = float((sympify('0')))
gvalueRight = lambdify(t, sympify('-sin(2*pi*t)'))
qvalueRight = float((sympify('0')))
leftBC = RobinCondition(gvalueLeft, qvalueLeft)
rightBC = RobinCondition(gvalueRight, qvalueRight)
#new code

pdeProblem = PDEProblem(rho, mu, beta, sigma, f, leftBC, rightBC, initC)

#y_true = np.vectorize(lambda x, t: np.cos(np.pi * x / 2 + 2 * np.pi * t))

N = 51
T = 21
theta = 1/2

disc = LinearDiscretizer(pdeProblem=pdeProblem)
mesh = disc.buildMesh(N)
q0 = disc.buildInitialSolution(mesh)

nodes, _ = mesh
px.line(y=[q0, pdeProblem.initC(nodes)])


qs = [q0]
dT = 1/(T-1)
for i in range(1, T):
    ti = (i - 0.5) * dT
    M, A, l = disc.discretize(mesh, ti)

    dqi = ThomasSolver(A, l).solve(M + dT*theta*A, l - mmul(A, qs[-1]))
    qi = qs[-1] + dT * dqi
    qs.append(qi)


df = pd.DataFrame(qs).T
df['x'] = nodes
df = df.melt(id_vars='x', var_name='t', value_name='uh')
df['t'] *= np.round(dT, 2)

#df['u'] = df.apply(lambda row: y_true(row['x'], row['t']), axis=1)

#df['u'] = np.vectorize(y_true)(df['x'].values, df['t'].values)

df['u'] = y_true(df['x'], df['t'])

print(q0)
print(len(qs))
print(df)
print(df['uh'][100])

fig = px.line(df, x='x', y=['uh', 'u'], animation_frame='t', range_y=(-1.1, 1.1))


fig.show(renderer="browser")