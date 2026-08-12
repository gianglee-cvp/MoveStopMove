using UnityEngine;

public class WeaponBase : GameUnit
{
    [SerializeField] protected Vector3 localTF; 
    [SerializeField] protected Vector3 localRT;
    public bool CheckActiveWeapon()
    {
        Debug.Log(gameObject.activeSelf);
        return gameObject.activeSelf == false;
    }
    public void OnInit(Transform rightHandTf)
    {
        gameObject.SetActive(true);
        // tf.SetParent(rightHandTf , false);
        tf.localPosition = localTF;
        tf.localRotation = Quaternion.Euler(localRT); 
    }
    public void OnDeactive()
    {
        gameObject.SetActive(false);
        Debug.Log("b");
    }
    public void Active()
    {
        gameObject.SetActive(true);
    }
}