using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExplosion : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//StartCoroutine(LateExplode());
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			LateExplode().Run();
		}
	}

	IEnumerator LateExplode()
	{
		yield return new WaitForSeconds(2f);
		Explode();
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
		Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

		foreach(Collider enemy in colliders)
		{
			Rigidbody rb = enemy.GetComponent<Rigidbody>();

			if (rb != null && enemy.name != "Grenade" && enemy.tag != "Player")
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
