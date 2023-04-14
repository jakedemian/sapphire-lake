using UnityEngine;

public class CloudMove : MonoBehaviour {
    public float moveSpeed;

    private void Awake() {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * moveSpeed;
    }
}