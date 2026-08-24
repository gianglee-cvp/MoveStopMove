using UnityEngine;

public class Shield : GameUnit
{
    public void OnInit()
    {
        tf.localPosition = Vector3.zero;
        tf.localRotation = Quaternion.Euler(Vector3.zero);         
        tf.localScale = Vector3.one;
    }
}
