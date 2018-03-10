using Svelto.ECS.Example.Survive.HUD;
using Svelto.ECS.Example.Survive.Player.Bonus;
using Svelto.Tasks;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerMedkitEngine : MultiEntityViewsEngine<HUDEntityView, PlayerBonusEntitityView>
	{
		public PlayerMedkitEngine(ISequencer playerHealSequence)
		{
			_taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(UpdateTick()).SetScheduler(StandardSchedulers.updateScheduler);
			_playerHealSequence = playerHealSequence;
		}

		IEnumerator UpdateTick()
		{
			while (true)
			{
				var playerMedkitComponent = _playerBonusEntityView.playerMedkitComponent;
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
		ISequencer _playerHealSequence;
	}
}
