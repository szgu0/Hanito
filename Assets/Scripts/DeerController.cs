using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace StarterAssets
{
    public class DeerController : MonoBehaviour
    {
        public Transform player; // 玩家对象
        public float chaseRange = 10f; // 鹿追玩家的范围
        public float patrolRange = 100f; // 巡逻半径
        public float walkSpeed = 3.5f; // 行走速度
        public float runSpeed = 5f; // 奔跑速度

        private NavMeshAgent agent;
        private Animator animator;
        private Vector3 patrolTarget;
        private bool isChasing = false;
        private bool isEat = false;
        public Transform EatTarget;
public GameObject EatImage;


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            SetRandomPatrolTarget();
        }

        public void GoodLightOn()
        {
            StartChasing();
            agent.SetDestination(player.position);
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!isEat)
            {
                if (distanceToPlayer <= chaseRange) // 检测是否追玩家
                {
                    StartChasing();
                    agent.SetDestination(player.position);
                }
                else
                {
                    if (isChasing) // 退出追逐后重新设置巡逻点
                    {
                        StopChasing();
                        SetRandomPatrolTarget();
                    }

                    if (!agent.pathPending && agent.remainingDistance < 0.5f) // 巡逻点到达后设置新目标
                    {
                        SetRandomPatrolTarget();
                    }
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GamePlayerController player = other.GetComponent<GamePlayerController>();
                if (player != null)
                {
                    StartEating();
                    player.Dead();
                    other.transform.parent = EatTarget;
                    other.transform.position = EatTarget.position;
                    EatImage.SetActive(true);
                }
            }
        }

        void SetRandomPatrolTarget()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1))
            {
                patrolTarget = hit.position;
                agent.SetDestination(patrolTarget);
            }
        }

        void StartChasing()
        {
            isChasing = true;
            agent.speed = runSpeed;
            animator.SetBool("IsChasing", true); // 切换到奔跑动画
        }

        void StopChasing()
        {
            isChasing = false;
            agent.speed = walkSpeed;
            animator.SetBool("IsChasing", false); // 切换到行走动画
        }

        void StartEating()
        {
            animator.SetTrigger("Eat"); // 切换到行走动画
            isEat = true;
        }
    }
}