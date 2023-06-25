using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    public float passthroughTime = 0.5f;
    public CapsuleCollider2D playerCollider;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetButtonDown("Jump"))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            //if player is on oneway platform get reference
            currentOneWayPlatform = collision.gameObject;
            Debug.Log("Got it");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            //remove reference
            currentOneWayPlatform = null;
            Debug.Log("Lost it");
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        //ignore collision for a small window
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(passthroughTime);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

    }
}
