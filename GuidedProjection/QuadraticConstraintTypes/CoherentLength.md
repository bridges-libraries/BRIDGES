# Coherent Length

#### Definition

The `CoherentLength` quadratic constraint ensures that a segment length, defined as the distance between two point variables, <em><span style="text-decoration : underline">p</span><sub>s</sub></em> and <em><span style="text-decoration : underline">p</span><sub>e</sub></em>, is equal to a scalar variable <em>l</em>, representing the length of the segment. In other words, the constraint updates the scalar variable and the two points variables so that the distance between the points equals the length.

#### Requirements

To use the `CoherentLength` constraint the following variables must be defined in the solver:
- <em><span style="text-decoration : underline">p</span><sub>s</sub></em> : Point variable corresponding to the start of the segment.
- <em><span style="text-decoration : underline">p</span><sub>e</sub></em> : Point variable corresponding to the end of the segment.
- <em>l</em> : Scalar variable representing the length of the segment.

#### Suggestions

When vertices and segment lengths are both added as variables in the solver, the `CoherentLength` constraint must be added to the solver. Otherwise, any energy or constraint using the length variables will produce unreliable results.

## Constraint Formulation

The formulation of the `CoherentLength` constraint reads :

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/CoherentLength/CoherentLength-Formulation.svg" alt="Formulation of the CoherentLength constraint" height="40"/>
  <!-- Raw LaTeX : < \left ( \underline{p}_{e} - \underline{p}_{s} \right ) , \left ( \underline{p}_{e} - \underline{p}_{s} \right ) > - l^{2} = 0 -->
  <br></br>
</center>

Hence, in the case where the points are from a three-dimensional space, the local problem can be expressed as follow:

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/CoherentLength/CoherentLength-xReduced.svg" alt="Reduced vector x for the CoherentLength constraint" height="200"/>
  <!-- Raw LaTeX : \underline{x}_{red} = \begin{bmatrix} x_{s} \\ y_{s} \\ z_{s} \\ x_{e} \\ y_{e} \\ z_{e} \\ l \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/CoherentLength/CoherentLength-Hi.svg" alt="Quadratic part of the CoherentLength constraint" height="200"/>
  <!-- Raw LaTeX : \underline{\underline{H}}_{i,red} = \begin{pmatrix} 2 & 0 & 0 & -2 & 0 & 0 & 0 \\ 0 & 2 & 0 & 0 & -2 & 0 & 0 \\ 0 & 0 & 2 & 0 & 0 & -2 & 0 \\ -2 & 0 & 0 & 2 & 0 & 0 & 0 \\ 0 & -2 & 0 &  0 & 2 & 0 & 0 \\ 0 & 0 & -2 & 0 & 0 & 2 & 0 \\ 0 & 0 & 0 & 0 & 0 & 0 & -2 \\ \end{pmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/CoherentLength/CoherentLength-bi.svg" alt="Linear part of the CoherentLength constraint" height="200"/>
  <!-- Raw LaTeX : \underline{b}_{i} = \begin{bmatrix} 0 \\ 0 \\ 0 \\ 0 \\ 0 \\ 0 \\ 0 \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/CoherentLength/CoherentLength-ci.svg" alt="Constant part of the CoherentLength constraint" height="40"/>
  <!-- Raw LaTeX : c_{i} = 0 -->
  <br></br>
</center>

In the framework, the `CoherentLength` constraint is not restricted to the three-dimensional case. The dimension of the space is given at initialisation of the constraint.
