using TMPro;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TMP_Text _text;

    public void FadeInCanvasOverTime()
    {
        _canvasGroup.alpha += Time.deltaTime;
        _text.alpha += Time.deltaTime;
    }
}
