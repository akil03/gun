using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public float shootForce;
    public float shootDelay;
    public GameObject bloodParticle;
    public AudioClip shootClip, impactClip;
    Rigidbody rb;

    bool isTargetHit;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Fire();
        
    }
	
	// Update is called once per frame
	void Update () {
        
        
	}

    public void Fire()
    {
        rb.AddRelativeForce(Vector3.forward * shootForce, ForceMode.Impulse);
        GameManager.instance.PlaySound(shootClip);
        Destroy(gameObject, shootDelay);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            PlayerController.instance.ReachNextPosition(collision.transform);
            Destroy(collision.gameObject);
            //Spawner.instance.DelayedRandomeSpawn();
            isTargetHit = true;
            PlayerController.instance.IncreaseSpeed();
            GameManager.instance.PlaySound(impactClip);
            Instantiate(bloodParticle, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            isTargetHit = true;
            GameManager.instance.PlaySound(impactClip);
            Instantiate(bloodParticle, collision.transform.position, Quaternion.identity);
            collision.transform.GetComponent<PlayerController>().PlayDead();
        }
    }

    private void OnDestroy()
    {
        if (!isTargetHit)
            PathSpawner.instance.Levels[0].GetComponent<LevelProperties>().Enemies[0].SetAim();
    }
}
