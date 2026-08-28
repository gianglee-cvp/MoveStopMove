using System.Collections.Generic;
using UnityEngine;
public class Obstacle : MonoBehaviour
{
    [SerializeField] protected Transform tf;
    [SerializeField] protected float range;
    [SerializeField] protected float timeCheck;
    [SerializeField] protected LayerMask layer;
    [SerializeField] protected Renderer mesh;
    protected Material[] renderMat;
 
    [SerializeField] protected List<Material> baseMat = new List<Material>();
    [SerializeField] protected Material fadeMat; 
    protected float timer;
    [SerializeField] protected bool isInside;
    void Awake()
    {
        timer = 0f;
        isInside = false;
        renderMat = mesh.materials;
    }
    void Update()
    {
        if(!GameManager.Instance.IsGameState(Enum_GameState.Play)) return;
        if(timer > timeCheck)
        {
            CheckSphere();
        }
        timer += Time.deltaTime;
    }
    public void CheckSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(tf.position , range , layer);
        isInside = false;
        foreach(var col in colliders)
        {
            Character ch = CacheComponent<Collider,Character>.Get(col);
            if (ch is Player)
            {
                isInside = true;
                break;
            }
        }
        if (isInside)
        {
            for(int i = 0 ; i < renderMat.Length ; i++)
            {
                renderMat[i] = fadeMat;
            }
        }
        else
        {
            for(int i = 0 ; i < renderMat.Length ; i++)
            {
                renderMat[i] = baseMat[i];
            }
        }
        mesh.materials = renderMat;
    }

}