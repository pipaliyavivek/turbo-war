using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamFollow : MonoBehaviour
{
    public static SimpleCamFollow instance;
    [SerializeField] internal Transform m_Target;
    [SerializeField] private Vector3 m_Padding;
    [SerializeField] private float m_FollowSpeed;
    float Temp_y = 5f;
    internal bool IsPlayerInSide = false;
    [SerializeField] private Vector3 m_CameraPivot;

    private void Awake() => instance = this;

    private void Start()
    {
        //   Gamemanager.Instance.Start();
        //SetcamerastartPos();
        //m_Target = GameObject.FindGameObjectWithTag("Player").transform.parent.parent;
        //transform.position = m_Target.position + m_Padding;
    }

    private void Update()
    {
        if (!m_Target) return;
        if (IsPlayerInSide)
        {
            transform.LookAt(m_Target);
            Vector3 pos = Vector3.MoveTowards(transform.position, m_Target.position + m_Padding, Time.deltaTime * m_FollowSpeed);
            pos.y = m_CameraPivot.y;
            /* pos.x = m_CameraPivot.x;*/
            /* if (m_CameraPivot.z != 0)
             {
                 m_Padding.z = m_CameraPivot.z;
             }*/
            var NewPos = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * m_FollowSpeed);
            transform.position = NewPos;
        }
        else
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, m_Target.position + m_Padding, Time.deltaTime * m_FollowSpeed);
            pos.y = Temp_y;//comment Latest
            transform.position = pos;
        }
    }
    public void SetcamerastartPos()
    {
        //    m_CameraPivot = Player.Instance.Camerapivot;
        Debug.Log("Set camera start pos ");
        // m_Target = GameObject.FindGameObjectWithTag("Player").transform.parent.parent;
        //  m_Padding = Gamemanager.Instance.Level.transform.GetChild(1).localPosition;
        //  m_Target = Gamemanager.Instance.Level.transform.Find("Player");
        //  transform.position = m_Target.position + m_Padding;
        Temp_y = transform.position.y;
    }
}



