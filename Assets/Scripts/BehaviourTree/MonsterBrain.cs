using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBrain : MonoBehaviour
{
    public NodeBase currentNode;

    private bool isTicking;

    void RegisterTreeConditionals(NodeBase node)
    {
        if(node.parent != null)
            node.conditions.AddRange(node.parent.conditions);

        node.RegisterConditionals();

        for (int i = 0; i < node.children.Count; i++)
        {
            RegisterTreeConditionals(node.children[i]);
        }
    }

    void Start()
    {
        RegisterTreeConditionals(currentNode);
        currentNode.OnTransition();
    }

    void Update()
    {
        if(!isTicking)
            StartCoroutine(TickNode());

        for (int i = 0; i < currentNode.children.Count; i++)
        {
            if (currentNode.children[i].CanEnter()) 
            {
                StopAllCoroutines();
                isTicking = false;
                currentNode.OnExit();
                currentNode = currentNode.children[i];
                currentNode.OnTransition();
                break;
            }
        }

        if (!currentNode.CanEnter())
        {
            StopAllCoroutines();
            isTicking = false;
            currentNode.OnExit();
            currentNode = currentNode.parent;
            currentNode.OnTransition();
        }
    }

    IEnumerator TickNode()
    {
        isTicking = true;
        currentNode.Tick();
        yield return new WaitForSeconds(currentNode.tickDelay);
        isTicking = false;
    }
}
