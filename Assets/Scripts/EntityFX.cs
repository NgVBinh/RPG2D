using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Hit FX infor")]
    [SerializeField] private float hitFXDuration;
    [SerializeField] private Material hitMaterial;
    private Material lastMaterial;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMaterial = spriteRenderer.material;
        Invoke("HitEffect", 3);
    }


    private IEnumerator HitEffect()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(hitFXDuration);
        spriteRenderer.material = lastMaterial;
    }

    private void HitBlinkRed()
    {
        if(spriteRenderer.color!= Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancelBlinkRed()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
