import math

import streamlit as st, pandas as pd
from plotly import express as px
from sympy import sympify, symbols, lambdify

from femsolvers.discretizers.LinearDiscretizer import LinearDiscretizer
from linearsolvers.ThomasSolver import ThomasSolver
from forms import DataInputForm, NodeInputForm
import numpy as np
from norms.EnergyNormCalculator import *
from norms.SobolevNormCalculator import *

# from norms.EnergyNormCalculator import *
# from norms.SobolevNormCalculator import *
# from errors.DirichletErrorCalculator import *
# from errors.NeumannErrorCalculator import *
# from otherstuff.EquationInviscosity import *
from pdeproblems.BoundaryCondition import RobinCondition
from pdeproblems.ParabolicPDEProblem import PDEProblem


def mmul(a, b):
    r = np.zeros_like(b)
    r[0] = a[0, 1] * b[0] + a[0, 2] * b[1]
    for i in range(1, len(a) - 1):
        r[i] = a[i, 0] * b[i - 1] + a[i, 1] * b[i] + a[i, 2] * b[i + 1]
    r[-1] = a[-1, 0] * b[-2] + a[-1, 1] * b[-1]

    return r


def display_bc_type_selectors():
    with st.container():
        st.subheader('Boundary conditions')
        boundary_cond_types = {}
        bc_options = ['Dirichlet', 'Neumann', 'Robin']
        for column, side in zip(st.columns(2), ['left', 'right']):
            label = f'$\large{{{side.capitalize()}\ condition}}$'
            boundary_cond_types[side] = column.selectbox(
                label, bc_options, key=f'{side}_bc_select')
        return boundary_cond_types


def main():
    st.set_page_config(page_title='Finite Elements Method',
                       page_icon='🧶', layout='centered')

    problem_data_tab, node_tab, graph_tab = st.tabs(
        ['Boundary conditions', 'Nodes', 'Solution'])

    with problem_data_tab:
        bound_conditions_types = display_bc_type_selectors()
        DataInputForm(bound_conditions_types).display()

    with node_tab:
        NodeInputForm().show_node_iteration_inputs()

        if 'discretizer' in st.session_state:
            with st.spinner('Finding solutions'):
                disc = st.session_state['discretizer']
                N = st.session_state['nodescount']
                T = st.session_state['timenodes']
                pdeProblem = st.session_state['pdeproblem']
                theta = 1 / 2

                mesh = disc.buildMesh(N)
                q0 = disc.buildInitialSolution(mesh)

                nodes, _ = mesh
                px.line(y=[q0, pdeProblem.initC(nodes)])

                qs = [q0]
                dT = 1 / (T - 1)
                for i in range(1, T):
                    ti = (i - 0.5) * dT
                    M, A, l = disc.discretize(mesh, ti)

                    dqi = ThomasSolver(A, l).solve(M + dT * theta * A, l - mmul(A, qs[-1]))
                    qi = qs[-1] + dT * dqi
                    qs.append(qi)

                df = pd.DataFrame(qs).T
                df['x'] = nodes
                df = df.melt(id_vars='x', var_name='t', value_name='uh')
                df['t'] *= np.round(dT, 2)

                if 'true_solution' in st.session_state:
                    y_column_names = ['uh', 'u']
                    y_true = st.session_state['true_solution']
                    df['u'] = y_true
                    df['u'] = df.apply(lambda row: y_true(row['x'], row['t']), axis=1)
                else:
                    y_column_names = ['uh']

    with graph_tab:
        st.plotly_chart(px.line(df, x='x', y=y_column_names, animation_frame='t', range_y=(-1.0, 1.0)))
        time_node_for_table = st.select_slider('Time node:', options=df['t'])
        if st.button("Calculate"):
            list_of_energy_norms = []
            energy_norm_calc = EnergyNormCalculator(pdeProblem)
            df_for_table = df[df['t'] == time_node_for_table]
            energy_norm_vector = energy_norm_calc.calculate(df_for_table['x'], df_for_table['uh'], time_node_for_table)
            list_of_energy_norms.append(energy_norm_vector.sum())
            sobolev_norm_calc = SobolevNormCalculator(pdeProblem)
            sobolev_norm_vector = sobolev_norm_calc.calculate(df_for_table['x'], df_for_table['uh'], time_node_for_table)
            df_for_table = df_for_table.reset_index(drop=True)
            df_for_table = df_for_table.drop(columns='t')
            st.dataframe(df_for_table)
            st.write(f'Energy norm: {round(energy_norm_vector.sum(), 5)}')
            st.write(f'Sobolev norm: {round(sobolev_norm_vector.sum(), 5)}')

if __name__ == "__main__":
    main()
