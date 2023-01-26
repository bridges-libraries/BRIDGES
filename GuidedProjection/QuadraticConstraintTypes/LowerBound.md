# Lower Bound

#### Definition

The `LowerBound` quadratic constraint ensures that a scalar value <em>l</em> remains above a given limit <em>σ</em>.

#### Requirements

To use the `LowerBound` constraint the following variables must be defined in the solver:
- <em>l</em> : Scalar variable being constrained.
- <em>λ</em> : Scalar variable used as dummy value.

#### Suggestions

If vertices and segment lengths are both added as variables in the solver, the [`CoherentLength`](./CoherentLength.md) constraint is generally used to enforce the equality between the length variable and the distance of two points. In this case, it is recommended to use the `LowerBound` constraint on the lengths to assure the  positivity of the variables. Indeed, the [`CoherentLength`](./CoherentLength.md) assures the equality between the squared values.

## Constraint Formulation

The formulation of the `LowerBound` quadratic constraint reads :

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/LowerBound/LowerBound-Formulation.svg" alt="Formulation of the LowerBound constraint" height="40"/>
  <!-- Raw LaTeX : l = \sigma + \lambda ^{2} -->
  <br></br>
</center>

Hence, the local problem can be expressed as follow:

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/LowerBound/LowerBound-xReduced.svg" alt="Reduced vector x for the LowerBound constraint" height="80"/>
  <!-- Raw LaTeX : \underline{x}_{red} = \begin{bmatrix} l \\ \lambda \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/LowerBound/LowerBound-Hi.svg" alt="Quadratic part of the LowerBound constraint" height="80"/>
  <!-- Raw LaTeX : \underline{\underline{H}}_{i,red} = \begin{pmatrix} 0 & 0 \\ 0 & 2 \\ \end{pmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/LowerBound/LowerBound-bi.svg" alt="Linear part of the LowerBound constraint" height="80"/>
  <!-- Raw LaTeX : \underline{b}_{i} = \begin{bmatrix} -1 \\ 0 \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/LowerBound/LowerBound-ci.svg" alt="Constant part of the LowerBound constraint" height="40"/>
  <!-- Raw LaTeX : c_{i} = \sigma -->
  <br></br>
</center>
