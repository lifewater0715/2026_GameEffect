using UnityEngine;

public class week07_Water : MonoBehaviour
{
    [Header("water")]
    public Renderer waterRanderer;

    [Header("waterDir")]
    public Vector2 waterDir;

    [Header("waterSpeed")]
    public float waterSpeed;

    Material material;
    Vector2 offset;


    void Awake()
    {
        material = waterRanderer.material;
        offset = Vector2.zero;
    }

    void Update()
    {
        offset += waterDir * waterSpeed * Time.deltaTime;

        offset.x = Mathf.Repeat(offset.x,1f);
        offset.y = Mathf.Repeat(offset.y,1f);

        material.SetVector("_WaterOffset",new Vector4(offset.x,offset.y,0f,0f));
    }
}
