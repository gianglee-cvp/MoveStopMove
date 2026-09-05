using UnityEngine;

public class WeaponBase : GameUnit
{
    public void OnInit()
    {
        gameObject.SetActive(true);
        // tf.SetParent(rightHandTf , false);
        tf.localPosition = Vector3.zero;
        tf.localRotation = Quaternion.Euler(Vector3.zero); 
        tf.localScale = Vector3.one;
    }
    public void OnDeactive()
    {
        gameObject.SetActive(false);
    }
    public void Active()
    {
        gameObject.SetActive(true);
    }
}