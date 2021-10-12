﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("Game/Enemy")]
public class Enemy : MonoBehaviour
{
    Transform m_transform;

    Animator m_ani;

    Player m_player;

    NavMeshAgent m_agent;

    float m_moveSpeed = 2.5f;

    float m_rotSpeed = 5.0f;

    float m_timer = 2;

    int m_life = 5;

    protected EnemySpawn m_spawn;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;

        m_ani = this.GetComponent<Animator>();

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_moveSpeed;

        m_agent.SetDestination(m_player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player.m_life <= 0)
        {
            return;
        }

        m_timer -= Time.deltaTime;

        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle")
            && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);

            if(m_timer > 0)
            {
                return;
            }

            if (Vector3.Distance(m_transform.position, m_player.transform.position) < 1.5f)
            {
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);
            }
            else
            {
                m_timer = 1;

                m_agent.SetDestination(m_player.transform.position);

                m_ani.SetBool("run", true);
            }
        }
        
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run")
            && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.transform.position);

                m_timer = 1;
            }

            if (Vector3.Distance(m_transform.position, m_player.transform.position) <= 1.5f)
            {
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);
            }
        }
        
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack")
            && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("attack", false);

            RotateTo();

            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);

                m_timer = 5;

                m_player.OnDamage(1);
            }
        }
        
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.death")
           && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("death", false);

            if (stateInfo.normalizedTime >= 1.0f)
            {
                GameManager.Instance.SetScore(100);

                m_spawn.m_enemyCount--;

                Destroy(this.gameObject);
            }
        }
    }

    void RotateTo()
    {
        Vector3 targetDir = m_player.transform.position - m_transform.position;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir,
            m_rotSpeed * Time.deltaTime, 0);

        m_transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;

        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);

            m_agent.ResetPath();
        }
    }

    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }
}
