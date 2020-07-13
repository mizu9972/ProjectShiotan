using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStageName : MonoBehaviour
{
    [Header("遷移先ステージ名")]
    public string StageName;
    
    public string GetName()
    {
        return StageName;
    }
}
