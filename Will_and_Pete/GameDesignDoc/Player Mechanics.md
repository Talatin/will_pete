#Player
## [[Player Movement]]

Movement should be snappy and controllable.
Double jump and wall slide for together with world environment should provide a multitude of ways to get to their target.
### Walk/Run
The player can move left and right on the ground and in the air.
The player can move less direct when in the air.

### Jump
Players can do up to 2 Jumps


### Wall Slide / Jump
- When jumping against a wall. The player can repress the jump input to jump diagonally off the wall.
- Jumping off the wall reduces air control significantly for a short duration.
	- This helps the player jump further in the direction they want while simultaneously blocking them from just re-attaching to the same wall again. 
- When the player is tight against a wall and moving *against* it they will fall slower in a sliding animation.

>[!hint]+ WallJumpIdea
>Try letting the player only "attach" to the wall when the y velocity is negativ
>this could make the player really use the slide feature and use full jumps when climbing between 2 walls


## [[Player Shooting]]
Both Players Share a rifle.
We want the Characters to be able to throw the rifle.


