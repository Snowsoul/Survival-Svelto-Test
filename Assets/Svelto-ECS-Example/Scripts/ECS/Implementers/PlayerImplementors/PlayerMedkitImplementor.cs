using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerMedkitImplementor : MonoBehaviour, IImplementor, IPlayerMedkitComponent
	{
		public bool colided { get { return _colided; } set { _colided = value; } }
		public int healthBonus { get { return _healthBonus; } }
		public int id { get { return _id; } set { _id = value; } }

		public void DestroyBox()
		{
			Destroy(gameObject);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				colided = true;
				_id = other.gameObject.GetInstanceID();
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.tag == "Player")
				colided = false;
		}

		bool _colided = false;
		int _healthBonus = 50;
		int _id;
	}
}
