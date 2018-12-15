using UnityEngine;

public class RigidbodyInfo {
	public Vector2 velocity;
	public bool isKinematic;
	public float gravityScale;
	public float mass;

	public RigidbodyInfo(Vector2 velocity, bool isKinematic, float gravityScale, float mass)
	{
		this.velocity = velocity;
		this.isKinematic = isKinematic;
		this.gravityScale = gravityScale;
		this.mass = mass;
	}
}
