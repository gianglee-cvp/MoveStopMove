using UnityEngine;

public class Hair: GameUnit
{
    [SerializeField] Vector3 localTF;
    [SerializeField] Vector3 localRT;
    public void OnInit()
    {
        tf.localPosition = localTF;
        tf.localRotation = Quaternion.Euler(localRT);         
        tf.localScale = Vector3.one;
    }
}

public class Shield : GameUnit
{
    [SerializeField] Vector3 localTF;
    [SerializeField] Vector3 localRT;

    public void OnInit()
    {
        tf.localPosition = localTF;
        tf.localRotation = Quaternion.Euler(localRT);
        tf.localScale = Vector3.one;
    }
}
