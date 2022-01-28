using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float flySpeed = 0f;
    [Tooltip("How long the projectile will exist for (in seconds) untill it automatically self destructs. Use zero of a negitive value for infinite.")]
    public float expireTime = 0;

    public float projectileHeat = 0f;

    float deathClock = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (expireTime > 0f)
        {
            deathClock += Time.deltaTime;
            if (deathClock >= expireTime) { Destroy(this.gameObject); }
        }
        gameObject.transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + " has collided with " + other.gameObject.name);
        Attackable otherScript = other.GetComponent<Attackable>();
        if (otherScript != null)
        {
            otherScript.Hit(projectileHeat, false);
        }

        //replace this line with a generate explosion prefab, if using it.

        Destroy(this.gameObject);
    }
}
