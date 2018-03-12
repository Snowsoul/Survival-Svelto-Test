using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Survive.Enemies;
using Svelto.Tasks;
using Svelto.ECS.Example.Survive.HUD;
using Svelto.Factories;
using System.Collections.Generic;

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class PlayerGunShootingEngine : MultiEntityViewsEngine<GrenadeEntityView, GunEntityView, PlayerEntityView, HUDEntityView>, 
        IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            _taskRoutine.Start();
        }
        
        public PlayerGunShootingEngine(EnemyKilledObservable enemyKilledObservable, ISequencer damageSequence, 
			IRayCaster rayCaster, ITime time, Factories.IGameObjectFactory gameobjectFactory, 
			ISequencer grenadeDropSequence, IEntityFactory entityFactory)
        {
            _enemyKilledObservable = enemyKilledObservable;
            _enemyDamageSequence   = damageSequence;
            _rayCaster             = rayCaster;
            _time                  = time;
            _taskRoutine           = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick())
                                               .SetScheduler(StandardSchedulers.physicScheduler);
			_gameObjectFactory = gameobjectFactory;
			_entityFactory = entityFactory;
		}

		protected override void Add(HUDEntityView entityView)
		{
			_hudEntityView = entityView;
		}

		protected override void Remove(HUDEntityView entityView)
		{
			_hudEntityView = null;
		}

		protected override void Add(GunEntityView entityView)
        {
            _playerGunEntityView = entityView;
        }

        protected override void Remove(GunEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerGunEntityView = null;
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _playerEntityView = entityView;
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerEntityView = null;
        }

        IEnumerator Tick()
        {
            while (_playerEntityView == null || _playerGunEntityView == null) yield return null;
            
            while (true)
            {
                var playerGunComponent = _playerGunEntityView.gunComponent;

                playerGunComponent.timer += _time.deltaTime;
                
                if (_playerEntityView.inputComponent.fire &&
                    playerGunComponent.timer >= _playerGunEntityView.gunComponent.timeBetweenBullets)
                    Shoot(_playerGunEntityView);

				if (_playerEntityView.inputComponent.rmb)
					LaunchGrenade();

                yield return null;
            }
        }

		IEnumerator ResetGrenadeCooldownAfterTime()
		{
			// Reset the grenade cooldown after 2 seconds and update the slider
			// Could be done better... but will do for now to show the functionality

			_hudEntityView.grenadeHUDComponent.sliderValue = 0;
			yield return new WaitForSeconds(0.5f);
			_hudEntityView.grenadeHUDComponent.sliderValue = 0.25f;
			yield return new WaitForSeconds(0.5f);
			_hudEntityView.grenadeHUDComponent.sliderValue = 0.50f;
			yield return new WaitForSeconds(0.5f);
			_hudEntityView.grenadeHUDComponent.sliderValue = 0.75f;
			yield return new WaitForSeconds(0.5f);
			_hudEntityView.grenadeHUDComponent.sliderValue = 1f;

			var grenadeSpawnerComponent = _playerGunEntityView.grenadeSpawnerComponent;
			grenadeSpawnerComponent.grenadeCooldown = false;
		}

		void LaunchGrenade()
		{
			var grenadeSpawnerComponent = _playerGunEntityView.grenadeSpawnerComponent;

			if (!grenadeSpawnerComponent.grenadeCooldown)
			{
				var go = _gameObjectFactory.Build(grenadeSpawnerComponent.grenadePrefab);

				List<IImplementor> implementors = new List<IImplementor>();

				go.GetComponentsInChildren(implementors);

				_entityFactory.BuildEntity<GrenadeEntityDescriptor>(
							   go.GetInstanceID(), implementors.ToArray());

				go.transform.position = grenadeSpawnerComponent.position;
				go.transform.rotation = Quaternion.identity;

				grenadeSpawnerComponent.grenadeCooldown = true;

				ResetGrenadeCooldownAfterTime().Run();
			}
		}

		void UpdateBulletsHUD()
		{
			_hudEntityView.bulletsManagerComponent.RemoveBullet();
		}

        void Shoot(GunEntityView playerGunEntityView)
        {
            var playerGunComponent    = playerGunEntityView.gunComponent;
            var playerGunHitComponent = playerGunEntityView.gunHitTargetComponent;
			var bulletsManagerComponent = _hudEntityView.bulletsManagerComponent;

			// If the player doesn't have enough bullets then don't shoot anymore
			if (!bulletsManagerComponent.HasEnoughBullets())
				return;

			playerGunComponent.timer = 0;

            Vector3 point;
            var     entityHit = _rayCaster.CheckHit(playerGunComponent.shootRay, playerGunComponent.range, ENEMY_LAYER, SHOOTABLE_MASK | ENEMY_MASK, out point);

			UpdateBulletsHUD();

            if (entityHit != -1)
            {
                PlayerTargetEntityView targetComponent;
                //note how the GameObject GetInstanceID is used to identify the entity as well
                if (entityViewsDB.TryQueryEntityView(entityHit, out targetComponent))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, point, entityHit, EntityDamagedType.PlayerTarget);
                    _enemyDamageSequence.Next(this, ref damageInfo);

                    playerGunComponent.lastTargetPosition = point;
                    playerGunHitComponent.targetHit.value = true;

                    return;
                }
            }

            playerGunHitComponent.targetHit.value = false;
        }

        void OnTargetDead(int targetID)
        {
            ///
            /// Pay attention to this bit. The engine is querying a
            /// PaleryTargetEntityView and not a EnemyEntityView.
            /// this is more than a sophistication, it actually the implementation
            /// of the rule that every engine must use its own set of
            /// EntityViews to promote encapsulation and modularity
            ///
            var playerTarget = entityViewsDB.QueryEntityView<PlayerTargetEntityView>(targetID);
            var targetType   = playerTarget.playerTargetComponent.targetType;

            _enemyKilledObservable.Dispatch(ref targetType);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            OnTargetDead(token.entityDamagedID);
        }

		protected override void Add(GrenadeEntityView entityView)
		{
			Debug.Log("add grenade?");
			_grenadeEntityView = entityView;
			var grenadeComponent = _grenadeEntityView.grenadeComponent;

			grenadeComponent.spawned = true;
		}

		protected override void Remove(GrenadeEntityView entityView)
		{
			_grenadeEntityView = null;
		}

		readonly EnemyKilledObservable _enemyKilledObservable;
        readonly ISequencer            _enemyDamageSequence;
        readonly IRayCaster            _rayCaster;

        PlayerEntityView _playerEntityView;
        GunEntityView    _playerGunEntityView;
        GrenadeEntityView    _grenadeEntityView;
		HUDEntityView _hudEntityView;
        
        readonly ITime _time;
        readonly ITaskRoutine     _taskRoutine;
		readonly IGameObjectFactory _gameObjectFactory;
		private readonly IEntityFactory _entityFactory;
		static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK     = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER    = LayerMask.NameToLayer("Enemies");
    }
}