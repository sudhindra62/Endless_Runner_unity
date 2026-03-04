# Analytics Engine Documentation

## Overview

The Analytics Engine is a modular system designed to track player behavior and provide insights into player engagement and frustration. It is built around a central `PlayerAnalyticsManager` that collects and manages data from various game systems.

## Core Components

*   **`PlayerAnalyticsManager.cs`**: The heart of the analytics system. It manages the lifecycle of a session's analytics data and is responsible for collecting data from other game components.

*   **`SessionAnalyticsData.cs`**: A data container that stores all the analytics information for a single gameplay session. This includes metrics like session duration, dodge success rates, reaction times, and death counts.

*   **`BehaviorTrendAnalyzer.cs`**: This class analyzes data across multiple sessions to identify long-term trends in player behavior, such as "rage quit" patterns.

*   **`FrustrationDetector.cs`**: This component uses a set of rules to analyze a player's session data in real-time and determine if they are likely becoming frustrated.

## Integration

The Analytics Engine is integrated with the following game systems:

*   **`GameFlowController.cs`**: Starts and ends analytics sessions.
*   **`PlayerMovement.cs`**: Tracks dodges, reaction times, and deaths.
*   **`ScoreManager.cs`**: Logs the player's peak combo.
*   **`ReviveManager.cs`**: Logs player revives.
*   **`AdaptiveDifficultyManager.cs`**: Uses the analytics data to trigger adaptive difficulty adjustments.
