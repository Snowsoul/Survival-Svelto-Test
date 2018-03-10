using Svelto.ECS.Example.Survive.HUD;
using Svelto.ECS.Example.Survive.Player.Bonus;
using Svelto.Tasks;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerAmmoBoxEngine : MultiEntityViewsEngine<HUDEntityView, PlayerBonusEntitityView>
	{
		public PlayerAmmoBoxEngine()
		{
			_taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(UpdateTick()).SetScheduler(StandardSchedulers.updateScheduler);
		}

		IEnumerator UpdateTick()
		{
			while (true)
			{
				var playerAmmoBoxComponent = _playerBonusEntityView.playerAmmoBoxComponent;
				var bulletsManagerComponent = _hudEntityView.bulletsManagerComponent;

				if(playerAmmoBoxComponent.colided)
				{
					bulletsManagerComponent.ResetBullets();
					playerAmmoBoxComponent.colided = false;
				}

				yield return null;
			}
		}

		protected override void Add(PlayerBonusEntitityView entityView)
		{
			_playerBonusEntityView = entityView;
			_taskRoutine.Start();
		}

		protected override void Remove(PlayerBonusEntitityView entityView)
		{
			_taskRoutine.Stop();
			_playerBonusEntityView = null;
		}

		protected override void Add(HUDEntityView entityView)
		{
			_hudEntityView = entityView;
		}

		protected override void Remove(HUDEntityView entityView)
		{
			_hudEntityView = null;
		}

		

		HUDEntityView _hudEntityView;
		PlayerBonusEntitityView _playerBonusEntityView;
		readonly ITaskRoutine _taskRoutine;
	}
}
