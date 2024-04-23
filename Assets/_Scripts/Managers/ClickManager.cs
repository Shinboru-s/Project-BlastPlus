using Criaath.MiniTools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private EventTrigger _eventTrigger;
    [SerializeField] private LayerMask _baseTileLayer;
    [SerializeField] private float _raycastDistance = 10f;
    private Camera _mainCamera;
    private bool _clickable;
    public UnityEvent OnClick;

    private void Start()
    {
        _mainCamera = Camera.main;

        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => CheckClick((PointerEventData)data));
        _eventTrigger.triggers.Add(entry);
    }
    public void CheckClick(PointerEventData pointerEventData)
    {
        if (CanClickable() is not true) return;
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(pointerEventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, _raycastDistance, _baseTileLayer);

        if (hit.collider == null) return;

        BaseTile baseTile = hit.collider.GetComponent<BaseTile>();

        if (baseTile == null)
        {
            CriaathDebugger.Log("Click Manager", Color.green, $"{hit.collider.gameObject.name} is in BaseTile layer!", Color.white);
            return;
        }
        BlastableManager.Instance.TryToBlast(baseTile.MatrixPosition);
        OnClick?.Invoke();
    }
    public void ToggleClickable(bool state)
    {
        _clickable = state;
    }
    private bool CanClickable() => _clickable;
}
