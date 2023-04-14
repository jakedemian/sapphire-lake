using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UISuccessModal : UIModal {
    public Image fishPortrait;
    public TextMeshProUGUI fishName;
    private CanvasScaler cs;

    public void Show(string _fishName, string fishPortraitPath) {
        fishName.text = _fishName;
        fishPortrait.sprite = Resources.Load<Sprite>(fishPortraitPath);

        AudioManager.i.PlaySuccessModalSound();
        base.Show();
    }

    public void Hide() {
        EventManager.i.e_SuccessModalClosed();
        base.Hide();
    }
}
