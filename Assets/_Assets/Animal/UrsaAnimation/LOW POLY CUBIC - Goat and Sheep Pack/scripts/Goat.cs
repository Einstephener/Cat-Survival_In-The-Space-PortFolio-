using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Pathfinding;
using Unity.VisualScripting;

namespace Ursaanimation.CubicFarmAnimals
{
    public class Goat : MonoBehaviour
    {
        private AIPath aiPath;
        private AIDestinationSetter destinationSetter;
        private Animator animator;
        private AudioSource audioSource;
        public float wanderRadius = 20f;
        public float idleTime = 4f;

        private string walkForwardAnimation = "walk_forward";
        //private string walkBackwardAnimation = "walk_backwards";
        //private string runForwardAnimation = "run_forward";
        //private string turn90LAnimation = "turn_90_L";
        //private string turn90RAnimation = "turn_90_R";
        private string trotAnimation = "trot_forward";
        private string sittostandAnimation = "sit_to_stand";
        private string standtositAnimation = "stand_to_sit";
        private string idleAnimation = "idle";

        private enum GoatState { Walking, Idle, Troting, Running, Sitting }
        private GoatState currentState;

        void Start()
        {
            aiPath = GetComponent<AIPath>();
            destinationSetter = GetComponent<AIDestinationSetter>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            GameObject targetParent = GameObject.Find("GoatTargets");
            if (targetParent == null)
            {
                targetParent = new GameObject("GoatTargets");
            }

            if (destinationSetter.target == null)
            {
                GameObject targetObject = new GameObject("Target");
                targetObject.transform.parent = targetParent.transform;
                destinationSetter.target = targetObject.transform;
            }

            currentState = GoatState.Idle;
            StartCoroutine(WanderRoutine());
        }

        IEnumerator WanderRoutine()
        {
            while (true)
            {
                DecideNextAction();
                Vector3 randomPoint = GetRandomPoint(transform.position, wanderRadius);

                switch (currentState)
                {
                    case GoatState.Walking:
                        animator.Play(walkForwardAnimation);
                        destinationSetter.target.position = randomPoint;

                        while (aiPath.pathPending || !aiPath.reachedEndOfPath)
                        {
                            yield return null;
                        }

                        break;

                    case GoatState.Troting:
                        aiPath.maxSpeed = 2f;
                        PlaySound();
                        animator.Play(trotAnimation);
                        destinationSetter.target.position = randomPoint;

                        while (aiPath.pathPending || !aiPath.reachedEndOfPath)
                        {
                            yield return null;
                        }

                        aiPath.maxSpeed = 1f;
                        break;

                    case GoatState.Sitting:
                        aiPath.canMove = false;
                        animator.Play(standtositAnimation);
                        yield return new WaitForSeconds(8f);

                        animator.Play(sittostandAnimation);
                        yield return new WaitForSeconds(2f);
                        aiPath.canMove = true;
                        break;

                    case GoatState.Idle:
                    default:
                        aiPath.canMove = false;
                        animator.Play(idleAnimation);
                        yield return new WaitForSeconds(idleTime);
                        aiPath.canMove = true;
                        break;
                }
            }
        }


        private void DecideNextAction()
        {
            // 10% 확률로 앉기, 20% 가만히, 20% 종종 걸음, 50% 걷기
            float randomValue = Random.value;
            if (randomValue < 0.1f)
            {
                currentState = GoatState.Sitting;
            }
            else if (randomValue < 0.3f)
            {
                currentState = GoatState.Idle;
            }
            else if (randomValue < 0.5f)
            {
                currentState = GoatState.Troting;
            }
            else
            {
                currentState = GoatState.Walking;
            }
        }

        Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            randomPos.y = Terrain.activeTerrain.SampleHeight(randomPos) + Terrain.activeTerrain.transform.position.y;
            return randomPos;
        }
        private void PlaySound()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }
}
