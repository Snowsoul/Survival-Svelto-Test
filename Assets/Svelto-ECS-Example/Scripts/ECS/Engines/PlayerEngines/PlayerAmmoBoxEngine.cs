using Svelto.ECS.Example.Survive.HUD;
using Svelto.ECS.Example.Survive.Player.Bonus;
using Svelto.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerAmmoBoxEngine : MultiEntityViewsEngine<HUDEntityView, PlayerAmmoboxEntityView>
	{
		public PlayerAmmoBoxEngine(ISequencer playerPickupSequence)
		{
			_playerPickupSequence = playerPickupSequence;
		}

		IEnumerator UpdateTick(int entityID)
		{
			while (true)
			{
				var playerBonusEntityView = _playerBonusEntityViews[entityID];
				var playerAmmoBoxComponent = playerBonusEntityView.playerAmmoBoxComponent;
				var bulletsManagerComponent = _hudEntityView.bulletsManagerComponent;

				// If the player colided with the ammo box and he doesn't have all the bullets then reset his bullets
				// to the initial bullets count

				if(playerAmmoBoxComponent.colided 
					&& bulletsManagerComponent.currentBullets < bulletsManagerComponent.totalBullets)
				{
					bulletsManagerComponent.ResetBullets();
					playerAmmoBoxComponent.colided = false;
					playerAmmoBoxComponent.DestroyBox();
					// Tell the Bonus Spawner Engine that the ammobox was removed from the scene
					var pickupInfo = new PickupInfo(playerAmmoBoxComponent.id, SpawnerTypes.Ammobox);
					_playerPickupSequence.Next(this, ref pickupInfo);
				}

				yield return null;
			}
		}

		protected override void Add(PlayerAmmoboxEntityView entityView)
		{
			_playerBonusEntityViews.Add(entityView);

			_taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine()
				.SetEnumerator(UpdateTick(_playerBonusEntityViews.Count -1))
				.SetScheduler(StandardSchedulers.updateScheduler);

			_taskRoutine.Start();
		}

		protected override void Remove(PlayerAmmoboxEntityView entityView)
		{
			_taskRoutine.Stop();
			_playerBonusEntityViews = null;
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
		List<PlayerAmmoboxEntityView> _playerBonusEntityViews = new List<PlayerAmmoboxEntityView>();
		ITaskRoutine _taskRoutine;
		ISequencer _playerPickupSequence;
	}
}
