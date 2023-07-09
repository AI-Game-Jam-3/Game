using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Character character;
    public InteractInScene interactInScene;
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInChildren<Character>();
        interactInScene = GetComponentInChildren<InteractInScene>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            character.MoveDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            character.MoveDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            character.MoveDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            character.MoveDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            character.SetGoingToShout(true);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            interactInScene.Interactive();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            character.CurrentGate?.Transfer(character.transform);
        }
        else if (SoundAccept.Instance.isShout)
        {
            character.SetGoingToShout(true);

            SoundAccept.Instance.isShout = false;
        }
        else
        {
            character.MoveDirection = Vector2.zero;
        }
    }



}
