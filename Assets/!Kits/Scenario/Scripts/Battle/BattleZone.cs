using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BattleZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float battleDuration = 30f;

    [Header("Generators")]
    [SerializeField] private EnemyGenerator[] enemyGenerators;

    [Header("Battle zone objects")]
    [SerializeField] private GameObject zoneBlock;
    [SerializeField] private GameObject fixedCamera;
    [SerializeField] private GameObject chest;

    [Header("Events")]
    [SerializeField] private UnityEvent onBattleStarted;
    [SerializeField] private UnityEvent onBattleFinished;

    private bool battleStarted = false;
    private bool battleFinished = false;

    private void Start()
    {
        zoneBlock.SetActive(false);
        fixedCamera.SetActive(false);
        chest.SetActive(false);

        StartStopGenerators(false);
    }

    public void StartBattle()
    {
        if (battleStarted) return;

        battleStarted = true;

        zoneBlock.SetActive(true);
        fixedCamera.SetActive(true);

        StartStopGenerators(true);

        onBattleStarted?.Invoke();
        StartCoroutine(BattleCoroutine());
    }

    private IEnumerator BattleCoroutine()
    {
        yield return new WaitForSeconds(battleDuration);

        FinishBattle();
    }

    private void FinishBattle()
    {
        battleFinished = true;
        StartStopGenerators(false);

        zoneBlock.SetActive(false);
        fixedCamera.SetActive(false);
        chest.SetActive(true);

        onBattleFinished?.Invoke();
    }

    private void StartStopGenerators(bool start)
    {
        foreach (EnemyGenerator generator in enemyGenerators)
        {
            if (generator != null)
            {
                if (start)
                    { generator.StartGenerating(this); }
                else
                    { generator.StopGenerating(); }
            }
        }
    }
}
