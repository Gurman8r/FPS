# FPS

Combat System Framework in Unity	

---------------------------------
Main Types
---------------------------------

- Unit (Any object that can take damage or could conceivably require a healthbar)
	- Players
	- Enemies
	- Explosives
	- Vehicles
- Object (An object that can interact with Units (needs a better name))
	- Bullets
	- Blades
	- Lasers
	- Explosions
	- Kill Zones 
- Item (An object that can be owned/used by a Unit)
	- Consumables
	- Weapons
	- Spells
	- Tools
	- Abilities
	
---------------------------------
Unit Behaviours
---------------------------------

- UnitTriggers
	- Basically an event system for each unit
- UnitInventory
	- Manages units' held/stored items 
- UnitMotor
	- Controls physics & movement
- UnitVision
	- Tracks where units are looking and what they can see

---------------------------------
Unit Controllers
---------------------------------

- BaseUnitController
	- StaticUnit - Unit which DOES NOT move or require navigation 
		- BasicUnit - Unit which just sits there
	- DynamicUnit - Unit which DOES move or require navigation 
		- HumanoidUnit - Unit able to use Items
			- PlayerUnit - Humanoid controlled by a Player
			- RobotUnit - Humanoid controlled by an AI
				- DummyUnit - Unit used for training/practice

---------------------------------
