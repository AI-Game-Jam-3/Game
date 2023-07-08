using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInChildren<Character>();
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
        else
        {
            character.MoveDirection = Vector2.zero;
        }



    }



}
