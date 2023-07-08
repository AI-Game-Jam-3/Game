using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditorInternal;
using static UnityEngine.UI.CanvasScaler;

public class Character : MonoBehaviour
{
    [LabelText("移动方向")]
    public Vector2 MoveDirection;
    [LabelText("原始位置")]
    public Vector3 OriginPos;

    public bool IsGoingToShout;

    List<MapUnit> LightedUnitsCache;
    MapUnit LastCharacterUnit;
    public bool IsCouldShout;
    public bool IsFadeOut;
    public void SetGoingToShout(bool Shout)
    {
        IsGoingToShout = Shout;
    }
    // Start is called before the first frame update
    void Start()
    {
        IsCouldShout = true;
        IsFadeOut = false;

        //StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {


        
        if (MoveDirection != Vector2.zero)
        {
            Move();
            if(LastCharacterUnit != null)
            {
                if (LastCharacterUnit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    Color color = Color.black;

                    color.a = 1;
                    spriteRenderer.color = color;

                }
            }
        }
        LightCharacterUnit();

        if (IsCouldShout && IsGoingToShout)
        {

            IsCouldShout = false;
            IsFadeOut = false;
            PostUpdateMapExplore();
            //TODO 时间设置
            Invoke("ResetMapExplore", 1.0f);
            
            IsGoingToShout = false;
        }
        LightFireUpdate();

    }

    void LightCharacterUnit()
    {
        
        Vector2 CharacterXY = MapManager.Instance.Pos2Coord(this.transform.position);
        int CurX = (int)CharacterXY.x;
        int CurY = (int)CharacterXY.y;
        MapUnit PlayerUnit = MapManager.Instance.currentMap.MapUnits[CurY, CurX];
        LastCharacterUnit = PlayerUnit;
        if (PlayerUnit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            Color color = Color.white;

            color.a = 0;
            spriteRenderer.color = color;
            
        }
    }

    public void Move()
    {
        var currentMap = MapManager.Instance.currentMap;
        var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
        var targetCoord = MapManager.Instance.CalculateUnitCoord(currentCoord, MoveDirection);
        if(targetCoord == null)
        {
            return;
        }
        var unit = currentMap.GetUnit(targetCoord.Value);
        if(unit != null && unit.UnitType == "Wall")
        {
            return;
        }
        transform.Translate(MoveDirection * MapManager.Instance.CELL_SIZE);
    }

    [Button("返回原始位置")]
    void BackToOriginPos()
    {
        transform.position = OriginPos;
    }


    IEnumerator LightOffUnitsSlowly()
    {
        var speed = 0.005f;
        var alpha = 0.0f;
        while (alpha < 1 )
        {
            if (!IsFadeOut)
            {
                yield return null;
            }
            alpha = Mathf.MoveTowards(alpha, 1, speed);
            //renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, a);
            //Debug.Log("alpha:" + alpha.ToString());

            if ( LightedUnitsCache != null && LightedUnitsCache.Count > 0)
            {
                foreach (MapUnit unit in LightedUnitsCache)
                {

                    if (unit != null)
                    {
                        if (unit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                        {
                            Color color = Color.black;

                            if (unit.bIsWall)
                            {
                                color.a = alpha;
                                spriteRenderer.color = color;
                                //spriteRenderer.color = Color.blue;
                            }
                            else
                            {
                                color.a = alpha;
                                spriteRenderer.color = color;
                                //spriteRenderer.color = Color.white;
                            }
                        }
                    }
                }
            }

            yield return null;
        }
        IsCouldShout = true;

        LightFireUpdate();
        yield break;
    }

    void ResetMapExplore()
    {

       StartCoroutine(LightOffUnitsSlowly());

    }

    void LightFireUpdate()
    {
        //处理已经被点亮的火把
        foreach (MapUnit torchunit in MapManager.Instance.TorchUnitsGroup)
        {
            // Debug.Log("LightedUnits: " + unit.transform.position);
            if (torchunit != null)
            {
                if (torchunit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    Color color = Color.white;
                    //color.a = 1;
                    //spriteRenderer.color = color;

                    if (torchunit.bIsWall)
                    {
                        color.a = 1;
                        spriteRenderer.color = color;
                    }
                    else
                    {
                        color.a = 0;
                        spriteRenderer.color = color;
                    }
                }
            }
        }
    }

    void PostUpdateMapExplore()
    {
        //Debug.Log("Current Character Position: " + character.transform.position);
        Vector2 CharacterXY = MapManager.Instance.Pos2Coord(this.transform.position);
        int CurX = (int)CharacterXY.x;
        int CurY = (int)CharacterXY.y;

        //List<MapUnit> ExpolreUnits = MapManager.Instance.ExpolreNeighborKernel(7, CurX, CurY);
        List<MapUnit> LightedUnits = MapManager.Instance.GetLightedArea(3, CurX, CurY);
        ////foreach (MapUnit unit in ExpolreUnits)
        ////{
        ////    Debug.Log("ExpolreUnit: " + unit.transform.position);
        ////}
        ////Debug.Log("=======================");


        List<MapUnit> ExpolreUnits = GetComponent<WaveShooter>().ShootRays(7);

        if (LightedUnitsCache != null && LightedUnitsCache.Count > 0)
        {
            foreach (MapUnit unit in LightedUnitsCache)
            {
                if (unit != null)
                {
                    if (unit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                    {
                        Color color = Color.black;

                        if (unit.bIsWall)
                        {
                            color.a = 1;
                            spriteRenderer.color = color;
                            //spriteRenderer.color = Color.blue;
                        }
                        else
                        {
                            color.a = 1;
                            spriteRenderer.color = color;
                            //spriteRenderer.color = Color.white;
                        }

                    }
                }
            }
        }


        foreach (MapUnit unit in ExpolreUnits)
        {
            //Debug.Log("LightedUnits: " + unit.transform.position);
            if (unit != null)
            {
                if (unit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    Color color = Color.white;

                    if (unit.bIsWall)
                    {
                        color.a = 1;
                        spriteRenderer.color = color;
                    }
                    else
                    {
                        color.a = 0;
                        spriteRenderer.color = color;
                    }
                }
            }
        }
        LightedUnitsCache = ExpolreUnits;

        foreach (MapUnit proctectedUnit in LightedUnits)
        {
            if (proctectedUnit != null)
            {
                if (proctectedUnit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    Color color = Color.white;

                    if (proctectedUnit.bIsWall)
                    {
                        color.a = 1;
                        spriteRenderer.color = color;
                    }
                    else
                    {
                        color.a = 0;
                        spriteRenderer.color = color;
                    }
                }
                LightedUnitsCache.Add(proctectedUnit);
            }
        }


        LightFireUpdate();

        IsFadeOut = true;


    }



}
