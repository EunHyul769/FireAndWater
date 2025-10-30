using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ObjectType { Buton, Lever, Pool, Gate, Box, Slide}

public enum ObjectElement {Fire, Water, None}

public class InteractObject : MonoBehaviour
{
    [SerializeField] protected ObjectType Type;
    [SerializeField] protected ObjectElement Element;

    [SerializeField] protected bool IsInteractable;

    [SerializeField] protected int InteractPlayerNum;

    [SerializeField] protected GameObject targetSlide;

    void Awake()
    {
        if(targetSlide == null)
        {
            Debug.Log($"{name} has no target slide!");
        }
    }

    public virtual void Interact()
    {

    }

    public virtual void InteractOut()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        InteractPlayerNum++;
        Debug.Log($"{this.name} nearby {InteractPlayerNum}OB");
        //Interact(); //test call
        if(Type == ObjectType.Gate)
        {
            Interact();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        InteractPlayerNum--;
        if (InteractPlayerNum <= 0)
        {
            //InteractOut();  //test call
        }
        
        if(Type == ObjectType.Gate)
        {
            InteractOut();
        }
    }
}
