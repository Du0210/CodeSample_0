namespace HDU.Managers
{
    using UnityEngine;

    public class GoodsManager : IManager
    {
        public GoodsData GoodsData { get => Managers.Save.UserData.Goods; private set => Managers.Save.UserData.Goods = value; }
        public int CardCost { get; private set; }

        public void Init()
        {
            
        }

        public void Clear()
        {
            
        }

        public void AddOrRemoveCost(int amount, bool isAdd) 
        {
            if (isAdd)
                CardCost += amount;
            else
                CardCost -= amount;

            Managers.Event.InvokeEvent(Define.CoreDefine.EEventType.OnDrawCost);
        } 
    }
}