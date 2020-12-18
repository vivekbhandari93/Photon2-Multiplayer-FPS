using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Shooting : MonoBehaviourPunCallbacks
{
    float weaponRange = 100f;

    [Header("Camera Parameter")]
    [SerializeField] Camera fpsCamera;

    [Header("Health Parameters")]
    [SerializeField] float healthPoints = 100f;
    [SerializeField] float currentHealth;
    Slider healthBar;

    [Header("Particles VFX")]
    [SerializeField] ParticleSystem hitVFX;

    private void Start()
    {
        currentHealth = healthPoints;
        
        healthBar = FindObjectOfType<HealthBar>().GetComponent<Slider>();
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
        //ToggleHurtRedImage();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= Mathf.Epsilon)
        {
            Die();
        }
    }

    /*
    private void ToggleHurtRedImage()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(FadeOut(0.5f));
            StartCoroutine(FadeIn(0.3f));
        }
    }


    IEnumerator FadeOut(float time)
    {
        while (hurtRedImage.alpha <= 1f)
        {
            hurtRedImage.alpha += Time.deltaTime / time;
            yield return null;
        }
    }


    IEnumerator FadeIn(float time)
    {
        while (hurtRedImage.alpha >= Mathf.Epsilon)
        {
            hurtRedImage.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }*/
    

    private void UpdateHealthBar()
    {
        if (photonView.IsMine)
        {
            healthBar.value = currentHealth / healthPoints;
        }
    }


    private void Die()
    {
        if (photonView.IsMine)
        {
            GetComponent<Animator>().SetTrigger("isIdleDeath");
            GetComponent<PlayerMovementController>().enabled = false;

            Invoke("LeaveRoom", 3f);
        }
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
