using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSqure : MonoBehaviour
{
    public Texture portal_tex;
    public Texture ground_tex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curSqaurePos = transform.position;
        Vector2 CharacterXY = MapManager.Instance.Pos2Coord(curSqaurePos);
        int CurX = (int)CharacterXY.x;
        int CurY = (int)CharacterXY.y;
        MapUnit currentUnit = MapManager.Instance.currentMap.MapUnits[CurY, CurX];

        if (currentUnit.UnitType == "TransferGate")
        {
            Material material = GetComponent<SpriteRenderer>().material;

            material.SetTexture("_BaseMap", portal_tex);
        }
        else
        {
            Material material = GetComponent<SpriteRenderer>().material;

            material.SetTexture("_BaseMap", ground_tex);
        }
    }
}
