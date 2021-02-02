using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int _itemCode;

    private SpriteRenderer spriteRenderer;

    // Getter and setter for item based on code
    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    // populate sprite renderer.
    // Item class is attached to a game object
    // will have child with sprite renderer componentn
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren <SpriteRenderer>();
    }

    private void Start()
    {
        if( ItemCode != 0 )
        {
            Init(ItemCode);
        }
    }

    public void Init(int itemCodeParam)
    {

    }
}
