# Architecture

Current project: Unity 6, 2D casual board game with turn-based board movement inspired by Munchkin.

Main scene: `Assets/Scenes/BoardGame.unity`

## Main Systems

### BoardTile
File: `Assets/Scripts/Board/BoardTile.cs`

MonoBehaviour attached to each board tile object (`Tile_00` ... `Tile_09`).

Responsibilities:
- stores stable `index`;
- stores current `TileType`;
- exposes `Enter()` and `Entered` event;
- draws simple gizmos for editor visualization;
- keeps scene object name stable as `Tile_XX`.

Tile names do not include tile type because tile type is dynamic.

### BoardManager
File: `Assets/Scripts/Board/BoardManager.cs`

MonoBehaviour on `BoardRoot`.

Responsibilities:
- stores serialized ordered list of `BoardTile`;
- exposes `GetTile(int index)`;
- exposes `GetNextTile()`;
- tracks `currentIndex`;
- supports cyclic path via `cyclePath`;
- advances current tile through `AdvanceToNextTile()`.

### DiceSystem
File: `Assets/Scripts/Board/DiceSystem.cs`

MonoBehaviour on `DiceSystem`.

Responsibilities:
- `Roll()` returns a random value from `1` to `6`;
- invokes `OnDiceRolled(int value)`.

No UI logic.

### PlayerMover
File: `Assets/Scripts/Board/PlayerMover.cs`

MonoBehaviour on `Player`.

Responsibilities:
- receives `BoardManager` via serialized reference;
- moves player between tiles with coroutine;
- stops briefly on each tile;
- keeps player offset inside tile;
- calls callback after movement completes.

It does not resolve tile effects and does not use `Update()`.

### TurnSystem
Files:
- `Assets/Scripts/Board/TurnSystem.cs`
- `Assets/Scripts/Board/TurnState.cs`

MonoBehaviour on `BoardRoot`.

Responsibilities:
- coordinates turn flow;
- prevents rolling unless state is `WaitingForRoll`;
- coordinates `DiceSystem`, `PlayerMover`, `BoardManager`, and `TileEffectSystem`;
- exposes C# events for state and turn milestones.

Current states:
- `WaitingForRoll`
- `RollingDice`
- `MovingPlayer`
- `ResolvingTile`
- `TurnEnded`

### TileEffectSystem
Files:
- `Assets/Scripts/Board/TileEffectSystem.cs`
- `Assets/Scripts/Board/ITileEffect.cs`
- effect classes in `Assets/Scripts/Board/*TileEffect.cs`

MonoBehaviour on `BoardRoot`.

Responsibilities:
- receives current `BoardTile`;
- selects effect by `TileType`;
- calls matching `ITileEffect.Resolve(BoardTile tile)`;
- currently all effects only write `Debug.Log`.

Current mapping:
- `RandomEvent` -> `EventTileEffect`
- `RareEvent` -> `RareTileEffect`
- `Battle` -> `BattleTileEffect`
- `Buff` -> `BuffTileEffect`
- `Debuff` -> `DebuffTileEffect`

`HealTileEffect` exists for future HP-related work but is not currently mapped to `TileType`.

### PlayerStats
File: `Assets/Scripts/Board/PlayerStats.cs`

MonoBehaviour on `Player`.

Responsibilities:
- stores `currentHp`, `maxHp`, `level`;
- max HP is currently `5`;
- clamps HP to `0..maxHp`;
- exposes `TakeDamage(int amount)`, `Heal(int amount)`, `SetLevel(int level)`;
- exposes `OnHpChanged` and `OnLevelChanged`.

No UI logic.

### HudView
Files:
- `Assets/Scripts/Board/HudView.cs`
- `Assets/Scripts/Board/InventorySlotView.cs`

MonoBehaviour on `Player HUD`.

Responsibilities:
- displays level text through `TextMeshProUGUI`;
- displays HP through array of 5 `Image` hearts;
- displays passive inventory slots through `InventorySlotView`;
- subscribes to `PlayerStats` events.

`InventorySlotView` only displays an empty or occupied visual state. It has no inventory gameplay logic.

### Card System
Files:
- `Assets/Scripts/Board/CardData.cs`
- `Assets/Scripts/Board/CardSystem.cs`
- `Assets/Scripts/Board/CardHandView.cs`
- `Assets/Scripts/Board/CardView.cs`
- `Assets/Scripts/Board/ICardEffect.cs`
- `Assets/Scripts/Board/*CardEffect.cs`

Responsibilities:
- `CardData` is a ScriptableObject for card data and card sprite;
- `CardSystem` stores current hand, max 3 cards;
- `CardSystem.UseCard(CardData card)` dispatches by `CardData.CardId`;
- `CardHandView` displays hand in UI;
- `CardView` displays one ready-made card sprite and reports clicks;
- used cards are removed from hand after a successful effect.

Current card effect mapping:
- `small_heal` -> `SmallHealCardEffect`, calls `PlayerStats.Heal(1)`;
- `shield` -> `ShieldCardEffect`, placeholder log only;
- `lucky_hit` -> `LuckyHitCardEffect`, placeholder log only.

`CardSystem` receives `PlayerStats` through a serialized Inspector reference. UI classes do not apply card effects.

## Scene Object Links

Current scene structure includes:
- `BoardRoot`
  - `BoardManager`
  - `TurnSystem`
  - `TileEffectSystem`
  - board tiles
  - `BoardPath_Line`
- `DiceSystem`
  - `DiceSystem`
- `Tile_00/Player`
  - `PlayerMover`
  - `PlayerStats`
- `Board UI Canvas`
  - `Roll Dice Button`
  - `Player HUD`
  - `CardHand`
- `CardSystem`
  - `CardSystem`
- `EventSystem`

Dependencies are assigned through serialized Inspector references.

## Turn Flow

1. UI requests `TurnSystem.TryRollDice()`.
2. `TurnSystem` checks state is `WaitingForRoll`.
3. `TurnSystem` switches to `RollingDice`.
4. `DiceSystem.Roll()` returns `1..6`.
5. `TurnSystem` switches to `MovingPlayer`.
6. `PlayerMover.MoveSteps(steps, callback)` moves player through board tiles.
7. Movement callback fires.
8. `TurnSystem` reads current tile from `BoardManager`.
9. `TurnSystem` switches to `ResolvingTile`.
10. `TileEffectSystem.ResolveTile(currentTile)` resolves effect by `TileType`.
11. `TurnSystem` switches to `TurnEnded`.
12. `TurnSystem` returns to `WaitingForRoll`.

## Implemented vs MVP

Implemented:
- board tiles and ordered cyclic path;
- dice roll;
- player movement;
- turn orchestration;
- tile effect dispatch;
- HP/level data;
- HUD display;
- passive inventory slot display;
- card data, hand display, card effect dispatch;
- `Small Heal` restores 1 HP and is consumed after use.

MVP only:
- tile effects only log messages;
- `Shield` and `Lucky Hit` card effects only log messages;
- inventory slots are visual only;
- battle/event/buff/debuff do not apply gameplay effects yet.
