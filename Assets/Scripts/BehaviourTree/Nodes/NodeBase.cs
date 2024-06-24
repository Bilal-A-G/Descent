using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeBase : MonoBehaviour
{
    public float tickDelay;
    public List<NodeBase> children;
    public NodeBase parent;
    [SerializeField] public List<Conditional> conditions;

    private void Start() { return; }
    private void Update() { return; }

    public void RegisterConditionals()
    {
        for (int i = 0; i < conditions.Count; i++) 
        {
            Conditional currentCondition = conditions[i];
            currentCondition.condition.Subscribe((bool isTrue) => currentCondition.SetSatisfied(isTrue));

            conditions[i] = currentCondition;
        }
    }

    public bool CanEnter()
    {
        bool canEnter = true;

        for( int i = 0; i < conditions.Count; i++)
        {
            if (!conditions[i].isSatisfied)
            {
                canEnter = false;
                break;
            }
        }

        return canEnter;
    }

    public virtual void Tick() { return; }
    public virtual void OnTransition() { return; }
    public virtual void OnExit() {  return; }
}

[System.Serializable]
public class Conditional
{
    public EventObject condition;
    [HideInInspector] public bool isSatisfied;

    public void SetSatisfied(bool isTrue)
    {
        this.isSatisfied = isTrue;
    }
} 
