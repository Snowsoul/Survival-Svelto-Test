using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
	public class BulletsManagerImplementor : MonoBehaviour, IImplementor, IBulletsManagerComponent
	{

		void Awake()
		{
			_currentBullets = _totalBullets;
		}

		public int totalBullets
		{
			get { return _totalBullets; }
			set { _totalBullets = value; }
		}

		public int currentBullets
		{
			get { return _currentBullets; }
			set { _currentBullets = value; }
		}

		public bool HasEnoughBullets()
		{
			return _currentBullets > 0;
		}

		public void RemoveBullet()
		{
			if (HasEnoughBullets())
			{
				_currentBullets -= 1;
				bullets[_currentBullets].enabled = false;
			}
		}

		public void ResetBullets()
		{
			_currentBullets = 10;

			foreach (Image bullet in bullets)
			{
				bullet.enabled = true;
			}
		}


		int _totalBullets = 10;
		int _currentBullets;
		public Image[] bullets;
	}
}