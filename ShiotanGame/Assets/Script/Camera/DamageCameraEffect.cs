using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCameraEffect : MonoBehaviour
{

    [SerializeField, Header("ダメージ演出マテリアル")]
    private Material DamageMat = null;
    [SerializeField, Header("ダメージ演出テクスチャ")]
    private Texture DamageTex;
    [SerializeField, Header("ダメージ演出色")]
    private Color DamageColor = new Color(0, 0, 0, 1);
    // Start is called before the first frame update
    void Start()
    {
        DamageMat.SetTexture("_DamageTex", DamageTex);
        InActive();
    }

    //色変化
    public void Active()
    {
        DamageMat.SetColor("_Color", DamageColor);
    }
    public void InActive()
    {
        DamageMat.SetColor("_Color", new Color(0, 0, 0, 1));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, DamageMat);
    }
}
