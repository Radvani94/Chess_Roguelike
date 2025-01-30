using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    public List<OverlayTile> _overlayTiles;

    private bool _overlaySetCreated;
    public bool OverlaySetCreated { get { return _overlaySetCreated; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        //Tilemap[] tileMaps = gameObject.GetComponentsInChildren<Tilemap>();

        map = new Dictionary<Vector2Int, OverlayTile>();
        Tilemap tileMap = gameObject.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tileMap.cellBounds;
        _overlayTiles = new List<OverlayTile>();
        _overlaySetCreated = false;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        
                        OverlayTile overlayTile = null;
                        Vector3 cellWorldPosition = Vector3.zero;
 
                        overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);
                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = 34;

                        //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                        overlayTile.gridLocation = tileLocation;
                        map.Add(tileKey, overlayTile);
                        _overlayTiles.Add(overlayTile);
                    }
                    else
                    {
                    }
                }
            }
        }
        _overlaySetCreated = true;
        Debug.Log("Map Created");

    }

    public void GetOverlayTiles( out List<OverlayTile> _tileSet)
    {
        if (_overlaySetCreated) _tileSet = _overlayTiles;
        else _tileSet = null;
    }
}
