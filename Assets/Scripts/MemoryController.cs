using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryController : MonoBehaviour
{
	[SerializeField] private MainController main;
    [SerializeField] private ParticleSystem particle;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			main.playerGetPiece();
            Instantiate(particle, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
