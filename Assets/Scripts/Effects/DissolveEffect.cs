
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public float dissolveSpeed = 1f;
    public float dissolveAmount = 0f;

    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
