using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private Transform target;

    [Header("Base Offset")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotationOffset;

    [Header("Offset Per Size")]
    [SerializeField] private float offsetYPerSize = 2f;
    [SerializeField] private float offsetZPerSize = 1.5f;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float offsetSmoothSpeed = 5f;

    private float size = 1f;

    private Vector3 currentOffset;
    private Vector3 targetOffset;

    public void OnInit()
    {
        transform.rotation = Quaternion.Euler(rotationOffset);

        currentOffset = offset;
        targetOffset = offset;

        if (target != null)
        {
            transform.position = target.position + currentOffset;
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        UpdateTargetOffset();

        // Smooth offset
        currentOffset = targetOffset;
        // Smooth follow
        Vector3 targetPosition = target.position + currentOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }

    private void UpdateTargetOffset()
    {
        targetOffset = new Vector3(
            offset.x,
            offset.y + (size - 1f) * offsetYPerSize,
            offset.z - (size - 1f) * offsetZPerSize
        );
    }

    public void SetSize(float newSize)
    {
        size = Mathf.Max(1f, newSize);
    }
}