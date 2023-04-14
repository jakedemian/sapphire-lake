using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player;

    private Vector2 playerPosition => player.position;
    private Vector3 targetPosition =>
        new Vector3(playerPosition.x, playerPosition.y, transform.position.z);

    private void Update() {
        if (DebugHelper.i.freezeCameraPosition) return;

        transform.position = targetPosition;
    }
}
