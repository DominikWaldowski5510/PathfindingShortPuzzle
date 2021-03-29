using UnityEngine;

//handles display of selected object
public class SelectedInteractable : MonoBehaviour
{
    [SerializeField] private GameObject selectable = null;             //stores reference to gameobject which shows if unit is selected


    //disables selected object by default
    private void Start()
    {
        DisableSelection();
    }

    //Runs when object selection has to be enabled
    public void EnableSelection()
    {
        selectable.SetActive(true);
    }

    //runs when object selection has to be disabledw
    public void DisableSelection()
    {
        selectable.SetActive(false);
    }
}
