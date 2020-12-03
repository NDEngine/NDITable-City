using UnityEngine;
using xFrame.NDITable.Unity.EventSystems;
using xFrame.Unity;

public abstract class MarkerController : XComponent {
    public abstract void MarkerAppear(MarkerEventData data);

    public abstract void MarkerUpdate(MarkerEventData data);

    public abstract void MarkerDisappear(MarkerEventData data);

    protected void MarkerPositionToWorldPosition(float x, float y) {
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 position = new Vector3(x, y, screenPosition.z);

        var markerPositionInWorld = Camera.main.ScreenToWorldPoint(position);

        transform.position = markerPositionInWorld;
    }
}
