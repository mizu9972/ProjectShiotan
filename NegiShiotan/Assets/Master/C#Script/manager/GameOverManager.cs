using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField, Header("HPでのゲームオーバーキャンバス")]
    private GameObject HPGameOverObj = null;
    [SerializeField, Header("残機でのゲームオーバーキャンバス")]
    private GameObject ZankiGameOverObj = null;

    // Start is called before the first frame update
    void Start()
    {
        HPGameOverObj.SetActive(false);
        ZankiGameOverObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HPGameOverFunction()
    {

        HPGameOverObj.SetActive(true);
    }

    public void ZankiGameOverFunction()
    {
        ZankiGameOverObj.SetActive(true);

    }
}
