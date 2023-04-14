using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class LoadingIndicator : MonoBehaviour {
    [SerializeField] private GameObject loadingIndicator;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private bool usesSuccessText;
    [SerializeField] private string successText;
    [SerializeField] private float hideAfterSeconds;
    [SerializeField] private GameObject loadingIndicatorImage;

    private void Awake() {
        EventManager.i.onLoadDataStart += OnStart;
        EventManager.i.onSaveDataStart += OnStart;
        EventManager.i.onLoadDataComplete += OnComplete;
        EventManager.i.onSaveDataComplete += OnComplete;
    }

    private void OnDestroy() {
        EventManager.i.onLoadDataStart -= OnStart;
        EventManager.i.onSaveDataStart -= OnStart;
        EventManager.i.onLoadDataComplete -= OnComplete;
        EventManager.i.onSaveDataComplete -= OnComplete;
    }

    private void OnStart() {
        loadingIndicator.SetActive(true);
        if (loadingIndicatorImage != null) {
            loadingIndicatorImage.SetActive(true);
        }
    }

    private void OnComplete() {
        if (usesSuccessText) {
            loadingText.text = successText;
            if (loadingIndicatorImage != null) {
                loadingIndicatorImage.SetActive(false);
            }
        }

        Hide();
    }

    private void Hide() {
        if (hideAfterSeconds > 0f) {
            StartCoroutine(DoHideAfterSeconds());
        } else {
            loadingIndicator.SetActive(false);
        }
    }

    private IEnumerator DoHideAfterSeconds() {
        yield return new WaitForSeconds(hideAfterSeconds);
        loadingIndicator.SetActive(false);
    }
}
