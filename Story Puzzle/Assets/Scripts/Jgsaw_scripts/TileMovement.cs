using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMovement : MonoBehaviour
{
    public Tile tile { get; set; }
    
    private Vector3 mOffset = new Vector3(0.0f, 0.0f, 0.0f);

    private SpriteRenderer mSpriteRenderer;

    public delegate void DelegateOnTileInPlace(TileMovement tm);
    public DelegateOnTileInPlace onTileInPlace;

    void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();

    }
    private Vector3 GetCorrectPositionn()
    {
        return new Vector3(
            tile.xIndex * 100f, 
            tile.yIndex * 100f, 
            0f);
    }

    private void OnMouseDown()
    {
        if (!GameApp.Instance.TileMovementEnabled) return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        mOffset = transform.position - 
            Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                0.0f));

        // tiles sorting
        Tile.tilesSorting.BringToTop(mSpriteRenderer);
    }

    private void OnMouseDrag()
    {
        if (!GameApp.Instance.TileMovementEnabled) return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 curScreenPoint = new Vector3(
            Input.mousePosition.x, 
            Input.mousePosition.y, 
            0.0f);
        Vector3 curPosition = 
            Camera.main.ScreenToWorldPoint(curScreenPoint) + 
            mOffset;
        transform.position = curPosition;
    }

    private void OnMouseUp()
    {
        if (!GameApp.Instance.TileMovementEnabled) return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        float dist = (transform.position - 
            GetCorrectPositionn()).magnitude;
        
        if (dist > 40.0f)
            AudioManager.instance.PlaySFX("DropTile");
        else 
        {
            AudioManager.instance.PlaySFX("CorrectTile");

            transform.position = GetCorrectPositionn();
            onTileInPlace?.Invoke(this);
            Tile.tilesSorting.SendToBottom(mSpriteRenderer);
        }
    }
}
 