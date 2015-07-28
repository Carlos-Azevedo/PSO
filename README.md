# PSO
Multiple Particle Swarm Optimization algorithms in C# with unit tests

To use this .dll just create a PSO Swarm of the type you want, ClassicSwarm, StableSwarm, InertiaSwarm or Frankenstein Swarm.

Each swarm has a specific PSO.Parameters type associated with its creation which contains all the required attributes.

It is important to note that the NumberOfParticleSets value in the creation parameters is used to iterate over the particles
in concurrently. If the SolutionFunction being evaluated is not thread-safe this must be set to 1.
