using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExplosion : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			LateExplode().Run();
		}
	}


	IEnumerator LateExplode()
	{
		yield return new WaitForSeconds(0.7f);
		Explode();
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
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

		var meshComponents = transform.GetComponentsInChildren<MeshRenderer>();

		foreach (var mesh in meshComponents)
		{
			mesh.enabled = false;
		}

		transform.GetComponentInChildren<ParticleSystem>().Play();
		transform.GetComponentInChildren<Light>().transform.gameObject.SetActive(false);
		transform.GetComponentInChildren<Rigidbody>().isKinematic = true;

		Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

		foreach(Collider enemy in colliders)
		{
			Rigidbody rb = enemy.GetComponent<Rigidbody>();

			if (rb != null && enemy.tag == "Enemy")
			{
				Debug.Log("Explode");

				rb.AddExplosionForce(1000f, transform.position, 5f);
				rb.drag = 0;
				rb.angularDrag = 0.5f;
				LateStopDragging(rb).Run();
			}

		}
	}
}
