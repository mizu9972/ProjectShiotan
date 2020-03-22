using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    [SerializeField, Header("GameManager")]
    GameObject m_GameManager = null;

    NavMeshAgent m_NavAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        m_NavAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    //プレイヤーを追いかける
    private void Chase()
    {
        var Player_ = m_GameManager.GetComponent<GameManagerScript>().Player;
        if (Player_ != null)
        {
            m_NavAgent.destination = Player_.transform.position;
        }
    }
}
