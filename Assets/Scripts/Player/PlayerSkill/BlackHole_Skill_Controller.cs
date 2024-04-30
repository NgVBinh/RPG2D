using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    private bool CanGrow = true;
    private int maxSizeScale;
    private float speedGrow;
    private bool canShrink;
    private float speedShrink;

    private bool canCreateHotkeys = true;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private List<KeyCode> keys;

    private bool cloneAttackReleased;
    private int amountAttack;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;

    private List<Transform> enemysTarget;
    private List<GameObject> createdHotkey;

    private float blackhldTimer;
    public bool playerCanExitsState { get; private set; }
    public void SetupBlackHole(int amountAttack, float cloneAttackCooldown, int maxSizeScale, float speedGrow, float speedShrink,float blackholdDuration)
    {
        this.amountAttack = amountAttack;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.maxSizeScale = maxSizeScale;
        this.speedGrow = speedGrow;
        this.speedShrink = speedShrink;
        this.blackhldTimer = blackholdDuration;
        enemysTarget = new List<Transform>();
        createdHotkey = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        blackhldTimer -= Time.deltaTime;
        if (blackhldTimer < 0)
        {
            if(enemysTarget.Count > 0)
            {
                blackhldTimer = Mathf.Infinity;
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (CanGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSizeScale, maxSizeScale), speedGrow * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), speedShrink * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }

    }

    private void ReleaseCloneAttack()
    {
        if (enemysTarget.Count <= 0) return;

        DestroyHotkeys();
        cloneAttackReleased = true;
        canCreateHotkeys = false;

        PlayerManager.instance.player.MakeTransprent(true);
    }

    private void CloneAttackLogic()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (cloneAttackReleased && cloneAttackTimer < 0 && amountAttack>0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            // set x position clone
            float xOffset = (Random.Range(0, 2) == 0 ? 2 : -2);
            SkillManager.instance.cloneSkill.CreateClone(enemysTarget[Random.Range(0, enemysTarget.Count)], new Vector3(xOffset, 0));

            amountAttack--;
            if (amountAttack <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);
                //FinishBlackholeAbility();
            }
        }
       
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkeys();
        playerCanExitsState = true;
        cloneAttackReleased = false;
        canShrink = true;

        PlayerManager.instance.player.MakeTransprent(false);

    }

    private void DestroyHotkeys()
    {
        if (createdHotkey.Count <= 0) return;

        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);
    private void CreateHotKey(Collider2D collision)
    {
        if (keys.Count <= 0)
        {
            Debug.Log("Not enough hot key");
            return;
        }

        if (!canCreateHotkeys) { return; }

        collision.GetComponent<Enemy>().FreezeTime(true);
        GameObject newKey = Instantiate(keyPrefab, collision.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        createdHotkey.Add(newKey);
        KeyCode newKeycode = keys[Random.Range(0, keys.Count)];
        newKey.GetComponent<Keycode_Controller>().SetupKeycode(newKeycode, collision.transform, this);
        keys.Remove(newKeycode);
    }

    public void AddEnemyTarget(Transform enemyTransform) => enemysTarget.Add(enemyTransform);

}
