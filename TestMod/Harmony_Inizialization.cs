using CreatureInfo;
using Customizing;
using Harmony;
using Inventory;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TestMod
{
    public class Harmony_Patch
    {
        //Применение патчей и изменение базовых настроек библиотеки SharpZipLib
        public Harmony_Patch()
        {
            try
            {
                LoadText.Init();
                SaveHandler.SetOldSave();
                ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
                HarmonyInstance harmonyInstance = HarmonyInstance.Create("Lobotomy.bleba.Multiplayer");
                harmonyInstance.Patch(typeof(AlterTitleController).GetMethod("Start", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.AddAlterUITranspiler)));
                harmonyInstance.Patch(typeof(NewTitleScript).GetMethod("Start", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.AddNewUITranspiler)));
                harmonyInstance.Patch(typeof(DeployUI).GetMethod("OnClickStartGame", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickStartGameTranspiler)));
                harmonyInstance.Patch(typeof(EscapeUI).GetMethod("OnClickButton", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnEscapeClickButtonTranspiler)));
                harmonyInstance.Patch(typeof(EscapeUI).GetMethod("RestartAtCheckpoint", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnRestartTranspiler)));
                harmonyInstance.Patch(typeof(GameSceneController).GetMethod("OnClickRestart", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnRestartTranspiler)));
                harmonyInstance.Patch(typeof(GameSceneController).GetMethod("OnClickNextDay", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickNextDayTranspiler)));
                harmonyInstance.Patch(typeof(ResultScreen.ReportButton).GetMethod("OnPointerClick", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnMoveNextStageTranspiler)));
                harmonyInstance.Patch(typeof(CustomizingWindow).GetMethod("Confirm", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.ConfirmTranspiler)));
                harmonyInstance.Patch(typeof(InventoryUI).GetMethod("OnClickEquipment", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickEquipmentTranspiler)));
                harmonyInstance.Patch(typeof(SefiraPanel).GetMethod("AddAgent", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.AddAgentTranspiler)));
                harmonyInstance.Patch(typeof(SefiraPanel).GetMethod("InitialAddAgent", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.InitialAddAgentTranspiler)));
                harmonyInstance.Patch(typeof(SefiraPanel).GetMethod("OnRemoveAgent", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnRemoveAgentTranspiler)));
                harmonyInstance.Patch(typeof(DeployAgentList).GetMethod("DeployAgent", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.DeployAgentTranspiler)));
                harmonyInstance.Patch(typeof(Sefira).GetMethod("InitOfficerGroup", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.InitOfficerGroupTranspiler)));
                harmonyInstance.Patch(typeof(WorkerModel).GetMethod("MoveToMovable", AccessTools.all, null, new Type[] { typeof(MovableObjectNode), typeof(bool) }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.WorkerMoveToMovableTranspiler)));
                harmonyInstance.Patch(typeof(WorkerModel).GetMethod("MoveToNode", AccessTools.all, null, new Type[] { typeof(string) }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.WorkerMoveToNodeByIdTranspiler)));
                harmonyInstance.Patch(typeof(WorkerModel).GetMethod("MoveToNode", AccessTools.all, null, new Type[] { typeof(MapNode), typeof(bool) }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.WorkerMoveToNodeByLinkTranspiler)));
                harmonyInstance.Patch(typeof(CommandWindow.CommandWindow).GetMethod("OnClick", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickTranspiler)));
                harmonyInstance.Patch(typeof(AgentModel).GetMethod("ForcelyCancelWork", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.ForcelyCancelWorkTranspiler)));
                harmonyInstance.Patch(typeof(UseSkill).GetMethod("ProgressWork", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.ProgressWorkTranspiler)));
                harmonyInstance.Patch(typeof(GameManager).GetMethod("Pause", AccessTools.all, null, new Type[] { }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.PauseTranspiler)));
                harmonyInstance.Patch(typeof(GameManager).GetMethod("Resume", AccessTools.all, null, new Type[] { }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.ResumeTranspiler)));
                harmonyInstance.Patch(typeof(GameManager).GetMethod("SetPlaySpeed", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.SetPlaySpeedTranspiler)));
                harmonyInstance.Patch(typeof(EscapeUI).GetMethod("OpenWindow", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.EscapeTranspiler)));
                harmonyInstance.Patch(typeof(ManualUI).GetMethod("OpenManual", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.EscapeTranspiler)));
                harmonyInstance.Patch(typeof(EscapeUI).GetMethod("CloseWindow", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OutEscapeTranspiler)));
                harmonyInstance.Patch(typeof(OptionUI).GetMethod("Close", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OutOptionOrManualTranspiler)));
                harmonyInstance.Patch(typeof(ManualUI).GetMethod("CloseManual", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OutOptionOrManualTranspiler)));
                harmonyInstance.Patch(typeof(ResearchWindow).GetMethod("OnDropEvent", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnResearchSelectTranspiler)));
                harmonyInstance.Patch(typeof(ResearchWindow).GetMethod("OnConfirm", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.ConfirmResearchTranspiler)));
                harmonyInstance.Patch(typeof(ResearchWindow).GetMethod("OnDiscard", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.DiscardResearchTranspiler)));
                harmonyInstance.Patch(typeof(WorkerModel).GetMethod("TakeDamage", AccessTools.all, null, new Type[] { typeof(UnitModel), typeof(DamageInfo) }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.TakeDamageWorkerTranspiler)));
                harmonyInstance.Patch(typeof(WorkerModel).GetMethod("TakeDamageWithoutEffect", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.TakeDamageWithoutEffectWorkerTranspiler)));
                harmonyInstance.Patch(typeof(CreatureInfoWindow).GetMethod("OnTryPurchase", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnTryPurchaseTranspiler)));
                harmonyInstance.Patch(typeof(CreatureInfoEquipmentRoot).GetMethod("OnClickArmor", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickCreateEquipTranspiler)));
                harmonyInstance.Patch(typeof(CreatureInfoEquipmentRoot).GetMethod("OnClickWeapon", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickCreateEquipTranspiler)));
                harmonyInstance.Patch(typeof(UnitModel).GetMethod("AttachEGOgift", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.AttachEGOgiftTranspiler)));
                harmonyInstance.Patch(typeof(CreatureOverloadManager).GetMethod("ActivateOverload", AccessTools.all, null, new Type[] { typeof(int), typeof(OverloadType), typeof(float), typeof(bool), typeof(bool), typeof(bool), typeof(long[]) }, null), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.ActivateOverloadTranspiler)));
                harmonyInstance.Patch(typeof(CreatureModel).GetMethod("MoveToNode", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.CreatureToNodeTranspiler)));
                harmonyInstance.Patch(typeof(CreatureModel).GetMethod("MoveToMovable", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.CreatureToMovableTranspiler)));
                harmonyInstance.Patch(typeof(AgentModel).GetMethod("SetWaitingPassage", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.SetWaitingPassageTranspiler)));
                harmonyInstance.Patch(typeof(AgentModel).GetMethod("Suppress", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.SuppressTranspiler)));
                harmonyInstance.Patch(typeof(CreatureSelectUI).GetMethod("OnClickUnit", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnClickUnitTranspiler)));
                harmonyInstance.Patch(typeof(CreatureSelectUI).GetMethod("Init", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.InitSelectUITranspiler)));
                harmonyInstance.Patch(typeof(ExpandSefiraPanel).GetMethod("OnClick", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.SelectToExpandTranspiler)));
                harmonyInstance.Patch(typeof(ExpandUI).GetMethod("OnClickConfirm", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnExpandTranspiler)));
                harmonyInstance.Patch(typeof(ExpandUI).GetMethod("OnClickCancel", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.OnCancelExpandTranspiler)));
                harmonyInstance.Patch(typeof(AgentModel).GetMethod("UpdatePrimaryStat", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.UpdatePrimaryStatTranspiler)));
                harmonyInstance.Patch(typeof(AgentModel).GetMethod("UpdateBestRwbp", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.UpdateBestRwbpTranspiler)));
                harmonyInstance.Patch(typeof(CreatureModel).GetMethod("TakeDamage", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.CreatureTakeDamageTranspiler)));
                harmonyInstance.Patch(typeof(CreatureModel).GetMethod("SubQliphothCounter", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.SubQliphothCountTranspiler)));
                harmonyInstance.Patch(typeof(CreatureModel).GetMethod("AddQliphothCounter", AccessTools.all), null, null, new HarmonyMethod(typeof(GameDataHandler), nameof(GameDataHandler.AddQliphothCountTranspiler)));
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", ex.ToString());
            }
        }
    }
}