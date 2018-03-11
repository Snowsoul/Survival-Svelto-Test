using Svelto.ECS.Example.Survive.Player.Gun;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerGrenadeImplementor: MonoBehaviour, IImplementor, IGrenadeComponent
    {
		public bool spawned { set; get; }
		public GameObject instance { set; get; }

		public PlayerGrenadeImplementor()
		{

		}
    }
}