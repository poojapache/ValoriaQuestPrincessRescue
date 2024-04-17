1. Start scene file: Scenes/StartScene

2. How to play
2a. Use WASD keys to move the player in the scene. 
2b. Collect collectibles like swords, keys, energy potions and gems throughout the maze. 
2c. You will need a specified number of keys to unlock each door.
2d. You will need a specified amount of energy level to defeat each ghost. If you come in contact with a ghost while not having enough energy, the game will end.
2e. The ghosts are AI controlled and run away from the player. The player needs to catch the ghosts to collect keys
2f. There are some dummy/fake doors in the maze. If you unlock such a door, the game ends.
2g. You need to be able to collect all the keys and defeat all the ghosts to open the golden door and win the game.

3. Known problem areas:
3a. Camera movement behind player - the camera offset needs to be perfected so that the skybox outside the maze is not visible
3b. Rigid body physics of the player- the player sometimes starts to float away after a strong collision
3c. Ghost AI glitch - sometimes requires time to move to state where it has to run away from the player

4. Manifest

Mackenzie Thies:
Implemented PlayerAnimController with adjusted animations from imports 
Processed user input in conjunction with the animator and blend tree
Code Files Changed: PlayerController.cs, RootMotion.cs

Swati Murugappan:
Implemented 5 collectable gems and associated components along the maze for character to pick up in order to unlock final door and fill princess’ tiara
Created Enemy 1 and Enemy 1 Twin weeble wobble characters with associated animations that patrol back and forth and guard one of the gems in the final room 
Code Files Changed: PlayerController.cs

Crystal Phiri:
Implemented code for energy potion, created AI ghost enemy (procedural State Machine, scripts, navmesh, waypoints, floor), created animation state machine for doors, and placed items in the appropriated places
Assets: ghost, big ghost, keys, energy potion, door, floor 7 & 8
Code Files Changes: DoorController.cs, PlayerController.cs, G3PatrolAndRunaway.cs, PatrolAndRunaway.cs

Pooja Pache:
Implemented code for Start Scene, In game Menu and Background Music
Assets: all assets in Start Scene: terrain, canvas, monster. Maze walls, ceilings, directional lights, torches, floors in main game scene
Code Files Changes: GameStarters.cs, GameQuitter.cs, PauseMenuToggle.cs, OnClickStart.cs, ResumeGame.cs

Sanjana Date:
Implemented code for player interaction with keys, doors and ghosts. Added audio cues for all interactions. Implemented dummy doors.
Assets: doors, ghosts, camera, UI Canvas for collectibles info
Code Files Changes: CameraController.cs, GameDoorController.cs, PlayerController.cs, PatrolAndRunaway.cs

