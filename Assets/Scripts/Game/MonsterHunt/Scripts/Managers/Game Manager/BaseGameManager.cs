using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class BaseGameManager : MonoBehaviour
{
    protected const string GAMEDATA_KEY = "MonsterHuntGame";
    public enum GameState { Initialization, MainMenu, PortalCreation, Spawning, Battle, 
        GameOver, Win, Exit, Restart, NextLevel }

   
    public event Action OnMainMenuActivating;
    public event Action OnPortalCreating; 
    public event Action OnPortalCreated; 
    public event Action<int, Vector3, Quaternion> OnSpawning; 
    public event Action<int> OnBattling; 
    public event Action<BaseMonsterController> OnMonsterCreated;
    public event Action<BaseMonsterController> OnMonsterDead;
    public event Action<BaseMonsterController> OnMonsterAttacking;
    public event Action<BaseMonsterController> OnMonsterDamage;
    public event Action<MissileController> OnMissileDead;
    public event Action<float> OnPlayerDamage;
    public event Action OnPlayerDead;
    public event Action<bool> OnStatusPortalChanged; 
    public event Action<int> OnPlayerFired;
    public event Action OnMonsterFired;
    public event Action<float> OnGameOver; 
    public event Action OnRestart;
    public event Action<int> OnScoreUpdated;
    public event Action OnMonstersSpawned;
    public event Action OnWinLevel;
    public event Action OnScoreIncrementing;
    public event Action OnScoreIncremented;

    protected void RaiseMainMenuActivating() => OnMainMenuActivating?.Invoke();
    protected void RaisePortalCreating() => OnPortalCreating?.Invoke();
    protected void RaisePortalCreated() => OnPortalCreated?.Invoke();
    protected void RaiseSpawning(int level, Vector3 position, Quaternion rotation) => OnSpawning?.Invoke(level, position, rotation);
    protected void RaiseBattling(int level) => OnBattling?.Invoke(level);
    protected void RaiseMonsterCreated(BaseMonsterController monster) => OnMonsterCreated?.Invoke(monster);
    protected void RaiseMonsterDead(BaseMonsterController monster) => OnMonsterDead?.Invoke(monster);
    protected void RaiseMonsterAttacking(BaseMonsterController monster) => OnMonsterAttacking?.Invoke(monster);
    protected void RaiseMonsterDamage(BaseMonsterController monster) => OnMonsterDamage?.Invoke(monster);
    protected void RaiseMissileDead(MissileController missil) => OnMissileDead?.Invoke(missil);
    protected void RaisePlayerDamage(float currentHealthPercentage) => OnPlayerDamage?.Invoke(currentHealthPercentage);
    protected void RaisePlayerDead() => OnPlayerDead?.Invoke();
    protected void RaiseStatusPortalChanged(bool status) => OnStatusPortalChanged.Invoke(status);
    protected void RaisePlayerFired(int gunIndex) => OnPlayerFired?.Invoke(gunIndex);
    protected void RaiseMonsterFired() => OnMonsterFired?.Invoke();
    protected void RaiseGameOver(float delay) => OnGameOver?.Invoke(delay);
    protected void RaiseRestart() => OnRestart?.Invoke();
    protected void RaiseScoreUpdated(int score) => OnScoreUpdated?.Invoke(score);
    protected void RaiseMonstersSpawned() => OnMonstersSpawned?.Invoke();
    protected void RaiseWinLevel() => OnWinLevel?.Invoke();
    protected void RaiseScoreIncrementing() => OnScoreIncrementing?.Invoke();
    protected void RaiseScoreIncremented() => OnScoreIncremented?.Invoke();

    protected Transform portal;
    protected Camera arCamera;
    protected Transform player;
    protected List<MonsterController> monsters;
    protected List<MissileController> missiles;
    protected GameplayData gameplayData;
    protected SceneController sceneController;

    public ReadOnlyCollection<MonsterController> Monsters => monsters.AsReadOnly();
    public ReadOnlyCollection<MissileController> Missiles => missiles.AsReadOnly();
    public Transform Player => player;
    public Transform Portal => portal;
    public Camera ARCamera => arCamera;
    public Vector3 PlayerForward => player.forward;
    public Vector3 PlayerPosition => player ? player.position : Vector3.zero;
    protected int Score
    {
        get { return gameplayData.Score; }
        set
        {
            gameplayData.Score = value;
            RaiseScoreUpdated(gameplayData.Score);
        }
    }

    private GameState currentState;

    protected GameState CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;

            switch (currentState)
            {
                case GameState.Initialization:
                    Initialization();
                    break;
                case GameState.MainMenu:
                    MainMenu();
                    break;
                case GameState.PortalCreation:
                    PortalCreation();
                    break;
                case GameState.Spawning:
                    Spawning();
                    break;
                case GameState.Battle:
                    Battle();
                    break;
                case GameState.GameOver:
                    GameOver();
                    break;
                case GameState.Win:
                    Win();
                    break;
                case GameState.Exit:
                    Exit();
                    break;
                case GameState.Restart:
                    Restart();
                    break;
                default:
                    break;
            }
        }
    }

    protected abstract void Initialization();
    protected abstract void MainMenu();
    protected abstract void PortalCreation();
    protected abstract void Spawning();
    protected abstract void Battle();
    protected abstract void GameOver();
    protected abstract void Win();
    protected abstract void Exit();
    protected abstract void Restart();

}
