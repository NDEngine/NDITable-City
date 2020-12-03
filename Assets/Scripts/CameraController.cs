using Google.Protobuf.WellKnownTypes;
using SkyfallServices;
using UnityEngine;
using xFrame.NDITable.Unity;
using xFrame.NDITable.Unity.EventSystems;

public class CameraController : MarkerController {
    public SkyfallClientComponent Client;

    private bool createScheme = false;

    public override void MarkerAppear(MarkerEventData data) {
        if (!Client.IsConnected())
            return;

        if (createScheme) {
            var pos = CoordMappingSchemeManager.Instance.Mapping(
                new Vector3((float)(data.Data.X / Screen.width), (1.0f - (float)(data.Data.Y / Screen.height))));

            FreeCallRequest request = new FreeCallRequest();

            request.Params.Add(Any.Pack(new StringValue { Value = "SetCameraPositionAndDirection" }));
            request.Params.Add(Any.Pack(new Vec3 { X = pos.x, Y = pos.y, Z = pos.z }));
            request.Params.Add(Any.Pack(new FloatValue { Value = (float)data.Data.Angle + GetComponent<Marker>().AngleOffset }));

            Client.RequestOneway("CommonService@freeCall", request);
        }
    }

    public override void MarkerUpdate(MarkerEventData data) {
        if (!Client.IsConnected())
            return;

        if (!createScheme) {
            createScheme = true;

            StringValue request = new StringValue {Value = "City"};

            Client.Request("CommonService@mapping", request, response => {
                var positions = (MappingPositions) response;

                var anchor = AlignAnchorGroupTable.Instance.GetComponent("City");

                var anchor1 = GetAnchor(anchor.Index1.GetComponent<RectTransform>().anchoredPosition);
                var anchor2 = GetAnchor(anchor.Index2.GetComponent<RectTransform>().anchoredPosition);
                var anchor3 = GetAnchor(anchor.Index3.GetComponent<RectTransform>().anchoredPosition);

                var scheme = new CoordMappingScheme(new Vector4[] {
                    anchor1,
                    anchor2,
                    anchor3
                }, new Vector4[] {
                    new Vector3(positions.Index1.X, positions.Index1.Y, positions.Index1.Z),
                    new Vector3(positions.Index2.X, positions.Index2.Y, positions.Index2.Z),
                    new Vector3(positions.Index3.X, positions.Index3.Y, positions.Index3.Z),
                });

                CoordMappingSchemeManager.Instance.Register("City", scheme);
            });
        }

        if (createScheme) {
            var pos = CoordMappingSchemeManager.Instance.Mapping(
                new Vector3((float)(data.Data.X / Screen.width), (1.0f - (float)(data.Data.Y / Screen.height))));

            FreeCallRequest freeCallRequest = new FreeCallRequest();

            freeCallRequest.Params.Add(Any.Pack(new StringValue { Value = "SetCameraPositionAndDirection" }));
            freeCallRequest.Params.Add(Any.Pack(new Vec3 { X = pos.x, Y = pos.y, Z = pos.z }));
            freeCallRequest.Params.Add(Any.Pack(new FloatValue { Value = (float)data.Data.Angle + GetComponent<Marker>().AngleOffset }));

            Client.RequestOneway("CommonService@freeCall", freeCallRequest);
        }
    }

    public override void MarkerDisappear(MarkerEventData data) { }

    private static Vector3 GetAnchor(Vector3 source) {
        return new Vector3(Mathf.Abs(source.x / Screen.width), Mathf.Abs(source.y / Screen.height), 1.0f);
    }
}
