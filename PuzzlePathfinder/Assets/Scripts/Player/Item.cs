using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool hasBeenPickedUp = false;
    [SerializeField] private Sprite itemIcon = null;
    [SerializeField] AgentController parentObj = null;
    private Rigidbody rb;
    public Sprite ItemIcon { get => itemIcon; }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        ResetRigidBody();
    }

    private void ResetRigidBody()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    //allows the player to pick up the object
    private void OnTriggerEnter(Collider collision)
    {
        if(hasBeenPickedUp == false && collision.transform.tag == "Unit")
        {
            AgentController parentObjToUse = collision.transform.GetComponent<AgentController>();
            if (parentObjToUse.CanPickAnotherItem == true)
            {
                hasBeenPickedUp = true;
                rb.useGravity = false;
                rb.isKinematic = true;
                if (parentObj != null)
                {
                    parentObj.HeldItem = null;
                }

                parentObj = parentObjToUse;
                parentObj.PickUpObject(this.gameObject);
            }
        }
    }

    //Allows player to drop the object
    public void DropObject()
    {
        ResetRigidBody();
        hasBeenPickedUp = false;     
    }

}
