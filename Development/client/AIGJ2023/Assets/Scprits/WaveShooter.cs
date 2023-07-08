using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveShooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button("Shoot")]
    public List<MapUnit> ShootRays(int kernelSize)
    {
        // TODO: ¿¼ÂÇCellSize
        List<MapUnit> visiableUnits = new List<MapUnit>();
        Debug.Assert(kernelSize % 2 != 0);
        int range = kernelSize / 2;
        for(int y = -range; y <= range; y++)
        {
            for(int x = -range; x  <= range; x++) {
                var dir = new Vector2(x, y);
                var centerPos = transform.position;
                var centerCoord = MapManager.Instance.Pos2Coord(centerPos);
                var newCoord = centerCoord + dir;
                if (MapManager.Instance.IsOutSide((int)newCoord.x, (int)newCoord.y))
                {
                    continue;
                }
                MapUnit targetUnit = MapManager.Instance.currentMap.GetUnit(newCoord);
                Vector3? targetPos = targetUnit.transform.position;
                if (targetPos.HasValue)
                {
                    var vec = targetPos.Value - centerPos;
                    vec = new Vector3(vec.x, vec.y, 0);
                    var hit = Physics2D.Raycast(transform.position, vec.normalized, vec.magnitude, LayerMask.GetMask("Wall"));
                    if(hit.collider != null)
                    {
                        //var spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                        //if (spriteRenderer != null)
                        //{
                        //    spriteRenderer.color = Color.black;
                        //}
                    }
                    else
                    {

                        if(targetUnit != null)
                        {
                            visiableUnits.Add(targetUnit);
                        }
                    }
                }

            }
        }
        return visiableUnits;
    }
}
