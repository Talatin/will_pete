#player 

# [[Player Movement]]
Movement should be snappy and controllable.

## Walk/Run
Moving should feel snappy

The player can move left and right on the ground and in the air.
The player has less control when moving in the air.
## Jump
Jumping should feel funny.

Fast and controllable.
- Can hold the button to jump higher
- Letting go early leads to very small jumps
## Wall Slide / Jump
Feels like parkour and stunts

- When jumping against a wall. The player can repress the jump input to jump diagonally off the wall.
- Jumping off the wall reduces air control significantly for a short duration.
	- This helps the player jump further in the direction they want while simultaneously blocking them from just re-attaching to the same wall again. 
- When the player is tight against a wall and moving *against* it they will fall slower in a sliding animation.

---
# [[Player Shooting]]
Shooting should be impactful and feel strong

We currently have two design approaches for shooting
1. Player can shoot whenever and aims with the right stick constantly
2. Player can shoot only when kneeling down into a stationary aim mode

>[!question]- Shooting Types Discussion
>1. Type One
>	1. Pros
>		1. Freedom to the player
>		2. Knockback mechanic for movement
>		3. Jump Shots
>	2. Cons
>		1. How do we aim? (Left or right stick)
>		2. Less impactful
>2. Type Two
>	1. Pros
>		1. Impactful firing pose
>		2. Creativity from limits
>	2. Cons
>		1. Restricting

Both Players Share a rifle.
Players can pass the rifle by throwing it.
## Weapon Throwing
Should feel like a combination of fun and risk

The weapon can be thrown via player input.
The throw takes current player velocity into consideration.
This way we can throw further when running and jumping

The whole mechanic is inherently risky due to the fact, that the weapon can be lost to the level/world
