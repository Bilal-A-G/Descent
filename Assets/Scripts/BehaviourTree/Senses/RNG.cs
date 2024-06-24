using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNG : MonoBehaviour
{
    public List<ProbabilityEvent> probabilityEvents;
    public float rollDelay;

    private bool m_isRolling;
    private float m_currentDelay;

    private void Start()
    {
        m_currentDelay = rollDelay;

        for (int i = 0; i < probabilityEvents.Count; i++)
        {
            ProbabilityEvent currentEvent = probabilityEvents[i];
            currentEvent.rollOn.Subscribe((bool isTrue) => currentEvent.m_canRoll = currentEvent.invertBool ? isTrue : !isTrue);

            probabilityEvents[i] = currentEvent;
        }
    }

    private void Update()
    {
        if (!m_isRolling)
            StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        m_isRolling = true;
        bool invokedPrevious = false;

        for (int i = 0; i < probabilityEvents.Count; i++)
        {
            if (invokedPrevious)
            {
                probabilityEvents[i].eventObject.Invoke(false);
                continue;
            }

            if (probabilityEvents[i].alwaysTrigger)
            {
                probabilityEvents[i].eventObject.Invoke(true);
                m_currentDelay = probabilityEvents[i].rollDelay;
                break;
            }

            float value = Random.value;
            bool passes = (value <= probabilityEvents[i].probability) || probabilityEvents[i].m_canRoll;
            probabilityEvents[i].eventObject.Invoke(passes);
            m_currentDelay = passes ? probabilityEvents[i].rollDelay : m_currentDelay;
            invokedPrevious = passes;
        }
        yield return new WaitForSeconds(m_currentDelay);
        m_isRolling = false;
    }
}

[System.Serializable]
public class ProbabilityEvent
{
    public EventObject eventObject;
    [Range(0.0f, 1.0f)]
    public float probability;
    public bool alwaysTrigger;
    public float rollDelay;

    public bool invertBool;
    public EventObject rollOn;
    [HideInInspector]
    public bool m_canRoll;
}
