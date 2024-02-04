import streamlit as st
from sympy import symbols, sympify, lambdify
import Discretizer as fm
from femsolvers.discretizers.LinearDiscretizer import LinearDiscretizer
from pdeproblems.ParabolicPDEProblem import PDEProblem
from pdeproblems.BoundaryCondition import DirichletCondition, RobinCondition

class DataInputForm:

    def __init__(self, bcTypes):
        self._bcTypes = bcTypes

    def display(self) -> None:
        with st.form(key='data_inputs'):
            st.subheader(f'Input values:')
            self._rho = DataInputForm.show_function_input('⍴', st)
            self._mu = DataInputForm.show_function_input('\mu', st)
            self._beta = DataInputForm.show_function_input('β', st)
            self._sigma = DataInputForm.show_function_input('σ', st)
            self._f = DataInputForm.show_function_input('f', st)
            self._initC = DataInputForm.show_function_input('Initial Solution', st)

            left_column, right_column = st.columns(2)
            self._leftBC = DataInputForm.show_boundary_conditions_inputs('left', self._bcTypes, left_column)
            self._rightBC = DataInputForm.show_boundary_conditions_inputs('right', self._bcTypes, right_column)

            is_submitted = st.form_submit_button('Save', type='primary')
            if is_submitted:
                self.create_and_initialize_problem()

    def create_and_initialize_problem(self) -> PDEProblem:

        x = symbols('x')
        t = symbols('t')
        rho = lambdify([x, t], sympify(self._rho))
        mu = lambdify([x, t], sympify(self._mu))
        beta = lambdify([x, t], sympify(self._beta))
        sigma = lambdify([x, t], sympify(self._sigma))
        f = lambdify([x, t], sympify(self._f))
        initC = lambdify(x, sympify(self._initC))

        def create_and_initialize_bound_conditions(bc):
            bcType = bc['type']
            bcValues = bc['values']
            if bcType == 'Dirichlet':
                return DirichletCondition(lambdify(t, sympify(bcValues['uvalue'])))
            elif bcType in ['Neumann', 'Robin']:
                return RobinCondition(lambdify(t, sympify(bcValues['gvalue'])),
                                      float((sympify(bcValues['qvalue']))))

        leftBC = create_and_initialize_bound_conditions(self._leftBC)
        rightBC = create_and_initialize_bound_conditions(self._rightBC)

        st.session_state['pdeproblem'] = PDEProblem(rho, mu, beta, sigma, f, leftBC, rightBC, initC)
        st.success('Saved successfully!', icon='✔')

    @staticmethod
    def show_function_input(name, cnt):
        return cnt.text_input(f'$\large{{ {name} }}$ ', key=f'{name}_input')

    @staticmethod
    def show_boundary_conditions_inputs(side, bound_condition_types, column):
        bound_condition_type = bound_condition_types[side]
        column.subheader(f'{side.capitalize()} {bound_condition_type}')

        values_for_bound_conditions = {}
        if bound_condition_type == 'Dirichlet':
            if side == 'left':
                x_val = '0'
            else:
                x_val = '1'
            values_for_bound_conditions['uvalue'] = column.text_input(
                f'$\large{{u({x_val})=}}$', key=f'uvalue_{side}_input')
        elif bound_condition_type == 'Neumann':
            values_for_bound_conditions['gvalue'] = column.text_input(
                '$\large{g=}$', key=f'gvalue_{side}_input')
            values_for_bound_conditions['qvalue'] = '0'
        elif bound_condition_type == 'Robin':
            values_for_bound_conditions['gvalue'] = column.text_input(
                '$\large{g=}$', key=f'gvalue_{side}_input')
            values_for_bound_conditions['qvalue'] = column.text_input(
                '$\large{q=}$', key=f'qvalue_{side}_input')
        else:
            column.error(f'Unknown boundary condition type {bound_condition_type}', icon='❌')
            st.stop()

        return {'type': bound_condition_type, 'values': values_for_bound_conditions}


class NodeInputForm:

    def show_node_iteration_inputs(self) -> None:
        if not 'pdeproblem' in st.session_state:
            st.info('No data')
            return

        with st.form(key='node_inputs'):
            self._n = int(st.number_input('Input number of nodes', min_value=1, max_value=200, value=5, step=1))
            self._t = int(st.number_input('Input number of time nodes', min_value=1, max_value=200, value=5, step=1))

            x = symbols('x')
            t = symbols('t')
            self._u = st.text_input('Exact solution', help='Optional', key='precise_solution_input')
            is_submitted = st.form_submit_button('Calculate', type='primary')
            if is_submitted:
                self.initialize_discretizer()

    def initialize_discretizer(self) -> fm.Discretizer:
        pdeproblem = st.session_state['pdeproblem']
        st.session_state['discretizer'] = LinearDiscretizer(pdeProblem=pdeproblem)
        st.session_state['nodescount'] = self._n
        st.session_state['timenodes'] = self._t
        if self._u:
            x = symbols('x')
            t = symbols('t')
            u = lambdify([x, t], sympify(self._u))
            y_true_arg = u
            #y_true = np.vectorize(lambda x, t: y_true_arg) #np.cos(np.pi * x / 2 + 2 * np.pi * t)
            st.session_state['true_solution'] = y_true_arg
            st.success(f'Solutions were found with precise solution y={self._u}', icon='✔')
        elif 'true_solution' in st.session_state:
            del st.session_state['true_solution']
            st.success('Solutions were found', icon='✔')
