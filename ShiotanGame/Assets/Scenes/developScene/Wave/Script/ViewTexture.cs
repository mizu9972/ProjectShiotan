using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTexture : MonoBehaviour
{
    public GameObject DepthScanCamera = null;
    private DispDepth m_DispDepthCS;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        TexSet();
    }

    void TexSet()
    {
        if(DepthScanCamera == null)
        {
            return;
        }
        m_DispDepthCS = DepthScanCamera.GetComponent<DispDepth>();
        RenderTexture renderTex = m_DispDepthCS.getDepthRenderTexture();

        Material myMat = this.GetComponent<Renderer>().material;
        myMat.SetTexture("_MainTex", renderTex);
    }
}
