using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapPlayerUI : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private MinimapPlayerRotation[] rotations;
    private RectTransform rt;
    private Image image;

    private void Awake() {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnGUI() {
        float rotationZ = player.eulerAngles.z;
        int closestAngleIndex = GetClosestAngleIndex(rotationZ);
        image.sprite = rotations[closestAngleIndex].sprite;
    }

    private int GetClosestAngleIndex(float rotationZ) {
        int closestIndex = 0;
        float minAngleDifference = Mathf.Abs(Mathf.DeltaAngle(rotationZ, rotations[0].angle));

        for (int i = 1; i < rotations.Length; i++) {
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(rotationZ, rotations[i].angle));

            if (angleDifference < minAngleDifference) {
                minAngleDifference = angleDifference;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}

[System.Serializable]
struct MinimapPlayerRotation {
    public Sprite sprite;
    public float angle;
}
