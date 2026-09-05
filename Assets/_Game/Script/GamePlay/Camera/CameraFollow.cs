using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [System.Serializable]
    private class CameraConfig
    {
        public Vector3 offset;
        public Vector3 rotation;
        public bool followTarget;
    }
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private CameraConfig mainMenuConfig;
    [SerializeField] private CameraConfig shopWeaponConfig;
    [SerializeField] private CameraConfig shopSkinConfig;
    [SerializeField] private CameraConfig playConfig;

    [Header("Offset Per Size")]
    [SerializeField] private float offsetYPerSize = 2f;
    [SerializeField] private float offsetZPerSize = 1.5f;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    private Dictionary<Enum_GameState, CameraConfig> stateConfigs;
    private Transform tf;
    public Transform TF
    {
        get
        {
            if(tf == null)
            {
                tf = transform;
            }
            return tf; 
        }
    }

    private float size = 1f;
    private CameraConfig currentConfig;
    private bool isFollowingTarget;
    private Vector3 currentOffset;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    public void OnInit()
    {
        BuildStateConfigs();
        desiredPosition = TF.position;
        desiredRotation = TF.rotation;
        ChangeByState(Enum_GameState.Loading, true);
    }

    private void Update()
    {
        if (isFollowingTarget && target != null)
        {
            currentOffset = GetOffsetWithSize(currentConfig.offset);
            desiredPosition = target.position + currentOffset;
        }

        TF.position = Vector3.Lerp(TF.position, desiredPosition, followSpeed * Time.deltaTime);
        TF.rotation = Quaternion.Slerp(TF.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
    }

    public void ChangeByState(Enum_GameState state, bool instant = false)
    {
        if (stateConfigs == null)
        {
            BuildStateConfigs();
        }

        if (stateConfigs != null && stateConfigs.TryGetValue(state, out CameraConfig config))
        {
            ApplyConfig(config, instant);
        }
    }

    public void SetSize(float newSize)
    {
        size = Mathf.Max(1f, newSize);
        if (currentConfig != null)
        {
            currentOffset = GetOffsetWithSize(currentConfig.offset);
            if (target != null)
            {
                desiredPosition = target.position + currentOffset;
            }
        }
    }

    private void ApplyConfig(CameraConfig config, bool isGoToDesDirect)
    {
        if (config == null)
        {
            return;
        }

        currentConfig = config;
        isFollowingTarget = config.followTarget;
        currentOffset = GetOffsetWithSize(config.offset);
        desiredRotation = Quaternion.Euler(config.rotation);

        if (target != null)
        {
            desiredPosition = target.position + currentOffset;
        }

        if (isGoToDesDirect)
        {
            TF.position = desiredPosition;
            TF.rotation = desiredRotation;
        }
    }

    private Vector3 GetOffsetWithSize(Vector3 baseOffset)
    {
        return new Vector3(
            baseOffset.x,
            baseOffset.y + (size - 1f) * offsetYPerSize,
            baseOffset.z - (size - 1f) * offsetZPerSize
        );
    }
    public Camera GetMainCam()
    {
        return cam;
    }

    private void BuildStateConfigs()
    {
        stateConfigs = new Dictionary<Enum_GameState, CameraConfig>
        {
            { Enum_GameState.MainMenu, mainMenuConfig },
            { Enum_GameState.ShopWeapon, shopWeaponConfig },
            { Enum_GameState.ShopSkin, shopSkinConfig },
            { Enum_GameState.Play, playConfig }
        };
    }

}
