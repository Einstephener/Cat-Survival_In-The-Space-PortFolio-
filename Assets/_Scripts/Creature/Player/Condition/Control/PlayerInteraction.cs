using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class PlayerInteraction : MonoBehaviour
{
    #region Fields
    [Header("Value")]
    [SerializeField] private float checkRate = 0.1f;
    [SerializeField] private float maxCheckDistance = 5f;
    private float lastCheckTime;

    [Header("Layers")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask natureLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private LayerMask GroundLayer;

    [Header("Interaction")]
    [HideInInspector] public GameObject currentInteractObject;
    [HideInInspector] public GameObject waterObject;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public GameObject natureObject;
    [HideInInspector] public GameObject installtionItemObject;
    private IInteractable currentInteractable;

    private TextMeshProUGUI promptText;
    private Camera playerCamera;
    #endregion

    private void Start()
    {
        playerCamera = Camera.main;

        //ClearInteraction();
    }

    private void Update()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * maxCheckDistance, Color.red);

        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractableObjects();
        }
    }

    #region InteractionCheck

    
    private void CheckForInteractableObjects()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, interactableLayer))
        {
            HandleInteractableHit(hit);
        }
        else if (Physics.Raycast(ray, out hit, maxCheckDistance, enemyLayer))
        {
            HandleEnemyHit(hit);

        }
        else if (Physics.Raycast(ray, out hit, maxCheckDistance, natureLayer))
        {
            HandleNatureHit(hit);

        }
        else if (Physics.Raycast(ray, out hit, maxCheckDistance, waterLayer))
        {
            HandleWaterHit(hit);
        }
        else
        {
            ClearInteraction();
        }
        
    }

    private void HandleInteractableHit(RaycastHit hit)
    {
        Item _item = hit.collider.gameObject.GetComponent<Item>(); // [11/29 - 능권이가 추가함]

        if (_item is Installation)
        {
            installtionItemObject = hit.collider.gameObject;
        }
        
        if (hit.collider.gameObject != currentInteractObject)
        {
            currentInteractObject = hit.collider.gameObject;
            currentInteractable = hit.collider.GetComponent<IInteractable>();
            SetPromptText();
        }
    }

    private void HandleEnemyHit(RaycastHit hit)
    {
        if (hit.collider.gameObject != enemyObject)
        {
            enemyObject = hit.collider.gameObject;
        }
    }

    private void HandleNatureHit(RaycastHit hit)
    {
        if (hit.collider.gameObject != natureObject)
        {
            natureObject = hit.collider.gameObject;
        }
    }

    private void HandleWaterHit(RaycastHit hit)
    {
        if (hit.collider.gameObject != currentInteractObject)
        {
            currentInteractObject = hit.collider.gameObject;
            waterObject = currentInteractObject;
            PromptTextActive(true);
            promptText.text = $"<b>[E]</b>\n{"DrinkWater"}";
        }
    }

    private void ClearInteraction()
    {
        currentInteractObject = null;
        enemyObject = null;
        natureObject = null;
        currentInteractable = null;
        PromptTextActive(false);

    }
    #endregion

    private void SetPromptText()
    {
        if (currentInteractable != null)
        {
            PromptTextActive(true);
            promptText.text = $"<b>[E]</b>\n{currentInteractable.GetInteractPrompt()}";
        }
    }
    private void PromptTextActive(bool isActive)
    {
        if (promptText == null)
        {
            Main.UI.PromtText.gameObject.SetActive(isActive);
            promptText = Main.UI.PromtText;
        }
        else
        {
            promptText.gameObject.SetActive(isActive);
        }
    }
    public void OnInteract(InputValue value)
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnInteract();
            ClearInteraction();
        }
    }
}
