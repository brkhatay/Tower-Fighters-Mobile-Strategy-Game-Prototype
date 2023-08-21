# Tower-Fighters-Mobile-Strategy-Game-Prototype

This project was designed 2-player multiplayer game. With using Photon Network tools. Using Unity 2021.3.22f1.

<p align="center">
  <img src="https://github.com/brkhatay/Tower-Fighters-Mobile-Strategy-Game-Prototype/blob/ReadSourse/LOGO.png" alt="" width="600" height="300">
</p>

### Input System

Unity New Input System was used in this project and mobile devices were primarily designed.
  
### A* Path Finding
In this game, players control their characters with the A* (start) pathfinding algorithm. Grid is created according to the Renderer Mesh Bound of the Plane object in the ground part of the game. First, the Playable Character is selected, and then the desired point is selected, the path is calculated by going around the obstacles.

<p align="center">
  <img src="https://github.com/brkhatay/Tower-Fighters-Mobile-Strategy-Game-Prototype/blob/ReadSourse/A_Star.gif" alt="" width="600" height="300">
</p>

### Fight System
Every time the players collect 3 collectibles, the ball in front of the castle they are defending is fired and the counter tower is damaged. The appearance of the tower changes with each damage. The tower that is hit 6 times in total is destroyed. The player wins the game and 1 experience point.

<p align="center">
  <img src="https://github.com/brkhatay/Tower-Fighters-Mobile-Strategy-Game-Prototype/blob/ReadSourse/Fight.gif" alt="" width="600" height="300">
</p>

### Collectibles
A collectible is created by randomly selecting one of the unblocked points on the created Grid.
  
### Matchmaking
Players are primarily matched with players close to their own level. A random player is assigned that you are not a player close to your own level.

### Left Game
If any player leaves or closes the game, the other player returns to the main menu.

<p align="center">
  <img src="https://github.com/brkhatay/Tower-Fighters-Mobile-Strategy-Game-Prototype/blob/ReadSourse/LeftGame.gif" alt="" width="600" height="300">
</p>

### Game Play

<p align="center">
  <img src="https://github.com/brkhatay/Tower-Fighters-Mobile-Strategy-Game-Prototype/blob/ReadSourse/GamePlay.gif" alt="" width="600" height="500">
</p>

# End
### brkhatay@gmail.com

