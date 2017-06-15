using UnityEngine;
using System.Collections;
using System;

public class RayController : MonoBehaviour
{
	public GameObject destino;
	public GameObject interactable;

	private GameObject interactableGO;
	private GameObject destinoGO;

	private GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) {
			Destroy (destinoGO);
			Destroy (interactableGO);
			if (hit.collider.transform.tag == "Floor") {
				destinoGO = Instantiate (destino, hit.point, Quaternion.LookRotation (hit.normal)) as GameObject;
			} else if (hit.collider.transform.tag == "Interactable") {
				interactableGO = Instantiate (interactable, hit.point, Quaternion.LookRotation (hit.normal)) as GameObject;
			}
		}

		if (Input.GetMouseButtonDown (0)) {

			if (destinoGO != null) {
				// Teletransporte (hit);

				StopAllCoroutines ();
				// DesplazamientoSmooth (hit);
				TiroParabolico (hit);
			} else if (interactableGO != null) {
				// Hacer lo necesario para interactuar
				try {
					Debug.Log ("Interactuando con " + hit.collider.name);
				} catch (Exception e) {
					return;
				}

			}
		}
	}

	void Teletransporte (RaycastHit hit)
	{
		player.transform.position = new Vector3 (hit.point.x, 1, hit.point.z);
	}

	void TiroParabolico (RaycastHit hit)
	{
		StartCoroutine (TiroParabolicoCoroutine (player.transform.position, new Vector3 (hit.point.x, 1, hit.point.z)));
	}

	IEnumerator TiroParabolicoCoroutine (Vector3 posOrigen, Vector3 posDestino)
	{
		float aux = 0.0f;
		Vector3 lerpVector;
		while (transform.position != posDestino) {
			lerpVector = Vector3.Lerp (posOrigen, posDestino, aux);
			player.transform.position = new Vector3 (lerpVector.x, Mathf.Sin (Mathf.LerpAngle (0, Mathf.PI, aux)) * 2.0f + 1.0f, lerpVector.z);
			aux += 0.005f;
			yield return new WaitForSeconds (0.01f);
		}
	}

	void DesplazamientoSmooth (RaycastHit hit)
	{
		StartCoroutine (DesplazamientoSmoothCoroutine (player.transform.position, new Vector3 (hit.point.x, 1, hit.point.z)));
	}

	IEnumerator DesplazamientoSmoothCoroutine (Vector3 posOrigen, Vector3 posDestino)
	{
		float aux = 0.0f;
		while (transform.position != posDestino) {
			player.transform.position = Vector3.Lerp (posOrigen, posDestino, aux);
			aux += 0.005f;
			yield return new WaitForSeconds (0.01f);
		}
	}

}
