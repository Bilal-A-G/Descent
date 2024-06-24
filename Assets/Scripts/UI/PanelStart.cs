using UnityEngine;
using UnityEngine.Events;

public class PanelStart : MonoBehaviour
{
    [SerializeField] UnityEvent _start;

    void Start() => _start.Invoke();
}
