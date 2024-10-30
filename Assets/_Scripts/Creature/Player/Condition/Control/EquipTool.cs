using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float fishingDistance;
    public float InstallationDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;
    public bool doesGatherFish;
    public bool doesGatherInstallation;
    public bool doesGatherEat;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera _camera;

    public float useStamina;

    private void Awake()
    {
        _camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    public override void OnAttacking()
    {
        Debug.Log($"EquipTool - OnAttacking() : Weaopn Attacking");
        if(doesGatherInstallation || doesGatherEat)
        {
            return;
        }

        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("OnAttack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    public override void OnEating()
    {
        Debug.Log($"EquipTool - OnEating() : Posion Eating");
        if (!doesGatherEat)
        {
            return;
        }

        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("OnEat");
            Invoke("OnCanAttack", attackRate);
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }
    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out ResourceObject resourceObject))
            {
                resourceObject.Gather(hit.point, hit.normal);
            }


            //if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damageable))
            //{
            //    damageable.TakePhysicalDamage(damage);
            //}
        }
    }
    public void OnFishing()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, fishingDistance))
        {
            if (doesGatherFish && hit.collider.TryGetComponent(out ResourceObject resourceObject))
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Water"))
                {
                    return;
                }
                Debug.Log($"EquipTool - OnFishing() : {hit.collider.gameObject.layer}");
                //resourceObject.Fishing(hit.point);
                // #test #임시 #Errer #에러

            }

        }
    }

    public void OnEat()
    {
        Debug.Log("EquipTool - OnEat() - AnimationClip Event : Potion : Item - Use()");
        //OnEating();
        Main.Inventory.inventoryUI.selectSlot.curSlot.itemData.item.Use();
    }

    public void OnInstallation()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InstallationDistance))
        {
            if (doesGatherInstallation && hit.collider.TryGetComponent(out Resource resource))
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    return;
                Debug.Log($"EquipTool - OnInstallation() : {hit.collider.gameObject.layer}");
                // 설치 해야 함..
            }
        }
    }
}
