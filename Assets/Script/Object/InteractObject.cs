using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { Buton, Leber, Pool, Gate, Box, Slide}

public enum ObjectElement {Fire, Water, None}

public class InteractObject : MonoBehaviour
{
    [SerializeField] private ObjectType Type;
    [SerializeField] private ObjectElement Element;

    [SerializeField] protected bool IsInteractable;

    [SerializeField] protected int InteractPlayerNum;

    public virtual void Interact()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        InteractPlayerNum++;
    }

    void OriggerExit2D(Collider2D collision)
    {
        InteractPlayerNum--;        
    }
}
