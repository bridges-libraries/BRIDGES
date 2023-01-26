# Vector Length

#### Definition

The `VectorLength` quadratic constraint ensures that a vector variable <em><span style="text-decoration : underline">v</span></em> has a length equal to a given scalar value <em>l</em>.

#### Requirements

To use the `VectorLength` constraint the following variables must be defined in the solver:
- <em><span style="text-decoration : underline">v</span></em> : Vector variable with a finite number of coordinates.

#### Suggestions

No remarks.

## Constraint Formulation

The formulation of the `UpperBound` quadratic constraint reads :

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/VectorLength/VectorLength-Formulation.svg" alt="Formulation of the VectorLength constraint" height="40"/>
  <!-- Raw LaTeX : \left < \underline{v} , \underline{v} \right > - l^{2} = 0 -->
  <br></br>
</center>

which, in the case where the vector is from a three-dimensional space, develops to :

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/VectorLength/VectorLength-Developed.svg" alt="Developed formulation of the VectorLength constraint" height="40"/>
  <!-- Raw LaTeX : x_{v}^{2} + y_{v}^{2} + z_{v}^{2} - l^{2} = 0 -->
  <br></br>
</center>

Hence, the local problem can be expressed as follow:

<center>
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/VectorLength/VectorLength-xReduced.svg" alt="Reduced vector x for the VectorLength constraint" height="80"/>
  <!-- Raw LaTeX : \underline{x}_{red} = \begin{bmatrix} x_{v} \\ y_{v} \\ z_{v} \\ \end{bmatrix}  -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/VectorLength/VectorLength-Hi.svg" alt="Quadratic part of the VectorLength constraint" height="80"/>
  <!-- Raw LaTeX : \underline{\underline{H}}_{i,red} = \begin{pmatrix} -2 & 0 & 0 \\ 0 & -2 & 0 \\ 0 & 0 & -2 \\ \end{pmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/VectorLength/VectorLength-bi.svg" alt="Linear part of the VectorLength constraint" height="80"/>
  <!-- Raw LaTeX : \underline{b}_{i} = \begin{bmatrix} 0 \\ 0 \\ 0 \\ \end{bmatrix} -->
  ,
  <img src="../../Images/GuidedProjection/QuadraticConstraintTypes/VectorLength/VectorLength-ci.svg" alt="Constant part of the VectorLength constraint" height="40"/>
  <!-- Raw LaTeX : c_{i} = l^{2} -->
  <br></br>
</center>

In the framework, the `VectorLength` constraint is not restricted to the three-dimensional case. The dimension of the space is given at initialisation of the constraint.
