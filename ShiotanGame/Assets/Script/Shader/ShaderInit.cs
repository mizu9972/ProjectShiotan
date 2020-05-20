using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//マテリアルを個別のパラメータで初期化する
//setメソッドの引数とシェーダー側のプロパティの名前と違う場合は失敗するので注意
public class ShaderInit : MonoBehaviour
{
    [SerializeField, Header("色")]
    private Color color;
    [SerializeField, Header("テクスチャ")]
    private Texture texture;

    private Material m_mat = null;
    // Start is called before the first frame update
    void Start()
    {
        m_mat = this.gameObject.GetComponent<Renderer>().material;

        m_mat.SetColor("_Color", color);
        m_mat.SetTexture("_MainTex", texture);
    }

}
