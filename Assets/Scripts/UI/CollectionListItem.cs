using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionListItem : MonoBehaviour {
    [SerializeField] private Image uiFishImage;
    [SerializeField] private TextMeshProUGUI uiNameText;
    [SerializeField] private TextMeshProUGUI uiCountText;

    public void Init(string fishName, int catchCount) {
        uiFishImage.sprite = DataInstance.i.GetFishPortaitByName(fishName);

        if (catchCount > 0) {
            uiNameText.text = fishName;
            uiCountText.text = $"x{catchCount}";
        } else {
            uiNameText.text = "???";
            uiCountText.text = "";
            uiFishImage.color = new Color(0, 0, 0, 1); // silhouette only

        }
    }
}
