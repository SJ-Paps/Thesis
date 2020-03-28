using Boo.Lang;
using SJ.GameEntities.Characters;
using SJ.GameInput;
using SJ.Management;
using UniRx;
using UnityEngine;

namespace SJ.GameEntities.Controllers
{
    public class CharacterInputController : UnityController<Character, Character.Order>
    {
        private class InputActionWithOrder
        {
            public InputAction action;
            public Character.OrderType orderType;

            public InputActionWithOrder(InputAction action, Character.OrderType orderType)
            {
                this.action = action;
                this.orderType = orderType;
            }
        }

        private class AxisActionWithOrder
        {
            public AxisAction action;
            public Character.OrderType orderType;

            public AxisActionWithOrder(AxisAction action, Character.OrderType orderType)
            {
                this.action = action;
                this.orderType = orderType;
            }
        }

        private const string MoveLeftKeyGroupName = "MoveLeft";
        private const string MoveRightKeyGroupName = "MoveRight";
        private const string WalkKeyGroupName = "Walk";
        private const string RunKeyGroupName = "Run";

        [Header("Input Actions")]
        [Space]
        [SerializeField]
        private AxisAction moveAction;

        [Space]
        [SerializeField]
        private ToggleHoldKeyAction walkAction;

        [Space]
        [SerializeField]
        private ToggleHoldKeyAction runAction;

        private List<AxisActionWithOrder> axisActions = new List<AxisActionWithOrder>();
        private List<InputActionWithOrder> nonAxisActions = new List<InputActionWithOrder>();

        protected override void SJAwake()
        {
            GroupInputActions();
            LoadInputActions();
        }

        private void GroupInputActions()
        {
            //Axis Actions
            axisActions.Add(new AxisActionWithOrder(moveAction, Character.OrderType.Move));

            //Other Actions
            nonAxisActions.Add(new InputActionWithOrder(walkAction, Character.OrderType.Walk));
            nonAxisActions.Add(new InputActionWithOrder(runAction, Character.OrderType.Run));
        }

        private void LoadInputActions()
        {
            Repositories.GetGameInputSettingsRepository()
                .GetSettings()
                .Do(UpdateInputSettings)
                .Subscribe();
        }

        private void UpdateInputSettings(GameInputSettings settings)
        {
            var keyGroups = settings.GetGroups();

            moveAction.SetPositives(
                keyGroups[MoveRightKeyGroupName].main,
                keyGroups[MoveRightKeyGroupName].alternative
                );

            moveAction.SetNegatives(
                keyGroups[MoveLeftKeyGroupName].main,
                keyGroups[MoveLeftKeyGroupName].alternative
                );

            walkAction.SetKeys(
                keyGroups[WalkKeyGroupName].main,
                keyGroups[WalkKeyGroupName].alternative
                );

            walkAction.Hold = settings.holdWalkKey;

            runAction.SetKeys(
                keyGroups[RunKeyGroupName].main,
                keyGroups[RunKeyGroupName].alternative
                );
        }

        protected override void SJUpdate()
        {
            for (int i = 0; i < axisActions.Count; i++)
                axisActions[i].action.UpdateStatus();

            for (int i = 0; i < axisActions.Count; i++)
                if (axisActions[i].action.WasTriggeredThisFrame())
                    controllable.SendOrder(new Character.Order(axisActions[i].orderType, axisActions[i].action.AxisValue));

            for (int i = 0; i < nonAxisActions.Count; i++)
                if (nonAxisActions[i].action.WasTriggeredThisFrame())
                    controllable.SendOrder(new Character.Order(nonAxisActions[i].orderType, 0));
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

        protected override void OnPostLoad(object data)
        {
            CharacterInputControllerSaveData saveData = (CharacterInputControllerSaveData)data;

            SetControllable(SJUtil.FindGameEntityByEntityGUID<Character>(saveData.controllableGUID));
        }
    }
}