using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PartyZone : MonoBehaviour
{
    [SerializeField] float turnSpeed = 5f;

    static readonly int IsWaving = Animator.StringToHash("IsWaving");

    Animator[] partyGuests;
    Transform spaceship;
    Quaternion[] startRotations;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;

        List<Animator> found = new List<Animator>();
        foreach (GameObject dancer in GameObject.FindGameObjectsWithTag("Dancer"))
        {
            if (dancer.TryGetComponent(out Animator animator))
                found.Add(animator);
        }
        partyGuests = found.ToArray();

        startRotations = new Quaternion[partyGuests.Length];
        for (int i = 0; i < partyGuests.Length; i++)
            startRotations[i] = partyGuests[i].transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        FTMController ship = other.GetComponentInParent<FTMController>();
        if (ship == null)
            return;

        spaceship = ship.transform;
        SetWaving(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<FTMController>() == null)
            return;

        spaceship = null;
        SetWaving(false);
    }

    void Update()
    {
        float t = 1f - Mathf.Exp(-turnSpeed * Time.deltaTime);

        for (int i = 0; i < partyGuests.Length; i++)
        {
            Transform guest = partyGuests[i].transform;
            Quaternion target = startRotations[i];

            if (spaceship is not null)
            {
                Vector3 toShip = spaceship.position - guest.position;
                toShip.y = 0f;
                if (toShip.sqrMagnitude > 0.001f)
                    target = Quaternion.LookRotation(toShip);
            }

            guest.rotation = Quaternion.Slerp(guest.rotation, target, t);
        }
    }

    void SetWaving(bool waving)
    {
        foreach (Animator guest in partyGuests)
            guest.SetBool(IsWaving, waving);
    }
}
