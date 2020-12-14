using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class Shooting : MonoBehaviourPunCallbacks
{
    float weaponRange = 100f;

    [Header("Camera Parameter")]
    [SerializeField] Camera fpsCamera;

    [Header("Health Parameters")]
    [SerializeField] float healthPoints = 100f;
    [SerializeField] float currentHealth;
    HealthBar healthBar;
    public bool isDead = false;

    [Header("Particles VFX")]
    [SerializeField] ParticleSystem hitVFX; 

    private void Start()
    {
        currentHealth = healthPoints;
        healthBar = FindObjectOfType<HealthBar>();
        UpdateHealthBar();
    }


    public void Fire()
    {
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, weaponRange))
        {
            if(hit.collider.CompareTag("Player") && !hit.collider.GetComponent<PhotonView>().IsMine)
            {
                Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
                hit.collider.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10f);
            }
        }
    }


    [PunRPC]
    public void TakeDamage(float damage, PhotonMessageInfo info)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= Mathf.Epsilon)
        {
            Die();
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName + ".");
        }

        
    }


    private void UpdateHealthBar()
    {
        if (photonView.IsMine)
        {
            healthBar.transform.GetComponent<Image>().fillAmount = currentHealth / healthPoints;
        }
    }


    private void Die()
    {
        if (photonView.IsMine)
        {
            GetComponent<Animator>().SetTrigger("isIdleDeath");
            isDead = true;
        }
    }
}
