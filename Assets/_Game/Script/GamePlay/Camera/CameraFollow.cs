using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(0, 20f, -14.5f);
    [SerializeField] private Vector3 rotationOffset;

    [SerializeField] private float followSpeed = 5f;
    //TODO cho vao init 
    void Awake()
    {
        transform.rotation = Quaternion.Euler(rotationOffset);
        transform.position = target.position + offset;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}