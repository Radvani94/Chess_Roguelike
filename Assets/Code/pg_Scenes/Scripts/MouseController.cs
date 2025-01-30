using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEngine.Events;

public class MouseController : MonoBehaviour
{

    public MapManager _MMInstance;
    private List<OverlayTile> _overlaySet;
    public float speed;
    public GameObject characterPrefab;
    public CharacterInfo character;

    private PathFinder pathFinder;
    private List<OverlayTile> path = new List<OverlayTile>();

    private UnityAction ClickAction;
    private bool input_click;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Vector2 startPositionPreset;

    [SerializeField] private GameObject token;


    [SerializeField] private Tilemap _playTiles;

    public Tilemap PlayTiles { get { return _playTiles; } }

    private void Awake()
    {
        ClickAction = new UnityAction(ClickInput);
    }
    private void OnEnable()
    {
        ChessEventSystem.StartListening("Input_Click", ClickAction);
    }

    private void OnDisable()
    {
        ChessEventSystem.StopListening("Input_Click", ClickAction);
    }

    private void ClickInput()
    {
        input_click = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MMInstance = MapManager.Instance;
        pathFinder = new PathFinder();

        //Vector2 mousePos2D = new Vector2(startPosition.transform.position.x, startPosition.transform.position.y);
        StartCoroutine(StartDelayCoroutine());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
        if(focusedTileHit.HasValue)
        {
            /*Debug.Log("Focused Tile: "+focusedTileHit.Value.collider.gameObject);*/
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (input_click)
            {
                overlayTile.GetComponent<OverlayTile>().ShowTile();

                if(character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile);
                }
                else
                {
                    path = pathFinder.FindPath(character.activeTile, overlayTile);
                }

                input_click = false;
            }
        }

        if(path.Count>0)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

       
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+0.001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }

    private void PositionCharacterOnTile(Vector3 tilePos)
    {
        Vector2Int mousePos2D = new Vector2Int((int)tilePos.x, (int)tilePos.y);
        OverlayTile tile;
        _MMInstance.map.TryGetValue(mousePos2D, out tile);
        Debug.Log("Got Value: " + tile);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 100f);
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("LOOP: "+ hit.collider.gameObject);
            OverlayTile t;
            if (hit.collider.gameObject.TryGetComponent<OverlayTile>(out t))
            {
                Debug.Log("FOUND TILE: " + t.gameObject);
            }

        }
        Debug.Log("END OF LOOP");
        //token.transform.position = _playTiles.CellToWorld(cellPosition1);
        /*transform.position = worldPosition;*/
        /*character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;*/
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
        foreach(RaycastHit2D hit in hits)
        {
            OverlayTile t;
            if(hit.collider.gameObject.TryGetComponent<OverlayTile>(out t))
            {
                return hit;
            }

        }
        return null;
    }

    public void PlaceCharacterOnTile(Vector2 _pos2D)
    {

        Vector2Int mousePos2D = new Vector2Int((int)_pos2D.x, (int)_pos2D.y);
        OverlayTile tile;
        _MMInstance.map.TryGetValue(mousePos2D, out tile); 
        Debug.Log("Tile Ref: "+tile);

        RaycastHit2D[] hits = Physics2D.RaycastAll(_pos2D, Vector2.zero, 100f);
        
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("FOR EACH LOOP");
            Debug.Log(hit.collider.gameObject);
            OverlayTile t;
            if (hit.collider.gameObject.TryGetComponent<OverlayTile>(out t))
            {                
                OverlayTile overlayTile = t;
                transform.position = overlayTile.transform.position;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;
                overlayTile.GetComponent<OverlayTile>().ShowTile();
                Debug.Log(overlayTile.gridLocation);
                token.transform.position = overlayTile.gridLocation;

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile);
                }
                else
                {
                    character.transform.position = overlayTile.transform.position;
                }
            }
            else
            {
                //if its a tilemap tile
            }

        }
        Debug.Log("End of Loop");
    }

    IEnumerator StartDelayCoroutine()
    {
        
        yield return new WaitForSeconds(1);
        startPositionPreset = new Vector2(58, 230);
        startPosition.transform.position = Camera.main.ScreenToWorldPoint(startPositionPreset);
        Vector2 pos = new Vector2(startPosition.transform.position.x, startPosition.transform.position.y);
        if (_MMInstance.OverlaySetCreated)
        {
            _MMInstance.GetOverlayTiles(out _overlaySet);
        }
        PlaceCharacterOnTile(pos);
    }
    /*
        Get Vec2 pos
        convert to world pos
        find Overlay Tile
        spawn Player on tile     
     */


}
