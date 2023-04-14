using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour {
    public Sprite[] sprites;
    public float secondsPerFrame;

    private Image image;
    private int index;
    private float timer;

    private void Awake() {
        image = GetComponent<Image>();
        image.sprite = sprites[0];

        if (secondsPerFrame == 0f) {
            Debug.LogError("AnimateImage's secondsPerFrame is 0, " +
            $"animation will not play properly.  Object = {gameObject.name}");
        }
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer > secondsPerFrame) {
            timer = 0f;
            index = index == sprites.Length - 1 ? 0 : index + 1;
            image.sprite = sprites[index];
        }
    }
}
