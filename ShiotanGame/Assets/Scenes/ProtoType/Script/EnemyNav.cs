using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    [SerializeField, Header("GameManager")]
    protected GameObject m_GameManager = null;

    [SerializeField, Header("追跡")]
    protected bool isChaseActive = true;

    protected NavMeshAgent m_NavAgent;
    protected GameObject m_TargetObject = null;
    protected GameObject m_Player = null;

    private Vector3 InitPosition;
    protected void Init()
    {
        InitPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        m_NavAgent = this.GetComponent<NavMeshAgent>();
        m_Player = m_GameManager.GetComponent<GameManagerScript>().Player;

        m_TargetObject = m_Player;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isChaseActive)
    //    {
    //        Chase();
    //    }
    //}

    //プレイヤーを追いかける
    protected void Chase()
    {
        if (m_TargetObject != null)
        {
            m_NavAgent.destination = m_TargetObject.transform.position;
        }
        else
        {
            m_NavAgent.destination = InitPosition;
        }
    }
}
