using UnityEngine;
using System.Collections;

public class UIMinigameContainer : UIModal {
    public void OnStart() {
        base.Show();
    }

    public void OnWin() {
        base.Hide();
    }

    public void OnLose() {
        base.ShakeAndHide();
    }
}
