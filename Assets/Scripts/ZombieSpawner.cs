using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform player;
    float spawnInterval = 3f;
    int spawnCountPerWave = 2;

    [Header("Spawn Area")]
    float spawnRadius = 20f;
    float minDistance = 15f;

    // 
    private int normalSpawnCounter = 0;

    private void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

        InvokeRepeating(nameof(SpawnWave), 1f, spawnInterval);
    }

    void SpawnWave()
    {
        for (int i = 0; i < spawnCountPerWave; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        if (!player)
            return;

        int maxTry = 10;

        for (int i = 0; i < maxTry; i++)
        {
            Vector3 dir = Random.insideUnitSphere;
            dir.y = 0;
            dir.Normalize();

            float dist = Random.Range(minDistance, spawnRadius);

            Vector3 pos = player.position + dir * dist;

            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                ZombieType zType;

                // 일반 4 : 패스트 1
                if (normalSpawnCounter < 4)
                {
                    zType = ZombieType.Normal;
                    normalSpawnCounter++;
                }
                else
                {
                    zType = ZombieType.Fast;
                    normalSpawnCounter = 0;
                }

                ZombiePool.Inst.Spawn(zType, hit.position);
                return;
            }
        }
    }
}
