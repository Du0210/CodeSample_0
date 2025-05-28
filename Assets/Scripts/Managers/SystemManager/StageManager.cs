namespace HDU.Managers
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using Define;
    using System.Threading;
    using System;
    using Define = HDU.Define.CoreDefine;

    public class StageManager : IManager
    {
        public StageData @StageData { get => Managers.Save.UserData.Stage; private set => Managers.Save.UserData.Stage = value; }
        public CoreDefine.EBattleState BattleState { get; private set; }
        public CoreDefine.EBattleLoop BattleLoop { get; private set; }
        public int StageIndex { get; set; }
        public int StageTimer { get; set; }
        public int CurWave { get => _curWave; set => _curWave = value; }

        private CancellationTokenSource _cancelBattleLoop;

        private int _curWave = 0;

        public void Clear()
        {
            _cancelBattleLoop?.Cancel();
        }

        public void Init()
        {

        }

        public void EnterStage()
        {
            Managers.Scene.LoadScene(CoreDefine.ESceneType.BattleScene);
            Managers.Event.RegistEvent(Define.EEventType.OnKillPlayer, CheckWaveFinsh_Player);
            Managers.Event.RegistEvent(Define.EEventType.OnKillEnemy, CheckWaveFinsh_Enemy);
        }

        public void ExitStage()
        {
            SetUnitsBattleState(CoreDefine.EBattleState.None);
            Managers.Scene.LoadScene(CoreDefine.ESceneType.LobbyScene);
            Managers.Event.RemoveEvent(Define.EEventType.OnKillPlayer, CheckWaveFinsh_Player);
            Managers.Event.RemoveEvent(Define.EEventType.OnKillEnemy, CheckWaveFinsh_Enemy);
        }

        public void Retry()
        {
            Managers.Event.RemoveEvent(Define.EEventType.OnKillPlayer, CheckWaveFinsh_Player);
            Managers.Event.RemoveEvent(Define.EEventType.OnKillEnemy, CheckWaveFinsh_Enemy);
            EnterStage();
        }

        public void SetUnitsBattleState(CoreDefine.EBattleState state)
        {
            if (state == BattleState)
                return;

            BattleState = state;
            Managers.Event.InvokeEvent(CoreDefine.EEventType.OnChangedBattleState);
        }

        public void SetBattleLoop(CoreDefine.EBattleLoop state)
        {
            BattleLoop = state;
        }

        public void CheckWaveFinsh_Enemy()
        {
            if (BattleLoop != Define.EBattleLoop.Battle)
                return;
                
            if (Managers.Spawn.EnemyUnitList.Count <= 0)
            {
                SetBattleLoop(Define.EBattleLoop.Ready);
            }
        }

        public void CheckWaveFinsh_Player()
        {
            if (Managers.Spawn.PlayerUnitDict.Count <= 0)
            {
                SetBattleLoop(Define.EBattleLoop.Ready);
                _cancelBattleLoop.Cancel();
                EndStage(false).Forget();
            }
        }

        public async UniTaskVoid StartBattleLoop()
        {
            _cancelBattleLoop?.Cancel();
            _cancelBattleLoop = new CancellationTokenSource();
            var token = _cancelBattleLoop.Token;

            await InitStage(token);

            while (_curWave <= HDU.Define.CoreDefine.MAXWAVE)
            {
                await StartReadyPhase(token);
                if (token.IsCancellationRequested) return;

                await MoveDirect(token, true);
                if (token.IsCancellationRequested) return;

                await StartCombatPhase(token);
                if (token.IsCancellationRequested) return;

                _curWave++;
                if (_curWave > CoreDefine.MAXWAVE)
                    break;

                await MoveDirect(token, false);
                if (token.IsCancellationRequested) return;
            }

            await EndStage(true);
        }

        private async UniTask InitStage(CancellationToken token)
        {
            SetUnitsBattleState(CoreDefine.EBattleState.Wait);
            
            Managers.Goods.AddOrRemoveCost(Managers.Goods.CardCost, false);
            StageTimer = Define.MAXREADYTIMECOUNT;
            _curWave = 1;
            
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawWave);
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawBattleTimer, StageTimer);
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawCost);

            await Managers.Card.GiveFirstCards(); // 3초대기
            Managers.Event.InvokeEvent(Define.EEventType.OnSetFirstUnitSetting);

        }

        private async UniTask MoveDirect(CancellationToken tokken, bool isSpawnEnemy)
        {
            SetUnitsBattleState(Define.EBattleState.Wait);
            Managers.Event.InvokeEvent(Define.EEventType.OnFollowCamUpdate, true);
            Managers.Event.InvokeEvent(Define.EEventType.OnMoveSpawnerNext);
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawWave);

            if (isSpawnEnemy)
                Managers.Event.InvokeEvent(Define.EEventType.OnSpawnEnemy);

            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            //Spawner 위치 지정
            Managers.Event.InvokeEvent(Define.EEventType.OnFollowCamUpdate, false);

        }

        private async UniTask StartReadyPhase(CancellationToken tokken)
        {
            StageTimer = Define.MAXREADYTIMECOUNT;
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawBattleTimer, StageTimer);
            Managers.Card.GiveCard();
            SetUnitsBattleState(Define.EBattleState.Wait);
            SetBattleLoop(Define.EBattleLoop.Ready);

            Managers.Goods.AddOrRemoveCost(20 * _curWave, true);

            while (StageTimer > 0 && BattleLoop == Define.EBattleLoop.Ready)
            {
                if (_cancelBattleLoop.Token.IsCancellationRequested)
                    return;

                await UniTask.Delay(TimeSpan.FromSeconds(1));
                StageTimer--;
                Managers.Event.InvokeEvent(Define.EEventType.OnDrawBattleTimer, StageTimer);
            }
            Managers.Event.InvokeEvent(Define.EEventType.OnHideCard);
        }

        private async UniTask StartCombatPhase(CancellationToken tokken)
        {
            SetUnitsBattleState(CoreDefine.EBattleState.Battle);
            StageTimer = (int)Managers.Data.GetStageTableData(StageIndex).TimeLimit;
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawBattleTimer, StageTimer);
            SetBattleLoop(Define.EBattleLoop.Battle);

            while (StageTimer > 0 && BattleLoop == Define.EBattleLoop.Battle)
            {
                if (_cancelBattleLoop.Token.IsCancellationRequested)
                    return;

                await UniTask.Delay(TimeSpan.FromSeconds(1));
                StageTimer--;
                Managers.Event.InvokeEvent(Define.EEventType.OnDrawBattleTimer, StageTimer);
            }
            Debug.Log("Loop Exit");
        }

        private async UniTask EndStage(bool isClear)
        {
            _cancelBattleLoop?.Cancel();

            var data = Managers.Stage.StageData;
            if (isClear)
            {
                data.StageScoreList[(int)CoreDefine.EStageType.First][StageIndex - 1] = 3;
                data.StageIndexList[(int)CoreDefine.EStageType.First]++;
            }
            else
                data.StageScoreList[(int)CoreDefine.EStageType.First][StageIndex - 1] = 3;

            Managers.Save.CacheDirtyData(Define.ESaveType.StageData);
            Managers.Event.InvokeEvent(CoreDefine.EEventType.OnOpenResultPanel, isClear);
        }

        public void ExitBattle()
        {
            if(_cancelBattleLoop != null && !_cancelBattleLoop.IsCancellationRequested)
            {
                _cancelBattleLoop.Cancel();
                ExitStage();
            }
        }

        public int GetStageScore(CoreDefine.EStageType stageType, int stageIndex)
        {
            if (stageType == CoreDefine.EStageType.MaxCount)
            {
                Debug.LogError("GetStageScore : stageType is MaxCount");
                return 0;
            }

            if(stageIndex >= CoreDefine.MAXSTAGECOUNT)
            {
                Debug.LogError("GetStageScore : stageIndex is out of range");
                return 0;
            }
            return StageData.StageScoreList[(int)stageType][stageIndex];
        }
    }
}