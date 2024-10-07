using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class PlayerInteraction : MonoBehaviour
{
    #region Fields
    [Header("Value")]
    [SerializeField] private float checkRate = 0.5f;
    [SerializeField] private float maxCheckDistance = 5f;
    private float lastCheckTime;

    [Header("Layers")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask natureLayer;
    [SerializeField] private LayerMask waterLayer;

    [Header("Interaction")]
    [HideInInspector] public GameObject currentInteractObject;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public GameObject natureObject;
    private IInteractable currentInteractable;

    public TextMeshProUGUI promptText;
    private Camera playerCamera;
    #endregion

    private void Start()
    {
        playerCamera = Camera.main;
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
            promptText.gameObject.SetActive(true);
            promptText.text = $"<b>[E]</b> {"DrinkWater"}";
        }
    }

    private void ClearInteraction()
    {
        currentInteractObject = null;
        enemyObject = null;
        natureObject = null;
        currentInteractable = null;
        promptText.gameObject.SetActive(false);
    }
    #endregion

    private void SetPromptText()
    {
        if (currentInteractable != null)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = $"<b>[E]</b> {currentInteractable.GetInteractPrompt()}";
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
