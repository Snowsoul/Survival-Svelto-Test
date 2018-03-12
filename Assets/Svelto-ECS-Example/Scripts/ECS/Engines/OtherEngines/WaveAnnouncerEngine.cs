using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.HUD
{
	public class WaveAnnouncerEngine : SingleEntityViewEngine<HUDEntityView>, IStep<WaveStartInfo>
	{
		public WaveAnnouncerEngine(ISequencer enemySpawnSequencer)
		{
			_enemySpawnSequencer = enemySpawnSequencer;
			IntervaledTick().Run();
		}

		void SpawnWave(int enemies, float scale)
		{
			var waveStartInfo = new WaveStartInfo(enemies, scale);
			_enemySpawnSequencer.Next(this, ref waveStartInfo);
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

					if (!_waveSpawned)
					{
						SpawnWave(_enemies, _scale);
						_waveSpawned = true;
					}
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

		public void Step(ref WaveStartInfo token, int condition)
		{
			if (condition == WaveStatus.Stop)
			{
				_waveCounter++;
				_enemies = _enemies * _waveCounter;

				if (_scale < 3f)
				{
					_scale = _scale + 0.5f;
				}

				_guiEntityView.waveWaitingTimeComponent.Reset();
				_guiEntityView.enemiesLeftComponent.isEnabled = false;
				_waveSpawned = false;
			}
		}

		bool _waveSpawned = false;
		HUDEntityView _guiEntityView;
		ISequencer _enemySpawnSequencer;
		int _enemies = 2;
		float _scale = 1;
		int _waveCounter = 1;
		readonly WaitForSecondsEnumerator _waitForSeconds = new WaitForSecondsEnumerator(1);
	}
}