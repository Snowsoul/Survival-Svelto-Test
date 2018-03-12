using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerAmmoBoxImplementor : MonoBehaviour, IImplementor, IPlayerAmmoBoxComponent
	{
		public bool colided { get { return _colided; } set { _colided = value; } }
		public int id { get; set; }

		public void DestroyBox()
		{
			Destroy(gameObject);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
				colided = true;
		}

		void OnTriggerExit(Collider other)
		{
			if (other.tag == "Player")
				colided = false;
		}

		bool _colided = false;
	}
}
