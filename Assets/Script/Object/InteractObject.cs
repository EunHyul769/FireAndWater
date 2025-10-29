using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { Buton, Leber, Pool, Gate, Box, Slide}

public enum ObjectElement {Fire, Water, None}

public class InteractObject : MonoBehaviour
{
    [SerializeField] private ObjectType Type;
    [SerializeField] private ObjectElement element;

    [SerializeField] private bool isInteractable;

    protected virtual void Interact()
    {

    }
}
