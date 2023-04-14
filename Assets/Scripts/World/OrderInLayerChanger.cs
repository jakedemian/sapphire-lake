using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerChanger : MonoBehaviour {
    private Transform player;
    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (player.position.y > transform.position.y && sr.sortingOrder != 5) {
            sr.sortingOrder = 5;
        } else if (player.position.y <= transform.position.y && sr.sortingOrder != 0) {
            sr.sortingOrder = 0;
        }
    }
}
