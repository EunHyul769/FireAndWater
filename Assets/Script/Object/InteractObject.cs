using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ObjectType { Buton, Leber, Pool, Gate, Box, Slide}

public enum ObjectElement {Fire, Water, None}

public class InteractObject : MonoBehaviour
{
    [SerializeField] private ObjectType Type;
    [SerializeField] private ObjectElement Element;

    [SerializeField] protected bool IsInteractable;

    [SerializeField] protected int InteractPlayerNum;

    [SerializeField] protected GameObject targetSlide;

    void Awake()
    {
        if(targetSlide == null)
        {
            Debug.LogError($"{name} has no target slide!");
        }
    }

    public virtual void Interact()
    {

    }

    public virtual void InteractOut()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        InteractPlayerNum++;
        Debug.Log($"{this.name} nearby {InteractPlayerNum}OB");
        Interact(); //test call
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        InteractPlayerNum--;
        if(InteractPlayerNum <= 0)
        {
            InteractOut();  //test call
        }  
    }
}
