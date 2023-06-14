# Ability Description
Check [AbilityBase.cs](AbilityBase.cs):  
- `playerType`: specify what PlayerType can pick this Ability. More 
about `PlayerType` in [Appendix](#playertype)
- `weight`: Ability with higher weight will appear more often for player to pick when Level Up

# Ability Setup
- `AbilityCollection allAbility`: Acts as a database stores all of 
created abilities
- `PlayerType typeAny`: 



# Appendix
## PlayerType
- Player choose one PlayerType before entering a stage
- PlayerType is used only for AbilitySystem
- There are 2 major types: Common (specify by type `Any`) and OwnType
    + type `Any`: If Ability has this type, then any player can choose this ability when Level Up
    + OwnType: like `Assasin`, `Soldier` etc. Ability with this type
    can only be chosen by player with same type in the stage