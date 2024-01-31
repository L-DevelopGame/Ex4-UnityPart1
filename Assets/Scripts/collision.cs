using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class collision : MonoBehaviour
{
    public string tags;
   
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == tags)
        {
            Debug.Log(other.GetComponent<Rigidbody2D>().velocity.x);
            if (other.GetComponent<Rigidbody2D>().velocity.x > 3)
            {
                Destroy(this.gameObject);
            }

        }
    }
   
}
