using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private NavMeshAgent agent;                     //reference to agent script which handles movement and pathing
    private bool hasDestination = false;                //check whenever we have an destination or not
    private GameObject destinationFlag = null;          //flag that gets displayed when we click on a travel location
    private Animator anim;                      //reference to animation script
    [Header("Pick ups")]
    [SerializeField] private Transform heldItemPosition = null;
    private GameObject heldItem = null;
    [Header("Player Active States")]
    [SerializeField] private Renderer[] playerColourRend = null;
    [SerializeField] private Material activeMaterial = null;
    [SerializeField] private Material inactiveMaterial = null;
    [SerializeField] private bool isActive = true;
    private bool canPickAnotherItem = true;
    //Determines the type of the animation
    private enum AnimationSequences
    {
        Idle,
        Walking,
        Carrying
    }
    private AnimationSequences animSequence;

    public GameObject HeldItem { get => heldItem; set => heldItem = value; }
    public bool IsActive { get => isActive;  }
    public bool CanPickAnotherItem { get => canPickAnotherItem; }

    //sets up the agents default behaviours
    private void Start()
    {
        //Initializers
        anim = this.GetComponent<Animator>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        //Instantiators
        GameObject go = Instantiate(GameManager.instance.FlagObject) as GameObject;
        destinationFlag = go;
        destinationFlag.SetActive(false);
        canPickAnotherItem = true;
        //Pathing
        agent.enabled = false;
        hasDestination = false;

        //Animation
        anim.enabled = true;
        animSequence = AnimationSequences.Idle;
        anim.SetInteger("ActionType", (int)animSequence);
        SetActiveState();
    }

    private void SetActiveState()
    {
        if(isActive == true)
        {
            for (int i = 0; i < playerColourRend.Length; i++)
            {
                playerColourRend[i].material = activeMaterial;
            }
        }
        else
        {
            for (int i = 0; i < playerColourRend.Length; i++)
            {
                playerColourRend[i].material = inactiveMaterial;
            }
        }
    }

    //sets agents to active or inactive state based on value received
    public void SetNewActiveState(bool _isActive)
    {
        isActive = _isActive;
        SetActiveState();
    }

    //handles the movement of the agent
    private void Update()
    {
        if(hasDestination == true)
        {
            float distance = Vector3.Distance(destinationFlag.transform.position, this.transform.position);
            if(distance <= 1.1f)
            {
                hasDestination = false;
                destinationFlag.gameObject.SetActive(false);
                agent.enabled = false;
                if (heldItem)
                {
                    animSequence = AnimationSequences.Carrying;
                }
                else
                {
                    animSequence = AnimationSequences.Idle;
                }
                anim.SetInteger("ActionType", (int)animSequence);
            }
        }

        if (heldItem != null)
        {
            heldItem.transform.position = heldItemPosition.transform.position;
            heldItem.transform.rotation = heldItemPosition.transform.rotation;
        }
    }

    //Moves the agent to specified coordinates
    public void MoveAgentTowards(Vector3 destination)
    {
        destinationFlag.SetActive(true);
        destinationFlag.transform.position = destination;
        destinationFlag.transform.position = new Vector3(destinationFlag.transform.position.x,
            destinationFlag.transform.position.y + 1f, destinationFlag.transform.position.z);
        agent.enabled = true;
        hasDestination = true;
        if (heldItem)
        {
            animSequence = AnimationSequences.Carrying;
        }
        else
        {
            animSequence = AnimationSequences.Walking;  
        }
        anim.SetInteger("ActionType", (int)animSequence);
        agent.SetDestination(destinationFlag.transform.position);
    }

    //Picks up the object 
    public void PickUpObject(GameObject pickedUpObject)
    {
        heldItem = pickedUpObject;
        heldItem.transform.position = heldItemPosition.transform.position;
        heldItem.transform.rotation = heldItemPosition.transform.rotation;
        heldItem.transform.SetParent(heldItemPosition);
        canPickAnotherItem = false;
    }

    //drops the item from the player
    public void DropItem()
    {
        //sets position and parenting
        heldItem.transform.parent = null;
        heldItem.transform.position = new Vector3(heldItem.transform.position.x,
            1,
            heldItem.transform.position.z);
        heldItem.GetComponent<Item>().DropObject();
        heldItem = null;
        StartCoroutine(DropSequenceReset());
    }

    private IEnumerator DropSequenceReset()
    {
        yield return new WaitForSeconds(1);
        canPickAnotherItem = true;
    }
}
