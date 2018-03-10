using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.HUD
{
	public class WaveAnnouncerEngine : SingleEntityViewEngine<HUDEntityView>
	{
		public WaveAnnouncerEngine()
		{
			IntervaledTick().Run();
		}

		IEnumerator IntervaledTick()
		{
			while (true)
			{	
				yield return _waitForSeconds;

				if (_guiEntityView.waveWaitingTimeComponent.secondsRemaining > 1)
				{
					_guiEntityView.waveWaitingTimeComponent.secondsRemaining -= 1;
				}
				else
				{
					_guiEntityView.enemiesLeftComponent.isEnabled = true;

					yield return null;
					break;
				}
			}
		}

		protected override void Add(HUDEntityView entityView)
		{
			_guiEntityView = entityView;
		}

		protected override void Remove(HUDEntityView entityView)
		{
			_guiEntityView = null;
		}

		HUDEntityView _guiEntityView;
		readonly WaitForSecondsEnumerator _waitForSeconds = new WaitForSecondsEnumerator(1);
	}
}