# Changelog
All notable changes to this project will be documented in this file.

For game design changelog, visit [GameDesign Changelog](https://docs.google.com/spreadsheets/d/1oSoVJ0jk9w2Vpz4AcT9sktsVrlLd9k-s05p6IKA5R1k/edit?gid=1113732660#gid=1113732660)

## [1.0.5] - 26/07/2024
### Added
- Two additional new coop events:
  > **Chained Together:** Players will be chained together and cannot move freely outside of the chain's length.
  > **Capture The Point:** Players have to go to all certain points before the timer runs out.
- Three new Level Up buffs: Fire Rate, AOE Meteor, and Time Warp.
- Add a basic shop system to buy upgrades.
- Add a drone for single-player gameplay.
- Add halo effects for items.
- Add object transparency for environmental props when the player moves behind it.

### Changed
- Refactor the Level Up buff system.
- Adjust lighting and environment.
- Adjust item size.

### Fixed
- Fix a bug where Level Up buff not applying due to wrong key matching.
- Fix a bug where downed players can revive each other.
- Fix a bug where gun sounds act abnormally.
- Fix a bug where enemies still go toward downed players.
- Fix a bug where choosing a gun before joining a room does not save the chosen gun.
- Fix a bug where stacking speed buffs makes players run abnormally fast.

## [1.0.4] - 19/07/2024
### Added
- Six new level-up buffs. Activate when the players level up. Each player can choose one permanent buff out of three random buffs that appear on the screen.
- Two new coop events:
  > **Shared attributes**: All players will share attributes like HP, ...
  
  > **Quick time events**: The players must complete certain side objectives in a time frame: *Kill X enemies, do not pick X power up.*
  
  > When the players complete the coop event, their level will be increased by one.
- Add a CRIT system for players.
- Sound VFX: BGM, buttons / guns / enemies sounds.
- UI for leveling up, coop events.

### Changed
- Rework server game state.
- Change the level-up system to not based on the level to avoid a large amount of stat gained.

### Fixed
- Fix a bug where the server not synchronizing when two players kill the same enemy or pick up an item at the same time.
- Fix a bug where the item drop rate of enemies is always 100%.

## [1.0.3] - 12/07/2024
### Added
- Six new pick-up items to boost the player - Using a dictionary to manage all items buff and duration through a `powerUpManager`.
- A simple resurrection feature - the alive player can stand near the downed player to revive them up.
- More VFX for enemies' behavior like exploding, meteor summoning, and healing.
- UI for player's Health Point (HP), Level.
- Pause, Quit, Resume UI.
- A scoreboard for enemies killed by each player.

### Changed
- Map with smoother terrain and more decoration props.
- Removed `dgram` library for processing data from the server, instead implemented detailed data processing functions.
- Smoothen synchronizing players moving and shooting by Interpolation and Extrapolation.

### Fixed
- Cumulated velocity when colliding with the environment causes the player to "fly" off the map - Fix by clamping and resetting the normalized velocity of the player every time the player collides with the environment.
- Player's health did not get clamped, resulting in a large amount of HP cumulated when pickup multiple Health Packs - Fix by clamping HP.

## [1.0.2] - 05/07/2024
### Added
- Map and environment.
- Collision between environment and player.
- Joystick for mobile gameplay.
- Gameplay assets.
- UI Login and choose starting gun.
- Manage terrain on the map with a dictionary.

### Changed
- Fine-tune enemy animations.
- Fine-tune enemy and character collider.
- Manage and destroy bullets through a bullet dictionary.

### Fixed
- Character sinking/flying due to collision with the terrain - Normalized movement vector to avoid changing Y-vector when colliding with slightly higher terrain.
- Enemy spawning causes lag - Move enemies to an object pool.

## [1.0.1] - 01/07/2024
### Added
- Player, guns, enemies attributes design.
- Three new Bullet configs.
- Seven new enemy behaviors.
- Enemy spawning.
- Camera following player.

### Changed
- Change multiplayer to UDP method.
- Get playerId through the server. 
- Refactor shooting method.

### Fixed
- Only one player can shoot at a time

## [1.0.0] - 25/06/2024
### Added
- Simple server.
- Simple player movement.
- Simple shooting mechanic.
- Simple enemy.
- Basic game design.
