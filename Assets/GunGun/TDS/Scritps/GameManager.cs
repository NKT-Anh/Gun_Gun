using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public enum GameState
{
    STARTING,
    PLAYING,
    PAUSE,
    GAMEOVER
}

public class GameManager : Singleton<GameManager>
{
    public static GameState State;

    [SerializeField] private Map m_mapPrefab;
    [SerializeField] private Player m_playerPrefab;
    [SerializeField] private Enemy[] m_EnemyPrefab;
    [SerializeField] private GameObject m_spamnVfx;
    [SerializeField] private float m_EnemySpawnTime;
    [SerializeField] private int m_maxPlayerLife;
    [SerializeField] private int m_PlayerStartingLife;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCameraE;

    private Map m_map;
    private Player m_player;
    private PlayerStats m_playerStats;
    private int m_currenLife;

    public Player Player { get => m_player; private set => m_player = value; }

    public int CurrenLife
    {
        get => m_currenLife;
        set
        {
            m_currenLife = value;
            m_currenLife = Mathf.Clamp(m_currenLife, 0, m_maxPlayerLife);
        }
    }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    private void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCameraE = FindObjectOfType<CinemachineVirtualCamera>();
        Init();
    }

    private void Init()
    {
        State = GameState.STARTING;
        m_currenLife = m_PlayerStartingLife;
        spawnMap_Player();
        GUIManager.Ins.ShowHomeGUI(true);
    }

    private void spawnMap_Player()
    {
        if (m_mapPrefab == null) return;

        m_map = Instantiate(m_mapPrefab, Vector3.zero, Quaternion.identity);
        m_player = Instantiate(m_playerPrefab, m_map.playerSpawnPoint.position, Quaternion.identity);

        if (virtualCamera != null)
        {
            virtualCamera.Follow = m_player.transform;
        }
    }

    public void PlayGame()
    {
        State = GameState.PLAYING;
        GUIManager.Ins.ShowGameGUI(true);
        m_playerStats = Player.PlayerStats;

        if (m_player == null || m_playerStats == null) return;

        GUIManager.Ins.UpdateLife(m_currenLife);
        GUIManager.Ins.UpDateHpInfo(Player.CurrentHP, m_playerStats.hp);
        GUIManager.Ins.UpDateLevelInfo(m_playerStats.level, m_playerStats.xp, m_playerStats.leverUpXP);
        GUIManager.Ins.ShowCoinGUI(Prefs.coins);
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        var randomEnemy = GetRanDomEnemy();
        if (randomEnemy == null || m_map == null) return;
        StartCoroutine(SpawnEnemy_Coroutine(randomEnemy));
    }

    private Enemy GetRanDomEnemy()
    {
        if (m_EnemyPrefab == null || m_EnemyPrefab.Length <= 0) return null;
        int randomIdx = UnityEngine.Random.Range(0, m_EnemyPrefab.Length);
        return m_EnemyPrefab[randomIdx];
    }

    private IEnumerator SpawnEnemy_Coroutine(Enemy randomEnemy)
    {
        yield return new WaitForSeconds(3f);

        while (State == GameState.PLAYING)
        {
            if (m_map.RandomAiSP == null) break;

            Vector3 SP = m_map.RandomAiSP.position;

            if (m_spamnVfx != null)
            {
                Instantiate(m_spamnVfx, SP, Quaternion.identity);
            }

            yield return new WaitForSeconds(2f);
            Instantiate(randomEnemy, SP, Quaternion.identity);
            yield return new WaitForSeconds(m_EnemySpawnTime);
            yield return null;
        }
    }

    public void GameOverCheck(Action LostLife = null, Action OnDead = null)
    {
        if (m_currenLife <= 0) return;

        m_currenLife--;
        LostLife?.Invoke();

        if (m_currenLife <= 0)
        {
            State = GameState.GAMEOVER;
            OnDead?.Invoke();
            Debug.Log("gameover !!!!!!!!!!!!!!!");
        }
    }

    public void ResetPlayerStats()
    {
        if (m_playerStats != null)
        {
            m_playerStats.level = 1;
            m_playerStats.hp = 25;
            m_playerStats.damage = 1;
            m_playerStats.damageUp = 1;
            m_playerStats.xp = 0;
            m_playerStats.leverUpXP = 50;
            m_playerStats.Save();

            GUIManager.Ins.UpdateUI(m_playerStats);
            Debug.Log("Player stats reset to level 1 : " + m_playerStats.damage);
        }
    }



    public void OnResetButtonClicked()
    {
        ResetPlayerStats();
        Debug.Log("da click");
    }
}
