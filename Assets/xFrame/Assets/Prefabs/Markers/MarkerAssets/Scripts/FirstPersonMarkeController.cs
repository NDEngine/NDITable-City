using TMPro;
using UnityEngine;
using xFrame.NDITable.Unity;
using xFrame.NDITable.Unity.EventSystems;
using xFrame.Unity;

public class FirstPersonMarkeController : MarkerController {
    public string MarkerName;  

    private Marker marker;
    private AnimationEvents events;

    private GameObject rotation;
    private GameObject tips;

    private TextMeshProUGUI text;

    void Awake() {
        events = GetComponent<AnimationEvents>();
        marker = GetComponent<Marker>();

        rotation = Root.GetChild(0).gameObject;
        tips = Root.GetChild(3).gameObject;

        text = tips.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Reset() {
        if (string.IsNullOrEmpty(MarkerName)) {
            MarkerName = "第一人称视角";
        }
    }

    public override void MarkerAppear(MarkerEventData data) {
        this.text.text = MarkerName;

        MarkerPositionToWorldPosition((float)data.Data.X, (float)data.Data.Y);

        events.GotoAndPlay(1);
    }

    public override void MarkerUpdate(MarkerEventData data) {
        if (marker != null) {
            if (marker.MarkerFollow) {
                MarkerPositionToWorldPosition((float) data.Data.X, (float) data.Data.Y);
            }

            rotation.transform.localEulerAngles = new Vector3(0, (float)data.Data.Angle, 0);
            tips.SetActive(!(data.Data.Angle > 80.0f && data.Data.Angle < 250.0f));
        }
    }

    public override void MarkerDisappear(MarkerEventData data) {
        events.GotoAndPlay(71);
    }

    public void SetText(string value) {
        MarkerName = value;

        this.text.text = MarkerName;
    }
}
