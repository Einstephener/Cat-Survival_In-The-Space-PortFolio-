using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Pathfinding;
using Unity.VisualScripting;

namespace Ursaanimation.CubicFarmAnimals
{
    public class Goat : MonoBehaviour
    {
        public AIPath aiPath;
        public AIDestinationSetter destinationSetter;
        public Animator animator;
        public float wanderRadius = 10f;
        public float idleTime = 3f;

        private string walkForwardAnimation = "walk_forward";
        //private string walkBackwardAnimation = "walk_backwards";
        //private string runForwardAnimation = "run_forward";
        //private string turn90LAnimation = "turn_90_L";
        //private string turn90RAnimation = "turn_90_R";
        private string trotAnimation = "trot_forward";
        private string sittostandAnimation = "sit_to_stand";
        private string standtositAnimation = "stand_to_sit";
        private string idleAnimation = "idle";

        private bool isSitting = false; // 현재 앉아있는지 여부

        //private enum GoatState { IdLE, Walking, Troting, Running, SitandStanding }
        //private GoatState currentState;

        void Start()
        {
            aiPath = GetComponent<AIPath>();
            destinationSetter = GetComponent<AIDestinationSetter>();
            animator = GetComponent<Animator>();

            if (destinationSetter.target == null)
            {
                GameObject targetObject = new GameObject("Target");
                destinationSetter.target = targetObject.transform;
            }
            StartCoroutine(WanderRoutine());
            //currentState = GoatState.Idle;
        }

        IEnumerator WanderRoutine()
        {
            while (true)
            {
                Vector3 randomPoint = GetRandomPoint(transform.position, wanderRadius);
                destinationSetter.target.position = randomPoint;

                animator.Play(walkForwardAnimation);

                while (aiPath.pathPending || aiPath.reachedEndOfPath == false)
                {
                    yield return null;
                }

                // 목적지에 도착 후 Idle 애니메이션
                animator.Play(idleAnimation);
                yield return new WaitForSeconds(idleTime);

                // 일정 확률로 앉기
                if (!isSitting && Random.value < 0.5f) // 50% 확률로 앉기
                {
                    animator.Play(sittostandAnimation);
                    yield return new WaitForSeconds(1f); // 애니메이션이 끝날 때까지 대기
                    isSitting = true; // 앉아있는 상태로 설정
                }

                // 앉아있는 동안 대기
                if (isSitting)
                {
                    yield return new WaitForSeconds(idleTime);

                    // 일어나는 애니메이션
                    animator.Play(standtositAnimation);
                    yield return new WaitForSeconds(1f); // 애니메이션이 끝날 때까지 대기
                    isSitting = false; // 다시 서 있는 상태로 설정
                }
            }
        }

        /*private void DecideNextAction()
        {
            // 30% 확률로 앉기, 20% 풀 뜯기, 50% 걷기
            float randomValue = Random.value;
            if (randomValue < 0.3f)
            {
                currentState = GoatState.Sitting;
            }
            else if (randomValue < 0.5f)
            {
                currentState = GoatState.Grazing;
            }
            else
            {
                currentState = GoatState.Walking;
            }
        }*/

        Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            randomPos.y = Terrain.activeTerrain.SampleHeight(randomPos) + Terrain.activeTerrain.transform.position.y;
            return randomPos;
        }

        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.W))
            {
                animator.Play(walkForwardAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.Play(runForwardAnimation);
            }*/
        }
    }
}
