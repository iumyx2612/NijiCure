# Ability Description
## Base Ability
Check [AbilityBase.cs](AbilityBase.cs):
- `playerType`: specify what PlayerType can pick this Ability. More 
about `PlayerType` in [Appendix](#playertype)
- `weight`: Ability with higher weight will appear more often for player to pick when Level Up
## Damage Ability Base
Check [DamageAbilityBase.cs](DamageAbilityBase.cs)

# Ability Manager
Check [AbilityManager.cs](AbilityManager.cs)
## Setup
- `AbilityCollection allAbility`: Acts as a database stores all of 
created abilities
- `PlayerType typeAny`: The type `Any` in [PlayerType](#playertype)
- `List<AbilityBase> availableAbilities`: This is the database of what Abilities can be chosen in Scene,
this List will contains all Abilities type `Any` AND Ability of OwnType. This will be setup in Awake
- `PlayerTypeAndStartingAbility mapping`: To retrieve OwnType and starting Ability of OwnType
- `AbilityCollection currentAbilities`: All Abilities that the player currently have
## How Ability Works
- First, Ability is Initialize when chosen (Each Ability has it own Initialize method)


# Appendix
## PlayerType
- Player choose one PlayerType before entering a stage
- PlayerType is used only for AbilitySystem
- There are 2 major types: Common (specify by type `Any`) and OwnType
    + type `Any`: If Ability has this type, then any player can choose this ability when Level Up
    + OwnType: like `Assasin`, `Soldier` etc. Ability with this type
    can only be chosen by player with same type in the stage