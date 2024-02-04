import math
import streamlit as st
import numpy as np
import pandas as pd
import time
from plotly import express as px
from errors.Incoherence import Incoherence
from errors.ImplicitDirichletLinearErrorCalculator import ImplicitDirichletErrorCalculator
from errors.ImplicitNeumannLinearErrorCalculator import ImplicitNeumannErrorCalculator
from femnorms.FEMEnergyNormCalculator import FEMEnergyNormCalculator
from linearsolvers.ThomasSolver import ThomasSolver
from input_forms import PDEInputForm, FEMInputForm
from femnorms.FEMNormSobolevCalculator import FEMSobolevNormCalculator


def display_boundary_condition_selectors():
    with st.container():
        st.subheader('Boundary condition types')
        boundary_conditions = {}
        bc_options = ['Dirichlet', 'Neumann', 'Robin']
        for col, side in zip(st.columns(2), ['left', 'right']):
            label = f'$\large{{{side.capitalize()}\ boundary\ condition}}$'
            boundary_conditions[side] = col.selectbox(
                label, bc_options, key=f'{side}_bc_select')
        return boundary_conditions


def main():
    st.set_page_config(page_title='Finite Elements Method',
                       page_icon='💎', layout='centered')

    problem_tab, setup_tab, solution_tab = st.tabs(['PDE Problem', 'FEM Setup', 'Solution'])

    with problem_tab:
        boundary_conditions = display_boundary_condition_selectors()
        PDEInputForm(boundary_conditions).display()

    with setup_tab:
        FEMInputForm().display()

        if 'FEMDiscretizer' in st.session_state:
            iterations = st.session_state['numofiter']
            fem_FEMDiscretizer = st.session_state['FEMDiscretizer']
            num_intervals, n = st.session_state['femnodescount']
            with st.spinner('Computing...'):
                time.sleep(0.5)
                df_list = []
                n_list = []
                elements_list = []
                for i in range(iterations):
                    n_list.append(n-1)
                    df, (A, l) = fem_FEMDiscretizer.discretize(n)
                    thomas_solver = ThomasSolver(A, l)
                    df['uh'] = thomas_solver.solve()
                    if 'exact_solution' in st.session_state:
                        y_cols = ['uh', 'u']
                        exact_solution = st.session_state['exact_solution']
                        df['u'] = exact_solution(df['x'])
                    else:
                        y_cols = ['uh']
                    df_list.append(df)
                    node_indices = np.arange(n)
                    elements = np.column_stack((node_indices[:-1], node_indices[1:]))
                    elements_list.append(elements)
                    n = (n * 2) - 1
                    num_intervals = (num_intervals * 2) - 1

                with solution_tab:
                    st.subheader('Solution graph:')
                    for i in range(iterations):
                        # fig = px.(df_list[i], x='x', y=y_cols, template='plotly_dark',
                        #               title=f'Solution u=uh(x), n={n_list[i]}')
                        #
                        # st.plotly_chart(fig)

                        st.plotly_chart(px.scatter(df, x='x', y=y_cols, template='plotly_dark', title='Solution u=uh(x)'))

    with solution_tab:
        st.subheader('Calculation table:')
        pdeproblem = st.session_state["pdeproblem"]
        energy_norms = []
        energy_norms_exact_solution = []
        energy_norms_diff = []
        sobolev_norms = []
        sobolev_norms_exact_solution = []
        sobolev_norms_diff = []
        dirichlet_estimators = []
        neumann_estimators = []
        equation_incoherence = []

        q_list = [None] * iterations
        df_set = pd.DataFrame(n_list, columns=['n'])
        calculate_energy_norm = FEMEnergyNormCalculator(pdeproblem)
        calculate_sobolev_norm = FEMSobolevNormCalculator(pdeproblem)
        calculate_dirichlet_estimators = ImplicitDirichletErrorCalculator(pdeproblem)
        calculate_neumann_estimators = ImplicitNeumannErrorCalculator(pdeproblem)
        calculate_incoherence = Incoherence(pdeproblem)

        for i in range(iterations):

            energy_norm_list = calculate_energy_norm.calculate(df_list[i]['x'], df_list[i]['uh'])
            energy_norms.append(energy_norm_list.sum())

            sobolev_norm_list = calculate_sobolev_norm.calculate(df_list[i]['x'], df_list[i]['uh'])
            sobolev_norms.append(sobolev_norm_list.sum())

            dirichlet_error = calculate_dirichlet_estimators.calculate(df_list[i]['x'], elements_list[i],
                                                             df_list[i]['uh'])
            dirichlet_estimators.append(dirichlet_error.sum())

            neumann_error = calculate_neumann_estimators.calculate(df_list[i]['x'], elements_list[i],
                                                         df_list[i]['uh'])
            neumann_estimators.append(neumann_error.sum())

            equation_inviscosity = calculate_incoherence.calculate(df_list[i]['x'], df_list[i]['uh'])
            equation_incoherence.append(equation_inviscosity.sum())

            if i >= 3:
                numerator = math.log2(abs((energy_norms[i] - energy_norms[i - 1]) / (
                        energy_norms[i - 1] - energy_norms[i - 2])))
                denominator = math.log2(abs((energy_norms[i - 1] - energy_norms[i - 2]) / (
                        energy_norms[i - 2] - energy_norms[i - 3])))
                q = numerator / denominator
                q_list[i] = q

        if 'exact_solution' in st.session_state:
            for i in range(iterations):
                energy_norm_vector_exact_solution = calculate_energy_norm.calculate(df_list[i]['x'],
                                                                              df_list[i]['u'])
                energy_norms_exact_solution.append(energy_norm_vector_exact_solution.sum())

                sobolev_norm_vector_exact_solution = calculate_sobolev_norm.calculate(df_list[i]['x'],
                                                                                df_list[i]['u'])
                sobolev_norms_exact_solution.append(sobolev_norm_vector_exact_solution.sum())

            for i in range(iterations):
                energy_norms_diff.append(energy_norm_list.sum() - energy_norm_vector_exact_solution.sum())
                sobolev_norms_diff.append(
                    sobolev_norm_list.sum() - sobolev_norm_vector_exact_solution.sum())

        df_set['Incoherence'] = equation_incoherence
        df_set['Dirichlet Estimator'] = dirichlet_estimators
        df_set['Neumann Estimator'] = neumann_estimators
        df_set['Energy Norm'] = energy_norms
        df_set['Sobolev Norm'] = sobolev_norms

        if 'exact_solution' in st.session_state:
            df_set['[EXACT] Energy Norm'] = energy_norms_exact_solution
            df_set['[EXACT] Sobolev Norm'] = sobolev_norms_exact_solution
            df_set['Energy Norm Diff'] = energy_norms_diff
            df_set['Sobolev Norm Diff'] = sobolev_norms_diff
        df_set['Order of convergence'] = q_list

        st.dataframe(df_set)

if __name__ == "__main__":
    main()