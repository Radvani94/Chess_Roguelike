using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MouseController : MonoBehaviour
{
    public float speed;
    public GameObject characterPrefab;
    public CharacterInfo character;

    private PathFinder pathFinder;
    private List<OverlayTile> path = new List<OverlayTile>();

    private UnityAction ClickAction;
    private bool input_click;

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
        pathFinder = new PathFinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
        if(focusedTileHit.HasValue)
        {
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

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }
}
