using UnityEngine;

public class BaseTile : MonoBehaviour
{
    // blastable objelerin ustune oturacagi tile
    // ayni zamanda matrixte tutulacak tile
    // ustunde hangi tile oldugu bilgisi tutulmali ya da ayni matrix blastable icin de olmali
    // 

    public Vector2Int MatrixPosition;
    private bool _empty = true;

    #region Build-in
    void OnEnable()
    {
        GridManager.Instance.AddBaseTile(this);
    }
    void OnDisable()
    {
        GridManager.Instance.RemoveBaseTile(this);
    }
    #endregion

    public bool IsEmpty() { return _empty; }
    public void SetEmpty(bool empty) => _empty = empty;
    public void SetEmpty() => _empty = true;
}
