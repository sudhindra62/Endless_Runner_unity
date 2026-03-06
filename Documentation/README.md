# Endless Runner: Unity 6.3.7f1

**ROLE:** High-level project overview and entry point for new developers.

This document outlines the core vision, technical stack, and development guidelines for the Endless Runner project. It is the root file that establishes the project's identity and standards.

---

## Core Project Mandate

-   **Game:** A high-quality, stable, and scalable endless runner for the Android platform.
-   **Engine:** Unity 6.3.7f1 (LTS).
-   **Architecture:** Modular, event-driven, and built for long-term commercial success.

## Key Features:

*   **Procedural Pattern Engine:** Every run is a unique and challenging experience, with fair and deterministic obstacle generation.
*   **AAA Rotating World Theme Engine:** A data-driven system that manages weekly theme rotations and live-event overrides, ensuring a fresh visual and audio experience.
*   **Live-Service Ready:** Built from the ground up to support live events, remote configuration, and a dynamic in-game economy.
*   **Data-Driven Design:** Core gameplay systems are driven by `ScriptableObjects`, allowing for rapid iteration and balancing.
*   **Performance Optimized:** A strong focus on performance ensures a smooth experience, with no runtime material duplication or mid-game hitches.
*   **Event-Driven Architecture:** Decoupled systems communicate via C# events, making the codebase clean, scalable, and easy to maintain.

## Getting Started

1.  Clone the repository.
2.  Open the project in Unity `6000.3.7f1` or later.
3.  Open the `Main` scene in `Assets/Scenes`.
4.  Press `Play` to start the game.


---

## Folder Structure & Key Locations

-   **/Assets/Scripts/Managers:** Core singleton managers (e.g., `SaveManager`, `UIManager`).
-   **/Assets/Scripts/Core:** The procedural generation engine and its components.
-   **/Assets/Scripts/Player:** Player input, movement, and collision handling.
-   **/Assets/Scripts/PowerUps:** Logic for all in-game power-ups and their effects.
-   **/Assets/Scripts/UI:** All UI controllers and view components.
-   **/Documentation:** All 12 core documentation files, including this README.