using TestMod.PacketConvertation;
using CreatureGenerate;
using CreatureSelect;
using Customizing;
using HarmonyLib;
using Inventory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using WorkerSprite;
using static TestMod.InitOfficers;

namespace TestMod
{
    public static class GameDataHandler
    {
        static readonly Action<DeployUI, bool> gameStartetSetter = IlReflector.CreateSetter<DeployUI, bool>("_isGameStarted");
        static readonly Func<AgentModel, AgentTitleTypeInfo> prefixGetter = IlReflector.CreateGetter<AgentModel, AgentTitleTypeInfo>("prefix");
        static readonly Func<AgentModel, AgentTitleTypeInfo> suffixGetter = IlReflector.CreateGetter<AgentModel, AgentTitleTypeInfo>("suffix");
        static readonly Action<AgentModel, AgentTitleTypeInfo> prefixSetter = IlReflector.CreateSetter<AgentModel, AgentTitleTypeInfo>("prefix");
        static readonly Action<AgentModel, AgentTitleTypeInfo> suffixSetter = IlReflector.CreateSetter<AgentModel, AgentTitleTypeInfo>("suffix");
        static readonly Action<SefiraPanel, int> AgentCountSetter = IlReflector.CreateSetter<SefiraPanel, int>("_currentAgentCount");
        static readonly Func<SefiraPanel, int> AgentCountGetter = IlReflector.CreateGetter<SefiraPanel, int>("_currentAgentCount");
        static readonly IlReflector.GetSlotInvoker getSlotInvoker = IlReflector.CreateGetSlotInvoker();
        static readonly Func<DeployAgentList, List<DeployAgentSlot>> disabledListGetter = IlReflector.CreateGetter<DeployAgentList, List<DeployAgentSlot>>("_disabledList");
        static readonly IlReflector.SetListInvoker setListInvoker = IlReflector.CreateSetListInvoker();
        static readonly Func<WorkerModel, WorkerCommandQueue> CommandQueueGetter = IlReflector.CreateGetter<WorkerModel, WorkerCommandQueue>("commandQueue");
        static readonly Action<GameManager, PAUSECALL> currentPauseCallerSetter = IlReflector.CreateSetter<GameManager, PAUSECALL>("currentPauseCaller");
        static readonly Func<PlaySpeedSettingUI, List<PlaySpeedSettingUI.SpaceEvent>> spaceCalledGetter = IlReflector.CreateGetter<PlaySpeedSettingUI, List<PlaySpeedSettingUI.SpaceEvent>>("spaceCalled");
        static readonly IlReflector.UpdateGameSpeedInvoker updateGameSpeedInvoker = IlReflector.CreateUpdateGameSpeedInvoker();
        static readonly IlReflector.EscapeOpenInvoker escapeOpenInvoker = IlReflector.CreateEscapeOpenInvoker();
        static readonly IlReflector.EscapeCloseInvoker escapeCloseInvoker = IlReflector.CreateEscapeCloseInvoker();
        static readonly Func<ResearchWindow, bool> bossStateGetter = IlReflector.CreateGetter<ResearchWindow, bool>("_bossState");
        static readonly Action<ResearchWindow, bool> bossStateSetter = IlReflector.CreateSetter<ResearchWindow, bool>("_bossState");
        static readonly IlReflector.UpgradeInvoker upgradeInvoker = IlReflector.CreateUpgradeInvoker();
        static readonly Action<ResearchWindow, bool> isDraggingSetter = IlReflector.CreateSetter<ResearchWindow, bool>("_isDragging");
        static readonly Func<WorkerModel, List<BarrierBuf>> barrierListGetter = IlReflector.CreateGetter<WorkerModel, List<BarrierBuf>>("_barrierBufList");
        static readonly Action<WorkerModel, bool> revivalHpSetter = IlReflector.CreateSetter<WorkerModel, bool>("_revivalHp");
        static readonly Action<WorkerModel, bool> revivaledHpSetter = IlReflector.CreateSetter<WorkerModel, bool>("_revivaledHp");
        static readonly Action<WorkerModel, bool> revivalMentalSetter = IlReflector.CreateSetter<WorkerModel, bool>("_revivalMental");
        static readonly Action<WorkerModel, bool> revivaledMentalSetter = IlReflector.CreateSetter<WorkerModel, bool>("_revivaledMental");
        static readonly IlReflector.ObserveLevelChangedInvoker observeLevelInvoker = IlReflector.CreateObserveLevelChangedInvoker();
        static readonly IlReflector.SetCostInvoker setCostInvoker = IlReflector.CreateSetCostInvoker();
        static readonly Action<AgentModel, AgentAIState> AgentStateSetter = IlReflector.CreateSetter<AgentModel, AgentAIState>("_state");
        static readonly Action<AgentModel, bool> AutoSuppressingSetter = IlReflector.CreateSetter<AgentModel, bool>("_isAutoSuppressing");
        static readonly Action<CreatureSelectUI, bool> effectRunnedSetter = IlReflector.CreateSetter<CreatureSelectUI, bool>("effectRunned");
        static readonly Func<ExpandSefiraPanel, Timer> blinkTimerGetter = IlReflector.CreateGetter<ExpandSefiraPanel, Timer>("blinkTimer");
        static readonly Action<ExpandSefiraPanel, Image> CurrentBlinkSetter = IlReflector.CreateSetter<ExpandSefiraPanel, Image>("CurrentBlink");
        static readonly Action<ExpandUI, bool> LoadNextSceneSetter = IlReflector.CreateSetter<ExpandUI, bool>("_isLoadNextScene");
        static readonly Func<ExpandUI, bool> LoadNextSceneGetter = IlReflector.CreateGetter<ExpandUI, bool>("_isLoadNextScene");
        static readonly Action<ExpandUI, ExpandSefiraPanel> currentPanelSetter = IlReflector.CreateSetter<ExpandUI, ExpandSefiraPanel>("_currentPanel");
        static readonly Func<ExpandUI, SefiraEnum> selectedSefiraGetter = IlReflector.CreateGetter<ExpandUI, SefiraEnum>("_selectedSefira");
        static readonly Func<ExpandUI, Transform> tempParentGetter = IlReflector.CreateGetter<ExpandUI, Transform>("tempParent");
        static readonly Action<AgentModel, PassageObjectModel> currentWaitingPassageSetter = IlReflector.CreateSetter<AgentModel, PassageObjectModel>("_currentWaitingPassage");
        static readonly Action<AgentModel, RwbpType> bestRwbpSetter = IlReflector.CreateSetter<AgentModel, RwbpType>("_bestRwbp");
        static readonly Func<CreatureModel, int> qliphothCounterGetter = IlReflector.CreateGetter<CreatureModel, int>("_qliphothCounter");
        static readonly Action<CreatureModel, int> qliphothCounterSetter = IlReflector.CreateSetter<CreatureModel, int>("_qliphothCounter");
        static readonly IlReflector.UpdateQliphothCounterInvoker updateQliphothCounterInvoker = IlReflector.CreateUpdateQliphothCounterInvoker();
        public static readonly bool[] SefiraOfficersInit = new bool[13];
        /*public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == )
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }*/
        public static IEnumerable<CodeInstruction> AddAlterUITranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UIAdd), "AddAlterUI"))
            };
            foreach (CodeInstruction instruction in instructions)
            {
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> AddNewUITranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UIAdd), nameof(UIAdd.AddNewUI)))
            };
            foreach (CodeInstruction instruction in instructions)
            {
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> OnClickStartGameTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label? label = generator.DefineLabel();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            List<CodeInstruction> newInstructions = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()),
                new CodeInstruction(OpCodes.Brfalse, label),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldstr, "OnStartClick"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")),
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket"))
            };
            foreach (CodeInstruction instruction in instructions)
            {
                if (label != null)
                {
                    instruction.labels.Add(label.Value);
                    label = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void OnClickStartGame(DeployUI ui)
        {
            Notice.instance.Send(NoticeName.OnClickStartGame, new object[0]);
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
            {
                return;
            }
            if (!ui.StartAble)
            {
                return;
            }
            ui.canvas.gameObject.SetActive(false);
            ui.OnManagementStart();
            gameStartetSetter(ui, true);
        }
        public static IEnumerable<CodeInstruction> OnEscapeClickButtonTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo message = AccessTools.Method(typeof(SerializePacket), "Message");
            MethodInfo SendData = AccessTools.Method(typeof(MultiplayerController), "SendPacket");
            Label[] switchLabels = null;
            Label? labelIsConnected = null;
            Label returnLabel = generator.DefineLabel();
            CodeInstruction lastInstruction = null;
            foreach (CodeInstruction instruction in instructions)
            {
                lastInstruction = instruction;
                if (switchLabels != null && instruction.labels.Contains(switchLabels[0]))
                {
                    labelIsConnected = generator.DefineLabel();
                    Label originalLabel = switchLabels[0];
                    instruction.labels.Remove(originalLabel);
                    CodeInstruction entryInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    entryInstruction.labels.Add(originalLabel);
                    newInstructions.Add(entryInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelIsConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "Restart"));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, message));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                    newInstructions.Add(new CodeInstruction(OpCodes.Br, returnLabel));
                }
                if (switchLabels != null && instruction.labels.Contains(switchLabels[2]))
                {
                    labelIsConnected = generator.DefineLabel();
                    Label labelIsHost = generator.DefineLabel();
                    Label originalLabel = switchLabels[2];
                    instruction.labels.Remove(originalLabel);
                    CodeInstruction entryInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    entryInstruction.labels.Add(originalLabel);
                    newInstructions.Add(entryInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelIsConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelIsHost));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "OpenTitle"));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, message));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                    newInstructions.Add(new CodeInstruction(OpCodes.Br, returnLabel));
                    CodeInstruction firstNoHostInstruction = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SaveHandler), "SetOldSave"));
                    firstNoHostInstruction.labels.Add(labelIsHost);
                    newInstructions.Add(firstNoHostInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.DisconnectFromServer))));
                }
                if (switchLabels != null && instruction.labels.Contains(switchLabels[4]))
                {
                    labelIsConnected = generator.DefineLabel();
                    Label originalLabel = switchLabels[4];
                    instruction.labels.Remove(originalLabel);
                    CodeInstruction entryInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    entryInstruction.labels.Add(originalLabel);
                    newInstructions.Add(entryInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelIsConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "ToCheckpoint"));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, message));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                    newInstructions.Add(new CodeInstruction(OpCodes.Br, returnLabel));
                }
                if (instruction.opcode == OpCodes.Switch)
                {
                    switchLabels = (Label[])instruction.operand;
                }
                if (labelIsConnected != null)
                {
                    instruction.labels.Add(labelIsConnected.Value);
                    labelIsConnected = null;
                }
                newInstructions.Add(instruction);
            }
            lastInstruction.labels.Add(returnLabel);
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> OnRestartTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            List<CodeInstruction> newInstructions = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()),
                new CodeInstruction(OpCodes.Brfalse, labelToAttach),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldstr, "Restart"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")),
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")),
                new CodeInstruction(OpCodes.Ret)
            };
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> OnClickNextDayTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            List<CodeInstruction> newInstructions = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()),
                new CodeInstruction(OpCodes.Brfalse, labelToAttach),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldstr, "NextDay"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")),
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")),
                new CodeInstruction(OpCodes.Ret)
            };
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> OnMoveNextStageTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label[] switchLabels = null;
            Label? labelIsConnected = null;
            Label returnLabel = generator.DefineLabel();
            CodeInstruction lastInstruction = null;
            foreach (CodeInstruction instruction in instructions)
            {
                lastInstruction = instruction;
                if (instruction.opcode == OpCodes.Switch)
                {
                    switchLabels = (Label[])instruction.operand;
                }
                if (switchLabels != null && instruction.labels.Contains(switchLabels[2]))
                {
                    labelIsConnected = generator.DefineLabel();
                    Label originalLabel = switchLabels[2];
                    instruction.labels.Remove(originalLabel);
                    CodeInstruction entryInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    entryInstruction.labels.Add(originalLabel);
                    newInstructions.Add(entryInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelIsConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "NextStage"));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Br, returnLabel));
                }
                if (labelIsConnected != null)
                {
                    instruction.labels.Add(labelIsConnected.Value);
                    labelIsConnected = null;
                }
                newInstructions.Add(instruction);
            }
            lastInstruction.labels.Add(returnLabel);
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> ConfirmTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo SendData = AccessTools.Method(typeof(MultiplayerController), "SendPacket");
            MethodInfo SetAgentStatBonus = AccessTools.Method(typeof(CustomizingWindow), "SetAgentStatBonus");
            MethodInfo currAgent = AccessTools.Property(typeof(CustomizingWindow), "CurrentAgent").GetGetMethod();
            FieldInfo primaryStat = AccessTools.Field(typeof(AgentModel), "primaryStat");
            FieldInfo prefix = AccessTools.Field(typeof(GameDataHandler), nameof(prefixGetter));
            FieldInfo suffix = AccessTools.Field(typeof(GameDataHandler), nameof(suffixGetter));
            MethodInfo titleGetterInvoke = AccessTools.Method(typeof(Func<AgentModel, AgentTitleTypeInfo>), "Invoke");
            FieldInfo TitleId = AccessTools.Field(typeof(AgentTitleTypeInfo), "id");
            MethodInfo cost = AccessTools.Method(typeof(CustomizingWindow), "GetCost");
            FieldInfo uniqueScriptIndex = AccessTools.Field(typeof(AgentModel), "uniqueScriptIndex");
            Label? labelToAttach = null;
            bool inGeneratePath = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (inGeneratePath && instruction.opcode == OpCodes.Call && instruction.operand as MethodInfo == SetAgentStatBonus)
                {
                    inGeneratePath = false;
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CustomizingWindow), "CurrentData")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, primaryStat));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, prefix));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, titleGetterInvoke));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, TitleId));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, suffix));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, titleGetterInvoke));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, TitleId));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, cost));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "NewAgent")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                }
                else if (instruction.opcode == OpCodes.Call && instruction.operand as MethodInfo == SetAgentStatBonus)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, currAgent));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, currAgent));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, primaryStat));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, prefix));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, currAgent));
                    newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, titleGetterInvoke));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, TitleId));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, suffix));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, currAgent));
                    newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, titleGetterInvoke));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, TitleId));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, cost));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "ChangeAgent")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                }
                if (instruction.opcode == OpCodes.Stfld && instruction.operand as FieldInfo == uniqueScriptIndex) inGeneratePath = true;
            }
            return newInstructions;
        }
        public static void SetBuyedAgent(AgentData currData, int prefID, int suffID, int cost)
        {
            MoneyModel.instance.Pay(cost);
            AgentModel agentModel = AgentManager.instance.AddAgentModelCustom(currData);
            agentModel.iscustom = currData.isCustomName;
            agentModel.isUniqueCredit = currData.isUniqueCredit;
            agentModel.uniqueScriptIndex = currData.uniqueScriptIndex;
            prefixSetter(agentModel, AgentTitleTypeList.instance.GetData(prefID));
            suffixSetter(agentModel, AgentTitleTypeList.instance.GetData(suffID));
            DeployUI.instance.AddAgent(agentModel);
            Notice.instance.Send(NoticeName.OnWorkerReinforcementAccepted, new object[]
            {
                null
            });
        }
        public static void SetBonus(AgentModel agentModel, WorkerPrimaryStat stats, int prefID, int suffID, int cost)
        {
            MoneyModel.instance.Pay(cost);
            agentModel.primaryStat.hp = stats.hp;
            agentModel.primaryStat.mental = stats.mental;
            agentModel.primaryStat.work = stats.work;
            agentModel.primaryStat.battle = stats.battle;
            prefixSetter(agentModel, AgentTitleTypeList.instance.GetData(prefID));
            suffixSetter(agentModel, AgentTitleTypeList.instance.GetData(suffID));
            Notice.instance.Send(NoticeName.InitAgent, new object[]
            {
                        CustomizingWindow.CurrentWindow.CurrentAgent
            });
            Notice.instance.Send(NoticeName.OnWorkerReinforcementAccepted, new object[]
            {
                agentModel
            });
        }
        public static IEnumerable<CodeInstruction> OnClickEquipmentTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Stloc_0)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "EquipEGO")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }
        public static void OnEquip(AgentModel agent, EquipmentModel equipment)
        {
            InventoryUI.CurrentWindow.ItemController.OnEquipAction(equipment, agent);
            InventoryUI.CurrentWindow.AgentController.SetAgent(agent);
            if (InventoryUI.CurrentWindow.IsEnabled) InventoryUI.CurrentWindow.audioClipPlayer.OnPlayInList(2);
        }
        public static IEnumerable<CodeInstruction> AddAgentTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo OnPlayInList = AccessTools.Method(typeof(AudioClipPlayer), "OnPlayInList");
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == OnPlayInList)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(SefiraPanel), "Sefira").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Sefira), "sefiraEnum").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(AgentModel), "GetMovableNode")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MovableObjectNode), "GetCurrentNode")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "AddAgent")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }
        public static void AddAgent(SefiraPanel sefiraPanel, AgentModel agentModel, MapNode node)
        {
            agentModel.SetCurrentSefira(sefiraPanel.Sefira.indexString);
            sefiraPanel.allocateAgents.Add(agentModel);
            int currentAgentCount = AgentCountGetter(sefiraPanel);
            sefiraPanel.agentSlots[currentAgentCount].SetModel(agentModel);
            sefiraPanel.agentSlots[currentAgentCount].ui.SetColor(sefiraPanel.SefiraUIColor.imageColor);
            agentModel.GetMovableNode().SetCurrentNode(node);
            AgentCountSetter(sefiraPanel, 1 + currentAgentCount);
        }
        public static IEnumerable<CodeInstruction> InitialAddAgentTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            FieldInfo currAgentCount = AccessTools.Field(typeof(SefiraPanel), "_currentAgentCount");
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Stfld && instruction.operand as FieldInfo == currAgentCount)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(AgentModel), "GetMovableNode")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MovableObjectNode), "GetCurrentNode")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "InitAgentAdd")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> OnRemoveAgentTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label? label = generator.DefineLabel();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            List<CodeInstruction> newInstructions = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()),
                new CodeInstruction(OpCodes.Brfalse, label),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "AgentRemove")),
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket"))
            };
            foreach (CodeInstruction instruction in instructions)
            {
                if (label != null)
                {
                    instruction.labels.Add(label.Value);
                    label = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void RemoveAgent(SefiraPanel sefiraPanel, int index)
        {
            if (index < AgentCountGetter(sefiraPanel))
            {
                AgentModel model = sefiraPanel.agentSlots[index].Model;
                if (model == null)
                {
                    sefiraPanel.agentSlots[index].SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
                    sefiraPanel.agentSlots[index].ui.SetColor(sefiraPanel.SefiraUIColor.imageColor);
                }
                if (sefiraPanel.RemoveAgent(model))
                {
                    DeployUI.instance.agentList.AddAgent(model);
                }
            }
        }
        public static IEnumerable<CodeInstruction> DeployAgentTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label? label = generator.DefineLabel();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            List<CodeInstruction> newInstructions = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()),
                new CodeInstruction(OpCodes.Brfalse, label),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "DeployAgent")),
                new CodeInstruction(OpCodes.Call, multiplayerControllerInstance),
                new CodeInstruction(OpCodes.Call, SharedWriter),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket"))
            };
            foreach (CodeInstruction instruction in instructions)
            {
                if (label != null)
                {
                    instruction.labels.Add(label.Value);
                    label = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void DeployAgent(AgentModel agentModel)
        {
            if (DeployUI.instance.agentList.agentList.Contains(agentModel))
            {
                DeployAgentSlot slot = getSlotInvoker(DeployUI.instance.agentList, agentModel);
                if (slot != null)
                {
                    DeployUI.instance.agentList.agentList.Remove(agentModel);
                    DeployUI.instance.agentList.slotList.Remove(slot);
                    disabledListGetter(DeployUI.instance.agentList).Add(slot);
                    slot.RectTransform.SetParent(DeployUI.instance.agentList.disableParent);
                    slot.gameObject.SetActive(false);
                    setListInvoker(DeployUI.instance.agentList);
                    return;
                }
            }
        }
        public static IEnumerable<CodeInstruction> InitOfficerGroupTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            FieldInfo SefiraOfficersInit = AccessTools.Field(typeof(GameDataHandler), nameof(SefiraOfficersInit));
            MethodInfo AssignOfficerDept = AccessTools.Method(typeof(Sefira), "AssignOfficerDept");
            FieldInfo index = AccessTools.Field(typeof(Sefira), "index");
            MethodInfo MoveToNode = AccessTools.Method(typeof(WorkerModel), "MoveToNode", new Type[] { typeof(string) });
            LocalBuilder InitOfficers = generator.DeclareLocal(typeof(InitOfficers));
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Call && instruction.operand as MethodInfo == AssignOfficerDept)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, SefiraOfficersInit));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, index));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stelem_I1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloca, InitOfficers));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Constructor(typeof(InitOfficers), new[] { typeof(Sefira) })));
                }
                else if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == MoveToNode)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloca, InitOfficers));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_S, 7));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(InitOfficers), "OfficerToData")));
                }
                else if (instruction.opcode == OpCodes.Endfinally)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc, InitOfficers));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "OfficersInit")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, SefiraOfficersInit));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, index));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stelem_I1));
                }
            }
            return newInstructions;
        }
        public static void AddOfficers(Sefira sefira, List<OfficerData> officers)
        {
            foreach (OfficerData officer in officers)
            {
                OfficerModel model = OfficerManager.instance.CreateOfficerModel(sefira.indexString);
                model.instanceId = officer.id;
                model.SetCurrentNode(MapGraph.instance.GetNodeById(officer.currNodeId));
                model.name = officer.name;
                model.baseMaxHp = officer.MaxHp;
                model.hp = officer.MaxHp;
                model.baseMaxMental = officer.MaxMental;
                model.mental = officer.MaxMental;
                model.baseMovement = officer.baseMovement;
                model.spriteData.saveData = officer.setData;
                WorkerSpriteManager.instance.LoadWorkerSpriteSetBySaveData(model.spriteData);
                WorkerMoveToNode(model, officer.nodeId);
            }
            int count = sefira.officerList.Count;
            int num1 = 0;
            for (int i = 0; i < count; i++)
            {
                int num2 = sefira.openedDepartmentList.Count;
                if (num2 < 1)
                {
                    num2 = 1;
                }
                sefira.officerList[i].deptNum = num1++ % num2;
            }
        }
        public static IEnumerable<CodeInstruction> WorkerMoveToMovableTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? label = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
            nextInstruction.labels.Add(label.Value);
            newInstructions.Add(nextInstruction);
            label = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "WorkerMoveToMovable")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (label != null)
                {
                    instruction.labels.Add(label.Value);
                    label = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void WorkerMoveToNode(WorkerModel model, MovableObjectNode targetMovable, bool resetCommand)
        {
            if (targetMovable == null)
            {
                Debug.Log("WorkerMoveToNode targetMovable null");
                return;
            }
            WorkerCommandQueue workerCommand = CommandQueueGetter(model);
            if (resetCommand)
            {
                workerCommand.SetAgentCommand(WorkerCommand.MakeMove(targetMovable));
            }
            else
            {
                workerCommand.AddFirst(WorkerCommand.MakeMove(targetMovable));
            }
        }
        public static IEnumerable<CodeInstruction> WorkerMoveToNodeByIdTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? label = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
            nextInstruction.labels.Add(label.Value);
            newInstructions.Add(nextInstruction);
            label = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(GameDataHandler), nameof(SefiraOfficersInit))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(SefiraManager), nameof(SefiraManager.instance)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(WorkerModel), nameof(WorkerModel.currentSefira)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SefiraManager), nameof(SefiraManager.GetSefira), new Type[] { typeof(string) })));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Sefira), "index")));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Sub));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldelem_I1));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "WorkerMoveById")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (label != null)
                {
                    instruction.labels.Add(label.Value);
                    label = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void WorkerMoveToNode(WorkerModel worker, string targetNodeID)
        {
            WorkerCommandQueue workerCommand = CommandQueueGetter(worker);
            workerCommand.SetAgentCommand(WorkerCommand.MakeMove(MapGraph.instance.GetNodeById(targetNodeID)));
        }
        public static IEnumerable<CodeInstruction> WorkerMoveToNodeByLinkTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? label = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
            nextInstruction.labels.Add(label.Value);
            newInstructions.Add(nextInstruction);
            label = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, label));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "WorkerMoveByLink")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (label != null)
                {
                    instruction.labels.Add(label.Value);
                    label = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void WorkerMoveToNode(WorkerModel model, MapNode targetNode, bool resetCommand)
        {
            WorkerCommandQueue workerCommand = CommandQueueGetter(model);
            model.lastestMoveTarget = targetNode;
            if (resetCommand)
            {
                workerCommand.SetAgentCommand(WorkerCommand.MakeMove(targetNode));
            }
            else
            {
                workerCommand.AddFirst(WorkerCommand.MakeMove(targetNode));
            }
        }
        public static IEnumerable<CodeInstruction> OnClickTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo SendData = AccessTools.Method(typeof(MultiplayerController), "SendPacket");
            MethodInfo ForcelyCancelSuppress = AccessTools.Method(typeof(AgentModel), "ForcelyCancelSuppress");
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == ForcelyCancelSuppress)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "CancelSuppress")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Stloc_0)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CommandWindow.CommandWindow), nameof(CommandWindow.CommandWindow.SelectedWork)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "StartWorkCreature")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                }
                if (instruction.opcode == OpCodes.Stloc_2)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "StartWorkKit")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SendData));
                }
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> ForcelyCancelWorkTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo StopAction = AccessTools.Method(typeof(WorkerModel), "StopAction");
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "CancelWork")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            int neededInstruction = -1;
            foreach (CodeInstruction instruction in instructions)
            {
                if (neededInstruction >= 0)
                {
                    newInstructions.Add(instruction);
                    neededInstruction++;
                    if (neededInstruction == 6) neededInstruction = -1;
                    continue;
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == StopAction)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach.Value));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach.Value));
                    neededInstruction = 0;
                }
            }
            return newInstructions;
        }
        public static void ForcellyCancelWork(AgentModel model)
        {
            if (!model.canCancelCurrentWork)
            {
                return;
            }
            if (model.target != null && model.target.Unit != null)
            {
                model.target.Unit.room.OnCancelWork();
            }
            model.counterAttackEnabled = false;
            model.StopAction();
        }
        public static IEnumerable<CodeInstruction> ProgressWorkTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo AddBar = AccessTools.Method(typeof(IsolateRoom.WorkProgress), nameof(IsolateRoom.WorkProgress.AddBar));
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == AddBar)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "ProgressWorkResult")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }
        public static void ProgressWork(UseSkill skill, bool isSuccess)
        {
            skill.workCount++;
            if (isSuccess)
            {
                skill.successCount++;
                skill.targetCreature.script.OnSkillSuccessWorkTick(skill);
                skill.agent.history.AddWorkCubeCount(skill.skillTypeInfo.rwbpType, 1);
            }
            else
            {
                skill.failCount++;
                skill.targetCreature.script.OnSkillFailWorkTick(skill);
                DamageInfo damageInfo = skill.targetCreature.metaInfo.workDamage.Copy();
                RwbpType type = damageInfo.type;
                DefenseInfo defense = skill.agent.defense;
                UnitDirection dir = UnitDirection.LEFT;
                DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(skill.agent, type, defense, dir);
                skill.targetCreatureView.room.OnDamageInvoked(damageInfo);
            }
            skill.room.ProgressBar.AddBar(isSuccess);
            Notice.instance.Send(NoticeName.OnProcessWorkTick, new object[]
            {
                skill.targetCreature
            });
        }
        public static IEnumerable<CodeInstruction> PauseTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GameManager), nameof(GameManager.GetCurrentPauseCaller))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Pause")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void Pause(GameManager manager, PAUSECALL caller)
        {
            currentPauseCallerSetter(manager, caller);
            manager.state = GameState.PAUSE;
            updateGameSpeedInvoker(manager);
        }
        public static void OnPause(PlaySpeedSettingUI ui, PAUSECALL caller)
        {
            if (GameManager.currentGameManager.state == GameState.PLAYING)
            {
                ui.pauseFilter.gameObject.SetActive(true);
            }
            if (caller == PAUSECALL.OBSERVE)
            {
                ui.pauseFilter.sprite = ui.observeSprite;
            }
            else if (caller == PAUSECALL.ROULETTE)
            {
                ui.pauseFilter.gameObject.SetActive(false);
            }
            else if (caller == PAUSECALL.TUTORIAL)
            {
                ui.pauseFilter.gameObject.SetActive(false);
            }
            Pause(GameManager.currentGameManager, caller);
            foreach (PlaySpeedSettingUI.SpaceEvent spaceEvent in spaceCalledGetter(ui))
            {
                spaceEvent(false);
            }
        }
        public static IEnumerable<CodeInstruction> ResumeTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GameManager), nameof(GameManager.GetCurrentPauseCaller))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Resume")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void Resume(GameManager manager, PAUSECALL caller)
        {
            currentPauseCallerSetter(manager, caller);
            manager.state = GameState.PLAYING;
            updateGameSpeedInvoker(manager);
        }
        public static void OnResume(PlaySpeedSettingUI ui, PAUSECALL caller)
        {
            if (caller == PAUSECALL.OBSERVE)
            {
                ui.pauseFilter.sprite = ui.pauseDefSprite;
            }
            else if (caller == PAUSECALL.ROULETTE)
            {
                ui.pauseFilter.gameObject.SetActive(true);
            }
            Resume(GameManager.currentGameManager, caller);
            foreach (PlaySpeedSettingUI.SpaceEvent spaceEvent in spaceCalledGetter(ui))
            {
                spaceEvent(true);
            }
        }
        public static IEnumerable<CodeInstruction> SetPlaySpeedTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Speed")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void SetPlaySpeed(GameManager manager, int level)
        {
            if (level <= 0 || level > 3)
            {
                return;
            }
            if (EscapeUI.instance.IsOpened)
            {
                return;
            }
            manager.state = GameState.PLAYING;
            manager.gameSpeedLevel = level;
            updateGameSpeedInvoker(manager);
        }
        public static IEnumerable<CodeInstruction> EscapeTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "esc"));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void OpenEsc(EscapeUI ui)
        {
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
            {
                return;
            }
            escapeOpenInvoker(ui);
        }
        public static IEnumerable<CodeInstruction> OutEscapeTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(ManualUI), "Instance").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(ManualUI), "IsActivated").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(OptionUI), "Instance").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(OptionUI), "IsEnabled").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "outesc"));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void CloseEsc(EscapeUI ui)
        {
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
            {
                return;
            }
            escapeCloseInvoker(ui);
        }
        public static IEnumerable<CodeInstruction> OutOptionOrManualTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "outesc"));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void CloseOptions(OptionUI ui)
        {
            ui.ExecuteActions();
            if (!ui.IsEnabled)
            {
                return;
            }
            ui.IsEnabled = false;
            ui._controllerAnim.Hide();
            if (ui.BaseModBack != null)
            {
                UnityEngine.Object.Destroy(ui.BaseModBack);
                ui.BaseModBack = null;
            }
            GlobalGameManager.instance.SaveStateData();
        }
        public static void CloseManual(ManualUI ui)
        {
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
            {
                return;
            }
            if (!ui.IsActivated)
            {
                return;
            }
            ui.IsActivated = false;
            if (PlaySpeedSettingUI.instance != null && GameManager.currentGameManager.state != GameState.STOP)
            {
                PAUSECALL currentPauseCaller = GameManager.currentGameManager.GetCurrentPauseCaller();
                if (currentPauseCaller == PAUSECALL.ESCAPE)
                {
                    PlaySpeedSettingUI.instance.OnResume(PAUSECALL.ESCAPE);
                }
                else
                {
                    PlaySpeedSettingUI.instance.OnResume(PAUSECALL.MANAUL);
                }
            }
        }
        public static IEnumerable<CodeInstruction> OnResearchSelectTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = null;
            bool unboxing = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (unboxing && instruction.opcode == OpCodes.Stloc_0)
                {
                    unboxing = false;
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ResearchItemModel), "info")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ResearchItemTypeInfo), "id")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "SelectResearch")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
                else if (instruction.opcode == OpCodes.Isinst)
                {
                    unboxing = true;
                }
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> ConfirmResearchTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ResearchWindow), "selectedPanel")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(ResearchPanel), "CurrentModel").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ResearchItemModel), "info")));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ResearchItemTypeInfo), "id")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "ConfirmResearch")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void ConfirmResearch(ResearchWindow window)
        {
            DeployUI.instance.researchWindow.controller.Hide();
            if (bossStateGetter(window))
            {
                bossStateSetter(window, false);
                return;
            }
            ResearchItemModel currentModel = window.selectedPanel.CurrentModel;
            if (currentModel != null)
            {
                upgradeInvoker(window, currentModel);
            }
        }
        public static IEnumerable<CodeInstruction> DiscardResearchTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "DiscardResearch"));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Message")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void DiscardResearch(ResearchWindow window)
        {
            window.OnSetPanel(false);
            foreach (ResearchPanel researchPanel in window.panels)
            {
                researchPanel.PanelReset();
            }
            isDraggingSetter(window, false);
            window.OnExit(null);
        }
        public static IEnumerable<CodeInstruction> TakeDamageWorkerTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo Panic = AccessTools.Method(typeof(WorkerModel), "Panic");
            MethodInfo RecoverHP = AccessTools.Method(typeof(WorkerModel), "RecoverHP");
            MethodInfo RecoverMental = AccessTools.Method(typeof(WorkerModel), "RecoverMental");
            MethodInfo MakeSpatteredBlood = AccessTools.Method(typeof(WorkerModel), "MakeSpatteredBlood");
            LocalBuilder needMakeSpatteredBlood = generator.DeclareLocal(typeof(bool));
            LocalBuilder needRevialHp = generator.DeclareLocal(typeof(bool));
            LocalBuilder needRevialMental = generator.DeclareLocal(typeof(bool));
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            bool neededInstruction = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (neededInstruction)
                {
                    neededInstruction = false;
                    labelToAttach = generator.DefineLabel();
                    CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    instruction.MoveLabelsTo(nextInstruction);
                    newInstructions.Add(nextInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc, needMakeSpatteredBlood));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc, needRevialHp));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc, needRevialMental));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DamageInfo), "type")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DamageInfo), "specialDeadSceneName")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "WorkerDamage")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Call && instruction.operand as MethodInfo == MakeSpatteredBlood)
                {
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stloc, needMakeSpatteredBlood));
                }
                else if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == RecoverHP)
                {
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stloc, needRevialHp));
                }
                else if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == RecoverMental)
                {
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stloc, needRevialMental));
                }
                else if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == Panic) neededInstruction = true;
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> TakeDamageWithoutEffectWorkerTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo Panic = AccessTools.Method(typeof(WorkerModel), "Panic");
            MethodInfo RecoverHP = AccessTools.Method(typeof(WorkerModel), "RecoverHP");
            MethodInfo RecoverMental = AccessTools.Method(typeof(WorkerModel), "RecoverMental");
            LocalBuilder needRevialHp = generator.DeclareLocal(typeof(bool));
            LocalBuilder needRevialMental = generator.DeclareLocal(typeof(bool));
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            bool neededInstruction = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (neededInstruction)
                {
                    neededInstruction = false;
                    labelToAttach = generator.DefineLabel();
                    CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    instruction.MoveLabelsTo(nextInstruction);
                    newInstructions.Add(nextInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc, needRevialHp));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc, needRevialMental));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DamageInfo), "type")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DamageInfo), "specialDeadSceneName")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "WorkerDamageNoEffect")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == RecoverHP)
                {
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stloc, needRevialHp));
                }
                else if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == RecoverMental)
                {
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Stloc, needRevialMental));
                }
                else if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == Panic) neededInstruction = true;
            }
            return newInstructions;
        }
        public static void ApplyWorkerDamage(WorkerModel worker, UnitModel actor, float damage, bool needMakeSpatteredBlood, bool needRevivalHp, bool needRevivalMental, RwbpType rwbp, string specialDeadScene)
        {
            DamageInfo dmg = new DamageInfo(rwbp, 0);
            worker.Equipment.armor?.OnTakeDamage(actor, ref dmg);
            worker.Equipment.weapon?.OnTakeDamage(actor, ref dmg);
            worker.Equipment.gifts.OnTakeDamage(actor, ref dmg);
            if (dmg.type == RwbpType.R)
            {
                List<BarrierBuf> list = barrierListGetter(worker);
                for (int i = 0; i < list.Count; i++)
                {
                    damage = list[i].UseBarrier(RwbpType.R, damage);
                }
                float num3 = worker.hp;
                worker.hp -= damage;
                float value = num3 - worker.hp;
                worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                worker.MakeDamageEffect(RwbpType.R, value, worker.defense.GetDefenseType(RwbpType.R));
                if (needMakeSpatteredBlood)
                {
                    worker.MakeSpatteredBlood();
                }
            }
            else if (dmg.type == RwbpType.W)
            {
                List<BarrierBuf> list2 = barrierListGetter(worker);
                for (int j = 0; j < list2.Count; j++)
                {
                    damage = list2[j].UseBarrier(RwbpType.W, damage);
                }
                float num5 = worker.mental;
                float value2;
                if (worker.CannotControll() && worker.unconAction is Uncontrollable_RedShoes)
                {
                    num5 = worker.hp;
                    worker.hp -= damage;
                    value2 = worker.hp - (int)num5;
                    worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                }
                else if (worker.IsPanic() && !(worker is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
                {
                    worker.mental += damage;
                    value2 = worker.mental - num5;
                }
                else if (worker is OfficerModel)
                {
                    worker.mental -= damage;
                    worker.hp -= damage;
                    value2 = num5 - worker.hp;
                }
                else
                {
                    worker.mental -= damage;
                    value2 = num5 - worker.mental;
                    worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamageMental(worker, damage);
                }
                worker.MakeDamageEffect(RwbpType.W, value2, worker.defense.GetDefenseType(RwbpType.W));
            }
            else if (dmg.type == RwbpType.B)
            {
                List<BarrierBuf> list3 = barrierListGetter(worker);
                for (int k = 0; k < list3.Count; k++)
                {
                    damage = list3[k].UseBarrier(RwbpType.B, damage);
                }
                float num6 = worker.hp;
                worker.hp -= damage;
                float value3 = num6 - worker.hp;
                if (worker.IsPanic() && !(worker is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
                {
                    worker.mental += damage;
                }
                else
                {
                    worker.mental -= damage;
                    worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamageMental(worker, damage);
                }
                worker.MakeDamageEffect(RwbpType.B, value3, worker.defense.GetDefenseType(RwbpType.B));
                if (needMakeSpatteredBlood)
                {
                    worker.MakeSpatteredBlood();
                }
            }
            else if (dmg.type == RwbpType.P)
            {
                float num8 = damage / 100f;
                damage = worker.maxHp * num8;
                List<BarrierBuf> list4 = barrierListGetter(worker);
                for (int l = 0; l < list4.Count; l++)
                {
                    damage = list4[l].UseBarrier(RwbpType.P, damage);
                }
                float num9 = worker.hp;
                worker.hp -= damage;
                float value4 = num9 - worker.hp;
                worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                worker.MakeDamageEffect(RwbpType.P, value4, worker.defense.GetDefenseType(RwbpType.P));
            }
            else if (dmg.type == RwbpType.N)
            {
                float hp = worker.hp;
                worker.hp -= damage;
                float value5 = hp - worker.hp;
                worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                worker.MakeDamageEffect(RwbpType.N, value5, DefenseInfo.Type.NONE);
            }
            if (damage > 0f && worker.defense.GetMultiplier(dmg.type) > 1f)
            {
                worker.AddUnitBuf(new UnderAttackBuf());
            }
            if (worker.IsPanic() && (worker.CurrentPanicAction is PanicRoaming || worker.CurrentPanicAction is PanicOpenIsolate) && actor is AgentModel && !((AgentModel)actor).CannotControll() && !((AgentModel)actor).IsPanic())
            {
                worker.AddUnitBuf(new PanicUnderAttackBuf());
            }
            worker.hp = Mathf.Clamp(worker.hp, 0f, worker.maxHp);
            worker.mental = Mathf.Clamp(worker.mental, 0f, worker.maxMental);
            if (worker.invincible)
            {
                return;
            }
            if (worker.hp <= 0f)
            {
                if (needRevivalHp)
                {
                    worker.hp = 1f;
                    revivaledHpSetter(worker, true);
                    revivalHpSetter(worker, false);
                    worker.RecoverHP(worker.maxHp / 2f);
                }
                else
                {
                    if (specialDeadScene != "-")
                    {
                        worker.SetSpecialDeadScene(specialDeadScene);
                    }
                    worker.OnDie();
                }
            }
            else if (worker.IsPanic())
            {
                if (worker.mental >= worker.maxMental)
                {
                    worker.StopPanic();
                }
            }
            else if (worker.mental <= 0f)
            {
                if (needRevivalMental)
                {
                    revivaledMentalSetter(worker, true);
                    revivalMentalSetter(worker, false);
                    worker.RecoverMental(worker.maxMental / 2f);
                }
                else
                {
                    worker.Panic();
                }
            }
            worker.Equipment.armor?.OnTakeDamage_After(damage, dmg.type);
            worker.Equipment.weapon?.OnTakeDamage_After(damage, dmg.type);
            worker.Equipment.gifts.OnTakeDamage_After(damage, dmg.type);
        }
        public static void ApplyWorkerDamageNoEffect(WorkerModel worker, UnitModel actor, float damage, bool needRevivalHp, bool needRevivalMental, RwbpType rwbp, string specialDeadScene)
        {
            DamageInfo dmg = new DamageInfo(rwbp, 0);
            worker.Equipment.armor?.OnTakeDamage(actor, ref dmg);
            worker.Equipment.weapon?.OnTakeDamage(actor, ref dmg);
            worker.Equipment.gifts.OnTakeDamage(actor, ref dmg);
            if (dmg.type == RwbpType.R)
            {
                float num3 = worker.hp;
                worker.hp -= damage;
                float value = num3 - worker.hp;
                worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                worker.MakeDamageEffect(RwbpType.R, value, worker.defense.GetDefenseType(RwbpType.R));
            }
            else if (dmg.type == RwbpType.W)
            {
                float num5 = worker.mental;
                float value2;
                if (worker.CannotControll() && worker.unconAction is Uncontrollable_RedShoes)
                {
                    num5 = worker.hp;
                    worker.hp -= damage;
                    value2 = worker.hp - (int)num5;
                    worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                }
                else if (worker.IsPanic() && !(worker is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
                {
                    worker.mental += damage;
                    value2 = worker.mental - num5;
                }
                else if (worker is OfficerModel)
                {
                    worker.mental -= damage;
                    worker.hp -= damage;
                    value2 = num5 - worker.hp;
                }
                else
                {
                    worker.mental -= damage;
                    value2 = num5 - worker.mental;
                    worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamageMental(worker, damage);
                }
                worker.MakeDamageEffect(RwbpType.W, value2, worker.defense.GetDefenseType(RwbpType.W));
            }
            else if (dmg.type == RwbpType.B)
            {
                float num6 = worker.hp;
                worker.hp -= damage;
                float value3 = num6 - worker.hp;
                if (worker.IsPanic() && !(worker is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
                {
                    worker.mental += damage;
                }
                else
                {
                    worker.mental -= damage;
                    worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamageMental(worker, damage);
                }
                worker.MakeDamageEffect(RwbpType.B, value3, worker.defense.GetDefenseType(RwbpType.B));
            }
            else if (dmg.type == RwbpType.P)
            {
                float num8 = damage / 100f;
                damage = worker.maxHp * num8;
                float num9 = worker.hp;
                worker.hp -= damage;
                float value4 = num9 - worker.hp;
                worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                worker.MakeDamageEffect(RwbpType.P, value4, worker.defense.GetDefenseType(RwbpType.P));
            }
            else if (dmg.type == RwbpType.N)
            {
                float hp = worker.hp;
                worker.hp -= damage;
                float value5 = hp - worker.hp;
                worker.Equipment.kitCreature?.script.kitEvent.OnTakeDamagePhysical(worker, damage);
                worker.MakeDamageEffect(RwbpType.N, value5, DefenseInfo.Type.NONE);
            }
            if (damage > 0f && worker.defense.GetMultiplier(dmg.type) > 1f)
            {
                worker.AddUnitBuf(new UnderAttackBuf());
            }
            if (worker.IsPanic() && (worker.CurrentPanicAction is PanicRoaming || worker.CurrentPanicAction is PanicOpenIsolate) && actor is AgentModel && !((AgentModel)actor).CannotControll() && !((AgentModel)actor).IsPanic())
            {
                worker.AddUnitBuf(new PanicUnderAttackBuf());
            }
            worker.hp = Mathf.Clamp(worker.hp, 0f, worker.maxHp);
            worker.mental = Mathf.Clamp(worker.mental, 0f, worker.maxMental);
            if (worker.invincible)
            {
                return;
            }
            if (worker.hp <= 0f)
            {
                if (needRevivalHp)
                {
                    worker.hp = 1f;
                    revivaledHpSetter(worker, true);
                    revivalHpSetter(worker, false);
                    worker.RecoverHP(worker.maxHp / 2f);
                }
                else
                {
                    if (specialDeadScene != "-")
                    {
                        worker.SetSpecialDeadScene(specialDeadScene);
                    }
                    worker.OnDie();
                }
            }
            else if (worker.IsPanic())
            {
                if (worker.mental >= worker.maxMental)
                {
                    worker.StopPanic();
                }
            }
            else if (worker.mental <= 0f)
            {
                if (needRevivalMental)
                {
                    revivaledMentalSetter(worker, true);
                    revivalMentalSetter(worker, false);
                    worker.RecoverMental(worker.maxMental / 2f);
                }
                else
                {
                    worker.Panic();
                }
            }
            worker.Equipment.armor?.OnTakeDamage_After(damage, dmg.type);
            worker.Equipment.weapon?.OnTakeDamage_After(damage, dmg.type);
            worker.Equipment.gifts.OnTakeDamage_After(damage, dmg.type);
        }
        public static IEnumerable<CodeInstruction> OnTryPurchaseTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo TransactionCube = AccessTools.Method(typeof(CreatureModel), "TransactionCube");
            Label? labelToAttach = null;
            bool neededInstruction = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == TransactionCube)
                {
                    neededInstruction = true;
                }
                else if (neededInstruction && instruction.opcode == OpCodes.Brfalse)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureInfoWindow), "CurrentModel").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureInfoWindow), "ObserveInfo").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureInfoController), "AreaName").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CreatureInfoWindow), "_oldLevel")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "CreatureObserve")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }
        public static void Purchase(CreatureInfoWindow window, string areaName, CreatureObserveInfoModel model, int oldLevel, CreatureInfoController controller)
        {
            controller.OnPurchase();
            window.ObserveInfo.OnObserveRegion(areaName);
            int observationLevel = window.ObserveInfo.GetObservationLevel();
            if (oldLevel != model.GetObservationLevel())
            {
                observeLevelInvoker.Invoke(window, observationLevel);
                window.CurrentModel.AddObservationLevel();
            }
            window.PaymentAnimCTRL.SetTrigger("Payment");
            setCostInvoker(window);
        }
        public static IEnumerable<CodeInstruction> OnClickCreateEquipTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Stloc_1)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CreatureEquipmentMakeInfo), "equipTypeInfo")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(EquipmentTypeInfo), "id")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureInfoController), "CurrentModel").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureInfoController), "ObserveInfo").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "CreateEquipment")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        }  
        public static IEnumerable<CodeInstruction> AttachEGOgiftTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo SetLockState = AccessTools.Method(typeof(UnitEGOgiftSpace), "SetLockState");
            Label? labelToAttach = null;
            bool neededInstruction = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (neededInstruction)
                {
                    neededInstruction = false;
                    labelToAttach = generator.DefineLabel();
                    CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    instruction.MoveLabelsTo(nextInstruction);
                    newInstructions.Add(nextInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(EGOgiftModel), "metaInfo")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(EquipmentTypeInfo), "id")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "GetEGO")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == SetLockState)
                {
                    neededInstruction = true;
                }
            }
            return newInstructions;
        } 
        public static IEnumerable<CodeInstruction> ActivateOverloadTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo isConnected = AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldnull));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            bool neededInstruction = false;
            CodeInstruction moveInstructions = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (moveInstructions != null)
                {
                    instruction.MoveLabelsTo(moveInstructions);
                    moveInstructions = null;
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (neededInstruction && instruction.opcode == OpCodes.Blt)
                {
                    neededInstruction = false;
                    labelToAttach = generator.DefineLabel();
                    CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    moveInstructions = nextInstruction;
                    newInstructions.Add(nextInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, isConnected));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_3));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "OverloadedCreatures")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
                else if (instruction.opcode == OpCodes.Ldarg_1)
                {
                    neededInstruction = true;
                }
            }
            return newInstructions;
        } 
        public static IEnumerable<CodeInstruction> CreatureToNodeTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            Label labelHost = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelHost));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "CreatureMoveToNode")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_3));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            newInstructions.Add(new CodeInstruction(OpCodes.Br, labelToAttach));
            CodeInstruction instruction1 = new CodeInstruction(OpCodes.Ret);
            instruction1.labels.Add(labelHost);
            newInstructions.Add(instruction1);
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        } 
        public static IEnumerable<CodeInstruction> CreatureToMovableTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            Label labelHost = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelHost));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "CreatureMoveToMovable")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_3));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            newInstructions.Add(new CodeInstruction(OpCodes.Br, labelToAttach));
            CodeInstruction instruction1 = new CodeInstruction(OpCodes.Ret);
            instruction1.labels.Add(labelHost);
            newInstructions.Add(instruction1);
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        } 
        public static IEnumerable<CodeInstruction> SetWaitingPassageTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PassageObjectModel), "GetSefiraEnum")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "WaitingPassage")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        } 
        public static void SetWaitingPassage(AgentModel agent, PassageObjectModel passage)
        {
            currentWaitingPassageSetter(agent, passage);
        }
        public static IEnumerable<CodeInstruction> SuppressTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo SetAgentCommand = AccessTools.Method(typeof(WorkerCommandQueue), "SetAgentCommand");
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == SetAgentCommand)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "Suppress")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                }
            }
            return newInstructions;
        } 
        public static void Suppress(AgentModel agent, UnitModel target, bool isAuto = false)
        {
            CommandQueueGetter(agent).SetAgentCommand(WorkerCommand.MakeSuppressCommand(target));
            if (target is WorkerModel worker)
            {
                AgentStateSetter(agent, AgentAIState.SUPPRESS_WORKER);
                agent.targetWorker = worker;
            }
            else
            {
                AgentStateSetter(agent, AgentAIState.SUPPRESS_CREATURE);
                agent.target = target as CreatureModel;
            }
            agent.GetUnit().SelectIconForcelyEnable();
            AutoSuppressingSetter(agent, isAuto);
            Notice.instance.Send(NoticeName.OnCommandSuppress, new object[]
            {
            target
            });
        }
        public static IEnumerable<CodeInstruction> OnClickUnitTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureSelectUnit), "CreatureID").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "SelectAno")));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void OnClickUnit(CreatureSelectUI ui, CreatureSelectUnit unit)
        {
            if (unit == null)
            {
                Debug.Log("Selected abnormality null");
            }
            unit.TransSelected = true;
            unit.TransAnim.SetTrigger("OnClick");
            effectRunnedSetter(ui, true);
            if (unit.CreatureID == 100015L)
            {
                PlayerModel.instance.AddWaitingCreature(100014L);
            }
            else
            {
                PlayerModel.instance.AddWaitingCreature(unit.CreatureID);
            }
            CreatureGenerateInfoManager.Instance.OnUsed(unit.CreatureID);
            ui.GlobalControlAnim.SetTrigger("Close");
            ui.FadeoutEffect(3f);
            ui.TextBoxController.Hide();
        }
        public static IEnumerable<CodeInstruction> InitSelectUITranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), "instance").GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), "SharedWriter").GetGetMethod();
            MethodInfo PlayClip = AccessTools.Method(typeof(StoryBgm), "PlayClip");
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == PlayClip)
                {
                    labelToAttach = generator.DefineLabel();
                    Label hostLabel = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "IsConnected").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, hostLabel));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CreatureSelectUI), "Units")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldelem_Ref));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureSelectUnit), "CreatureID").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CreatureSelectUI), "Units")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldelem_Ref));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureSelectUnit), "CreatureID").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CreatureSelectUI), "Units")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldelem_Ref));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(CreatureSelectUnit), "CreatureID").GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), "InitSelectUI")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "SendPacket")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Br, labelToAttach));
                    CodeInstruction instruction1 = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    instruction1.labels.Add(hostLabel);
                    newInstructions.Add(instruction1);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), "AwaitSelectUIDataCoroutine")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MonoBehaviour), "StartCoroutine", new Type[] { typeof(System.Collections.IEnumerator) })));
                    newInstructions.Add(new CodeInstruction(OpCodes.Pop));
                }
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> SelectToExpandTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            MethodInfo OnPlayInList = AccessTools.Method(typeof(AudioClipPlayer), nameof(AudioClipPlayer.OnPlayInList));
            Label? labelToAttach = null;
            CodeInstruction moveLabels = null;
            bool neededInstruction = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (moveLabels != null)
                {
                    instruction.MoveLabelsTo(moveLabels);
                    moveLabels = null;
                }
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand as MethodInfo == OnPlayInList)
                {
                    neededInstruction = true;
                }
                else if (neededInstruction && instruction.opcode == OpCodes.Ret)
                {
                    neededInstruction = false;
                    labelToAttach = generator.DefineLabel();
                    moveLabels = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    newInstructions.Add(moveLabels);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(ExpandSefiraPanel), nameof(ExpandSefiraPanel.Sefira)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Sefira), nameof(Sefira.sefiraEnum)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.SelectToExpand))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
                }
                else neededInstruction = false;
            }
            return newInstructions;
        }
        public static void OnSelected(ExpandSefiraPanel panel)
        {
            panel.ExpandUiSound.OnPlayInList(1);
            panel.ExpandUiSound.OnPlayInList(2);
            ExpandUI.instance.TryOpenSefira(panel);
            Timer timer = blinkTimerGetter(panel);
            timer.StartTimer(1f);
            try
            {
                CurrentBlinkSetter(panel, panel.Filled[panel.Level].transform.GetChild(0).GetComponent<Image>());
                panel.Filled[panel.Level].transform.GetChild(0).gameObject.SetActive(true);
            }
            catch
            {
                timer.StopTimer();
            }
        }
        public static IEnumerable<CodeInstruction> OnExpandTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "Expand"));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.Message))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void Expand(ExpandUI ui)
        {
            if (LoadNextSceneGetter(ui))
            {
                return;
            }
            SefiraManager.instance.OpenSefiraWithCreature(selectedSefiraGetter(ui));
            currentPanelSetter(ui, null);
            LoadNextSceneSetter(ui, true);
            ui.appearAnim.SetTrigger("Disappear");
            ui.diaglogUI.GetComponent<UIController>().Hide();
        }
        public static IEnumerable<CodeInstruction> OnCancelExpandTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldstr, "CancelExpand"));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.Message))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void CancelExpand(ExpandUI ui)
        {
            ExpandSefiraPanel currentPanel = ui.CurrentPanel;
            currentPanelSetter(ui, null);
            Transform tempParent = tempParentGetter(ui);
            if (currentPanel != null)
            {
                currentPanel.transform.SetParent(tempParent);
                currentPanel.NameField.transform.SetParent(tempParent);
                currentPanel.NameField.transform.SetAsLastSibling();
                currentPanel.OnExit();
            }
            LoadNextSceneSetter(ui, false);
            ui.diaglogUI?.GetComponent<UIController>().Hide();
            if (ui.currentCharacter != null)
            {
                ui.appearAnim.SetTrigger("Disappear");
            }
            ui.BlockLayer.gameObject.SetActive(false);
        }
        public static IEnumerable<CodeInstruction> UpdatePrimaryStatTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            MethodInfo UpdateTitle = AccessTools.Method(typeof(AgentModel), nameof(AgentModel.UpdateTitle));
            Label? labelToAttach = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Call && instruction.operand as MethodInfo == UpdateTitle)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.Host)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(AgentModel), "prefix")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(AgentTitleTypeInfo), nameof(AgentTitleTypeInfo.id))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(AgentModel), "suffix")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(AgentTitleTypeInfo), nameof(AgentTitleTypeInfo.id))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.NewTitles))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
                }
            }
            return newInstructions;
        } 
        public static void UpdateAgentTitles(AgentModel agent, int prefixId, int suffixId)
        {
            prefixSetter(agent, AgentTitleTypeList.instance.GetData(prefixId));
            suffixSetter(agent, AgentTitleTypeList.instance.GetData(suffixId));
        }
        public static IEnumerable<CodeInstruction> UpdateBestRwbpTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            FieldInfo bestRwbp = AccessTools.Field(typeof(AgentModel), "_bestRwbp");
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), "Host").GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Stfld && instruction.operand as FieldInfo == bestRwbp)
                {
                    labelToAttach = generator.DefineLabel();
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Dup));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(AgentModel), "_bestRwbp")));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.BestRwbp))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
                }
            }
            return newInstructions;
        } 
        public static void UpdateAgentBestRwbp(AgentModel agent, RwbpType type)
        {
            bestRwbpSetter(agent, type);
        }
        public static IEnumerable<CodeInstruction> CreatureTakeDamageTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.Host)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            bool neededInstruction = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldarg_0 && neededInstruction)
                {
                    neededInstruction = false;
                    labelToAttach = generator.DefineLabel();
                    CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, multiplayerControllerInstance);
                    nextInstruction.MoveLabelsFrom(instruction);
                    newInstructions.Add(nextInstruction);
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
                    newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldloc_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DamageInfo), nameof(DamageInfo.type))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.CreatureTakeDamage))));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
                    newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                    newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
                }
                else neededInstruction = false;
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
                if (instruction.opcode == OpCodes.Stloc_0)
                {
                    neededInstruction = true;
                }
            }
            return newInstructions;
        }
        public static void CreatureTakeDamage(CreatureModel creature, UnitModel actor, float num, RwbpType type)
        {
            DamageInfo dmg = new DamageInfo(type, 0);
            if (!creature.script.CanTakeDamage(actor, dmg)) return;
            creature.Equipment.armor?.OnTakeDamage(actor, ref dmg);
            creature.Equipment.weapon?.OnTakeDamage(actor, ref dmg);
            creature.Equipment.gifts.OnTakeDamage(actor, ref dmg);
            if (creature.hp > 0f)
            {
                if (num >= 0f)
                {
                    float num3 = creature.hp;
                    creature.hp -= num;
                    float num4 = num3 - creature.hp;
                    creature.MakeDamageEffect(dmg.type, num4, creature.defense.GetDefenseType(dmg.type));
                    if (dmg.type == RwbpType.R || dmg.type == RwbpType.B || num4 > 1f) creature.MakeSpatteredBlood();
                    creature.script.OnTakeDamage(actor, dmg, num);
                }
                else if (num < 0f)
                {
                    float num5 = -num;
                    if (creature.maxHp - creature.hp >= num5) creature.hp += num5;
                    else creature.hp = creature.maxHp;
                    GameObject gameObject = Prefab.LoadPrefab("Effect/RecoverHP");
                    gameObject.transform.SetParent(creature.Unit.animTarget.transform);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localRotation = Quaternion.identity;
                }
            }
            creature.hp = Mathf.Clamp(creature.hp, 0f, creature.maxHp);
            if (creature.state == CreatureState.ESCAPE && creature.hp <= 0f) creature.Suppressed();
            creature.Equipment.armor?.OnTakeDamage_After(num, dmg.type);
            creature.Equipment.weapon?.OnTakeDamage_After(num, dmg.type);
            creature.Equipment.gifts.OnTakeDamage_After(num, dmg.type);
        }
        public static IEnumerable<CodeInstruction> SubQliphothCountTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            Label HostLabel = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.Host)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, HostLabel));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, SharedWriter);
            nextInstruction.labels.Add(HostLabel);
            newInstructions.Add(nextInstruction);
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.CreatureChangeQliphoth))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static IEnumerable<CodeInstruction> AddQliphothCountTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>();
            MethodInfo multiplayerControllerInstance = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.instance)).GetGetMethod();
            MethodInfo SharedWriter = AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.SharedWriter)).GetGetMethod();
            Label? labelToAttach = generator.DefineLabel();
            Label HostLabel = generator.DefineLabel();
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.IsConnected)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brfalse, labelToAttach));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(MultiplayerController), nameof(MultiplayerController.Host)).GetGetMethod()));
            newInstructions.Add(new CodeInstruction(OpCodes.Brtrue, HostLabel));
            newInstructions.Add(new CodeInstruction(OpCodes.Ret));
            CodeInstruction nextInstruction = new CodeInstruction(OpCodes.Call, SharedWriter);
            nextInstruction.labels.Add(HostLabel);
            newInstructions.Add(nextInstruction);
            newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_1));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SerializePacket), nameof(SerializePacket.CreatureChangeQliphoth))));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, multiplayerControllerInstance));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, SharedWriter));
            newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
            newInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MultiplayerController), nameof(MultiplayerController.SendPacket))));
            foreach (CodeInstruction instruction in instructions)
            {
                if (labelToAttach != null)
                {
                    instruction.labels.Add(labelToAttach.Value);
                    labelToAttach = null;
                }
                newInstructions.Add(instruction);
            }
            return newInstructions;
        }
        public static void CreatureChangeQliphoth(CreatureModel creature, bool AddOrSub)
        {
            if (AddOrSub && qliphothCounterGetter(creature) < creature.script.GetQliphothCounterMax()) 
            { 
                qliphothCounterSetter(creature, qliphothCounterGetter(creature) + 1);
                creature.script.AddedQliphothCounter();
                updateQliphothCounterInvoker(creature);
            }
            else if (qliphothCounterGetter(creature) > 0)
            { 
                qliphothCounterSetter(creature, qliphothCounterGetter(creature) - 1);
                creature.script.ReducedQliphothCounter();
                updateQliphothCounterInvoker(creature);
            }
        }
    }
    public static class IlReflector
    {
        public delegate DeployAgentSlot GetSlotInvoker(DeployAgentList target, AgentModel agent);
        public delegate void SetListInvoker(DeployAgentList target);
        public delegate void UpdateGameSpeedInvoker(GameManager target);
        public delegate void EscapeOpenInvoker(EscapeUI target);
        public delegate void EscapeCloseInvoker(EscapeUI target);
        public delegate void UpgradeInvoker(ResearchWindow target, ResearchItemModel model);
        public delegate void ObserveLevelChangedInvoker(CreatureInfoWindow target, int level);
        public delegate void SetCostInvoker(CreatureInfoWindow target);
        public delegate void UpdateQliphothCounterInvoker(CreatureModel target);
        public static Func<TTarget, TValue> CreateGetter<TTarget, TValue>(string fieldName)
        {
            Type type = typeof(TTarget);
            FieldInfo field = type.GetField(fieldName, AccessTools.all) ?? throw new ArgumentException($"Поле '{fieldName}' не найдено в классе {type.Name}");
            DynamicMethod method = new DynamicMethod($"Get_{fieldName}", typeof(TValue), new[] { typeof(TTarget) }, type, true);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            il.Emit(OpCodes.Ret);
            return (Func<TTarget, TValue>)method.CreateDelegate(typeof(Func<TTarget, TValue>));
        }
        public static Action<TTarget, TValue> CreateSetter<TTarget, TValue>(string fieldName)
        {
            Type type = typeof(TTarget);
            FieldInfo field = AccessTools.Field(type, fieldName) ?? throw new ArgumentException($"Поле '{fieldName}' не найдено в классе {type.Name}");
            DynamicMethod method = new DynamicMethod($"Set_{fieldName}", null, new[] { typeof(TTarget), typeof(TValue) }, type, true);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
            return (Action<TTarget, TValue>)method.CreateDelegate(typeof(Action<TTarget, TValue>));
        }
        public static GetSlotInvoker CreateGetSlotInvoker()
        {
            Type target = typeof(DeployAgentList);
            MethodInfo targetMethod = AccessTools.Method(typeof(DeployAgentList), "GetSlot");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_GetSlot", typeof(DeployAgentSlot), new Type[] { target, typeof(AgentModel) }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (GetSlotInvoker)dynamicMethod.CreateDelegate(typeof(GetSlotInvoker));
        }
        public static SetListInvoker CreateSetListInvoker()
        {
            Type target = typeof(DeployAgentList);
            MethodInfo targetMethod = AccessTools.Method(typeof(DeployAgentList), "SetList");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_SetList", typeof(void), new Type[] { target }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (SetListInvoker)dynamicMethod.CreateDelegate(typeof(SetListInvoker));
        }
        public static UpdateGameSpeedInvoker CreateUpdateGameSpeedInvoker()
        {
            Type target = typeof(GameManager);
            MethodInfo targetMethod = AccessTools.Method(typeof(GameManager), "UpdateGameSpeed");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_UpdateGameSpeed", typeof(void), new Type[] { target }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (UpdateGameSpeedInvoker)dynamicMethod.CreateDelegate(typeof(UpdateGameSpeedInvoker));
        }
        public static EscapeOpenInvoker CreateEscapeOpenInvoker()
        {
            Type target = typeof(EscapeUI);
            MethodInfo targetMethod = AccessTools.Method(typeof(EscapeUI), "OnOpenWindow");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_OnOpenWindow", typeof(void), new Type[] { target }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (EscapeOpenInvoker)dynamicMethod.CreateDelegate(typeof(EscapeOpenInvoker));
        }
        public static EscapeCloseInvoker CreateEscapeCloseInvoker()
        {
            Type target = typeof(EscapeUI);
            MethodInfo targetMethod = AccessTools.Method(typeof(EscapeUI), "OnCloseWindow");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_OnCloseWindow", typeof(void), new Type[] { target }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (EscapeCloseInvoker)dynamicMethod.CreateDelegate(typeof(EscapeCloseInvoker));
        }
        public static UpgradeInvoker CreateUpgradeInvoker()
        {
            Type target = typeof(ResearchWindow);
            MethodInfo targetMethod = AccessTools.Method(typeof(ResearchWindow), "Upgrade");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_Upgrade", typeof(void), new Type[] { target, typeof(ResearchItemModel) }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (UpgradeInvoker)dynamicMethod.CreateDelegate(typeof(UpgradeInvoker));
        }
        public static ObserveLevelChangedInvoker CreateObserveLevelChangedInvoker()
        {
            Type target = typeof(CreatureInfoWindow);
            MethodInfo targetMethod = AccessTools.Method(typeof(CreatureInfoWindow), "OnObserveLevelChanged");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_OnObserveLevelChanged", typeof(void), new Type[] { target, typeof(int) }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (ObserveLevelChangedInvoker)dynamicMethod.CreateDelegate(typeof(ObserveLevelChangedInvoker));
        }
        public static SetCostInvoker CreateSetCostInvoker()
        {
            Type target = typeof(CreatureInfoWindow);
            MethodInfo targetMethod = AccessTools.Method(typeof(CreatureInfoWindow), "SetCost");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_SetCost", typeof(void), new Type[] { target }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (SetCostInvoker)dynamicMethod.CreateDelegate(typeof(SetCostInvoker));
        }
        public static UpdateQliphothCounterInvoker CreateUpdateQliphothCounterInvoker()
        {
            Type target = typeof(CreatureModel);
            MethodInfo targetMethod = AccessTools.Method(typeof(CreatureModel), "UpdateQliphothCounter");
            DynamicMethod dynamicMethod = new DynamicMethod("Invoke_UpdateQliphothCounter", typeof(void), new Type[] { target }, target, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);
            return (UpdateQliphothCounterInvoker)dynamicMethod.CreateDelegate(typeof(UpdateQliphothCounterInvoker));
        }
    }
    public struct InitOfficers
    {
        public Sefira currSefira;
        public List<OfficerData> officers;
        public void OfficerToData(OfficerModel officer, MapNode mapNode)
        {
            officers.Add(new OfficerData
            {
                id = officer.instanceId,
                name = officer.name,
                MaxHp = officer.maxHp,
                MaxMental = officer.baseMaxMental,
                baseMovement = officer.baseMovement,
                setData = officer.spriteData.saveData,
                currNodeId = officer.GetCurrentNode().GetId(),
                nodeId = mapNode.GetId()
            });
        }
        public struct OfficerData
        {
            public long id;
            public string name;
            public int MaxHp;
            public int MaxMental;
            public float baseMovement;
            public WorkerSpriteSaveData setData;
            public string currNodeId;
            public string nodeId;
        }
        public InitOfficers(Sefira sefira)
        {
            currSefira = sefira;
            officers = new List<OfficerData>();
        }
    }
}
