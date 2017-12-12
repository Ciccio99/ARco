# ARco
AR procedural glass parabola generation between anchor points in space.

### Class CSCI - 716

### Team
- Alberto Scicali

# Final Report

### Description

This application tackles several issues in an augmented reality (AR) application. The application will allow the user to procedurally generate arc/bezier curves in AR - These arcs must be generated/tesselated on the fly depending on the start and end points of the arc. Furthermore, the arcs will simulate a glass material and as well as simulating a triangulated "shattering" effect when a user moves the camera towards a generated arc. Finally, 3D vornoi diagrams are explored through shaders to be added to to the arcs in the future.

### Detailed Application Background

As described above, this application falls under an artistic/game category which explores shaders, tesselation and voronoi diagrams. These problems are tackled in distinct ways, the "canvas" used to demonstrate these problems/solutions is an interactive AR application that allows the users to proceduraly generate glass, low-poly, arcs in their environment. 

Here are the ways in which these problems were tackled:

- Bezier curve calculations to create the points in which to tesselate along.
- Interactive Unity Editory components to determine the number of faces per "ring" of the arc, also know as the "smoothness".
- Tesselation between psuedo-randomly generated points along the bezier curve to create an abstract look to the arc.
- Applying a "glass" shader to the arc. This is done by taking the background texture of the camera, performing a refraction and  ray cast calculation from the vertex of the arc, and then sampling the camera's texture.
- The shattering affect is created throug geometry shaders. The tesselation triangles are shifted along their face normals by a certain distance (determined by the distance between the camera and the vertex). Unfortunately, geometry shaders are currently not available on Apple's Metal 2 Graphics API, which means this could not be visually present on the mobile application.
- 3D Voronoi diagrams are a complex computation geometry problem that can be simplified to a great degree through shaders. A shader, given a set of voronoi seeds in 3D space, can color any geometries surface depending on which seed a fragment points is closest too. Furthermore, these distance calculations can be performed with Euclidean and Manhattan distance calculations, resulting in varying 3D voronoi shapes.
(INSERT IMAGES OF DIFF DISTANCES)

![Alt text](/EuclideanVoronoi.png "Euclidean Distance 3D Voronoi Shader")
![Alt text](/manhattenVoronoi.png "Manhatten Distance 3D Voronoi Shader")

#### Application Example Video

https://youtu.be/KvpYeQTgOtk

#### Needs of the Problem Domain

- Tesselation of user adjustable geometry: this is used heavily in games, animation and modeling. Creating simple tools that allow users to quickly and easily create these dynamic shapes allow the user to efficiently proceed with their work.
- Glass Material in AR: As AR becomes more ubiqitous in our day to day lives, it'll become necessary to render 3D geometry that "fits" into a user's augmented space.
- 3D Voronoi Diagram: This is used in games and animation, especially in creating a destructible object. Voronoi diagram can help simulate the dustruction of geomtry. This process needs an optimal solution in order to quickly calculate destruction geometry, however, 3D voronoi diagram can be presented through shaders as well. This can be used to create abstract looking virtual objects or to create create spatial maps depending on voronoi seeds.

#### Related Problems/Applications
- There are other ARkit applications that make use of similar camera feed textures to recreate glass materials and objects that blend in with their environment, such as a chameleon. Here is an example video where a virtual chameleon is colored by the input of the camera feed:
    - https://www.youtube.com/watch?v=c0tlFw_0OEs

- 3D voronoi simulations have been tackled by numerous people with a variety of solutions, here is a list of a few references:
    - http://ieeexplore.ieee.org/document/7166162/
    - https://dl.acm.org/citation.cfm?id=311567
    - http://www.sciencedirect.com/science/article/pii/S0925772101000566
    - http://www.sciencedirect.com/science/article/pii/S0010448505000436

#### Application Example Videos
- Bezier Arc Tesselation
    - https://youtu.be/Nrab9OrbEqY
- Shatter Geometry Shader
    - https://youtu.be/b4jXso90uIc
- 3D Voronoi Shader
    - https://youtu.be/bJdkSebXGtI

#### Complexity Analysis

Due to the processing power of modern GPUs, the time complexity of these algorithms is negligble. Unity places a maximum vertex count of 65,000 on a single object. Before hitting this vertex limit my arc generation system would take ~21 milliseconds to generate. Furthermore, the current fragment shader based 3D voronoi diagram runs on the GPU and is difficult to measure the time complexity of a shader pass.


# Initial Propsal

### Summary
- Create an Augmented Reality (AR) app that allows the user to create glass arcs between AR anchor points in space
- The arcs will be a tesselated shape derived from a 3D vornoi diagram
- The arc will used procedurally generated seed points for the voronoi algorithm
- The arc will be interactable via "shattering" the arc into glass shards
- The shattering glass is also based on a vornoi algorithm

### Goals
- Demonstrate knowledge from the Computation Geometry Course
- Demonstrate knowldege of Voronoi Algorithms
- Allow user to create arcs between points in space
- Allow user to interact the with arcs
- Implement latest and relevant research involved in 3D vornoi algorithms and Augmented Reality

#### Stretch Goals
- Shattering and recombining glass shards

### System & Software
- MacOS & Windows 10
- Unity 3D 2017.1
- ARkit
- C#


### Papers/Research
- http://dl.acm.org/citation.cfm?id=2159623&CFID=806143792&CFTOKEN=51762423
- http://dl.acm.org/citation.cfm?id=1463504&CFID=806143792&CFTOKEN=51762423
- http://dl.acm.org/citation.cfm?id=2461934&CFID=806143792&CFTOKEN=51762423
