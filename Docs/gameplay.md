# Gameplay

Current project is a 2D casual board game prototype with turn-based dice movement.

## Core Loop

Current loop:

1. Player clicks roll dice button.
2. `TurnSystem` starts a turn if state is `WaitingForRoll`.
3. `DiceSystem` rolls `1..6`.
4. `PlayerMover` moves player by rolled steps.
5. When movement finishes, `TurnSystem` asks `TileEffectSystem` to resolve the tile.
6. Tile effect writes a `Debug.Log`.
7. Turn returns to `WaitingForRoll`.

There is currently one player token and one board path.

## Board

Scene: `Assets/Scenes/BoardGame.unity`

Board currently has 10 tiles:

- `Tile_00` -> `RareEvent`
- `Tile_01` -> `Buff`
- `Tile_02` -> `Debuff`
- `Tile_03` -> `Battle`
- `Tile_04` -> `RandomEvent`
- `Tile_05` -> `Buff`
- `Tile_06` -> `Battle`
- `Tile_07` -> `Debuff`
- `Tile_08` -> `Buff`
- `Tile_09` -> `Battle`

`BoardManager` stores these as an ordered serialized list and supports cyclic movement.

## Tile Types

Current `TileType` values:

- `RandomEvent`
- `RareEvent`
- `Battle`
- `Buff`
- `Debuff`

Current effect mapping:

- `RandomEvent` -> logs `Event tile resolved`
- `RareEvent` -> logs `Rare tile resolved`
- `Battle` -> logs `Battle tile resolved`
- `Buff` -> logs `Buff tile resolved`
- `Debuff` -> logs `Debuff tile resolved`

No tile effect currently changes HP, opens UI, starts battle, or grants items.

## Movement

Player object is child of the current tile.

`PlayerMover`:
- moves via coroutine;
- moves to the same local offset inside each target tile;
- stops briefly on each tile;
- calls callback after movement ends;
- does not resolve tile effects.

The player starts on `Tile_00`.

## HP and Level

`PlayerStats` is attached to `Player`.

Current values:
- `maxHp = 5`
- `currentHp = 5`
- `level = 1`

Available methods:
- `TakeDamage(int amount)`
- `Heal(int amount)`
- `SetLevel(int level)`

HP is clamped between `0` and `maxHp`.

HUD listens to `PlayerStats` events and updates:
- level text;
- 5 HP hearts.

No gameplay system currently deals damage or heals automatically.

## Cards

Cards are MVP.

`CardData` ScriptableObject contains:
- `cardId`
- `cardName`
- `description`
- `cardSprite`

Current card assets:
- `Assets/Data/Cards/SmallHeal.asset`
- `Assets/Data/Cards/Shield.asset`
- `Assets/Data/Cards/LuckyHit.asset`

`CardSystem` stores current hand with max 3 cards.

Current behavior:
- displays up to 3 cards in hand;
- clicking a card logs `Card used: <card name>`;
- no card applies gameplay effects yet;
- cards are displayed as complete ready-made sprites.

## Inventory

Inventory is not implemented as gameplay.

Current state:
- HUD has 3 passive inventory slots;
- `InventorySlotView` can show empty state or assigned icon;
- no item data model;
- no item pickup;
- no equipment effects;
- no drag-and-drop.

## Battle System

Battle system is not implemented.

Current battle tile behavior:
- landing on `Battle` tile logs `Battle tile resolved`.

No combat scene, enemy model, battle UI, damage rules, or rewards exist yet.

## Event System

Event system is not implemented.

Current event tile behavior:
- `RandomEvent` logs `Event tile resolved`;
- `RareEvent` logs `Rare tile resolved`.

No event windows or random event tables exist yet.
