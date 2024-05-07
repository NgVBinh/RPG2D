using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Hit FX infor")]
    [SerializeField] private float hitFXDuration;
    [SerializeField] private Material hitMaterial;
    private Material lastMaterial;

    [Header("Ailment")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMaterial = spriteRenderer.material;
        //Invoke("HitEffect", 3);
    }

    public void MakeTransprent(bool transprent)
    {
        if (transprent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator HitEffect()
    {
        Color currentColor = spriteRenderer.color;

        spriteRenderer.material = hitMaterial;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(hitFXDuration);
        spriteRenderer.color = currentColor;
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

    private void CanelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void igniteFXFor(float seconds,float repeatTimer)
    {
        InvokeRepeating("IgniteColorFX", 0, repeatTimer);
        Invoke("CanelColorChange", seconds);
    }

    // change beetwen two color
    private void IgniteColorFX()
    {
        if(spriteRenderer.color!= igniteColor[0])
        {
            spriteRenderer.color = igniteColor[0];
        }
        else
        {
            spriteRenderer.color = igniteColor[1];
        }
    }


    public void chillFXFor(float seconds, float repeatTimer)
    {
        InvokeRepeating("chillColorFX", 0, repeatTimer);
        Invoke("CanelColorChange", seconds);
    }

    // change beetwen two color
    private void chillColorFX()
    {
        if (spriteRenderer.color != chillColor[0])
        {
            spriteRenderer.color = chillColor[0];
        }
        else
        {
            spriteRenderer.color = chillColor[1];
        }
    }

    public void shockFXFor(float seconds, float repeatTimer)
    {
        InvokeRepeating("shockColorFX", 0, repeatTimer);
        Invoke("CanelColorChange", seconds);
    }

    // change beetwen two color
    private void shockColorFX()
    {
        if (spriteRenderer.color != shockColor[0])
        {
            spriteRenderer.color = shockColor[0];
        }
        else
        {
            spriteRenderer.color = shockColor[1];
        }
    }
}
