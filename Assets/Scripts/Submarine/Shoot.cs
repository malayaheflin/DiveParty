using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour, Tool
{
    [SerializeField] GameObject bullet, slot1, slot2, slot3, slot4;
    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] float force, reloadTime;
    private int numBullets = 4;
    private GameObject[] bulletIcons = new GameObject[4];

    void Start()
    {
        bulletIcons[0] = slot1;
        bulletIcons[1] = slot2;
        bulletIcons[2] = slot3;
        bulletIcons[3] = slot4;
        StartCoroutine("Reload");
    }
    public void ToolAction()
    {
        if (numBullets > 0)
        {
            DecreaseBullet();
            GameObject bulletInstance = Instantiate(bullet, bulletSpawnPos.position, transform.rotation);
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.up * force, ForceMode2D.Impulse);
            SoundMgr.Instance.PlaySound("sfx_gunshot");
        }
    }

    IEnumerator Reload()
    {
        while (true)

        {
            
            yield return new WaitForSeconds(reloadTime);
            numBullets = 4;
            foreach (GameObject bulletIcon in bulletIcons)
            {
                bulletIcon.SetActive(true);
            }
            //SoundMgr.Instance.PlaySound("sfx_reload");
        }
    }

    private void DecreaseBullet()
    {
        numBullets--;
        bulletIcons[numBullets].SetActive(false);
    }
}
