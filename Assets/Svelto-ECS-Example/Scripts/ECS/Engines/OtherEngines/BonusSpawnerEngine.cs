using Svelto.ECS.Example.Survive.Player;
using Svelto.ECS.Example.Survive.Player.Bonus;
using Svelto.Factories;
using Svelto.Tasks;
using Svelto.Tasks.Enumerators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
	public class BonusSpawnerEngine : MultiEntityViewsEngine<BonusSpawnerEntityView, PlayerAmmoboxEntityView, PlayerMedkitEntityView>, IEngine, IStep<PickupInfo>
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

		int FindRandomPoint(int max, List<int> spawnedPoints)
		{
			var pointIndex = Random.Range(0, max);
			// Search for a random index that is not already in use using recursion
			return !spawnedPoints.Contains(pointIndex) ? pointIndex : FindRandomPoint(max, spawnedPoints);
		}

		void SpawnBonusItem(SpawnerTypes bonusType, int spawnerID)
		{
			var bonusSpanwerComponent = _bonusSpawnerEntityViews[spawnerID].bonusSpawnerComponent;
			var prefab = bonusSpanwerComponent.prefab;
			var points = bonusSpanwerComponent.points;
			int random = FindRandomPoint(points.Length - 1, bonusSpanwerComponent.spawnedPoints);
			var point = points[random];

			if (!(bonusSpanwerComponent.bonusItemsSpawned < _maxItemsToSpawn))
				return;

			bonusSpanwerComponent.spawnedPoints.Add(random);

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

			bonusSpanwerComponent.bonusItemsSpawned += 1;
		}

		protected override void Add(BonusSpawnerEntityView entityView)
		{
			_bonusSpawnerEntityViews.Add(entityView);

			var spawnerType = entityView.bonusSpawnerComponent.spawnerType;
			var interval = entityView.bonusSpawnerComponent.interval;
			var spawnerID = _bonusSpawnerEntityViews.Count - 1;

			SpawnBonusItemsInterval(interval, spawnerType, spawnerID).Run();
		}

		protected override void Remove(BonusSpawnerEntityView entityView)
		{
			_bonusSpawnerEntityViews = null;
		}

		protected override void Add(PlayerAmmoboxEntityView entityView)
		{
			
			var bonusSpawnerEntityView = _bonusSpawnerEntityViews.Find(
				entity => entity.bonusSpawnerComponent.spawnerType == SpawnerTypes.Ammobox
			);

			if (bonusSpawnerEntityView != null)
			{
				var bonusSpawnerComponent = bonusSpawnerEntityView.bonusSpawnerComponent;
				var index = bonusSpawnerComponent.spawnedPoints[bonusSpawnerComponent.spawnedPoints.Count - 1];
				entityView.playerAmmoBoxComponent.id = index;
			}

		}

		protected override void Remove(PlayerAmmoboxEntityView entityView)
		{
			
		}

		protected override void Add(PlayerMedkitEntityView entityView)
		{
			var bonusSpawnerEntityView = _bonusSpawnerEntityViews.Find(
				entity => entity.bonusSpawnerComponent.spawnerType == SpawnerTypes.Medkit
			);

			if (bonusSpawnerEntityView != null)
			{
				var bonusSpawnerComponent = bonusSpawnerEntityView.bonusSpawnerComponent;
				var index = bonusSpawnerComponent.spawnedPoints[bonusSpawnerComponent.spawnedPoints.Count - 1];
				entityView.playerMedkitComponent.id = index;
			}
		}
		protected override void Remove(PlayerMedkitEntityView entityView)
		{
			
		}

		public void Step(ref PickupInfo token, int condition)
		{
			var type = token.type;
			var id = token.pickupID;

			var bonusSpawnerEntityView = _bonusSpawnerEntityViews.Find(
				entity => entity.bonusSpawnerComponent.spawnerType == type
			);

			var spawnedPoints = bonusSpawnerEntityView.bonusSpawnerComponent.spawnedPoints;
			var index = spawnedPoints.FindIndex(pointIndex => pointIndex == id);

			bonusSpawnerEntityView.bonusSpawnerComponent.spawnedPoints.RemoveAt(index);
			bonusSpawnerEntityView.bonusSpawnerComponent.bonusItemsSpawned -= 1;
		}

		List<BonusSpawnerEntityView> _bonusSpawnerEntityViews = new List<BonusSpawnerEntityView>();
		readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
		private IGameObjectFactory _gameObjectFactory;
		private IEntityFactory _entityFactory;

		int _maxItemsToSpawn = 5;
	}
}
