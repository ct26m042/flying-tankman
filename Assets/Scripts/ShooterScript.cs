using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterScript : MonoBehaviour
{
    [SerializeField] ShellPool shellPool;
    [SerializeField] Transform muzzle;
    [SerializeField] float cooldown = 2f;
    [SerializeField] float shellSpeed = 40f;
    [SerializeField] float shellLifetime = 5f;
    [SerializeField] float recoilForce = 5f;
    [SerializeField] float recoilAngle = 6f;
    [SerializeField] float recoilUpTime = 0.08f;
    [SerializeField] float recoilRecoverTime = 0.4f;

    InputAction attackAction;
    FTMController ship;
    Rigidbody rb;
    float nextShotTime;
    float recoilTimer = -1f;

    void Start()
    {
        attackAction = InputSystem.actions.FindActionMap("Player").FindAction("Attack");
        attackAction.Enable();

        ship = GetComponent<FTMController>();
        rb = GetComponent<Rigidbody>();

        if (muzzle == null)
            muzzle = transform;
    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame() && Time.time >= nextShotTime)
            Fire();

        UpdateRecoil();
    }

    void Fire()
    {
        nextShotTime = Time.time + cooldown;

        GameObject shell = shellPool.GetObject();
        IgnoreShipCollision(shell);
        shell.SetActive(true);
        shell.GetComponent<Shell>().Launch(muzzle.position, muzzle.rotation, muzzle.forward * shellSpeed, shellLifetime);

        rb.AddForce(-transform.forward * recoilForce, ForceMode.Impulse);
        recoilTimer = 0f;
    }

    void UpdateRecoil()
    {
        if (ship == null || recoilTimer < 0f)
            return;

        recoilTimer += Time.deltaTime;
        float angle;

        if (recoilTimer <= recoilUpTime)
            angle = Mathf.SmoothStep(0f, recoilAngle, recoilTimer / recoilUpTime);
        else if (recoilTimer <= recoilUpTime + recoilRecoverTime)
            angle = Mathf.SmoothStep(recoilAngle, 0f, (recoilTimer - recoilUpTime) / recoilRecoverTime);
        else
        {
            angle = 0f;
            recoilTimer = -1f;
        }

        ship.recoilPitch = -angle;
    }

    void IgnoreShipCollision(GameObject shell)
    {
        Transform player = transform;
        while (player.parent != null && player.tag != "Player")
            player = player.parent;

        Collider[] shipColliders = player.GetComponentsInChildren<Collider>(true);
        Collider[] shellColliders = shell.GetComponentsInChildren<Collider>(true);

        foreach (Collider shipCollider in shipColliders)
        {
            foreach (Collider shellCollider in shellColliders)
                Physics.IgnoreCollision(shipCollider, shellCollider);
        }
    }

}
