using Svelto.ECS.Example.Survive.Player.Gun;
using Svelto.Tasks;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerGrenadeEngine: MultiEntityViewsEngine<GunEntityView, GrenadeEntityView>, IEngine
    {
		IEnumerator UpdateTick()
		{
			while(true)
			{
				var grenadeComponent = _grenadeEntityView.grenadeComponent;
				var grenadeSpawnerComponent = _gunEntityView.grenadeSpawnerComponent;

				if (grenadeComponent.spawned && !grenadeComponent.isExploding)
				{
					grenadeComponent.isExploding = true;
					LateExplode().Run();
				}

				yield return null;
			}
		}


		IEnumerator LateExplode()
		{
			yield return new WaitForSeconds(0.7f);
			Explode();
			yield return new WaitForSeconds(1.3f);
			_grenadeEntityView.grenadeComponent.spawned = false;
		}

		IEnumerator LateStopDragging(Rigidbody rb)
		{
			yield return new WaitForSeconds(1f);
			StopDragging(rb);
		}

		IEnumerator EnableDragging(Rigidbody rb)
		{
			yield return new WaitForSeconds(0.2f);
			rb.drag = 0;
			rb.angularDrag = 0.5f;
		}

		void StopDragging(Rigidbody rb)
		{
			rb.angularDrag = float.MaxValue;
			rb.drag = float.MaxValue;
			EnableDragging(rb).Run();
		}

		void Explode()
		{

			// NOTE: 

			// This part could probably be improved by creating some VFX implementors, RigidBody implementors
			// Light Implementors and Mesh Implementors

			// Also could probably create a GrenadePhysicsEngine for more modularity to be able to reuse
			// the explosion part

			// As I am not yet a master of this framework I decided to keep the things in here to be able
			// to finish the other tasks

			var instance = _grenadeEntityView.grenadeComponent.instance;

			var meshComponents = instance.transform.GetComponentsInChildren<MeshRenderer>();

			foreach (var mesh in meshComponents)
			{
				mesh.enabled = false;
			}

			instance.transform.GetComponentInChildren<ParticleSystem>().Play();
			instance.transform.GetComponentInChildren<Light>().transform.gameObject.SetActive(false);
			instance.transform.GetComponentInChildren<Rigidbody>().isKinematic = true;

			Collider[] colliders = Physics.OverlapSphere(instance.transform.position, 5f);

			foreach (Collider enemy in colliders)
			{
				Rigidbody rb = enemy.GetComponent<Rigidbody>();

				if (rb != null && enemy.tag == "Enemy")
				{
					rb.AddExplosionForce(1000f, instance.transform.position, 5f);
					rb.drag = 0;
					rb.angularDrag = 0.5f;
					LateStopDragging(rb).Run();
				}

			}
		}

		protected override void Add(GrenadeEntityView entityView)
		{
			_grenadeEntityView = entityView;
			_taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(UpdateTick())
											  .SetScheduler(StandardSchedulers.updateScheduler);
			_taskRoutine.Start();
		}

		protected override void Remove(GrenadeEntityView entityView)
		{
			_grenadeEntityView = null;
			_taskRoutine.Stop();
		}

		protected override void Add(GunEntityView entityView)
		{
			_gunEntityView = entityView;
		}

		protected override void Remove(GunEntityView entityView)
		{
			_gunEntityView = null;
		}

		GrenadeEntityView _grenadeEntityView;
		GunEntityView _gunEntityView;
		ITaskRoutine _taskRoutine;
    }
}