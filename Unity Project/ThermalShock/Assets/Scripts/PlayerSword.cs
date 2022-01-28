using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{

    public float swordHeat = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Attackable hitObject = other.GetComponent<Attackable>();
        if (other.name != "Player" && hitObject != null) { hitObject.Hit(swordHeat, true); }
    }


}
