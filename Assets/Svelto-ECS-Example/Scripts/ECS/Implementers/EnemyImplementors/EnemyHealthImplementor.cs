using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyHealthImplementor : MonoBehaviour, IImplementor, IDestroyComponent, IHealthComponent
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.

        public int currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
		public int maxHealth { get; set; }

		void Awake ()
        {    // Setting the current health when the enemy first spawns.
            _currentHealth = startingHealth;
			maxHealth = currentHealth;
			destroyed = new DispatchOnChange<bool>(GetInstanceID());
            destroyed.NotifyOnValueSet(OnDestroyed);
        }

        void OnDestroyed(int sender, bool isDestroyed)
        {
            Destroy(gameObject);
        }

        public DispatchOnChange<bool> destroyed { get; private set; }

        int             _currentHealth;        // The current health the enemy has.
    }
}
