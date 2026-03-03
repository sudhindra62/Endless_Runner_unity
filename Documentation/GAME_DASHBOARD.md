# GAME_DASHBOARD.md: The Bridge

**ROLE:** Central command for all gameplay balance variables.
**AUTO-UPDATING LOGIC:** AI Guardian will read this file to inform code-level decisions and update it to reflect the latest state of the project.

---

| Variable                           | Value                                |
| ---------------------------------- | ------------------------------------ |
| **Player Speed (Initial)**         | 10                                   |
| **Player Speed (Max)**             | 30                                   |
| **Coin Value**                     | 1                                    |
| **Powerup Duration**               | 10s                                  |
| **Run Seed (Current)**             | `DYNAMIC (from RunSessionData)`      |
| **Max Regeneration Attempts**      | 20                                   |
| **Max Reaction Time (ms)**         | 1200ms                               |
| **Min Reaction Time (ms)**         | 200ms                                |
| **Min Obstacle Density**           | 0.1                                  |
| **Max Obstacle Density**           | 0.85                                 |
| **Min Safe Path Width (m)**        | 1.5m                                 |
| **Assumed Player Jump Height (m)** | 2.0m                                 |
| **Boss Mode Reaction Reduction**   | 50ms                                 |
| **Density Increase on Dodge**      | 0.005                                |
| **Maximum Unfairness Cap**         | 0.95                                 |
| **Global Difficulty Curve**        | `Linear (0,0 to 1,1)`                |
