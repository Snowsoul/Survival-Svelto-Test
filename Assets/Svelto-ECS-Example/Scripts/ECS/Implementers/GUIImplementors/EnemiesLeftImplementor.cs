using Svelto.ECS.Example.Survive.HUD;
using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
	public class EnemiesLeftImplementor : MonoBehaviour, IImplementor, IEnemiesLeftComponent
	{
		public int enemies { get { return _enemies; } set { _enemies = value; } }
		public bool isEnabled { get { return _enabled; } set { _enabled = value; _text.enabled = _enabled; } }

		void Awake()
		{
			// Set up the reference.
			_text = GetComponent<Text>();
		}

		int _enemies;
		bool _enabled = false;

		Text _text;
	}
}