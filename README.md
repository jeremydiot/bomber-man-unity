# BombermanUnity

## TODO

- Max time => remplissage du jeu à la fin / mort subite
- retirer max win
- bonus qui apparait aléatoirement

## Spécifications minimales

- 2 joueurs
  - 1 joueur apparait au coin haut au gauche
  - 1 joueur apparait au coin bas droite
  
- 1 niveau
 
- Plateau de 15 x 13 cases 195 cases
  - 30 mur incassable séparés de une case autour les un des autres
  - génération aléatoire de mur cassable

- Durée de la partie 200 secondes par défaut
- 3 points de vie par joueur par défaut

- Les bombes ont une porté de 1 case
- L'explosion des bombes ne traverse pas les murs incassables

- Possibilité de changer le temps et la vie avant la partie
- Bonus :
  - +1 bombe à la fois jusqu'à la fin du jeu / 10 caché derrière des murs cassables
  - +1 de porté jusqu'à la fin du jeu / 20 caché derrière des murs cassables
  - Portée infinie pendant 5 secondes qui détruit tout les murs sur son passage / 5 caché derrière des murs cassables

- A la fin la map se réduit, se remplit de case "mur incassable" tout autour une case à la fois
  - si un joueur est sur la case du mur qui apparait il meur

- Un joueur meur si il a plus de vie
- Se faire toucher par l'explosion d'une bombe enlève une vie (énemie ou personnelle)

- Musique 8bit
  
## Touches
- Joueur1
  - Z Q S D + Space
  
- Joueur2
  - ↑ ↓ → ← + Enter


## Architecture
- Assets
  - Prefabs  
    - Bombes
    - Fire
    - Walls
      - UnbreakableWalls
      - BreakablesWalls
    Bonus
      - UpBomb
      - UpRange
      - MegaBomb
  - Scenes
    - menu
    - map1 
  - Scripts
    - Bombes
      - snapBombes
      - explodeBombes
      - bombesSettings
    - Player
      - mouvements
      - health
      - dropBomb
    - GameManager
      - time
      - score
      - gameSettings
    - Map
      - generateGrid
      - generateRandomBreakableWall
      - generateBonuses
      - suddenDeath
    -Utils
  - Sprites