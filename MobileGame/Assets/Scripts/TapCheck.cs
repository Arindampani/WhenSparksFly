﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapCheck : MonoBehaviour
{

    private int lives = 1;
    private int raycastDistance = 10;
    private new Collider2D collider;
    private GameObject uIGameObject;
    
    public GameObject connectedMechanism;
    

    void Start()
    {
        uIGameObject = GameObject.Find("UI");
        collider = gameObject.GetComponent<Collider2D>();
    }
    void Update()
    {
        
        //for (int i = 0; i < Input.touchCount; i++)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePosition.x, mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "cage" && gameObject.transform.name == hit.collider.name)
            {

                // Cast a ray straight on all four directions
                RaycastHit2D hitDown = Physics2D.Raycast(new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y - collider.bounds.extents.y - 1) , Vector2.up, raycastDistance);
                RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + collider.bounds.extents.x + 0.5f, gameObject.transform.position.y), Vector2.right, raycastDistance);
                RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - collider.bounds.extents.x - 0.5f, gameObject.transform.position.y), Vector2.left, raycastDistance);
                RaycastHit2D hitUp = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + collider.bounds.extents.y + 1), Vector2.down, raycastDistance);


                // If it hits player while touching...
                if ((hitDown.collider != null && hitDown.collider.tag == "Player") || (hitUp.collider != null && hitUp.collider.tag == "Player") ||
                   (hitLeft.collider != null && hitLeft.collider.tag == "Player") || (hitRight.collider != null && hitRight.collider.tag == "Player"))
                {
                    //Debug.Log("test");
                    lives--;
                }

                
                if (lives == 0)
                {
                    mechaController MC = connectedMechanism.GetComponent<mechaController>();
                    Debug.Log("switched");
                    GameObject childBirdObject = gameObject.transform.GetChild(0).gameObject;
                    if (childBirdObject.activeSelf)
                    {
                      
                        childBirdObject.SetActive(false);
                        uIGameObject.GetComponent<UI>().birdCount++;
                        Debug.Log(uIGameObject.GetComponent<UI>().birdCount);
                        MC.withoutChildMecha();

                    }
                    else
                    {
                       if (uIGameObject.GetComponent<UI>().birdCount>0)
                        {

                            childBirdObject.SetActive(true);
                            if (uIGameObject.GetComponent<UI>().birdCount == 0)
                            {
                                uIGameObject.GetComponent<UI>().noMoreBirds = true;
                                lives = 1;
                                return;
                            }
                            uIGameObject.GetComponent<UI>().birdCount--;
                            Debug.Log(uIGameObject.GetComponent<UI>().birdCount);
                            MC.withChildMecha();
                        }

                    }
                    lives = 1;
                }
            }
        }
    }
}
