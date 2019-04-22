using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewPrefabTile", menuName = "Tiles/Prefab Tile")]
public class PrefabTile : TileBase
{
    public Sprite m_Sprite;
    public GameObject m_Prefab;
    private static Vector3 _spawnOffset = new Vector3(0.5f, 0.5f, 0.0f);

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = m_Sprite;
        tileData.gameObject = m_Prefab;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if(go != null)
        {
            go.transform.position += _spawnOffset;
        }
        return base.StartUp(position, tilemap, go);
    }
}
