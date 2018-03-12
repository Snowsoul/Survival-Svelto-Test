using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerMedkitImplementor : MonoBehaviour, IImplementor, IPlayerMedkitComponent
	{
		public bool colided { get { return _colided; } set { _colided = value; } }
		public int healthBonus { get { return _healthBonus; } }
		public int id { get; set; }
		public int instanceID { get; set; }

		public void DestroyBox()
		{
			Destroy(gameObject);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				instanceID = other.gameObject.GetInstanceID();
				colided = true;
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.tag == "Player")
				colided = false;
		}

		bool _colided = false;
		int _healthBonus = 50;
	}
}
