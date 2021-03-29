using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage = null;
    [SerializeField] private Sprite defaultSprite = null;
    private AgentController holdingParent = null;

    //resets the icon image back to its default visual
    public void ResetIcon()
    {
        iconImage.sprite = defaultSprite;
        holdingParent = null;
    }

    //drops the item on the ground where it is stationed
    public void DropItem()
    {
         if(holdingParent.HeldItem == true)
         {
            holdingParent.DropItem();
            ResetIcon();
         }
    }

    //Sets up selected Item
    public void SetSelectedItem(AgentController selectedObject)
    {
        holdingParent = selectedObject;
        iconImage.sprite = selectedObject.HeldItem.GetComponent<Item>().ItemIcon;
    }
}
