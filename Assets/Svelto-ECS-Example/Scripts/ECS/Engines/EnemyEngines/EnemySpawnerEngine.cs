using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using UnityEngine;
using Svelto.ECS.Example.Survive.HUD;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemySpawnerEngine : SingleEntityViewEngine<HUDEntityView>, IEngine, IStep<WaveStartInfo>, IStep<DamageInfo>
    {
        public EnemySpawnerEngine(Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory, ISequencer enemySpawnSequence)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
			_enemySpawnSequence = enemySpawnSequence;


		}

        IEnumerator IntervaledTick()
        {
//OK this is of fundamental importance: Never create implementors as Monobehaviour just to hold 
//data. Data should always been retrieved through a service layer regardless the data source.
//The benefit are numerous, including the fact that changing data source would require
//only changing the service code. In this simple example I am not using a Service Layer
//but you can see the point.          
//Also note that I am loading the data only once per application run, outside the 
//main loop. You can always exploit this trick when you now that the data you need
//to use will never change            
            var enemiestoSpawn = ReadEnemySpawningDataServiceRequest();

            while (true)
            {
//Svelto.Tasks allows to yield UnityYield instructions but this comes with a performance hit
//so the fastest solution is always to use custom enumerators. To be honest the hit is minimal
//but it's better to not abuse it.                
                yield return _waitForSecondsEnumerator;

                if (enemiestoSpawn != null)
                {
                    for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        var spawnData = enemiestoSpawn[i];

                        if (spawnData.timeLeft <= 0.0f)
                        {
                            // Find a random index between zero and one less than the number of spawn points.
                            int spawnPointIndex = Random.Range(0, spawnData.spawnPoints.Length);

                            // Create an instance of the enemy prefab at the randomly selected spawn point position and rotation.
                            var go = _gameobjectFactory.Build(spawnData.enemyPrefab);
                            
                            //I have been lazy here and retrieving the data directly from the Monobehaviour
                            //but still I am not creating an implementor for the purpose!
                            var data = go.GetComponent<EnemyAttackDataHolder>();
                            
                            //we are going to use a mix of implementors as Monobehaviours and
                            //normal implementors here:                           
                            List<IImplementor> implementors = new List<IImplementor>();
                            go.GetComponentsInChildren(implementors);
                            implementors.Add(new EnemyAttackImplementor(data.timeBetweenAttacks, data.attackDamage));
                            
                            //In this example every kind of enemy generates the same list of EntityViews
                            //therefore I always use the same EntityDescriptor. However if the 
                            //different enemies had to create different EntityViews for different
                            //engines, this would have been a good example where EntityDescriptorHolder
                            //could have been used to exploit the the kind of polymorphism explained
                            //in my articles.
                            _entityFactory.BuildEntity<EnemyEntityDescriptor>(
                                go.GetInstanceID(), implementors.ToArray());

                            var transform = go.transform;
                            var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                            transform.position = spawnInfo.position;
                            transform.rotation = spawnInfo.rotation;
							transform.localScale = new Vector3(_enemiesData.enemiesScale, _enemiesData.enemiesScale, _enemiesData.enemiesScale);

                            spawnData.timeLeft = spawnData.spawnTime;
                            _numberOfEnemyToSpawn--;
                        }

                        spawnData.timeLeft -= 1.0f;
                    }
                }
            }
        }

        static JSonEnemySpawnData[] ReadEnemySpawningDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/EnemySpawningData.json");
            
            JSonEnemySpawnData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemySpawnData>(json);
            
            return enemiestoSpawn;
        }

		void UpdateEnemiesLeftHUD(int enemiesLeft)
		{
			_hudEntityView.enemiesLeftComponent.enemies = enemiesLeft;

		}

		public void Step(ref WaveStartInfo token, int condition)
		{
			_enemiesData = token;
			_numberOfEnemyToSpawn = _enemiesData.enemiesToSpawn;
			_enemiesSpawned = _numberOfEnemyToSpawn;

			UpdateEnemiesLeftHUD(_enemiesSpawned);

			IntervaledTick().Run();
		}

		protected override void Add(HUDEntityView entityView)
		{
			_hudEntityView = entityView;
		}

		protected override void Remove(HUDEntityView entityView)
		{
			_hudEntityView = null;
		}

		public void Step(ref DamageInfo token, int condition)
		{
			if (condition == DamageCondition.Dead)
			{
				if (_enemiesSpawned > 1)
				{
					_enemiesSpawned--;
					UpdateEnemiesLeftHUD(_enemiesSpawned);
				}
				else
				{
					var spawnInfo = new WaveStartInfo(0);
					_enemySpawnSequence.Next(this, ref spawnInfo);
				}
			}
		}

		readonly Factories.IGameObjectFactory   _gameobjectFactory;
        readonly IEntityFactory                 _entityFactory;
		private readonly ISequencer _enemySpawnSequence;
		readonly WaitForSecondsEnumerator       _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);

        int     _numberOfEnemyToSpawn;
		int     _enemiesSpawned;
		WaveStartInfo _enemiesData;
		private HUDEntityView _hudEntityView;
	}
}
