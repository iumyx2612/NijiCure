# Changelog 22/6/2023: 
## Update player's UI
- Change `healthBarImageUpdate` from Vector2GameEvent to GameEvent (see [PlayerUIManager.cs](Scripts/Player/PlayerUIManager.cs)
 and [PlayerCombat.cs](Scripts/Player/PlayerCombat.cs))
- Update function `HealthBarImagePopUp` (see [PlayerUIManager.cs](Scripts/Player/PlayerUIManager.cs)) to support new 
`healthBarImageUpdate` GameEvent
- Update Raise of `healthBarImageUpdate` GameEvent (see [PlayerCombat.cs](Scripts/Player/PlayerCombat.cs))
## Add Crit Chance 
### Update PlayerTypeAndStartingAbility.cs
- Change name to [PlayerMapping.cs](Scripts/Player/PlayerMapping.cs)
- Add `critChance`
- Add [HideInInspector] to all field
- `critChance` is setup in Awake of [PlayerCombat.cs](Scripts/Player/PlayerCombat.cs) through PlayerData
### Update DamageAbilityBase.cs
- Add `critChance` and `currentCritChance` (Setup in [AbilityManager.cs](Scripts/Ability/AbilityManager.cs))
- Add function `SetupCritChance` to update `critChance`
### Update AbilityManager.cs
- During Awake, when setup Abilities for `allAbilities`, we also setup critChance using `SetupCritChance`
### Update all Ability inherit from DamageAbilityBase
- [ChuaTeNuaBaiData.cs](Scripts/Ability/Linh%20Lan/ChuaTeNuaBaiData.cs)
- [LanKnifeData.cs](Scripts/Ability/Linh%20Lan/LanKnifeData.cs)
### Update attachment scripts for DamageAbility Prefab
- Add `multiplier`: for crit damage multiplier
- Update function `OnTriggerEnter2D`: Support crit chance
- See updates in [ChuaTeNuaBai.cs](Scripts/Ability/Linh%20Lan/ChuaTeNuaBai.cs) and 
[LanKnife.cs](Scripts/Ability/Linh%20Lan/LanKnife.cs)
### Update EnemyCombat.cs
- Function `TakeDamage` support damage multiplier if crit
- Function `DamagePopupSequence` support Text UI display if crit 
## Update Linh Lan run animation
- New files added to [Images](Images/Linhlan/Run)
## Update Ban Mai and animation
- New files added to [Images](Images/BanMai)
- New AnimationController, see [Animation](Animations)