﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    public float health;
    [HideInInspector]
    public float maxHealth;
    public float mana;
    private HeroHealth healthBar;
    private HeroHealth healthBarMini;
    private PhotonView pv;



    [HideInInspector]
    public float priority = 1.0f;
    public float priorityRange;

    [HideInInspector]
    public bool FriendlyFire;


    public void Start()
    {
        pv = GetComponent<PhotonView>();
        healthBar = (HeroHealth)GameObject.Find("Health Bar").GetComponent("HeroHealth");
        healthBar.SetHealth(health);
        maxHealth = health;

    }
    [PunRPC]
    private void RPC_NecromancerWin()
    {
        SceneManager.LoadScene("Defeat");
    }

    /// <summary>
    /// Adjust the characters health according to the amount of damage taken
    /// </summary>
    /// <param name="damage">The amount of damage being dealt</param>
    public void takeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.Log("necromancer wins");
                //pv.RPC("RPC_NecromancerWin", RpcTarget.All);
                if (SceneManager.GetActiveScene().name != "SingleplayerGame")
                {
                    PlayerPrefs.SetInt("HeroesWin", 0);
                    PhotonNetwork.LoadLevel("EndScreen");
                }
            }
            else
            {
                Debug.Log("why");
            }

        }
    }

    /// <summary>
    /// Update the character's target priority
    /// </summary>
    public virtual void updatePriority()
    {
        priority = 1 - (priorityRange * health / maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (FriendlyFire)
        {
            Debug.Log("Friendly Fire on");
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Projectile"))
            {
                health--;
                


            }
        }
        else
        {
            Debug.Log("Friendly Fire off");
            if (collision.gameObject.CompareTag("Enemy"))
            {
                health--;
                healthBar.SetHealth(health);

                if (health <= 0)
                {
                    PhotonNetwork.Destroy(gameObject);
                    if (GameObject.FindGameObjectWithTag("Player") == null)
                    {
                        Debug.Log("necromancer wins");
                        //pv.RPC("RPC_NecromancerWin", RpcTarget.All);
                        if (SceneManager.GetActiveScene().name != "SingleplayerGame")
                        {
                            PlayerPrefs.SetInt("HeroesWin", 0);
                            PhotonNetwork.LoadLevel("EndScreen");
                        }
                    }
                    else
                    {
                        Debug.Log(GameObject.FindGameObjectWithTag("Player").ToString());
                    }
                
                }
            }
        }
    }
}
