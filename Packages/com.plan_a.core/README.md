# Puzzle Core – Reusable Local Package

This package contains reusable **utility**, **extension**, and **architecture-level** scripts
used throughout the technical test.

It is implemented as a **local embedded Unity package** to keep core logic decoupled from
presentation (UI, scenes, prefabs) and to demonstrate how this code could be
**plugged into another project with minimal changes**.

---

## Purpose

The goal of this package is to:

- Isolate **core gameplay logic** from Unity-specific presentation layers
- Encourage **clean separation of concerns**
- Improve **maintainability, testability, and reusability**
- Demonstrate a scalable architecture suitable for larger projects

All scripts in this package are designed to be **project-agnostic** and reusable.

---

## Contents

Depending on the test scope, this package may include:

- **Utility classes**
    - General-purpose helpers
    - Math, grid, or data helpers

- **Extension methods**
    - Extensions for collections, enums, or basic Unity types
    - Designed to reduce boilerplate and improve readability

- **Architecture / Core logic**
    - Game state handling (score, moves, rules)
    - Grid and block data models
    - Algorithms (e.g. flood-fill / adjacency checks)

The package intentionally avoids:
- UI code (Canvas, TMP, buttons, panels)
- Scene references
- Prefabs or assets tied to a specific project

---

## Design Principles

- **Pure C# where possible**
- Minimal dependency on `UnityEngine`
- No references to scripts in the `Assets/` folder
- Explicit assembly references (no implicit coupling)

This allows the package to be reused in:
- Other Unity projects
- Different puzzle game variants
- Prototyping or gameplay-focused tests

---

## Reuse in Another Project

To reuse this package in another Unity project:

1. Copy the package folder: `com.plan_a.core`
2. Add it as a local package in `manifest.json`
3. Reference the assembly from your project code

No additional setup should be required.

---

## Notes

This package was created specifically for the technical test,
but structured as production-ready code to reflect real-world workflows
and long-term maintainability.


