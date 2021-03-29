using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    private List<AgentController> selectedAgents = new List<AgentController>();              //reference to selected player agent
    private Camera cam;                     //reference to the main camera
    [Header("UI")]
    [SerializeField] private Transform agentInventoryPanel = null;
    [SerializeField] private ItemIcon itemIcon = null;
     
    [Header("Selection")]
    [SerializeField] private LayerMask selectableMask = 0;
    [SerializeField] private LayerMask pathingMask = 0;
    private bool dragSelecting;
    private Vector3 playerMouse;
    private void Start()
    {
        cam = Camera.main;
        DisableSelectedPanel();
    }

    private void Update()
    {
        LeftMouseClick();
        RightMouseClicks();
    }

    //disables panel that displays players currently Inventory held item
    private void DisableSelectedPanel()
    {
        itemIcon.ResetIcon();
        agentInventoryPanel.gameObject.SetActive(false);
    }

    #region LeftMouseClicks
    //Controls all the left mouse clicks 
    private void LeftMouseClick()
    { 
        if(Input.GetMouseButtonDown(0))
        {
            //makes mouse clicks not bounce through UI elements
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //deselects already selected characters before making a new selection
            for (int i = 0; i < selectedAgents.Count; i++)
            {
                selectedAgents[i].gameObject.GetComponent<SelectedInteractable>().DisableSelection();
            }
            agentInventoryPanel.gameObject.SetActive(false);
            selectedAgents.Clear();
            dragSelecting = true;
            playerMouse = Input.mousePosition;
            //draws a raycast to find new selectable character
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, selectableMask))
            {
                if(hit.transform.tag == "Unit")
                {
                    AgentController newAgent = hit.transform.GetComponent<AgentController>();
                    if (newAgent.IsActive)
                    {
                        selectedAgents.Add(newAgent);
                    }
                    if(selectedAgents.Count > 0)
                    {
                        selectedAgents[0].gameObject.GetComponent<SelectedInteractable>().EnableSelection();
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            foreach (GameObject selectable in GameManager.instance.AllActivePlayers)
            {
                if(IsWithinSelectionBounds(selectable))
                {
                    AgentController newAgent = selectable.transform.GetComponent<AgentController>();
                    if (newAgent.IsActive)
                    {
                        selectedAgents.Add(newAgent);
                        selectable.GetComponent<SelectedInteractable>().EnableSelection();
                    }
                  
                }
            }
            dragSelecting = false;          
        }
        //Display item of selected agent at position 0
        if (selectedAgents.Count > 0)
        {
            agentInventoryPanel.gameObject.SetActive(true);
            if (selectedAgents[0].HeldItem != null)
            {
                itemIcon.SetSelectedItem(selectedAgents[0]);   
            }
            else
            {
                itemIcon.ResetIcon();
            }
        }
    }
    #endregion

    #region Right Mouse clicks

    //handles right mouse clicks by the user.
    private void RightMouseClicks()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //draws a raycast to find new selectable character
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, pathingMask))
            {
                if (selectedAgents.Count > 0)
                {
                    for (int i = 0; i < selectedAgents.Count; i++)
                    {
                        selectedAgents[i].MoveAgentTowards(hit.point);
                    }
                }
            }
        }
    }
    #endregion

    #region Box Selection

    //checks if it is within bounds
    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!dragSelecting)
            return false;

        var viewportBounds = SelectionBox.GetViewportBounds(cam, playerMouse, Input.mousePosition);
        return viewportBounds.Contains(cam.WorldToViewportPoint(gameObject.transform.position));
    }

    //selection box drawing
    private void OnGUI()
    {
        if (dragSelecting)
        {
            // Create a rect from both mouse positions
            var rect = SelectionBox.GetScreenRect(playerMouse, Input.mousePosition);
            SelectionBox.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            SelectionBox.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
    #endregion
}
