using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a Guided Projection Algorithm solver.<br/>
    /// The algorithm is described in <see href="https://doi.org/10.1145/2601097.2601213"/>.
    /// </summary>
    public sealed class GuidedProjectionAlgorithm
    {
        #region Events

        // ---------- For Weigths ---------- //

        /// <summary>
        /// Event raised whenever <see cref="Energy"/> and <see cref="Constraint"/> weights needs to be updated.
        /// </summary>
        private event Action<int> WeigthUpdate;

        /// <summary>
        /// Raises the event which updates the <see cref="Energy"/> and the <see cref="Constraint"/> weights. 
        /// </summary>
        /// <param name="iteration"> Index of the current iteration. </param>
        private void OnWeigthUpdate(int iteration)
        {
            WeigthUpdate?.Invoke(iteration);
        }


        // ---------- For Quadratic Constraints ---------- //

        /// <summary>
        /// Event raised whenever the members of <see cref="LinearisedConstraintType"/> needs to be updated.
        /// </summary>
        private event Action LinearisedConstraintUpdate;

        /// <summary>
        /// Raises the event which updates the members of <see cref="LinearisedConstraintType"/>.
        /// </summary>
        private void OnLinearisedConstraintUpdate()
        {
            LinearisedConstraintUpdate?.Invoke();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Collection of variables of this <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        /// <remarks> The use of the <see cref="HashSet{T}"/> collection allows for efficient search, but does not provide indexing. </remarks>
        private readonly HashSet<Variable> _variables;

        /// <summary>
        /// List of energies of this <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        /// <remarks> The use of the <see cref="HashSet{T}"/> collection allows for efficient search, but does not provide indexing. </remarks>
        private readonly HashSet<Energy> _energies;

        /// <summary>
        /// List of constraints of this <see cref="GuidedProjectionAlgorithm"/>.
        /// <remarks> The use of the <see cref="HashSet{T}"/> collection allows for efficient search, but does not provide indexing. </remarks>
        /// </summary>
        private readonly HashSet<Constraint> _constraints;


        /// <summary>
        /// Vector containing the variables of this <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private DenseVector _x;


        // ---------- Utilities ---------- //

        /// <summary>
        /// Identity matrix multiplied by Epsilon*Epsilon.
        /// </summary>
        private CompressedColumn _epsEpsIdentity;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of components in the global vector X of this <see cref="GuidedProjectionAlgorithm"/>.  
        /// </summary>
        public int ComponentCount { get; private set; } = 0;

        /// <summary>
        /// Gets the number of energies of this <see cref="GuidedProjectionAlgorithm"/>.  
        /// </summary>
        public int EnergyCount => _energies.Count;

        /// <summary>
        /// Gets the number of constraints of this <see cref="GuidedProjectionAlgorithm"/>.  
        /// </summary>
        public int ConstraintCount => _constraints.Count;

        // ---------- Settings ---------- //

        /// <summary>
        /// Gets or sets the maximum number of iteration after which the solver is stopped.
        /// </summary>
        public int MaxIteration { get; set; }

        /// <summary>
        /// Gets index of the current iteration.
        /// </summary>
        /// <remarks> Zero means that no iteration was runned. </remarks>
        public int Iteration { get; internal set; }


        // ---------- For Solving ---------- //

        /// <summary>
        /// Gets or sets the weight of the distance to the previous iteration.
        /// </summary>
        public double Epsilon { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="GuidedProjectionAlgorithm"/> class.
        /// </summary>
        /// <param name="epsilon"> The weights of the distance to the previous iteration. </param>
        /// <param name="maxIteration"> The iteration index after which the solver is stopped. </param>
        public GuidedProjectionAlgorithm(double epsilon, int maxIteration)
        {
            // Instanciate Fields
            _variables = new HashSet<Variable>();
            _energies = new HashSet<Energy>();
            _constraints = new HashSet<Constraint>();

            // Initialize Properties
            MaxIteration = maxIteration;
            Iteration = 0;

            Epsilon = epsilon;
        }

        #endregion

        #region Methods

        // ---------- For Variables ---------- //

        /// <summary>
        /// Creates a variable from its components and adds it to this model.
        /// </summary>
        /// <param name="components"> Values of the variable's components. </param>
        /// <returns> The newly added <see cref="Variable"/>. </returns>
        public Variable AddVariable(params double[] components)
        {
            Variable variable = new Variable(components);

            _variables.Add(variable);
            ComponentCount += variable.Dimension;

            return variable;
        }


        /// <summary>
        /// Attempts to add the specified variable to this model.
        /// </summary>
        /// <param name="variable"> Variable to add. </param>
        /// <returns> <see langword="true"/> if the variable was added, <see langword="false"/> if the variable already exists in the model. </returns>
        public bool TryAddVariable(Variable variable)
        {
            if(_variables.Contains(variable)) // Complexity : O(1) 
            { 
                return false; 
            }
            else
            {
                _variables.Add(variable);
                ComponentCount += variable.Dimension;

                return true;
            }
        }


        // ---------- For Energies ---------- //

        /// <summary>
        /// Creates a new <see cref="Energy"/> with a constant weight and adds it to this model.
        /// </summary>
        /// <param name="energyType"> Energy type defining the local quantities of the energy. </param>
        /// <param name="variables"> 
        /// Variables composing the local vector localX.
        /// <para> See the <paramref name="energyType"/> class description to learn about the expected variables. The order of the variables matters.</para>
        /// </param>
        /// <param name="weight"> Weight of the energy. </param>
        public void AddEnergy(EnergyType energyType, IReadOnlyList<Variable> variables, double weight = 1.0)
        {
            Energy energy = new Energy(energyType, variables, weight);
            _energies.Add(energy);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> with a varying weight and adds it to this model.
        /// </summary>
        /// <param name="energyType"> Energy type defining the energy locally. </param>
        /// <param name="variables"> 
        /// Variables composing the local vector localX.
        /// <para> See the <paramref name="energyType"/> class description to learn about the expected variables. The order of the variables matters.</para>
        /// </param>
        /// <param name="weightFunction"> Function computing the weight from the iteration index. </param>
        public void AddEnergy(EnergyType energyType, IReadOnlyList<Variable> variables, Func<int,double> weightFunction)
        {
            Energy energy = new Energy(energyType, variables, 0.0);
            _energies.Add(energy);

            void energyWeightUpdater(int iteration) => energy.Weight = weightFunction(iteration);
            WeigthUpdate += energyWeightUpdater;
        }


        /// <summary>
        /// Attempts to add the specified energy to this model.
        /// </summary>
        /// <param name="energy"> Energy to add. </param>
        /// <returns> <see langword="true"/> if the energy was added, <see langword="false"/> if the energy already exists in the model. </returns>
        public bool TryAddEnergy(Energy energy)
        {
            if (_energies.Contains(energy)) // Complexity : O(1) 
            {
                return false;
            }
            else
            {
                _energies.Add(energy);

                return true;
            }
        }

        // ---------- For Quadratic Constraints ---------- //

        /// <summary>
        /// Creates a new <see cref="Constraint"/> with a constant weight and adds it to this model.
        /// </summary>
        /// <param name="constraintType"> Constraint type defining the constraint locally. </param>
        /// <param name="variables"> 
        /// Variables composing the local vector localX.
        /// <para> See the <paramref name="constraintType"/> class description to learn about the expected variables. The order of the variables matters.</para>
        /// </param>
        /// <param name="weight"> Weight for the constraint. </param>
        public void AddConstraint(ConstraintType constraintType, IReadOnlyList<Variable> variables, double weight = 1.0)
        {
            Constraint constraint = new Constraint(constraintType, variables, weight);
            _constraints.Add(constraint);

            if (constraintType is LinearisedConstraintType linearisedType)
            {
                LinearisedConstraintUpdate += () => linearisedType.UpdateLocal(variables); ;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Constraint"/> with a varying weight and adds it to this model.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> 
        /// Variables composing the local vector localX.
        /// <para> See the <paramref name="constraintType"/> class description to learn about the expected variables. The order of the variables matters.</para>
        /// </param>
        /// <param name="weightFunction"> Function computing the weight from the iteration index. </param>
        public void AddConstraint(ConstraintType constraintType, IReadOnlyList<Variable> variables, Func<int, double> weightFunction)
        {
            Constraint constraint = new Constraint(constraintType, variables, 0.0);
            _constraints.Add(constraint);

            void constraintWeightUpdater(int iteration) => constraint.Weight = weightFunction(iteration);
            WeigthUpdate += constraintWeightUpdater;

            if (constraintType is LinearisedConstraintType linearisedType)
            {
                LinearisedConstraintUpdate += () => linearisedType.UpdateLocal(variables); ;
            }
        }


        /// <summary>
        /// Attempts to add the specified constraint to this model.
        /// </summary>
        /// <param name="constraint"> Constraint to add. </param>
        /// <returns> <see langword="true"/> if the constraint was added, <see langword="false"/> if the constraint already exists in the model. </returns>
        public bool TryAddConstraint(Constraint constraint)
        {
            if (_constraints.Contains(constraint)) // Complexity : O(1) 
            {
                return false;
            }
            else
            {
                _constraints.Add(constraint);

                return true;
            }
        }

        // ---------- For Solving ---------- //

        /// <summary>
        /// Initialises the members of this <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        public void InitialiseX()
        {
            // ----- Global vector X ----- //

            double[] array = new double[ComponentCount];    // ComponentCount is updated

            int i_Comp = 0;

            foreach (Variable variable in _variables)
            {
                // Set the components in the global vector X
                for (int i_VarComp = 0; i_VarComp < variable.Dimension; i_VarComp++)
                {
                    array[i_Comp + i_VarComp] = variable[i_VarComp];
                }

                // Change the reference in the variable to point toward a segment of the global vector X
                variable.ChangeReference(array, i_Comp);

                
                i_Comp += variable.Dimension;
            }

            _x = new DenseVector(ref array);

            // ----- Utilities ----- //

            _epsEpsIdentity = CompressedColumn.Multiply(Epsilon * Epsilon, CompressedColumn.Identity(_x.Size));
        }

        /// <summary>
        /// Runs one iteration.
        /// </summary>
        /// <param name="useAsync"> Evaluates whether the iteration should use asynchronous programming or not. </param>
        public void RunIteration(bool useAsync)
        {
            // ----- Iteration Updates ----- //

            OnLinearisedConstraintUpdate();

            OnWeigthUpdate(Iteration);


            // ----- Formulate and Solve the System ----- //

            DenseVector x;
            if (useAsync)
            {
                var task = FormAndSolveSystem_Async();

                Task.WaitAll(task);

                x = task.Result;
            }
            else
            {
                x = FormAndSolveSystem();
            }


            // ----- Update Variables ----- //

            // Fill the _x with the actualised values
            for (int i = 0; i < x.Size; i++) { _x[i] = x[i]; }
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Computes the members of the system and solve it using Cholesky factorisation.
        /// </summary>
        /// <returns> The solution of the system. </returns>
        private DenseVector FormAndSolveSystem()
        {
            /******************** Iterate on the quadratic constraints to create H and r ********************/

            FormConstraintMembers(out CompressedColumn H, out DenseVector r);


            /******************** Iterate on the energies to create K and s ********************/

            FormEnergyMembers(out CompressedColumn K, out SparseVector s);


            /******************** Solve the minimisation problem ********************/

            CompressedColumn LHS; // Left hand side of the equation
            DenseVector RHS; // Right hand side of the equation

            if (H.NonZerosCount != 0 && K.NonZerosCount != 0)
            {
                CompressedColumn HtH = CompressedColumn.TransposeMultiplySelf(H);
                CompressedColumn KtK = CompressedColumn.TransposeMultiplySelf(K);
                
                LHS = CompressedColumn.Add(HtH, KtK);
                RHS = DenseVector.Add(CompressedColumn.TransposeMultiply(H, r), CompressedColumn.TransposeMultiply(K, s));
            }
            else
            {
                if (H.NonZerosCount != 0)
                {
                    LHS = CompressedColumn.TransposeMultiplySelf(H);
                    RHS = CompressedColumn.TransposeMultiply(H, r);
                }
                else if (K.NonZerosCount != 0)
                {
                    DenseVector tmp = new DenseVector(s.ToArray());

                    LHS = CompressedColumn.TransposeMultiplySelf(K);
                    RHS = CompressedColumn.TransposeMultiply(K, tmp);
                }
                else { throw new InvalidOperationException("The matrices H and K are empty."); }
            }

            LHS = CompressedColumn.Add(LHS, _epsEpsIdentity);
            RHS = DenseVector.Add(RHS, DenseVector.Multiply(Epsilon * Epsilon, _x));

            return LHS.SolveCholesky(RHS);
        }

        /// <summary>
        /// Computes the members of the system and solve it using Cholesky factorisation.
        /// </summary>
        /// <returns> The solution of the system. </returns>
        private async Task<DenseVector> FormAndSolveSystem_Async()
        {
            CompressedColumn LHS;   // Left hand side of the equation
            Vector RHS;        // Right hand side of the equation


            var task_FormConstraintMembers = FormConstraintMembers();
            var task_FormEnergyMembers = FormEnergyMembers();

            Task<(CompressedColumn Matrix, Vector Vector)> task_FormSomeMembers;
            Task<CompressedColumn> task_LHS; Task<Vector> task_RHS;

            Task finishedTask = await Task.WhenAny(task_FormConstraintMembers, task_FormEnergyMembers);
            if (finishedTask == task_FormConstraintMembers)
            {
                (CompressedColumn HtH, Vector Htr) = task_FormConstraintMembers.Result;

                task_LHS = Task.Run(() => CompressedColumn.Add(HtH, _epsEpsIdentity));
                task_RHS = Task.Run(() => DenseVector.Add(Htr, DenseVector.Multiply(Epsilon * Epsilon, _x)) as Vector);

                task_FormSomeMembers = task_FormEnergyMembers;
            }
            else if(finishedTask == task_FormEnergyMembers)
            {
                (CompressedColumn KtK, Vector Kts) = task_FormEnergyMembers.Result;

                task_LHS = Task.Run(() => CompressedColumn.Add(KtK, _epsEpsIdentity));
                task_RHS = Task.Run(() => DenseVector.Add(Kts, DenseVector.Multiply(Epsilon * Epsilon, _x)) as Vector);
                 
                task_FormSomeMembers = task_FormConstraintMembers;
            }
            else { throw new Exception("The finished task does not correspond to one of the supplied task."); }


            (CompressedColumn matrix, Vector vector) = await task_FormSomeMembers;


            List<Task> activeTasks = new List<Task> { task_LHS, task_RHS };
            while (activeTasks.Count > 0)
            {
                finishedTask = await Task.WhenAny(activeTasks);

                if (finishedTask == task_LHS)
                {
                    activeTasks.Remove(task_LHS);

                    LHS = task_LHS.Result;

                    task_LHS = Task.Run(() => CompressedColumn.Add(LHS, matrix));
                }
                else if (finishedTask == task_RHS)
                {
                    activeTasks.Remove(task_RHS);

                    RHS = task_RHS.Result;

                    task_RHS = Task.Run(() => DenseVector.Add(RHS, vector));
                }
                else { throw new Exception("The finished task does not correspond to one of the supplied task."); }
            }

            Task.WaitAll(task_LHS, task_RHS);

            LHS = task_LHS.Result;
            RHS = task_RHS.Result;

            return LHS.SolveCholesky(RHS);
        }

        #endregion


        #region Helper - Synchronous

        /// <summary>
        /// Forms the global system members derived from the constraints.
        /// </summary>
        /// <param name="H"> The global matrix H. </param>
        /// <param name="r"> The global vector r. </param>
        private void FormConstraintMembers(out CompressedColumn H, out DenseVector r)
        {
            DictionaryOfKeys dok_H = new DictionaryOfKeys(10 * _constraints.Count /* Random */);
            List<double> list_r = new List<double>(_constraints.Count);

            int constraintCount = 0;

            foreach (Constraint cstr in _constraints)
            {
                // Verifications
                if (cstr.Weight == 0d) { continue; }


                IReadOnlyList<Variable> variables = cstr.Variables;
                ConstraintType constraintType = cstr.Type;

                int localSize = constraintType.LocalHi.ColumnCount;

                // ----- Create the Row Indices ----- //

                // Translate the local indices of the constraint defined on localX into global indices defined on X.
                List<int> rowIndices = new List<int>(localSize);
                for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
                {
                    Variable variable = variables[i_Variable];
                    int offset = variable.Offset;
                    for (int i_VarComp = 0; i_VarComp < variable.Dimension; i_VarComp++) 
                    { 
                        rowIndices.Add(offset + i_VarComp); 
                    }
                }

                // ----- Create localX ----- //

                double[] components = new double[localSize];
                for (int i_Comp = 0; i_Comp < localSize; i_Comp++)
                {
                    components[i_Comp] = _x[rowIndices[i_Comp]];
                }
                DenseVector localX = new DenseVector(components);


                // ----- Compute Temporary Values ----- //

                // Compute HiX
                DenseVector tmp_Vect = SparseMatrix.Multiply(constraintType.LocalHi, localX);

                // Compute XtHiX
                double tmp_Val = DenseVector.TransposeMultiply(localX, tmp_Vect);


                // ----- For r ----- //

                if (constraintType.Ci == 0.0) { list_r.Add(cstr.Weight * 0.5 * tmp_Val); }
                else { list_r.Add(cstr.Weight * (0.5 * tmp_Val - constraintType.Ci)); }


                // ----- For H ----- //

                if (!(constraintType.LocalBi is null))
                {
                    tmp_Vect = DenseVector.Add(tmp_Vect, constraintType.LocalBi);
                }

                for (int i_Comp = 0; i_Comp < localSize; i_Comp++)
                {
                    if (tmp_Vect[i_Comp] == 0d) { continue;  }
                    dok_H.Add(cstr.Weight * tmp_Vect[i_Comp], constraintCount, rowIndices[i_Comp]);
                }

                constraintCount++;
            }


            H = new CompressedColumn(constraintCount, _x.Size, dok_H);
            r = new DenseVector(list_r.ToArray());
        }

        /// <summary>
        /// Forms the global system members derived from the energies.
        /// </summary>
        /// <param name="K"> The global matrix K. </param>
        /// <param name="s"> The global vector s. </param>
        private void FormEnergyMembers(out CompressedColumn K, out SparseVector s)
        {
            DictionaryOfKeys dok_K = new DictionaryOfKeys(10 * _energies.Count /* Random */);
            Dictionary<int, double> dict_s = new Dictionary<int, double>(_energies.Count);

            int energyCount = 0;
            foreach (Energy energy in _energies)
            {
                // Verifications
                if (energy.Weight == 0d) { continue; }

                IReadOnlyList<Variable> variables = energy.Variables;
                EnergyType energyType = energy.Type;

                int localSize = energyType.LocalKi.Size;

                // ----- Create the Row Indices ----- //

                // Translating the local indices of the constraint defined on localX into global indices defined on X.
                List<int> rowIndices = new List<int>(localSize);
                for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
                {
                    Variable variable = variables[i_Variable];
                    int offset = variable.Offset;
                    for (int i_VarComp = 0; i_VarComp < variable.Dimension; i_VarComp++)
                    {
                        rowIndices.Add(offset + i_VarComp);
                    }
                }

                // ----- For s ----- //

                if (!(energyType.Si == 0.0))
                {
                    dict_s.Add(energyCount, energy.Weight * energyType.Si);
                }

                // ----- For K ----- //

                foreach ((int rowIndex, double value) in energyType.LocalKi.NonZeros())
                {
                    dok_K.Add(energy.Weight * value, energyCount, rowIndices[rowIndex]);
                }

                energyCount++;
            }

            K = new CompressedColumn(energyCount, _x.Size, dok_K);
            s = new SparseVector(energyCount, ref  dict_s);
        }

        #endregion

        #region Helpers - Asynchronous 

        // ---------- Constraint Members ---------- //

        /// <summary>
        /// Forms the system members derived from the constraints.
        /// </summary>
        private async Task<(CompressedColumn HtH, Vector Htr)> FormConstraintMembers()
        {
            return await Task.Run(() =>
            {
                System.Collections.Concurrent.ConcurrentBag<(SortedDictionary<int, double> ColumnHt, double ValueR)> bag 
                    = new System.Collections.Concurrent.ConcurrentBag<(SortedDictionary<int,double> ColumnHt, double ValueR)>();

                Parallel.ForEach(_constraints, (Constraint cstr) =>
                {
                    // Verifications
                    if (cstr.Weight == 0d) { return; }


                    IReadOnlyList<Variable> variables = cstr.Variables;
                    ConstraintType constraintType = cstr.Type;

                    int localSize = constraintType.LocalHi.ColumnCount;

                    // ----- Devise LocalX ----- //

                    DenseVector localX = DeviseLocalX(localSize, variables, out int[] rowIndices);


                    // ----- Compute Temporary Values ----- //

                    // Compute HiX
                    DenseVector tmp_Vect = SparseMatrix.Multiply(constraintType.LocalHi, localX);

                    // Compute XtHiX
                    double tmp_Val = DenseVector.TransposeMultiply(localX, tmp_Vect);


                    // ----- For r ----- //

                    double valueR = 0d;
                    if (constraintType.Ci == 0.0) { valueR = cstr.Weight * 0.5 * tmp_Val; }
                    else { valueR = cstr.Weight * (0.5 * tmp_Val - constraintType.Ci); }


                    // ----- For Ht ----- //


                    if (!(constraintType.LocalBi is null))
                    {
                        tmp_Vect = DenseVector.Add(tmp_Vect, constraintType.LocalBi);
                    }
                    Dictionary<int, double> components = new Dictionary<int, double>(localSize);
                    for (int i_Comp = 0; i_Comp < localSize; i_Comp++)
                    {
                        if (tmp_Vect[i_Comp] == 0d) { continue; }
                        components.Add(rowIndices[i_Comp], cstr.Weight * tmp_Vect[i_Comp]);
                    }
                    SortedDictionary<int, double> columnHt = new SortedDictionary<int, double>(components);

                    // ----- Finally ----- //

                    bag.Add((columnHt, valueR));
                });


                (CompressedColumn Ht, DenseVector r) = AssembleConstraintMembers(_x.Size, bag);

                (CompressedColumn HtH, DenseVector Htr) = MultiplyConstraintMembers(Ht, r);

                return (HtH, Htr as Vector);
            });
        }


        /// <summary>
        /// Devises the component of LocalX.
        /// </summary>
        /// <param name="size"> Size of LocalX. </param>
        /// <param name="variables"> Variables contained in LocalX. </param>
        /// <param name="rowIndices"> The row indices of the components composing LocalX. </param>
        /// <returns> The dense vector LocalX. </returns>
        private DenseVector DeviseLocalX(int size, IReadOnlyList<Variable> variables, out int[] rowIndices)
        {
            // ----- Create the Row Indices ----- //

            // Translating the local indices of the constraint defined on LocalX into global indices defined on X.
            rowIndices = new int[size];

            int counter = 0;
            for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
            {
                Variable variable = variables[i_Variable];
                int offset = variable.Offset;
                for (int i_VarComp = 0; i_VarComp < variable.Dimension; i_VarComp++)
                {
                    rowIndices[counter] = offset + i_VarComp;
                    counter++;
                }
            }


            // ----- Create LocalX ----- //

            double[] components = new double[size];
            for (int i_Comp = 0; i_Comp < size; i_Comp++)
            {
                components[i_Comp] = _x[rowIndices[i_Comp]];
            }

            return new DenseVector(components);
        }


        /// <summary>
        /// Assembles the data to create the tranposed matrix Ht and the vector r.
        /// </summary>
        /// <param name="size"> Size of the global vector x. </param>
        /// <param name="bag"> Collection containing the components of the constraint members. </param>
        /// <returns> A pair containing the tranposed matrix Ht and the vector s. </returns>
        private (CompressedColumn Ht, DenseVector r) AssembleConstraintMembers(int size, 
            System.Collections.Concurrent.ConcurrentBag<(SortedDictionary<int, double> ColumnHt, double ValueR)> bag)
        {
            List<int> columnPointers = new List<int>();
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            List<double> list_r = new List<double>();

            columnPointers.Add(0);
            foreach ((SortedDictionary<int, double> ColumnHt, double ValueR) in bag)
            {
                foreach (KeyValuePair<int, double> component in ColumnHt)
                {
                    rowIndices.Add(component.Key);
                    values.Add(component.Value);
                }
                columnPointers.Add(values.Count);

                list_r.Add(ValueR);
            }

            CompressedColumn Ht = new CompressedColumn(size, bag.Count, columnPointers.ToArray(), rowIndices.ToArray(), values.ToArray());
            DenseVector r = new DenseVector(list_r.ToArray());

            return (Ht, r);
        }

        /// <summary>
        /// Multiplies the assembled constraint members to form the system members
        /// </summary>
        /// <param name="Ht"> The tranposed matrix Ht. </param>
        /// <param name="r"> The vector r. </param>
        /// <returns> A pair containing the necessary system members. </returns>
        private (CompressedColumn HtH, DenseVector Htr) MultiplyConstraintMembers(CompressedColumn Ht, DenseVector r)
        {
            Task<CompressedColumn> task_HtH = Task.Run(() =>
            {
                CompressedColumn H = CompressedColumn.Transpose(Ht);
                return CompressedColumn.Multiply(Ht, H);
            });

            Task<DenseVector> task_Htr = Task.Run(() =>
            {
                return CompressedColumn.Multiply(Ht, r);
            });

            Task.WaitAll(task_Htr, task_HtH);

            CompressedColumn HtH = task_HtH.Result;
            DenseVector Htr = task_Htr.Result;

            return (HtH, Htr);
        }


        // ---------- Energy Members ---------- //

        /// <summary>
        /// Forms the system members derived from the energies.
        /// </summary>
        private async Task<(CompressedColumn KtK, Vector Kts)> FormEnergyMembers()
        {
            return await Task.Run(() => 
            {
                System.Collections.Concurrent.ConcurrentBag<(SortedDictionary<int, double> ColumnKt, double ValueS)> bag 
                    = new System.Collections.Concurrent.ConcurrentBag<(SortedDictionary<int, double> ColumnKt, double ValueS)>();

                Parallel.ForEach(_energies, (Energy energy) =>
                {
                    // Verifications
                    if (energy.Weight == 0d) { return; }

                    IReadOnlyList<Variable> variables = energy.Variables;
                    EnergyType energyType = energy.Type;

                    int localSize = energyType.LocalKi.Size;

                    // ----- Create the Row Indices ----- //

                    // Translating the local indices of the constraint defined on LocalX into global indices defined on x.
                    List<int> rowIndices = new List<int>();
                    for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
                    {
                        Variable variable = variables[i_Variable];
                        int offset = variable.Offset;
                        for (int i_VarComp = 0; i_VarComp < variable.Dimension; i_VarComp++)
                        {
                            rowIndices.Add(offset + i_VarComp);
                        }
                    }

                    // ----- For s ----- //

                    double valueS = 0.0;
                    if (!(energyType.Si == 0.0))
                    {
                        valueS = energy.Weight * energyType.Si;
                    }

                    // ----- For Kt ----- //

                    Dictionary<int, double> components = new Dictionary<int, double>(localSize);
                    foreach ((int rowIndex, double value) in energyType.LocalKi.NonZeros())
                    {
                        components.Add(rowIndices[rowIndex], energy.Weight * value);
                    }

                    SortedDictionary<int, double> ColumnKt = new SortedDictionary<int, double>(components);

                    // ----- Finally ----- //

                    bag.Add((ColumnKt, valueS));
                });


                (CompressedColumn Kt, SparseVector s) = AssembleEnergyMembers(_x.Size, bag);
                
                (CompressedColumn KtK, SparseVector Kts) = MultiplyEnergyMembers(Kt, s);

                return (KtK, Kts as Vector);
            });
        }


        /// <summary>
        /// Assembles the data to create the tranposed matrix Kt and the vector s.
        /// </summary>
        /// <param name="size"> Size of the global vector x. </param>
        /// <param name="bag"> Collection containing the components of the constraint members. </param>
        /// <returns> A pair containing the tranposed matrix Kt and the vector s. </returns>
        private (CompressedColumn Kt, SparseVector s) AssembleEnergyMembers(int size, 
            System.Collections.Concurrent.ConcurrentBag<(SortedDictionary<int, double> ColumnKt, double ValueS)> bag)
        {
            List<int> columnPointers = new List<int>();
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            Dictionary<int, double> dict_s = new Dictionary<int,double>();

            columnPointers.Add(0);
            int counter = 0;
            foreach ((SortedDictionary<int, double> ColumnKt, double ValueS) in bag)
            {
                foreach (KeyValuePair<int, double> component in ColumnKt)
                {
                    rowIndices.Add(component.Key);
                    values.Add(component.Value);
                }
                columnPointers.Add(values.Count);

                if (ValueS != 0.0) { dict_s.Add(counter, ValueS); }
                

                counter++;
            }

            CompressedColumn Kt = new CompressedColumn(size, bag.Count, columnPointers.ToArray(), rowIndices.ToArray(), values.ToArray());
            SparseVector s = new SparseVector(bag.Count, ref dict_s);

            return (Kt, s);
        }

        /// <summary>
        /// Multiplies the assembled energy members to form the system members
        /// </summary>
        /// <param name="Kt"> The tranposed matrix Kt. </param>
        /// <param name="s"> The vector s. </param>
        /// <returns> A pair containing the necessary system members. </returns>
        private (CompressedColumn KtK, SparseVector Kts) MultiplyEnergyMembers(CompressedColumn Kt, SparseVector s)
        {
            Task<CompressedColumn> task_KtK = Task.Run(() =>
            {
                CompressedColumn K = CompressedColumn.Transpose(Kt);
                return CompressedColumn.Multiply(Kt, K);
            });

            Task<SparseVector> task_Kts = Task.Run(() =>
            {
                return CompressedColumn.Multiply(Kt, s);
            });

            Task.WaitAll(task_KtK, task_Kts);

            CompressedColumn KtK = task_KtK.Result;
            SparseVector Kts = task_Kts.Result;

            return (KtK, Kts);
        }

        #endregion
    }
}
