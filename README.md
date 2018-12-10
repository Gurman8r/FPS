# FPS

Working on an combat system in Unity.

It's really want it to be a sort of event system to handle lots of little interactions between0 a lot of different _things_.

---------------------------------
Unit
---------------------------------
Anything that can take damage or could conceivably require a healthbar
- Players
- Enemies
- Explosives
- Vehicles

---------------------------------
Object
---------------------------------
Anything that can interact with Units
- Bullets
- Blades
- Lasers
- Explosions

---------------------------------
Item
---------------------------------
Anything that can be used/held/owned by a Unit
- Consumables
- Weapons
- Spells
- Tools
- Abilities

---------------------------------
Unit Behaviours
---------------------------------

Units just hold data, they depend on the following behaviour scripts to _do something_ with the data.

- UnitTriggers
	- Basically an event system for each unit
- UnitInventory
	- Manages a unit's item usage
- UnitMotor
	- Controls an unit's physics & movement
- UnitVision
	- Tracks and updates where a unit is looking and what it can see.

---------------------------------
Unit Controllers
---------------------------------

Unit controllers are unit behaviours which supply inputs to the aforementioned behaviours

- BaseUnitController
	- StaticUnit (DOES NOT require NavMeshAgent)
		- BasicUnit (_Some_ destructable prop)
	- DynamicUnit (DOES require NavMeshAgent)
		- HumanoidUnit (Able to use Items)
			- PlayerUnit (Controlled by a Player)
			- RobotUnit (Controlled by an AI)
				- DummyUnit

---------------------------------
