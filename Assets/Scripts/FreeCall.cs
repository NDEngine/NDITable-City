using SkyfallServices;
using UnityEngine;
using xFrame.NDITable.Unity;
using xFrame.Skyfall.FreeCallManager;

public class FreeCall : MonoBehaviour {
    void Awake() {
        this.Register();
    }

    [FreeCall("SetCameraPositionAndDirection")]
    public void SetCameraPositionAndDirection(Vec3 position, float angle) {
        var cam = NDITableGameObjectTable.Instance.GetComponent("MainCamera").GetComponent<Camera>();

        cam.transform.position = new Vector3(position.X, cam.transform.position.y, position.Z);

        cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, angle, cam.transform.eulerAngles.z);
    }
}
