# NSGA-II Multi-Objective Optimization Component for Grasshopper


## 1. Non-dominated Sorting Genetic Algorithm II (NSGA-II)

The NSGA-II is an improved version of the original NSGA algorithm (an extension of the simple Genetic Algorithm (GA) for Multiple-Objective Optimization) that uses a fast Non-Dominated Sorting approach to subdivide the population into Fronts, based on how many individuals they dominate over. In this algorithm, an initial population of solutions breeds to produce offspring in a process of Crossover and Mutation similar to the classic GA. The union of this initial parent population and its offspring is then sorted using a Non-Dominated Sorting approach into a hierarchy of sub-populations (fronts) based on the ordering of Pareto dominance. The next generation of parents is then selected from these sub-populations based on both rank and crowding distance, in order to promote both a good-performing and diverse front of non-dominated solutions (Source: [Deb et al., 2002](https://ieeexplore.ieee.org/document/996017)).

## 2. Implementation

<p align="center">
  <img src="NSGA_II/Images/Grasshopper Components.jpg" width="600">
</p>

<p align="center">Figure 1 - NSGA-II component workflow</p>

This project implements the NSGA-II algorithm as a custom component for Grasshopper (see Figure 1). Similar to other optimization components for Grasshopper (e.g. Galapagos, Octopus), the developed component receives as inputs a collection of gene sliders as well as a number of fitness/objectives for the optimization. Once the optimization process starts, the component changes the values of the gene sliders to the evolved gene values and waits for the solution to propagate these changes and recalculate the new fitnesses. This process is repeated iteratively until the optimization reaches its end or is stopped by the user. To visualize the results, a custom Graphical User Interface was implemented using Windows Forms that works both as an editor to set-up the optimization settings and visualize the results once the optimization concludes.

### Plug-in Interface 

<p align="center">
  <img src="NSGA_II/Images/NSGA-II Editor Interface.png" width="800">
</p>

<p align="center"> Figure 2 - NSGA-II Editor interface </p>

The plug-in's interface (see Figure 2) can be opened with a mouse double-click on the Grasshopper NSGA-II component. The interface is divided into 5 parts:

<b>1. POPULATION SIZE</b>

Defines the number of solutions per generation. Allows a maximum size of 500 solutions per generation.

<b>2. OPTIMIZATION OPTIONS</b>

The Stop Condition of the optimization can be selected as either a given maximum number of generations, a given maximum time duration, or both. The Run Optimization button starts the optimization process, while the Stop button stops it once the current generation has been completed. Once stopped, the Stop button can be used to Reset a new optimization.

<b>3. FITNESSES OBJECTIVES</b>

Shows the plugged fitnesses and allows the user to name them and give them an objective - Minimize or Maximize.

<b>4. OPTIMIZATION STATISTICS</b>

Statistics showing the settings and progression of the optimization process.

<b>5. PARETO CHART</b>

Shows the resulting solutions as points in a 2D chart. Solution History represents all of the solutions tested since the start of the optimization process, while Pareto Solutions shows the solutions found in the Non-Dominated Pareto-Front of the last generation.

## 3. Results

Once an optimization is complete, the results will show in the Pareto Chart.

<p align="center">
  <img src="NSGA_II/Images/Results Chart.png" width="800">
</p>

<p align="center">Figure 3 - Optimization results</p>



### Algorithm pseudocode reference

[Deb, K., Pratap, A., Agarwal, S., & Meyarivan, T. A. M. T. (2002). A fast and elitist multiobjective genetic algorithm: NSGA-II. IEEE transactions on evolutionary computation, 6(2), pp. 182-197.](https://ieeexplore.ieee.org/document/996017)
