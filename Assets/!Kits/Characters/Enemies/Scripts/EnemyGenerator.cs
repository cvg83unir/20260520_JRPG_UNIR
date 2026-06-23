using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesPrefab;
    [SerializeField] float timeBetweenEnemies = 2.5f;
    [SerializeField] int maxEnemiesAlive = 3;

    private bool generating = false;
    private int enemiesAlive = 0;
    private Coroutine generateCoroutine;

    public void StartGenerating(BattleZone battleZone)
    {
        this.generating = true;

        if (generateCoroutine == null)
            { generateCoroutine = StartCoroutine(GenerateEnemies()); }
    }

    public void StopGenerating()
    {
        this.generating = false;

        if (generateCoroutine != null)
        {
            StopCoroutine(generateCoroutine);
            generateCoroutine = null;
        }
    }

    IEnumerator GenerateEnemies()
    {
        while (generating)
        {
            yield return new WaitForSeconds(timeBetweenEnemies);

            if (enemiesAlive < maxEnemiesAlive)
                { GenerateEnemy(); }
        }
    }

    private void GenerateEnemy()
    {
        int randomEnemy = Random.Range(0, enemiesPrefab.Length);

        GameObject newEnemy = Instantiate(enemiesPrefab[randomEnemy], transform.position, Quaternion.identity);

        enemiesAlive++;
    }
}
