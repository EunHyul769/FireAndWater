using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ObjectType { Buton, Lever, Pool, Gate, Box, Slide, Gem}

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

        if(Type == ObjectType.Buton)
        {
            Interact();
        }

        if(Type == ObjectType.Gate)
        {
            Interact();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        InteractPlayerNum--;
        if (InteractPlayerNum <= 0 && Type == ObjectType.Buton)
        {
            InteractOut();
        }

        if (Type == ObjectType.Gate)
        {
            InteractOut();
        }
    }
    
    protected bool PlayerCheck(GameObject target)
    {
        if (target.CompareTag("Player_Fire"))
        {
            if (Element == ObjectElement.Fire)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (target.CompareTag("Player_Water"))
        {
            if (Element == ObjectElement.Water)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
