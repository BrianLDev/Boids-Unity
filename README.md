# Boids

[Click here to check out the simulation on itch.io](https://torchfire-games.itch.io/boids)

This is my implementation of the famous "Boids" flocking algorithm, which was developed by Craig Reynolds in 1986 (link to Craig's website).

The boids--which happen to be fish in this implementation--follow 3 relatively simple rules: 

- **Separation** (avoid collisions)
- **Alignment** (match velocity of local boids)
- **Cohesion** (move towards the center of local flock

Yet out of these simple rules, seemingly complex and emergent behavior can develop, and the boids tend to form flocks and fly/swim in dynamic ways.

-----------------------

Sit back and relax, watching the fish boids swim around and form flocks or break off and go solo.  Or change the settings using the sliders to see how it affects their behavior.

Hit escape to bring up the respawn menu if needed.

-----------------------

Note: this version implements boids on an individual basis, i.e. each boid is a separate object/class running the functions of separation, alignment, and cohesion independently.  While this is a very clear and understandable way to implement the algorithm, it is also less efficient as each object needs to run separate updates, and data is spread out over memory which creates additional overhead.

For future versions, I plan to implement 3 other simulation methods, each increasingly more efficient, which will clearly show the performance differences.

Additional simulation methods to be implemented in the future:

- **Boid Manager (fast)** - A centralized manager will control all data related to the boids in one class, using only one update method, and storing data in arrays for improved efficiency. 
- **Boid Manager using C# Job System & Burst Compiler (faster)** - In addition to using  a centralized boid manager, this method will use the C# Job system along with Unity's burst compiler to drastically improve expensive computations like finding nearest neighbors.
- **Boid Manager using Unity's Entity Component System & C# Job System (fastest)** - this method will be implemented using Unity's Entity Component System (ECS), which is a data-oriented paradigm and completely different from the object oriented paradigm used in previous methods.