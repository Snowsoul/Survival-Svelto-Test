using Svelto.ECS.Example.Survive.HUD;
using Svelto.ECS.Example.Survive.Player.Bonus;
using Svelto.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerMedkitEngine : MultiEntityViewsEngine<HUDEntityView, PlayerAmmoboxEntityView, PlayerMedkitEntityView>, IEngine
	{
		public PlayerMedkitEngine(ISequencer playerHealSequence)
		{
			_playerHealSequence = playerHealSequence;
		}

		IEnumerator UpdateTick(int spawnerID)
		{
			while (true)
			{
				var playerMedkitEntityView = _playerMedkitEntityViews[spawnerID];

				if (playerMedkitEntityView == null)
					yield return null;

				var playerMedkitComponent = playerMedkitEntityView.playerMedkitComponent;
				var healthSliderComponent = _hudEntityView.healthSliderComponent;

				// If the player colided with the ammo box and he doesn't have all the bullets then reset his bullets
				// to the initial bullets count

				if(playerMedkitComponent.colided)
				{
					//healthSliderComponent.value += playerMedkitComponent.healthBonus;
					playerMedkitComponent.colided = false;
					playerMedkitComponent.DestroyBox();

					var healInfo = new HealInfo(playerMedkitComponent.healthBonus, playerMedkitComponent.id);
					_playerHealSequence.Next(this, ref healInfo);
				}

				yield return null;
			}
		}

		protected override void Add(PlayerAmmoboxEntityView entityView)
		{
			_playerAmmoboxEntityView = entityView;
		}

		protected override void Remove(PlayerAmmoboxEntityView entityView)
		{
			_taskRoutine.Stop();
			_playerAmmoboxEntityView = null;
		}

		protected override void Add(HUDEntityView entityView)
		{
			_hudEntityView = entityView;
		}

		protected override void Remove(HUDEntityView entityView)
		{
			_hudEntityView = null;
		}

		protected override void Add(PlayerMedkitEntityView entityView)
		{
			_playerMedkitEntityViews.Add(entityView);

			_taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine()
				.SetEnumerator(UpdateTick(_playerMedkitEntityViews.Count - 1))
				.SetScheduler(StandardSchedulers.updateScheduler);

			_taskRoutine.Start();

		}

		protected override void Remove(PlayerMedkitEntityView entityView)
		{
			_playerMedkitEntityViews = null;
		}

		HUDEntityView _hudEntityView;
		PlayerAmmoboxEntityView _playerAmmoboxEntityView;
		List<PlayerMedkitEntityView> _playerMedkitEntityViews = new List<PlayerMedkitEntityView>();
		ITaskRoutine _taskRoutine;
		ISequencer _playerHealSequence;
	}
}
