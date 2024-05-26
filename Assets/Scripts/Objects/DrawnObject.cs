using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnObject : MonoBehaviour
{
    //gets varaibles for the object
    protected Rigidbody rb;
    [SerializeField] private float dropForce = 30;

    //sets the rb
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //drops the object, makes it where the object can move and fall towards the ground
    public void Drop()
    {
        transform.SetParent(null);
        GameManager.Instance.player.GetComponent<PlayerData>().objectCarrying = null;
        GameManager.Instance.player.GetComponent<PlayerData>().canPickUp = true;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * dropForce);
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
        gameObject.layer = 6;
    }
}