# UI

Current UI is in `Assets/Scenes/BoardGame.unity`.

Main Canvas:
- `Board UI Canvas`

Canvas children:
- `Roll Dice Button`
- `Player HUD`
- `CardHand`

There is also an `EventSystem` in the scene.

## Roll Dice Button

Object: `Board UI Canvas/Roll Dice Button`

Scripts:
- Unity `Button`
- `DiceRollButtonController`

Responsibilities:
- requests `TurnSystem.TryRollDice()`;
- disables/enables itself based on `TurnState`;
- logs dice result through `TurnSystem.DiceRolled`.

The button does not directly call `DiceSystem.Roll()` or `PlayerMover.MoveSteps()`.

## Player HUD

Object: `Board UI Canvas/Player HUD`

Script:
- `HudView`

Displays:
- avatar placeholder image;
- level badge and `TextMeshProUGUI` level text;
- 5 HP hearts using `Image[] heartImages`;
- 3 passive inventory slots.

HUD depends on:
- `PlayerStats`;
- heart sprites;
- inventory slot views.

HUD only displays values and does not change HP or level.

## HUD Assets

Current placeholder HUD art:

- `Assets/Art/Board/HUD/AvatarPlaceholder.png`
- `Assets/Art/Board/HUD/HeartFull.png`
- `Assets/Art/Board/HUD/HeartEmpty.png`
- `Assets/Art/Board/HUD/InventorySlot.png`
- `Assets/Art/Board/HUD/TestItemIcon.png`

These are temporary placeholders.

## Inventory Slots

Objects:
- `Inventory Slot 01`
- `Inventory Slot 02`
- `Inventory Slot 03`

Script:
- `InventorySlotView`

Each slot has:
- background `Image`;
- icon `Image`;
- optional `itemIcon` Sprite.

Current behavior:
- empty slot hides icon;
- occupied slot shows assigned icon;
- slots are not buttons;
- slots have no gameplay logic.

## Card Hand

Object: `Board UI Canvas/CardHand`

Script:
- `CardHandView`

Children:
- `CardSlot_01`
- `CardSlot_02`
- `CardSlot_03`

Each card slot has:
- `Image`;
- `Button`;
- `CardView`.

Current behavior:
- hand displays up to 3 cards from `CardSystem.Hand`;
- empty slots are hidden;
- each `CardView` displays only `CardData.cardSprite`;
- clicking a card calls `CardSystem.UseCard(card)`;
- `UseCard` logs `Card used: <card name>`.

Card UI does not display separate name or description text because card art already contains complete visual content.

## Card Assets

Card data:
- `Assets/Data/Cards/SmallHeal.asset`
- `Assets/Data/Cards/Shield.asset`
- `Assets/Data/Cards/LuckyHit.asset`

Card sprites:
- `Assets/Art/Board/Cards/SmallHeal.png`
- `Assets/Art/Board/Cards/Shield.png`
- `Assets/Art/Board/Cards/LuckyHit.png`

## Working UI Elements

Currently working:
- roll dice button starts turn;
- roll dice button blocks duplicate roll during movement/resolution;
- HP hearts update from `PlayerStats`;
- level text updates from `PlayerStats`;
- inventory slot can show assigned icon;
- card hand displays 3 cards;
- card click logs card usage.

Not implemented:
- card animations;
- drag-and-drop;
- item tooltips;
- battle UI;
- event UI;
- settings UI for board scene.
