using UnityEngine;

namespace RaiNa.Unity.Samples
{
    public sealed class GenericTest : MonoBehaviour
    {
        private readonly Healable _human = new Human();
        private readonly Healable _enemy = new Enemy();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                HealingTower.Heal(_human);
            
            if (Input.GetKeyDown(KeyCode.A))
                HealingTower.Heal(_enemy);
        }
    }

    public static class HealingTower
    {
        public static void Heal<T>(T healable, int amout = 100) where T : Healable
        {
            var currentHP = healable.Health;
            healable.Heal(amout);
            Debug.Log($"HealTowerHeal: {healable.ID} from {currentHP} to {healable.Health}");
        }
    }

    public sealed class Human : Healable
    {
        public override string ID => id;
        private readonly string id = "Human";
    }

    public sealed class Enemy : Healable
    {
        public override string ID => id;
        private readonly string id = "Enemy";
    }

    public abstract class Healable
    {
        public abstract string ID { get; }
        public int Health { get; private set; } = 100;

        public void Heal(int amount) => Health += amount;
    }
}
