# Changelog
All notable changes to this project will be documented in this file.

For game design changelog, visit [GameDesign Changelog](https://docs.google.com/spreadsheets/d/1oSoVJ0jk9w2Vpz4AcT9sktsVrlLd9k-s05p6IKA5R1k/edit?usp=sharing)
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
