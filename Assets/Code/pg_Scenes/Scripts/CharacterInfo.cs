using UnityEngine;
using DG.Tweening;

public class CharacterInfo : MonoBehaviour
{

    public OverlayTile activeTile;
    private GameObject RendererSprite;
    private void Start()
    {
        RendererSprite = transform.Find("PlayerSprite").gameObject;
        RendererSprite.transform.DOLocalJump(new Vector3(0f, 0.1f, 0f), 0.2f, 1, 0.75f).SetLoops(-1);
        //RendererSprite.transform.DOLocalMoveY(0.2f, 1).SetLoops(-1).SetEase(Ease.InOutBounce);


    }

}
