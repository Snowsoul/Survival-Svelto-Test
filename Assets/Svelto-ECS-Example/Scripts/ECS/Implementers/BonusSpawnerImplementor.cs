using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;

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

		public GameObject[] Points;
		public int Interval = 3;
		public GameObject Prefab;

		public SpawnerTypes SpawnerType = SpawnerTypes.Ammobox;
	}
}
