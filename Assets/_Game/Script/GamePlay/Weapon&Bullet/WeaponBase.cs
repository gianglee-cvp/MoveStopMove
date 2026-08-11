using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected Transform tf;
    [SerializeField] protected Vector3 localTF; 
    [SerializeField] protected Vector3 localRT;
    public void OnInit(Transform rightHandTf)
    {
        gameObject.SetActive(true);
        tf.SetParent(rightHandTf , false);
        tf.localPosition = localTF;
        tf.localRotation = Quaternion.Euler(localRT); 
    }
    public void OnDeactive()
    {
        gameObject.SetActive(false);
    }
}