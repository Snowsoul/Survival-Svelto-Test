using Svelto.ECS.Example.Survive.Player.Gun;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerGrenadeSpanwerImplementor: MonoBehaviour, IImplementor, IGrenadeSpawnerComponent
    {
		public GameObject grenadePrefab { set { GrenadePrefab = value; } get { return GrenadePrefab; } }
		public bool grenadeCooldown { set { _grenadeCooldown = value; } get { return _grenadeCooldown; } }
		public Vector3 position { get { return transform.position; } }
		public GameObject GrenadePrefab;

		bool _grenadeCooldown = false;
    }
}