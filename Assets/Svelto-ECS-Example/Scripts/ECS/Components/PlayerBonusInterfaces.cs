using System.Collections.Generic;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
	public interface IBonusSpawnerComponent : IComponent
	{
		GameObject[] points { get; set; }
		int interval { get; set; }
		GameObject prefab { get; set; }
		SpawnerTypes spawnerType { get; set; }
		int bonusItemsSpawned { get; set; }
		List<int> spawnedPoints { get; set; }
	}
}

namespace Svelto.ECS.Example.Survive.Player.Bonus
{
	public interface IPlayerAmmoBoxComponent : IComponent
	{
		bool colided { get; set; }
		int id { get; set; }

		void DestroyBox();
	}

	public interface IPlayerMedkitComponent : IPlayerAmmoBoxComponent
	{
		int healthBonus { get; }
		int id { get; set; }
		int instanceID { get; set; }
	}
}
