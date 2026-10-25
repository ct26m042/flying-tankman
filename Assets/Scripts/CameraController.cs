using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform ship;

    Vector3 offset;
    Quaternion rotationOffset;

    void Start()
    {
        Quaternion facing = FacingWithoutRoll();
        offset = Quaternion.Inverse(facing) * (transform.position - ship.position);
        rotationOffset = Quaternion.Inverse(facing) * transform.rotation;
    }

    void LateUpdate()
    {
        Quaternion facing = FacingWithoutRoll();
        transform.position = ship.position + facing * offset;
        transform.rotation = facing * rotationOffset;
    }

    Quaternion FacingWithoutRoll()
    {
        return Quaternion.LookRotation(ship.forward, Vector3.up);
    }
}
