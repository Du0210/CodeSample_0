namespace HDU.Managers
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using Define = HDU.Define.CoreDefine;
    using System.Collections.Generic;
    using System;

    public class CardManager : IManager
    {
        public bool IsGetCardFinish { get; private set; } = false;
        public CardData[] DrawCardDatas;
        public Stack<CardData> CardDataStack;

        public void Clear()
        {
            
        }

        public void Init()
        {
            DrawCardDatas = HDU.Utils.CsUtils.InitializeArray<CardData>(3);
            CardDataStack = new Stack<CardData>();
        }
        
        public async UniTask GiveFirstCards()
        {
            SetCard(true);
            Managers.Event.InvokeEvent(Define.EEventType.OnOpenFirstCard);

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        }
        public void GiveCard()
        {
            SetCard(false);
            Managers.Event.InvokeEvent(Define.EEventType.OnOpenCard);
        }

        public void SetCard(bool isFirst)
        {
            for (int i = 0; i < 3; i++)
            {
                DrawCardDatas[i].Clear();

                Define.ECardType randCardType = (isFirst) ? (Define.ECardType)UnityEngine.Random.Range(0, 2) : Define.ECardType.Unit;
                switch (randCardType)
                {
                    case Define.ECardType.Cost:
                        DrawCardDatas[i].SetDataCost(UnityEngine.Random.Range(1, 4) * 5);
                        break;
                    case Define.ECardType.Unit:
                        int rand = UnityEngine.Random.Range(0,Managers.Unit.SelectedUnits.Count);

                        DrawCardDatas[i].SetData_Unit(Managers.Unit.SelectedUnits[rand]);
                        break;
                }
                CardDataStack.Push(DrawCardDatas[i]);
            }
        }



        public void SetCard()
        {
            for (int i = 0; i < 3; i++)
            {
                DrawCardDatas[i].Clear();
                DrawCardDatas[i].SetData_Unit((Define.EPlayerUnitType)UnityEngine.Random.Range(0, (int)Define.EPlayerUnitType.MaxCount));
            }
        }

        public class CardData
        {
            public Define.ECardType CardType;

            public Define.EPlayerUnitType UnitType;
            public int Cost;

            public void SetData_Unit(Define.EPlayerUnitType type)
            {
                CardType = Define.ECardType.Unit;
                UnitType = type;
            }

            public void SetDataCost(int amount)
            {
                CardType = Define.ECardType.Cost;
                Cost = amount;
            }

            public void Clear()
            {
                CardType = Define.ECardType.MaxCount;
                Cost = 0;
            }
        }
    }
}