using Svelto.ECS.Example.Survive.Player.Gun;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerGrenadeImplementor: MonoBehaviour, IImplementor, IGrenadeComponent
    {
		public bool spawned {
			set {
				_spawned = value;
				if (!_spawned)
					Destroy(gameObject);
			}
			get { return _spawned; }
		}
		public bool isExploding {
			set { _isExploding = value; }
			get { return _isExploding; }
		}

		public GameObject instance { get { return gameObject; } }


		bool _spawned = false;
		bool _isExploding = false;
    }
}