using System.ComponentModel;
using UnityEngine;
public enum BulletType
{
    Knight = 0, 
    Hammer = 1,
    Boomerang = 2
}

public class BulletBase : MonoBehaviour
{
    protected virtual BulletType Type => BulletType.Knight;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float range = 15f;
    [SerializeField] protected Rigidbody rb;
    protected Vector3 direction;

    protected Vector3 startPos;

    public void Init(Transform shootPoint)
    {
        startPos = shootPoint.position;
        direction = shootPoint.forward;

        transform.position = startPos;
        transform.rotation = shootPoint.rotation;
        // transform.rotation.y = shootPoint.rotation.y;
    }
    public virtual void Throw(Vector3 des)
    {
    }
    public bool CheckDisTance(Vector3 des)
    {
        Vector3 offset = transform.position - des ; 
        return (offset.sqrMagnitude < 0.01)?true : false;
    }

    // private void Update()
    // {
    //     transform.position += direction * speed * Time.deltaTime;

    //     float traveledSqr = (transform.position - startPos).sqrMagnitude;

    //     if (traveledSqr >= range * range)
    //     {
    //         gameObject.SetActive(false); // hoặc Destroy(gameObject);
    //     }
    // }
}