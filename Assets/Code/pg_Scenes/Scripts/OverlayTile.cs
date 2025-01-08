using UnityEngine;


public class OverlayTile : MonoBehaviour
{
    Color tile_default = new Color(1f, 0.3411214f, 0.2509804f, 1f);

    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation;

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = tile_default;
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }
}
