using RaiNa.Services.Dependency;
using RaiNa.Unity.Services.Dependency;
using UnityEngine;

namespace RaiNa.Unity.Services.Examples
{
    // Example usage of the global ServiceContainer.
    // This demonstrates how to register and resolve services using the global container.
    public class ServiceContainerUsageExample : MonoBehaviour {
        private void Start() {
            // Access the global container
            var globalContainer = ServiceContainerInitializer.Global;

            // Register a service (example: a game manager or utility service)
            // Singleton services are created once and shared across the application
            globalContainer.Register<IGameManager>(
                ServiceLifetime.Singleton,
                container => new GameManager());

            // Resolve the service
            var gameManager = globalContainer.Resolve<IGameManager>();
            gameManager.Initialize();

            // Create a child container for scoped services (e.g., for a level or scene)
            var levelContainer = globalContainer.CreateChildContainer("Level1");
            levelContainer.Register<ILevelController>(
                ServiceLifetime.Scoped,
                container => new LevelController());

            // Resolve from the child container
            var levelController = levelContainer.Resolve<ILevelController>();
            levelController.Setup();
        }
    }

    // Example service interfaces
    public interface IGameManager {
        void Initialize();
    }

    public interface ILevelController {
        void Setup();
    }

    // Example service implementations
    public class GameManager : IGameManager {
        public void Initialize() =>
            Debug.Log("GameManager initialized through global ServiceContainer");
    }

    public class LevelController : ILevelController {
        public void Setup() =>
            Debug.Log("LevelController setup in child container");
    }
}
