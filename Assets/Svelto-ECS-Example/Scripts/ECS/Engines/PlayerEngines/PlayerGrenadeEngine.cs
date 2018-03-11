using Svelto.ECS.Example.Survive.Player.Gun;
using Svelto.Tasks;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerGrenadeEngine: SingleEntityViewEngine<GunEntityView>, IEngine
    {
        public PlayerGrenadeEngine(ISequencer playerGrenadeDropSequence)
        {
			_taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(UpdateTick())
											  .SetScheduler(StandardSchedulers.updateScheduler);
		}

		IEnumerator UpdateTick()
		{
			while(true)
			{
				//var grenadeComponent = _gunEntityView.grenadeComponent;
				var grenadeSpawnerComponent = _gunEntityView.grenadeSpawnerComponent;

				//if (grenadeComponent.spawned && !grenadeSpawnerComponent.grenadeCooldown)
				//{
				//	LateExplode().Run();
				//}

				yield return null;
			}
		}


		IEnumerator LateExplode()
		{
			yield return new WaitForSeconds(0.7f);
			Explode();
			yield return new WaitForSeconds(2f);
			//Destroy(gameObject);
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

			//var meshComponents = transform.GetComponentsInChildren<MeshRenderer>();

			//foreach (var mesh in meshComponents)
			//{
			//	mesh.enabled = false;
			//}

			//transform.GetComponentInChildren<ParticleSystem>().Play();
			//transform.GetComponentInChildren<Light>().transform.gameObject.SetActive(false);
			//transform.GetComponentInChildren<Rigidbody>().isKinematic = true;

			//var grenadeComponent = _gunEntityView.grenadeComponent;

			//Collider[] colliders = Physics.OverlapSphere(grenadeComponent.instance.transform.position, 5f);

			//foreach (Collider enemy in colliders)
			//{
			//	Rigidbody rb = enemy.GetComponent<Rigidbody>();

			//	if (rb != null && enemy.tag == "Enemy")
			//	{
			//		Debug.Log("Explode");

			//		rb.AddExplosionForce(1000f, grenadeComponent.instance.transform.position, 5f);
			//		rb.drag = 0;
			//		rb.angularDrag = 0.5f;
			//		LateStopDragging(rb).Run();
			//	}

			//}
		}

		protected override void Add(GunEntityView entityView)
		{
			_gunEntityView = entityView;
			_taskRoutine.Start();
		}

		protected override void Remove(GunEntityView entityView)
		{
			_gunEntityView = null;
			_taskRoutine.Stop();
		}

		GunEntityView _gunEntityView;
		ITaskRoutine _taskRoutine;
    }
}