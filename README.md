# Senior Unity Game Developer – Practical Test

This repository contains the solution for the **Plan A – Senior Unity Game Developer Unity Practical Test**.

The goal of this project is to implement a simple puzzle game while demonstrating
clean architecture, maintainable code, and good Unity practices under time constraints.

https://github.com/user-attachments/assets/104f4a57-8b1a-437c-ae96-fb4ebbaf8c7f

---

## Project Overview

**Game Description**

- A mobile-only, portrait-mode puzzle game
- Grid size: **6 x 5**
- The player starts with **5 moves**
- Tapping a block collects all adjacent blocks of the same color (up, down, left, right)
- Points are awarded based on the number of collected blocks
- Empty spaces are filled by gravity and new random blocks
- The game ends when the player runs out of moves
- The game can be replayed indefinitely

---

## Unity Version

- **Unity 6000.0.64f1 (Unity 6)**

## Architecture Overview

The project is structured around a small set of core systems designed to keep gameplay logic decoupled, testable, and easy to extend.

At the center of the architecture is the `GameManager`, which acts as the composition root of the game. It owns the runtime game data, initializes core services, and drives the game state machine. Rather than embedding logic directly in the manager, behavior is delegated to states and services.

State management is handled through a lightweight state machine. Each state represents a high-level phase of the game (start, gameplay, game over). States are responsible for subscribing to and reacting to game events, and they control when transitions occur. This keeps flow control explicit and avoids hidden dependencies between systems.

Communication between systems is handled through a type-based `EventBus`. Gameplay actions such as block taps, board creation, or replay requests are dispatched as events and consumed by interested systems. This allows UI, gameplay, and state logic to evolve independently.

Shared architectural utilities (state machine, service locator, data binding, pooling) live inside an embedded core package. These scripts are intentionally designed to be game-agnostic, making them easy to reuse in future projects without modification.

UI updates are driven by a simple reactive data layer (`DataBind`). UI elements bind to data values (such as score or moves) and react automatically when those values change, reducing direct coupling between UI and gameplay code.

---

## Development Plan

This section outlines the approach followed to develop the practical test.

1. **Project skeleton**
   Start by creating a solid project skeleton, including all relevant settings and dependencies that add real value and help speed up development.
   For animations and delayed actions, a combination of **DOTween** and **UniTask** is used. This pairing is extremely powerful and helps keep the codebase clean and readable, without falling into coroutine-heavy workflows.

2. **Core embedded package**
   Create a core embedded package to host architectural scripts such as generic singleton patterns, state management, and other reusable systems.
   The goal is to keep these scripts fully decoupled from the game itself, so they can be easily reused in other projects.
   One key component here is `DataBinding`, a lightweight reactive data wrapper inspired by React’s `useState` hooks, designed to simplify UI updates in response to data changes.

3. **Prefab setup and UI structure**
   Create the main prefabs used throughout the game. While not strictly required, the HUD, Grid, and Game Over screen are separated into their own nested canvases to encapsulate layout redraws and keep UI responsibilities isolated.

4. **GameManager and state machine**
   Implement a `GameManager` singleton responsible for holding the game data and the main state machine.
   The state machine includes:

   * A state for game start / restart
   * A state for player interaction
   * A state for Game Over

   State transitions only occur from within states themselves, and states listen to game events to determine when transitions should happen.

5. **Service locator**
   Introduce a service locator to register and resolve core subsystems, avoiding tight coupling and excessive singleton usage.

6. **Event bus**
   Implement an `EventBus` service: a signal-based, type-driven event system that allows events to carry strongly typed payloads when needed.

7. **Game states and events**
   Define the required game states and events, such as board creation, block tapped, replay requested, and similar gameplay signals.

8. **Data layer**
   Create a basic data layer to store moves, turns, and other game-wide values that persist throughout a session.
   This layer also includes board configuration data (rows, columns, block types) and is designed so it can later be replaced or injected dynamically (for example, via an API or external configuration).

9. **ScriptableObjects usage**
   For the purposes of this test, two ScriptableObjects are used:

   * One defining default runtime values
   * One representing mutable runtime data, which is reset at the start of each session

10. **Board and pooling**
    The board listens to block-tapped events to handle block despawning and respawning.
    An object pooler is used for both grid slots and blocks, allowing the grid to be generated dynamically rather than being hardcoded to a 6x5 layout.

11. **Nice-to-haves**

    * Extra polish through juice and animations
    * Control over the number of block colors via game data

---

## Assumptions

* The grid size (6x5) represents **columns × rows**, but the implementation supports any grid size defined in game data.
* The game is strictly single-player and runs entirely client-side.
* Block connectivity follows the specification of **orthogonal neighbors only** (up, down, left, right).
* Performance constraints are modest due to the small grid size; clarity and maintainability are prioritized over extreme optimization.
* No persistence between sessions is required beyond the runtime of the application.
* Visual fidelity is secondary to gameplay correctness and architectural clarity, as per the scope of the test.

---

## Trade-offs

* A custom lightweight architecture (event bus, state machine, data binding) is used instead of third-party frameworks to better demonstrate design decisions and problem-solving approach.
* The `ServiceLocator` pattern is chosen for simplicity and speed of setup, acknowledging that a full dependency injection framework could offer stronger guarantees in a larger project.
* Some systems (such as pooling and data binding) are more flexible than strictly required for this test, intentionally trading simplicity for reusability and scalability.
* Animations and visual feedback are kept minimal to focus on core gameplay and system design within the time constraints.

