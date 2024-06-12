using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public CharacterData player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartLevel();
    }
}
