using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UILevelText : MonoBehaviour
{
    [SerializeField] private Transform camTF;
    [SerializeField] protected TMP_Text levelText;
    [SerializeField] protected TMP_Text nameText;

    void Awake()
    {
        camTF = CameraFollow.Instance.transform;
    }

    void LateUpdate()
    {
        if (camTF == null) return;

        Vector3 lookDirection = transform.position - camTF.position;
        if (lookDirection.sqrMagnitude <= 0f) return;

        transform.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
    }

    public void SetLevelText(int level)
    {
        levelText.text = level.ToString();
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }
}
