using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;
using System.Collections.Generic;

namespace Svelto.ECS.Example.Survive
{
	public enum SpawnerTypes
	{
		Medkit = 0,
		Ammobox = 1
	}

	public class BonusSpawnerImplementor : MonoBehaviour, IImplementor, IBonusSpawnerComponent
	{
		public GameObject[] points { get { return Points; } set { Points = value; } }
		public GameObject prefab { get { return Prefab; } set { Prefab = value; } }
		public int interval { get { return Interval; } set { Interval = value; } }
		public SpawnerTypes spawnerType { get { return SpawnerType; } set { SpawnerType = value; } }
		public int bonusItemsSpawned { get { return BonusItemsSpawned; } set { BonusItemsSpawned = value; } }
		public List<int> spawnedPoints { get { return _spawnedPoints; } set { _spawnedPoints = value; } }

		public GameObject[] Points;
		public int Interval = 3;
		public GameObject Prefab;
		public int BonusItemsSpawned = 0;
		List<int> _spawnedPoints = new List<int>();

		public SpawnerTypes SpawnerType = SpawnerTypes.Ammobox;
	}
}
