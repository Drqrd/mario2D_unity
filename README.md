<h1> New Super Mario Bros. Clone in Unity3d </h1>
Author: Justin D'Errico  <br>
Created: June, 2021 - Present <br>

<h2> Goal </h2>
The purpose of this project was to increase the complexity of the game from my first snake clone project. This was to develop my OOP skills, and make proper use of Unity3d components such as prefabs, animation clips, colliders, etc.
 
<h2> Interesting Aspects </h2>
<h3> UpdateInteractables.cs </h3>
<h4> Problem </h4>
In Unity3d, when you try and create a flat plane of multiple box colliders and run another box collider representing the player over top of it, the player box collider gets caught on the edges of each box collider
<h4> Solution </h4>
UpdateInteractables.cs contains scripts for creating a dynamic box collider to circumvent this issue. For instance, if there is a row of breakable blocks and Mario breaks one of the blocks, the original box collider will be adjusted and a new one will be generated to create two new colliders encompassing the remaining blocks. <br>
<p float="center">
  <img src="README Additions/Interactables_1.PNG" alt="Dynamic Collider Unbroken" width=45%> 
  <img src="README Additions/Interactables_2.PNG" alt="Dynamic Collider Broken" width=45%> 
</p>  
If the block is not breakable, the nearest block to the collision on the collider is found and plays a bump animation and/or sound as well.

<h2> Flaws </h2>
<h3> Function Cohesion and Coupling, Naming Conventions </h3>
The current function names, cohesion and scope can use some improvements. Due to how deep I am into this project, overhauling all the names and structuring functions properly would take 
more time than what its worth, but I am keeping this in mind for future projects.
<h3> Lack of Proper Documentation </h3>
Like the previous flaw, I have noticed that while I do add comments to my code, they are not comprehensive enough. Will also be addressed in future projects.
<h3> <strike> Poor GitHub Commit Practices </strike> </h3>
See Notes

<h2> Notes </h2>
<h3> GitHub Commits </h3>
Larger number of commits compared to previous projects stems from better practices / documentation regarding commits. <br>
No longer will I dump every change into one commit! I will target any issues from previous commits with greater precision forevermore!
