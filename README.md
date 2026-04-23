# 🚀 Endless Runner Mobile Game (Unity)

## 📌 Overview
A **production-grade Android game** built using Unity and C#, designed with a **modular, scalable architecture** and optimized for **real-time performance**.

This project demonstrates strong fundamentals in **system design, game physics, performance optimization, and backend integration**, aligned with production-level development practices.


## 🧠 Core Features

- 🎮 **Real-Time Gameplay Systems**
  - Physics-based movement and collision detection  
  - Smooth and responsive gameplay loop  

- 🤖 **AI-Driven Adaptive Difficulty**
  - Dynamic difficulty adjustment using **decision logic and behavioral patterns**  
  - Enhances engagement by adapting to player performance  

- ⚡ **Performance Optimization**
  - Object pooling to reduce memory allocations  
  - Efficient memory management and frame optimization  
  - Designed for **low-latency execution on mobile devices**  

- 🔗 **Backend Integration**
  - Firebase integration for analytics and real-time data tracking  
  - Enables monitoring of player behavior and system performance  

- 💰 **Monetization + Deployment**
  - Production-ready build pipeline  
  - Monetization system aligned with Play Store standards  


## ⚙️ System Architecture

- **Architecture:** Modular, event-driven system  
- **Core Systems:** GameManager, ObjectPool, LevelGenerator  
- **Design Pattern:** Singleton + Decoupled components  
- **Data Flow:** Gameplay → Events → Managers → UI  

### 📂 Project Structure
Assets/Scripts/
├── Core/ # GameManager, Singleton, ObjectPool
├── Gameplay/ # PlayerController, LevelGenerator, ObstacleSpawner
├── Managers/ # UIManager, ScoreManager, CurrencyManager
├── UI/ # UIPanel system and UI controllers
├── SceneSetup/ # Automated scene initialization


## 🚀 Setup Instructions

### 🏠 Home Scene
- Attach `HomeSceneSetup.cs` to initialize managers and UI  
- Auto-generates GameManager, UIManager, ScoreManager, CurrencyManager  

### 🎯 Main Scene
- Attach `MainSceneSetup.cs`  
- Assign Player prefab and Track prefabs  
- Automatically initializes gameplay systems and level generation  


## 📈 Impact

- ⚡ Reduced runtime overhead using **object pooling + optimized memory handling**  
- 🎯 Improved gameplay responsiveness through **real-time system optimization**  
- 📱 Ensured smooth performance across **low-end Android devices**  


## 🛠️ Tech Stack

- **Engine:** Unity  
- **Language:** C#  
- **Backend:** Firebase  
- **Concepts:** System Design, DSA, Real-time Systems, Optimization  


## 💡 Key Highlights

- Built as a **production-grade system**, not just a prototype  
- Demonstrates **end-to-end SDLC ownership**  
- Combines **game development + backend + performance engineering**  


## 🔗 Repository

👉 [View Source Code](https://github.com/sudhindra62/Endless_Runner_unity.git)


## 🚀 Future Improvements

- Advanced AI-based player behavior modeling  
- Enhanced analytics and personalization systems  
- Expanded monetization strategies  
