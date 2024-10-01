using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.5f;
    private float lastCheckTime;
    [SerializeField]
    private float maxCheckDistance = 5f;
    public LayerMask layerMask;

    public LayerMask enemyLayerMask;

    private GameObject curInteractGameObject;
    [HideInInspector]
    public GameObject enemyGameObject;
    private IInteractable curInteractable;


    public TextMeshProUGUI promptText;
    private Camera playerCameara;

    void Start()
    {
        playerCameara = Camera.main;
    }

    private void Update()
    {
        //test
        Debug.DrawRay(playerCameara.transform.position, playerCameara.transform.forward * maxCheckDistance, Color.green);

        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = playerCameara.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height/ 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else if (Physics.Raycast(ray, out hit, maxCheckDistance, enemyLayerMask))
            {
                if (hit.collider.gameObject != enemyGameObject)
                {
                    enemyGameObject = hit.collider.gameObject;
                }
            }
            else
            {
                curInteractGameObject = null;
                enemyGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt());
    }
    
    //아이템 획득
    public void OnInteract(InputValue value)
    {
        if (curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
