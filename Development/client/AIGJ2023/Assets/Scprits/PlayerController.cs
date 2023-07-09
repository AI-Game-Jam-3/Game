using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Character character;
    public InteractInScene interactInScene;

    public GameObject nextlevel;
    private LevelAnimation levelani = null;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInChildren<Character>();
        interactInScene = GetComponentInChildren<InteractInScene>();
        levelani = nextlevel.GetComponent<LevelAnimation>();
        //SoundAccept.Instance.OnShout += () =>
        //{
        //    character.SetGoingToShout(true);
        //};
        //SoundAccept.Instance.OnFire += () =>
        //{
        //    interactInScene.InteractWithFire();
        //};
        //SoundAccept.Instance.OnBreak += () =>
        //{
        //    interactInScene.InteractWithBreakableWall();
        //};

    }

    // Update is called once per frame
    void Update()
    {

        if (!levelani.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                character.MoveDirection = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                character.MoveDirection = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                character.MoveDirection = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
                character.CurrentGate?.Transfer(character.transform);
            }
            else if (SoundAccept.Instance.isShout)
            {
                character.SetGoingToShout(true);

                SoundAccept.Instance.isShout = false;
            }
            else if (SoundAccept.Instance.isFire)
            {
                interactInScene.InteractWithFire();

                SoundAccept.Instance.isFire = false;
            }
            else if (SoundAccept.Instance.isBreak)
            {
                interactInScene.InteractWithBreakableWall();

                SoundAccept.Instance.isBreak = false;
            }
            else
            {
                character.MoveDirection = Vector2.zero;
            }
        }

        
    }



}
