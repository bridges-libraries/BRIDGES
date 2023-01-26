# Upper Bound

#### Definition

The `UpperBound` quadratic constraint ensures that a scalar value <em>l</em> remains under a given limit <em>σ</em>.

#### Requirements

To use the `UpperBound` constraint the following variables must be defined in the solver:
- <em>l</em> : Scalar variable being constrained.
- <em>λ</em> : Scalar variable used as dummy value.

#### Suggestions

No remarks.

## Constraint Formulation

The formulation of the `UpperBound` quadratic constraint reads :

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/UpperBound/UpperBound-Formulation.svg" alt="Formulation of the UpperBound constraint" height="40"/>
  <!-- Raw LaTeX : l = \sigma + \lambda ^{2} -->
  <br></br>
</center>

Hence, the local problem can be expressed as follow:

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/UpperBound/UpperBound-xReduced.svg" alt="Reduced vector x for the UpperBound constraint" height="80"/>
  <!-- Raw LaTeX : \underline{x}_{red} = \begin{bmatrix} l \\ \lambda \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/UpperBound/UpperBound-Hi.svg" alt="Quadratic part of the UpperBound constraint" height="80"/>
  <!-- Raw LaTeX : \underline{\underline{H}}_{i,red} = \begin{pmatrix} 0 & 0 \\ 0 & 2 \\ \end{pmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/UpperBound/UpperBound-bi.svg" alt="Linear part of the UpperBound constraint" height="80"/>
  <!-- Raw LaTeX : \underline{b}_{i} = \begin{bmatrix} -1 \\ 0 \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/UpperBound/UpperBound-ci.svg" alt="Constant part of the UpperBound constraint" height="40"/>
  <!-- Raw LaTeX : c_{i} = \sigma -->
  <br></br>
</center>
