using SJ.GameEntities.Characters;
using SJ.GameInput;
using System;
using UnityEngine;

namespace SJ.GameEntities.Controllers
{
    public class CharacterInputController : UnityController<Character, Character.Order>
    {
        [Serializable]
        private struct ActionOrder
        {
            [SerializeField]
            public CharacterInputAction action;
            [SerializeField]
            public Character.OrderType[] orders;
        }

        [SerializeField]
        private ActionOrder[] keyOrders;

        protected override void SJAwake()
        {
            for (int i = 0; i < keyOrders.Length; i++)
                keyOrders[i].action = Instantiate(keyOrders[i].action);
        }

        protected override void SJUpdate()
        {
            for (int i = 0; i < keyOrders.Length; i++)
            {
                keyOrders[i].action.UpdateStatus();

                if (keyOrders[i].action.WasTriggeredThisFrame)
                {
                    var orders = keyOrders[i].orders;

                    for (int j = 0; j < orders.Length; j++)
                    {
                        controllable.SendOrder(new Character.Order(orders[i], keyOrders[i].action.AxisValue));
                    }
                }
            }
        }

        public class CharacterInputControllerSaveData
        {
            public string controllableGUID;
        }

        protected override object GetSaveData()
        {
            return new CharacterInputControllerSaveData() { controllableGUID = Controllable.EntityGUID };
        }

        protected override void LoadSaveData(object data)
        {

        }

        protected override void OnPostSave()
        {

        }

        protected override void OnPostLoad(object data)
        {
            CharacterInputControllerSaveData saveData = (CharacterInputControllerSaveData)data;

            SetControllable(SJUtil.FindGameEntityByEntityGUID<Character>(saveData.controllableGUID));
        }
    }
}