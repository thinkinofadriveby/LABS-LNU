import streamlit as st
from sympy import symbols, sympify, lambdify
from discretizers import Discretizer as fm
from pdeproblems.PDEProblem import PDEProblem, PDEDirichletCondition, PDERobinCondition


class PDEInputForm:

    def __init__(self, bcTypes):
        self._bcTypes = bcTypes

    def display(self) -> None:
        with st.form(key='pde_inputs'):
            st.subheader(f'Elliptic equation')
            self._mu = PDEInputForm.display_func_input('\mu', st)
            self._beta = PDEInputForm.display_func_input('\\beta', st)
            self._sigma = PDEInputForm.display_func_input('\sigma', st)
            self._f = PDEInputForm.display_func_input('f', st)

            left_cnt, right_cnt = st.columns(2)
            self._leftBC = PDEInputForm.display_bc_inputs('left', self._bcTypes, left_cnt)
            self._rightBC = PDEInputForm.display_bc_inputs('right', self._bcTypes, right_cnt)
            is_submitted = st.form_submit_button('Save', type='primary')
            if is_submitted:
                self.build_pdeproblem()

    def build_pdeproblem(self) -> PDEProblem:
        x = symbols('x')
        mu = lambdify(x, sympify(self._mu))
        beta = lambdify(x, sympify(self._beta))
        sigma = lambdify(x, sympify(self._sigma))
        f = lambdify(x, sympify(self._f))

        def build_pdecondition(bc):
            bcType = bc['type']
            bcValues = bc['values']
            if bcType == 'Dirichlet':
                return PDEDirichletCondition(float(sympify(bcValues['uvalue'])))
            elif bcType in ['Neumann', 'Robin']:
                return PDERobinCondition(float(sympify(bcValues['gvalue'])),
                                         float(sympify(bcValues['qvalue'])))

        leftBC = build_pdecondition(self._leftBC)
        rightBC = build_pdecondition(self._rightBC)

        st.session_state['pdeproblem'] = PDEProblem(mu, beta, sigma, f, leftBC, rightBC)
        st.success('PDE problem was successfuly defined.', icon='✔')

    @staticmethod
    def display_func_input(name, cnt):
        return cnt.text_input(f'$\large{{{name}={name}(x)=}}$', key=f'{name}_input')

    @staticmethod
    def display_bc_inputs(side, bcTypes, cnt):
        bcType = bcTypes[side]
        cnt.subheader(f'{side.capitalize()} {bcType}')

        values = {}
        if bcType == 'Dirichlet':
            x_val = '0' if side == 'left' else '1'
            values['uvalue'] = cnt.text_input(
                f'$\large{{u({x_val})=}}$', key=f'uvalue_{side}_input')
        elif bcType == 'Neumann':
            values['gvalue'] = cnt.text_input(
                '$\large{g=}$', key=f'gvalue_{side}_input')
            values['qvalue'] = '0'
        elif bcType == 'Robin':
            values['gvalue'] = cnt.text_input(
                '$\large{g=}$', key=f'gvalue_{side}_input')
            values['qvalue'] = cnt.text_input(
                '$\large{q=}$', key=f'qvalue_{side}_input')
        else:
            cnt.error(f'Unknown boundary condition type {bcType}', icon='❌')
            st.stop()

        return {'type': bcType, 'values': values}


class FEMInputForm:

    def display(self) -> None:
        if not 'pdeproblem' in st.session_state:
            st.info('Submit PDE problem first.')
            return

        with st.form(key='fem_inputs'):
            self._n = st.number_input('Number of nodes:', key='nodes_count_input',
                                      min_value=3, max_value=50, value=3, step=1, format='%d')
            self._iterations = st.slider('Number of iterations:', key='iterations', min_value=1, max_value=10, step=1)

            self._u = st.text_input('Exact solution:', help='If known', key='truesol_input')

            is_submitted = st.form_submit_button('Save', type='primary')
            if is_submitted:
                self.build_femdicretizer()


    def build_femdicretizer(self) -> fm.Discretizer:
        n = self._n
        N = self._n + 1


        st.session_state['femnodescount'] = (n, N)
        pdeproblem = st.session_state['pdeproblem']
        st.session_state['FEMDiscretizer'] = fm.Discretizer(pdeproblem)
        st.session_state['numofiter'] = self._iterations

        if self._u:
            x = symbols('x')
            st.session_state['exact_solution'] = lambdify(x, sympify(self._u))
            st.success(f'FEM problem was successfuly defined with exact solution y={self._u}.', icon='✔')
        elif 'exact_solution' in st.session_state:
            del st.session_state['exact_solution']
            st.success('FEM problem was successfuly defined with exact solution y=y(x).', icon='✔')
