using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Track : MonoBehaviour
{
    public Grid tilemapGrid;
    public Tilemap tilemapBase;
    public Tilemap tilemapInteractable;
    public List<Lane> lanes;
    public int laneLength;
    public Vector3Int baseVector;
    private Vector3Int offsetVector = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        offsetVector.Set(baseVector.x, baseVector.y, baseVector.z);
        tilemapBase.ClearAllTiles();
        for(int i = 0; i < lanes.Count; i++)
        {
            offsetVector.x += i;
            lanes[i].CreateLane(tilemapBase, offsetVector, laneLength);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
