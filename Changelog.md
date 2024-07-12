# Changelog
All notable changes to this project will be documented in this file.

For game design changelog, visit [GameDesign Changelog](https://docs.google.com/spreadsheets/d/1oSoVJ0jk9w2Vpz4AcT9sktsVrlLd9k-s05p6IKA5R1k/edit?gid=1113732660#gid=1113732660)
## [1.0.3] - 12/07/2024
### Added
- Six new pick-up items to boost the player - Using a dictionary to manage all items buff and duration through a `powerUpManager`.
- A simple resurrection feature - the alive player can stand near the downed player to revive them up.
- More VFX for enemies' behavior like exploding, meteor summoning, healing.
- UI for player's Health Point (HP), lLvel.
- Pause, Quit, Resume UI.
- A scoreboard for enemy killed by each player.

### Changed
- Map with smoother terrain and more decoration props.
- Removed `dgram` library for processing data from server, instead implemented detailed data processing functions.
- Smoothen synchronizing players moving and shooting by Interpolation and Extrapolation.

### Fixed
- Cumulated velocity when collide when environment cause player to "fly" off the map - Fix by clamping and resetting the normalized velocity of the player every time the player collide with the environment.
- Player's health did not get clamped, resulted in large amount of HP cumulated when pickup multiple Health Pack - Fix by clamping HP.

## [1.0.2] - 05/07/2024
### Added
- Map and environment.
- Collision between environment and player.
- Joystick for mobile gameplay.
- Gameplay assets.
- UI Login and choose starting gun.
- Manage terrain on map with a dictionary.

### Changed
- Fine-tune enemy animations.
- Fine-tune enemy and character collider.
- Manage and destroy bullets through a bullet dictionary.

### Fixed
- Character sinking/flying due to collision with terrain - Normalized movement vector to avoid changing Y-vector when collding with slightly higher terrain.
- Enemy spawning cause lag - Move enemy to an object pool.

## [1.0.1] - 01/07/2024
### Added
- Player, guns, enemies attributes design.
- Three new Bullet configs.
- Seven new enemy behaviors.
- Enemy spawning.
- Camera following player.

### Changed
- Change multiplayer to UDP method.
- Get playerId through server. 
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
