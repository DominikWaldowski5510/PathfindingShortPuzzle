using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private GameObject exitGame = null;                            //stores object that blocks advancement to next level
    [SerializeField] private List<PuzzleTrigger> puzzleTriggers = new List<PuzzleTrigger>();         //stores all puzzle components
    [SerializeField] private int completionIndexCount = 0;
    [SerializeField] private AgentController[] newAgentsToControl = null;
    //sets up default puzzle settings
    private void Start()
    {
        exitGame.SetActive(true);
        completionIndexCount = 0;
    }

    //disables the gate when the puzzle is completed
    private void CompletePuzzle()
    {
        EnablePlayersAfterCompetion();
        exitGame.SetActive(false);
    }

    //enables agents when puzzle is completed
    private void EnablePlayersAfterCompetion()
    {
        for (int i = 0; i < newAgentsToControl.Length; i++)
        {
            newAgentsToControl[i].SetNewActiveState(true);
        }
    }

    //performs a check to see if the puzzle is completed or not
    public void CheckForCompletion(bool _isCompleted)
    {
        if(_isCompleted)
        {
            if (completionIndexCount < puzzleTriggers.Count)
            {
                completionIndexCount++;
            }
        }
        else
        {
            if (completionIndexCount > 0)
            {
                completionIndexCount--;
            }
        }
        if (puzzleTriggers.Count == completionIndexCount)
        {
            CompletePuzzle();
        }
        else
        {
            exitGame.SetActive(true);
        }
    }

    
}
