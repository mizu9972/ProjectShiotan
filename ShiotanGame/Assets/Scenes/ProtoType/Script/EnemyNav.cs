using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    [SerializeField, Header("GameManager")]
    GameObject m_GameManager = null;

    private NavMeshAgent m_NavAgent;
    private GameObject m_TargetObject = null;
    // Start is called before the first frame update
    void Start()
    {
        m_NavAgent = this.GetComponent<NavMeshAgent>();
        m_TargetObject = m_GameManager.GetComponent<GameManagerScript>().Player;

    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    //プレイヤーを追いかける
    private void Chase()
    {
        if (m_TargetObject != null)
        {
            m_NavAgent.destination = m_TargetObject.transform.position;
        }
    }
}
