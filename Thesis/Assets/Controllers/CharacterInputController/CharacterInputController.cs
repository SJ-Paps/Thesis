using SJ.GameEntities.Characters;
using SJ.GameInput;
using SJ.Management;
using SJ.Tools;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace SJ.GameEntities.Controllers
{
    public class CharacterInputController : GameEntityController<Character, Character.Order>
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

        private class AxisActionWithOrder : InputActionWithOrder
        {
            public new AxisAction action;

            public AxisActionWithOrder(AxisAction action, Character.OrderType orderType) : base(action, orderType)
            {
                this.action = action;
            }
        }

        private const string MoveLeftKeyGroupName = "MoveLeft";
        private const string MoveRightKeyGroupName = "MoveRight";
        private const string WalkKeyGroupName = "Walk";
        private const string RunKeyGroupName = "Run";
        private const string JumpKeyGroupName = "Jump";
        private const string ActivateOrPullKeyGroupName = "Activate";

        [Header("Character Input Controller Configuration")]
        [SerializeField]
        private bool loadGameInputSettings = true;

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

        [Space]
        [SerializeField]
        private SimpleKeyAction jumpAction;

        [Space]
        [SerializeField]
        private SimpleKeyAction pullAction;

        private List<AxisActionWithOrder> axisActions = new List<AxisActionWithOrder>();
        private List<InputActionWithOrder> nonAxisActions = new List<InputActionWithOrder>();

        protected override void SJAwake()
        {
            GroupInputActions();

            if(loadGameInputSettings)
            {
                Management.Application.Instance.EventBus().Subscribe(ApplicationEvents.GameInputSettingsChanged, LoadInputActions);
                LoadInputActions();
            }

            base.SJAwake();
        }

        private void GroupInputActions()
        {
            //Axis Actions
            axisActions.Add(new AxisActionWithOrder(moveAction, Character.OrderType.Move));

            //Other Actions
            nonAxisActions.Add(new InputActionWithOrder(walkAction, Character.OrderType.Walk));
            nonAxisActions.Add(new InputActionWithOrder(runAction, Character.OrderType.Run));
            nonAxisActions.Add(new InputActionWithOrder(jumpAction, Character.OrderType.Jump));
            nonAxisActions.Add(new InputActionWithOrder(pullAction, Character.OrderType.Pull));
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

            jumpAction.SetKeys(
                keyGroups[JumpKeyGroupName].main,
                keyGroups[JumpKeyGroupName].alternative
                );

            pullAction.SetKeys(
                keyGroups[ActivateOrPullKeyGroupName].main,
                keyGroups[ActivateOrPullKeyGroupName].alternative
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