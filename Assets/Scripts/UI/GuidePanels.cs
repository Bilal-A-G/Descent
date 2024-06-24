using System.Collections;
using UnityEngine;

public class GuidePanels : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    public void ShowTipFew()
    {
        _panel.SetActive(true);
        StartCoroutine(ShowPanel());
    }

    IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(5f);
        _panel.SetActive(false);
    }
}
