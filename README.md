# 🏢 Simple Elevator System

A clean, modular simulation of an elevator system built in **C# / .NET**, following **SOLID principles** and layered architecture.  
This project demonstrates **domain-driven design (DDD)**, **state patterns**, and **service abstractions** for simulating elevator requests, movements, and coordination.

---

## 🚀 Features
- Multi-elevator & multi-floor **simulation**
- Handles **elevator requests** (calls + floor selections)
- Implements **state pattern** (`Idle`, `Moving`) for elevator behavior
- **Configurable settings** via `appsettings.json`
- **Separation of concerns** across Application, Domain, Infrastructure, UI, and Test layers
- Extensible design for future enhancements

---

## 🎮 Usage
- Run the simulation from ElevatorSystem.UI
- View elevator status in:
Console UI → text-based interface
Grid UI → visual grid display of elevators
---

## ⚙️ Getting Started

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download) or higher  
- A terminal/IDE like **Visual Studio**, **Rider**, or **VS Code**

### Installation & Run
```bash
# Clone the repository
git clone https://github.com/PradhumnPandey/Simple-Elevator-System.git

cd Simple-Elevator-System

# Build the solution
dotnet build

# Run the simulation
cd ElevatorSystem.UI
dotnet run

