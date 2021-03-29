using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    private Puzzle puzzleParent;
    [SerializeField] private Material[] activeMaterial = null;          //materials that chance on unit colision
    private bool isCompleted;                       //state of the puzzle piece
    private Renderer rend;                          //renderer component that allows materials to be changed

    //sets up default values
    private void Start()
    {
        rend = this.GetComponent<Renderer>();
        puzzleParent = this.gameObject.GetComponentInParent<Puzzle>();
        rend.material = activeMaterial[0];
        isCompleted = false;
    }

    //changes colour to green when player enters
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Unit" || other.transform.tag =="Object")
        {
            rend.material = activeMaterial[1];
            isCompleted = true;
            puzzleParent.CheckForCompletion(isCompleted);
        }
    }

    //changes colour to red when player exists
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Unit" || other.transform.tag == "Object")
        {
            rend.material = activeMaterial[0];
            isCompleted = false;
            puzzleParent.CheckForCompletion(isCompleted);
        }
    }

}
