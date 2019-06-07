using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewLane", menuName = "Lanes/Static Width Lane")]
public class StaticWidthLane : ScriptableObject
{
    public int Width { get { return _width; } }
    [SerializeField]
    private int _width;
    [SerializeField]
    private TileQueue _backgroundTiles;

    /// <summary>
    /// Draws lane with specified length and default width.
    /// </summary>
    /// <param name="background"></param>
    /// <param name="position"></param>
    /// <param name="laneLength"></param>
    /// <returns></returns>
    public int SetupLane(Tilemap background, Vector3Int position, int laneLength)
    {
        return LaneDrawer.DrawLane(background, _backgroundTiles, position, laneLength, _width);
    }
}
