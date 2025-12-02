How to get the project working

# Main Menu Scene
## Canvas > Panel
- Script -> MainMenu/MainMenu.cs
- Script -> MainMenu/ButtonHoverEffects.cs

### Canvas > Panel > UIDocument
- UI Document (click the circle with the dot)
    - Panel Settings -> PanelSettings
    - Source Asset -> MainMenuUI
- Script -> MainMenu/MainMenu.cs
- Script -> MainMenu/ButtonHoverEffects.cs

# GameScene
## Spaceship
- Script -> Spaceship/SpaceshipMovement.cs
    > *Input types: 0 = keyboard, 1 = head matching, 2 = angular momentum

### Spaceship > spaceship-concept
- Prefab -> Assets/spaceship-concept

### Spaceship > AsteroidSpawner
- Script -> Asteroids/AsteroidSpawner.cs
- Prefab -> Assets/Asteroid

### Spaceship > SpaceshipShoot
- Script -> Spaceship/SpaceshipShoot.cs
- Spaceship Model -> Scene/spaceship-concept
- Laser Object -> Assets/Laser

## Spaceship/Laser/Laser (prefab)
- Script (Laser)
- Speed (200)

## Asteroids/Prefabs/Asteroid
- Script -> Asteroids/asteroid.cs
