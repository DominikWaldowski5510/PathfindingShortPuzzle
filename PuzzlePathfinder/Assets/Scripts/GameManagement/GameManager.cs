using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject flagObject = null;
    private GameObject[] allActivePlayers = null;

    public GameObject[] AllActivePlayers { get => allActivePlayers; }
    public GameObject FlagObject { get => flagObject; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        allActivePlayers = GameObject.FindGameObjectsWithTag("Unit");
    }
}
