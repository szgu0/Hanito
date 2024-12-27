using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

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
        private bool isW = false;
        public Transform EatTarget;
        public GameObject EatImage;

        private bool firstTimehasing = true;
        private bool firstTimehasingend = true;

        public CinemachineVirtualCamera virtualCamera; // 绑定你的虚拟摄像机
        public float lerpDuration = 1.2f; // 移动时间

        private bool isLerping = false;
        private float elapsedTime = 0f;
        private Vector3 startPosition;
        private Transform originalFollow; // 保存原始的 Follow 对象

        public Vector3 endPosition;
        public bool isendPosition;
        public AudioSource m_MyAudioSource;




        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            SetRandomPatrolTarget();
        }

        public void GoodLightOn(Transform LayTransform)
        {
            isW = false;
            StartChasing();
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            agent.SetDestination(player.position);
            if (firstTimehasing)
            {
                agent.Warp(new Vector3(14.4f, 3.5f, -0.8f));
                transform.rotation = Quaternion.Euler(0, 90, 0);
                firstTimehasing = false;
                //Debug.Log("船");
                // 保存当前 Follow 对象并移除
                originalFollow = virtualCamera.Follow;
                virtualCamera.Follow = null;
                startPosition = virtualCamera.transform.position;
                isLerping = true;
                elapsedTime = 0f;
            }
            else if (distanceToPlayer > 10f) agent.Warp(player.position + new Vector3(-12f, 0, 0f));
            if(LayTransform != null)
            {
                endPosition = LayTransform.position;
                isendPosition = true;
            }
            else
            {
                isendPosition = false;
            }
            //Debug.Log(player.position + new Vector3(-8f, 0, 0f));
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!isEat)
            {
                if (isW)
                {
                    if (agent.remainingDistance < 0.5f && distanceToPlayer > 20f)
                    {
                        StopChasing();
                        SetRandomPatrolTarget();
                        isW = false;
                    }
                }
                else if (distanceToPlayer <= chaseRange) // 检测是否追玩家
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

            if (isLerping)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / lerpDuration); // 计算插值时间

                // Lerp 到目标位置
                virtualCamera.transform.position = Vector3.Lerp(startPosition, transform.position + new Vector3(4, 5f, -10f), t);

                if (t >= 1f) // 完成移动
                {
                    isLerping = false;
                    elapsedTime = 0f;
                    virtualCamera.Follow = originalFollow;
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&&!isW)
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
        public void MoveToExtendedPoint()
        {
            // 计算玩家与鹿的相对方向向量
            // Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 directionToPlayer = new Vector3(1f, 0f, 0.2f);

            // 延伸出目标点
            Vector3 extendedPoint = transform.position + directionToPlayer * 70f;
            if(firstTimehasingend)
            {
                firstTimehasingend = false;
                extendedPoint = new Vector3(234.2f,26.0f,58.4f);
            }
            if(isendPosition)
            {
                extendedPoint = endPosition;
            }

            // 设置 NavMeshAgent 移动到目标点
            StartChasing();
            agent.SetDestination(extendedPoint);
            isW = true;
        }

        public void StartChasing()
        {
            isChasing = true;
            agent.speed = runSpeed;
            animator.SetBool("IsChasing", true); // 切换到奔跑动画
            m_MyAudioSource.Play();
        }

        void StopChasing()
        {
            isChasing = false;
            agent.speed = walkSpeed;
            animator.SetBool("IsChasing", false); // 切换到行走动画
            m_MyAudioSource.Stop();
        }

        void StartEating()
        {
            animator.SetTrigger("Eat"); // 切换到行走动画
            isEat = true;
        }
    }
}