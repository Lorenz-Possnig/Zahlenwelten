# Todo:
- Change Balloon
    - on manipulation start: clone object (optional: rotate and recolor new object)
    - on manipulation stop:
    - if is in trigger -> validate and pop/yay
    - else -> float away
    - Pop:
        - sound effect
        - (optional: animation) -> set active false -> destroy later
    - Yay:
        - sound effect
        - snap into place, set not interactable
- Change brettl:
    - on trigger enter -> set is in trigger on balloon true
    - on trigger exit -> set is in trigger on balloon false
- Change near interaction grabbable to only grab near objects/actually touching
- UI
    - Main menu
    - level select
- Game moderation
    - TTS, which number to build?
    - Scoreboard -> list correctly built numbers of this session
- Brettln spawnen:
    - 3 GOs -> set active on scene load according to level