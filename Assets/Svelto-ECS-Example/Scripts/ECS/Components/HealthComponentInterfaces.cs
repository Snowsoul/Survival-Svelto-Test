using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    public interface IHealthComponent : IComponent
    {
        int currentHealth { get; set; }
		int maxHealth { get; set; }
    }

	public struct HealInfo
	{
		public int healAmmount { get; private set; }
		public int entityHealID { get; private set; }

		public HealInfo(int heal, int entity) : this()
		{
			healAmmount = heal;
			entityHealID = entity;
		}
	}

    public struct DamageInfo
    {
        public int damagePerShot { get; private set; }
        public Vector3 damagePoint { get; private set; }
        public int entityDamagedID { get; private set; }
        public EntityDamagedType entityType  { get; private set; }
        
        public DamageInfo(int damage, Vector3 point, int entity, EntityDamagedType edt) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
            entityDamagedID = entity;
            entityType = edt;
        }
    }

	public struct PickupInfo
	{
		public int pickupID { get; set; }
		public SpawnerTypes type { get; set; }

		public PickupInfo(int id, SpawnerTypes pickupType)
		{
			pickupID = id;
			type = pickupType;
		}
	}

	public struct WaveStartInfo
	{
		public int enemiesToSpawn { get; set; }
		public float enemiesScale { get; set; }

		public WaveStartInfo(int enemiesCount = 0, float scale = 1)
		{
			enemiesToSpawn = enemiesCount;
			enemiesScale = scale;
		}
	}

    public enum EntityDamagedType
    {
        EnemyTarget,
        PlayerTarget
    }
}
    
