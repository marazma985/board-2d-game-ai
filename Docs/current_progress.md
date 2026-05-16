# Current Progress

This document describes the current state of the Unity 6 board game project.

## Implemented

### Board

- `BoardGame.unity` scene exists.
- Board has 10 tiles: `Tile_00` through `Tile_09`.
- Each tile has `BoardTile` with `index` and `TileType`.
- `BoardManager` stores ordered serialized tile list.
- Board path is visualized through sprite segments under `BoardPath_Line`.
- Player starts on `Tile_00`.

### Turn Flow

- `DiceSystem` rolls `1..6`.
- `TurnSystem` coordinates turn states.
- Duplicate dice roll is blocked while not in `WaitingForRoll`.
- `PlayerMover` moves player by dice result.
- After movement, `TileEffectSystem` resolves current tile.
- Turn returns to `WaitingForRoll`.

### Tile Effects

Current effects are MVP and log only:

- `EventTileEffect`
- `RareTileEffect`
- `BattleTileEffect`
- `BuffTileEffect`
- `DebuffTileEffect`

`HealTileEffect` exists but is not currently mapped to a `TileType`.

### Player Stats and HUD

- `PlayerStats` exists on `Player`.
- Current HP starts at `5`.
- Max HP is `5`.
- Level starts at `1`.
- `HudView` displays level, 5 hearts, and 3 passive inventory slots.
- `InventorySlotView` can show empty state or assigned icon.

### Cards

- `CardData` ScriptableObject exists.
- `CardSystem` stores max 3 cards in hand.
- `CardHandView` displays card hand.
- `CardView` displays a complete card sprite and reacts to click.
- Clicking a card logs `Card used: <card name>`.

Current test cards:
- `Small Heal`
- `Shield`
- `Lucky Hit`

## Tested

Previously verified:

- dice roll returns values in `1..6`;
- roll button starts turn;
- duplicate roll is blocked during movement;
- tile effects dispatch and log correct messages;
- `PlayerStats.TakeDamage`, `Heal`, and `SetLevel` update HUD;
- inventory slot icon can be assigned and cleared;
- card hand displays 3 cards;
- clicking `CardSlot_01` logs `Card used: Small Heal`;
- console was clean after recent checks.

## Not Implemented Yet

- real tile effects;
- actual healing from cards or tiles;
- battle system;
- event windows;
- random event table;
- rare event table;
- buff/debuff gameplay rules;
- inventory gameplay model;
- item data assets;
- card effects;
- card cost/usage limits;
- card draw/discard pile;
- save/load;
- animations;
- final art.

## Known MVP Limitations

- Most visuals are placeholder sprites.
- Card sprites are generated placeholders.
- Cards do not apply effects.
- Inventory slots are passive visuals.
- Battle and event tiles only log messages.
- `HealTileEffect` exists but is not connected to current `TileType`.

## Recommended Next Step

Recommended next development step:

Implement card effect routing without applying complex gameplay yet.

Suggested path:

1. Add a card effect interface, for example `ICardEffect`.
2. Add focused placeholder effects for `Small Heal`, `Shield`, and `Lucky Hit`.
3. Let `CardSystem.UseCard(CardData card)` dispatch to an effect handler.
4. Start with `Small Heal` calling `PlayerStats.Heal(1)`.
5. Keep `CardView` and `CardHandView` UI-only.

Alternative next step:

Implement `BattleTileEffect` as a trigger into a separate battle flow, still with placeholder UI/logging only.
