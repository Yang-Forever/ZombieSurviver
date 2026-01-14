using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Spawn Rate")]
    public float spawnPerSecond = 1.5f;
    float spawnTimer;

    [Header("Spawn Area")]
    float spawnRadius = 20f;
    float minDistance = 15f;

    [Header("Difficulty")]
    public int difficultyLevel = 0;
    int normalCounter = 0;

    public static ZombieSpawner Inst = null;

    private void Awake()
    {
        Inst = this;

        if (player == null)
            player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (GameMgr.Inst.state != PlayerState.Play)
            return;

        spawnTimer += Time.deltaTime;

        float interval = 1f / spawnPerSecond;

        if (spawnTimer >= interval)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }
    }

    #region 스폰 로직
    void SpawnZombie()
    {
        if (!player) 
            return;

        const int maxTry = 30;

        for (int i = 0; i < maxTry; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float dist = Random.Range(minDistance, spawnRadius);

            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Vector3 rawPos = player.position + dir * dist;

            if (NavMesh.SamplePosition(rawPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                ZombiePool.Inst.Spawn(DecideZombieType(), hit.position);
                return;
            }
        }
    }

    ZombieType DecideZombieType()
    {
        int fastRate = Mathf.Clamp(difficultyLevel, 1, 5);

        if (normalCounter < fastRate)
        {
            normalCounter++;
            return ZombieType.Normal;
        }
        else
        {
            normalCounter = 0;
            return ZombieType.Fast;
        }
    }

    public void IncreaseDifficulty(int level)
    {
        difficultyLevel = level;

        // 전역 스텟 배율
        Zombie_Ctrl.NormalHpMul = 1f + level * 0.1f;
        Zombie_Ctrl.NormalSpeedMul = 1f + level * 0.05f;
        Zombie_Ctrl.NormalDmgMul = 1f + level * 0.1f;

        // 스폰량 증가
        spawnPerSecond = 1.5f + level * 0.5f;
    }

    public void SpawnBoss(int bossLevel)
    {
        if (!player)
            return;
        IncreaseBossDifficulty(bossLevel);

        const float bossSpawnDist = 18f;
        const int maxTry = 15;

        for (int i = 0; i < maxTry; i++)
        {
            Vector2 dir2D = Random.insideUnitCircle.normalized;
            Vector3 spawnPos = player.position + new Vector3(dir2D.x, 0f, dir2D.y) * bossSpawnDist;

            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2.5f, NavMesh.AllAreas))
            {
                ZombiePool.Inst.SpawnBoss(hit.position);
                return;
            }
        }
    }

    public void IncreaseBossDifficulty(int level)
    {
        // 전역 스텟 배율
        Zombie_Ctrl.BossHpMul = 1f + level * 0.5f;
        Zombie_Ctrl.BossSpeedMul = 1f + level * 0.05f;
        Zombie_Ctrl.BossDmgMul = 1f + level * 0.1f;
    }

    public void ResetSpawner()
    {
        difficultyLevel = 0;
        spawnPerSecond = 1.5f;
    }

    #endregion
}
