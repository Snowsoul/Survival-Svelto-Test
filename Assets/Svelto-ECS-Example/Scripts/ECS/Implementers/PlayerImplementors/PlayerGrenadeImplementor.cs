using Svelto.ECS.Example.Survive.Player.Gun;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerGrenadeImplementor: MonoBehaviour, IImplementor, IGrenadeComponent
    {
		public GameObject grenadePrefab { set { GrenadePrefab = value; } get { return GrenadePrefab; } }
		public Vector3 position { get { return transform.position; } }
		public GameObject GrenadePrefab;
    }
}