# PewPew
PewPew is a basic space invaders style game that I committed to making (with 2 buddies) in one week. 
It has imperfections, but I would much prefer to leave it untouched as a demonstration of what I can accomplish in a week.

My point of pride for this project is the upgrades system. It runs through scriptable objects. An upgrade is a wrapper for a PlayerPref which contains values for each tier within the upgrade.
These tiers are also scriptable objects. To set the tiers of an upgrade, simply drag and drop the tiers into the upgrade object. The upgrade types are generic, allowing for upgrades
to use whatever values make sense (numeric type for damage, float array for number of bullets and their angles, etc.).

Source:
- [Upgrade definitions](https://github.com/CatSandwich/PewPew/tree/master/Assets/Scripts/Player/Upgrades#upgrades) 
- [Upgrade objects](https://imgur.com/a/TXxRtcn) 
- [Implementation example](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Attack/Bullet.cs#L9)
- [Implementation example 2](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Player/PlayerController.cs#L71)
