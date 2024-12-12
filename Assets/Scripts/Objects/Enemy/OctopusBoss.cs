using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OctopusBoss : SingletonMono<OctopusBoss>
{
    private Animator anim;
    public RuntimeAnimatorController anim_NoMetal;

    public List<GameObject> doorList;

    public List<OctopusArm> armsList;
    public int Hp = 10;
    public bool isArmAttacking;

    public Transform hidePos;
    public Vector3 showPos;
    public Transform deadPos;
    public Transform suvivePos;

    public List<GameObject> treasuresPile;

    private bool isMetalRemoved;
    private bool isAppearing;
    private bool isHide;
    private bool hasAppear;
    public bool isAllMetalRemoved;
    private bool isDead;

    public float moveTime = 2f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        foreach (var door in doorList)
        {
            door.SetActive(false);
        }
        foreach (var treasure in treasuresPile)
        {
            treasure.SetActive(false);
        }

        showPos = this.transform.position;
        this.transform.position = hidePos.position;
    }

    private void Update()
    {
        if (hasAppear && !isAllMetalRemoved && !isDead)
            CallHandToAttack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAllMetalRemoved)
            return;
        if (collision.CompareTag("Submarine"))
        {
            ShipMovement.Instance.GetAttacked();
        }
        else if (collision.CompareTag("Beam"))
        {
            GetRidOfMetal();
        }
        else if (collision.CompareTag("Bullet"))
        {
            GetHurt(2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Submarine"))
        {
            BossHeadAttack();
        }
    }

    public void CheckAllMetalRemoved()
    {
        if(armsList[0].isMetalRemoved && armsList[1].isMetalRemoved && isMetalRemoved)
        {
            anim.SetTrigger("Dead");
            isAllMetalRemoved = true;

            SoundMgr.Instance.PlaySound("sfx_octopus_leaves");

            StartCoroutine(FleeAway());
        }
    }

    public void StartTheBossFight()
    {
        // Lock the door
        foreach (var door in doorList)
        {
            door.SetActive(false);
        }

        StartCoroutine(Appear());
    }

    public IEnumerator Appear()
    {
        foreach (var door in doorList)
        {
            door.SetActive(true);
        }
        anim.SetTrigger("Appear");
        yield return MoveTowardsPostion(true);
        hasAppear = true;
        SoundMgr.Instance.PlayMusic("music_boss");
    }

    public void BossHeadAttack()
    {
        if (isMetalRemoved)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            anim.SetTrigger("Attack");
        }
    }

    public void CallHandToAttack()
    {
        if (!isArmAttacking)
        {
            int i = Random.Range(0, 2);
            if (armsList[i].isMetalRemoved)
            {
                if(i == 0)
                {
                    armsList[1].gameObject.SetActive(true);
                    armsList[1].ShowUp();
                    isArmAttacking = true;
                }
                else if(i == 1)
                {
                    armsList[0].gameObject.SetActive(true);
                    armsList[0].ShowUp();
                    isArmAttacking = true;
                }
            }  
            else
            {
                armsList[i].gameObject.SetActive(true);
                armsList[i].ShowUp();
                isArmAttacking = true;
            }
        }
    }

    public void GetHurt(int hurt)
    {
        StopAllCoroutines();

        anim.SetTrigger("Hurt");

        SoundMgr.Instance.PlaySound("sfx_octopus_pain");

        Hp -= hurt;
        if(Hp <= 0)
        {
            StartCoroutine(BossDead());
        }
    }

    private IEnumerator BossDead()
    {
        isDead = true;
        foreach (var arm in armsList)
        {
            arm.gameObject.SetActive(false);
        }
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(2f);

        // Flee away
        float t = 0;
        Vector3 startPos;
        startPos = this.transform.position;

        SoundMgr.Instance.PlaySound("sfx_octopus_pain");
        while (t <= 2)
        {
            t += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPos, deadPos.position, t / 2);
            yield return null;
        }

        // Treasure
        foreach (var item in treasuresPile)
        {
            item.SetActive(true);
        }

        foreach (var door in doorList)
        {
            door.SetActive(false);
        }

        SoundMgr.Instance.PlayMusic("music_bgm");
    }

    private IEnumerator FleeAway()
    {
        yield return new WaitForSeconds(1f);

        // Flee away
        float t = 0;
        Vector3 startPos;
        startPos = this.transform.position;
        while (t <= 2)
        {
            t += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPos, suvivePos.position, t / 2);
            yield return null;
        }

        // Treasure
        foreach (var item in treasuresPile)
        {
            item.SetActive(true);
        }

        foreach (var door in doorList)
        {
            door.SetActive(false);
        }

        SoundMgr.Instance.PlayMusic("music_bgm");
    }

    private IEnumerator MoveTowardsPostion(bool isShow)
    {
        isAppearing = true;
        if (isShow)
        {
            float t = 0;
            while (t <= moveTime)
            {
                t += Time.deltaTime;
                this.transform.position = Vector3.Lerp(hidePos.position, showPos, t / moveTime);
                yield return null;
            }
        }
        else
        {
            float t = 0;
            while (t <= moveTime)
            {
                t += Time.deltaTime;
                this.transform.position = Vector3.Lerp(showPos, hidePos.position, t / moveTime);
                yield return null;
            }
        }
        isAppearing = false;
    }


    private void GetRidOfMetal()
    {
        if (!isMetalRemoved)
        {
            StopAllCoroutines();
            anim.runtimeAnimatorController = anim_NoMetal;
            isMetalRemoved = true;
            // Create Prefabs Of Metal

            CheckAllMetalRemoved();
        }

    }
}
