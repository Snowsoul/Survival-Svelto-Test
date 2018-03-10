using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerAmmoBoxImplementor : MonoBehaviour, IImplementor, IPlayerAmmoBoxComponent
	{
		public bool colided { get { return _colided; } set { _colided = value; } }

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				colided = true;
				Destroy(this.gameObject);
			}
		}

		bool _colided = false;
	}
}
