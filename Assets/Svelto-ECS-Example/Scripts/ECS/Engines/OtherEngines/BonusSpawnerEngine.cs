using Svelto.ECS.Example.Survive.Player;
using Svelto.Factories;
using Svelto.Tasks;
using Svelto.Tasks.Enumerators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
	public class BonusSpawnerEngine : SingleEntityViewEngine<BonusSpawnerEntityView>, IEngine
	{
		public BonusSpawnerEngine(Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory)
		{
			_gameObjectFactory = gameobjectFactory;
			_entityFactory = entityFactory;
		}

		IEnumerator SpawnBonusItemsInterval(int interval, SpawnerTypes bonusType, int spawnerID)
		{
			while (true)
			{
				yield return new WaitForSeconds(interval);
				SpawnBonusItem(bonusType, spawnerID);
			}
		}

		void SpawnBonusItem(SpawnerTypes bonusType, int spawnerID)
		{
			int random = Random.Range(0, 5);
			var bonusSpanwerComponent = _bonusSpawnerEntityViews[spawnerID].bonusSpawnerComponent;
			var prefab = bonusSpanwerComponent.prefab;
			var points = bonusSpanwerComponent.points;
			var point = points[random];

			var go = _gameObjectFactory.Build(prefab);

			List<IImplementor> implementors = new List<IImplementor>();

			go.GetComponentsInChildren(implementors);

			switch (bonusType)
			{
				case SpawnerTypes.Ammobox:
					_entityFactory.BuildEntity<PlayerAmmoboxEntityDescriptor>(
							   go.GetInstanceID(), implementors.ToArray());
				break;

				case SpawnerTypes.Medkit:
					_entityFactory.BuildEntity<PlayerMedkitEntityDescriptor>(
							   go.GetInstanceID(), implementors.ToArray());
				break;
			}
			

			go.transform.position = point.transform.position;
			go.transform.rotation = Quaternion.identity;
		}

		protected override void Add(BonusSpawnerEntityView entityView)
		{
			_bonusSpawnerEntityViews.Add(entityView);

			var spawnerType = entityView.bonusSpawnerComponent.spawnerType;
			var interval = entityView.bonusSpawnerComponent.interval;
			var spawnerID = _bonusSpawnerEntityViews.Count - 1;

			SpawnBonusItemsInterval(interval, spawnerType, spawnerID).ThreadSafeRun();
		}

		protected override void Remove(BonusSpawnerEntityView entityView)
		{
			_bonusSpawnerEntityViews = null;
		}

		List<BonusSpawnerEntityView> _bonusSpawnerEntityViews = new List<BonusSpawnerEntityView>();
		readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
		private IGameObjectFactory _gameObjectFactory;
		private IEntityFactory _entityFactory;
	}
}
