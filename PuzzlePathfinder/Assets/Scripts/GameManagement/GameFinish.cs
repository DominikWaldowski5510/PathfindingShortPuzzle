using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinish : MonoBehaviour
{
    private Renderer rend;
    [SerializeField] private Material completedLevelmaterial = null;
    [SerializeField] private Transform gameCompleteText = null;
    private bool isGameOver = false;

    //presets all start values
    private void Start()
    {
        rend = this.GetComponent<Renderer>();
        gameCompleteText.gameObject.SetActive(false);
        isGameOver = false;
    }

    //triggers game over when a unit walks into the collider
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Unit")
        {
            rend.material = completedLevelmaterial;
            gameCompleteText.gameObject.SetActive(true);
            isGameOver = true;
        }
    }

    //allows user to end application when space key is pressed
    private void Update()
    {
        if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Application.Quit();
            }
        }
    }
}
