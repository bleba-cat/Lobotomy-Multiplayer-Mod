using TestMod.PacketConvertation;
using CreatureInfo;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestMod
{
    public class PacketHandler
    {
        public static void Handler(NetPacketReader reader)
        {
            PacketType type = (PacketType)reader.GetInt();
            try
            {
                switch (type)
                {
                    case PacketType.Message:
                        {
                            string message = reader.GetString();
                            switch (message)
                            {
                                case "StartGame":
                                    {
                                        MultiplayerWindowController.instance.CloseMenu();
                                        if (AlterTitleController.Controller != null)
                                        {
                                            AlterTitleController.Controller.OnClickButton(2);
                                            break;
                                        }
                                        NewTitleScript.instance?.OnClickContinue();
                                        break;
                                    }
                                case "Restart":
                                    {
                                        GameManager.currentGameManager?.RestartGame();
                                        break;
                                    }
                                case "OpenTitle":
                                    {
                                        if (!MultiplayerController.instance.Host) SaveHandler.SetOldSave();
                                        if (GameManager.currentGameManager != null)
                                        {
                                            GameManager.currentGameManager.ReturnToTitle();
                                        }
                                        else
                                        {
                                            GlobalGameManager.instance.StoryReturnTitle();
                                        }
                                        MultiplayerWindowController.instance.OpenMenu();
                                        break;
                                    }
                                case "ToCheckpoint":
                                    {
                                        GameManager.currentGameManager?.ReturnToCheckPoint();
                                        break;
                                    }
                                case "OnStartClick":
                                    {
                                        GameDataHandler.OnClickStartGame(DeployUI.instance);
                                        Inventory.InventoryUI.CurrentWindow.CloseWindow();
                                        CreatureInfoWindow.CurrentWindow.CloseWindow();
                                        break;
                                    }
                                case "NextDay":
                                    {
                                        GameManager.currentGameManager.ClearStage();
                                        break;
                                    }
                                case "NextStage":
                                    {
                                        if (ResultScreen.instance.root.activeInHierarchy)
                                        {
                                            GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
                                            GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
                                        }
                                        ResultScreen.instance.report.GetButton(3).ScoreSound.OnPlayInList(3);
                                        Notice.instance.Send(NoticeName.OnClickNextDayAcceptInResult, new object[0]);
                                        if (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL)
                                        {
                                            ResultScreen.instance.nextScneAnim.transform.parent.gameObject.SetActive(true);
                                            ResultScreen.instance.nextScneAnim.SetTrigger("Start_Main");
                                        }
                                        break;
                                    }
                                case "esc":
                                    {
                                        if (EscapeUI.instance.IsOpened || ManualUI.Instance.IsActivated) break;
                                        GameDataHandler.OpenEsc(EscapeUI.instance);
                                        string SceneName = SceneManager.GetActiveScene().name;
                                        if (SceneName == "Main" && DeployUI.instance.IsGameStarted)
                                        {
                                            CameraMover.instance.StopMove();
                                            GameDataHandler.OnPause(PlaySpeedSettingUI.instance, PAUSECALL.ESCAPE);
                                        }
                                        break;
                                    }
                                case "outesc":
                                    {
                                        string SceneName = SceneManager.GetActiveScene().name;
                                        if (SceneName == "Main" && DeployUI.instance.IsGameStarted && EscapeUI.instance.IsOpened)
                                        {
                                            GameDataHandler.CloseEsc(EscapeUI.instance);
                                            CameraMover.instance.ReleaseMove();
                                            GameDataHandler.OnResume(PlaySpeedSettingUI.instance, PAUSECALL.ESCAPE);
                                            break;
                                        }
                                        else if (EscapeUI.instance.IsOpened) GameDataHandler.CloseEsc(EscapeUI.instance);
                                        if (OptionUI.Instance.IsEnabled) GameDataHandler.CloseOptions(OptionUI.Instance);
                                        if (ManualUI.Instance.IsActivated) GameDataHandler.CloseManual(ManualUI.Instance);
                                        break;
                                    }
                                case "DiscardResearch":
                                    {
                                        GameDataHandler.DiscardResearch(DeployUI.instance.researchWindow);
                                        break;
                                    }
                                case "Expand":
                                    {
                                        GameDataHandler.Expand(ExpandUI.instance);
                                        break;
                                    }
                                case "CancelExpand":
                                    {
                                        GameDataHandler.CancelExpand(ExpandUI.instance);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PacketType.RoomCode:
                        {
                            string roomcode = reader.GetString();
                            MultiplayerController.instance.roomCode = roomcode;
                            MultiplayerWindowController.instance.CreateCodeField.text = roomcode;
                            break;
                        }
                    case PacketType.PlayerList:
                        {
                            List<Player> players = MultiplayerController.instance.playerList;
                            players.Clear();
                            byte count = reader.GetByte();
                            for (byte i = 0; i < count; i++)
                            {
                                string nickname = reader.GetString();
                                bool IsHost = reader.GetBool();
                                bool IsConfirmed = reader.GetBool();
                                bool perm1 = reader.GetBool();
                                players.Add(new Player
                                {
                                    Nickname = nickname,
                                    IsHost = IsHost,
                                    IsConfirmed = IsConfirmed,
                                    Permissions = new PlayerPermissions
                                    {
                                        perm1 = perm1
                                    }
                                });
                            }
                            PlayerList.instance.CreateNewPlayerList(players, !MultiplayerController.instance.Host);
                            break;
                        }
                    case PacketType.ConfirmUser:
                        {
                            string nickname = reader.GetString();
                            MultiplayerController.instance.playerList.Find(p => p.Nickname == nickname).IsConfirmed = true;
                            PlayerList.instance.CreateNewPlayerList(MultiplayerController.instance.playerList, !MultiplayerController.instance.Host);
                            break;
                        }
                    case PacketType.KickUser:
                        {
                            string nickname = reader.GetString();
                            Player player = MultiplayerController.instance.playerList.Find(p => p.Nickname == nickname);
                            MultiplayerController.instance.playerList.Remove(player);
                            PlayerList.instance.CreateNewPlayerList(MultiplayerController.instance.playerList, !MultiplayerController.instance.Host);
                            break;
                        }
                    case PacketType.Save:
                        {
                            if (!MultiplayerController.instance.Host)
                            {
                                if (reader.AvailableBytes == 0) return;
                                byte[] save = new byte[reader.AvailableBytes];
                                for (int i = 0; i < save.Length; i++)
                                {
                                    save[i] = reader.GetByte();
                                }
                                SaveHandler.SetSave(save);
                                SerializePacket.Message(MultiplayerController.SharedWriter, "Ready");
                                MultiplayerController.instance.SendPacket(MultiplayerController.SharedWriter, 0);
                            }
                            break;
                        }
                    case PacketType.ChatMessage:
                        {
                            ChatController.instance.AddMessage(reader.GetString(), reader.GetString());
                            break;
                        }
                    case PacketType.NewAgent:
                        {
                            var (data, prefixId, suffixId, cost) = DeserializePacket.NewAgent(reader);
                            GameDataHandler.SetBuyedAgent(data, prefixId, suffixId, cost);
                            break;
                        }
                    case PacketType.ChangeAgent:
                        {
                            var (agent, stats, prefixId, suffixId, cost) = DeserializePacket.ChangeAgent(reader);
                            GameDataHandler.SetBonus(agent, stats, prefixId, suffixId, cost);
                            break;
                        }
                    case PacketType.EquipEGO:
                        {
                            long agentId = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(agentId) ?? AgentManager.instance.GetSpareAgent(agentId);
                            EquipmentModel equip = InventoryModel.Instance.GetEquipment(reader.GetLong());
                            GameDataHandler.OnEquip(agent, equip);
                            break;
                        }
                    case PacketType.AddAgent:
                        {
                            SefiraPanel panel = DeployUI.instance.sefiraList.sefiraPanels.Find(p => p.targetSefira == (SefiraEnum)reader.GetByte()).script;
                            long Id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(Id) ?? AgentManager.instance.GetSpareAgent(Id);
                            GameDataHandler.AddAgent(panel, agent, DeserializePacket.GetMapNode(reader));
                            break;
                        }
                    case PacketType.InitAgentAdd:
                        {
                            long id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id);
                            agent.GetMovableNode().SetCurrentNode(DeserializePacket.GetMapNode(reader));
                            break;
                        }
                    case PacketType.RemoveAgent:
                        {
                            SefiraPanel panel = DeployUI.instance.sefiraList.sefiraPanels.Find(p => p.targetSefira == (SefiraEnum)reader.GetByte()).script;
                            GameDataHandler.RemoveAgent(panel, reader.GetInt());
                            break;
                        }
                    case PacketType.DeployAgent:
                        {
                            long Id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(Id) ?? AgentManager.instance.GetSpareAgent(Id);
                            GameDataHandler.DeployAgent(agent);
                            break;
                        }
                    case PacketType.InitOfficers:
                        {
                            InitOfficers officers = DeserializePacket.InitOfficers(reader);
                            GameDataHandler.AddOfficers(officers.currSefira, officers.officers);
                            break;
                        }
                    case PacketType.WorkerMoveToMovable:
                        {
                            long id = reader.GetLong();
                            WorkerModel worker = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id) ?? (WorkerModel)OfficerManager.instance.GetOfficerList().First(o => o.instanceId == id);
                            GameDataHandler.WorkerMoveToNode(worker, reader.GetMovableNode(), reader.GetBool());
                            break;
                        }
                    case PacketType.WorkerMoveById:
                        {
                            long id = reader.GetLong();
                            WorkerModel worker = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id) ?? (WorkerModel)OfficerManager.instance.GetOfficerList().FirstOrDefault(o => o.instanceId == id);
                            if (worker == null) break;
                            GameDataHandler.WorkerMoveToNode(worker, reader.GetString());
                            break;
                        } 
                    case PacketType.WorkerMoveByLink:
                        {
                            long id = reader.GetLong();
                            WorkerModel worker = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id) ?? (WorkerModel)OfficerManager.instance.GetOfficerList().First(o => o.instanceId == id);
                            GameDataHandler.WorkerMoveToNode(worker, DeserializePacket.GetMapNode(reader), reader.GetBool());
                            break;
                        }
                    case PacketType.CancelWork:
                        {
                            long Id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(Id) ?? AgentManager.instance.GetSpareAgent(Id);
                            GameDataHandler.ForcellyCancelWork(agent);
                            break;
                        }
                    case PacketType.CancelSuppress:
                        {
                            long Id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(Id) ?? AgentManager.instance.GetSpareAgent(Id);
                            agent.ForcelyCancelSuppress();
                            break;
                        }
                    case PacketType.StartWorkCreature:
                        {
                            long agentId = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(agentId) ?? AgentManager.instance.GetSpareAgent(agentId);
                            CreatureModel creature = CreatureManager.instance.GetCreature(reader.GetLong());
                            long WorkId = reader.GetLong();
                            if (agent.GetState() == AgentAIState.MANAGE)
                            {
                                if (creature == agent.target)
                                {
                                    if (agent.currentSkill == null)
                                    {
                                        agent.ForcelyCancelWork();
                                    }
                                    break;
                                }
                                if (agent.currentSkill == null)
                                {
                                    agent.ForcelyCancelWork();
                                }
                                else
                                {
                                    agent.StopAction();
                                }
                            }
                            SkillTypeInfo data = SkillTypeList.instance.GetData(WorkId);
                            agent.ManageCreature(creature, data, CommandWindow.CommandWindow.CurrentWindow.GetWorkSprite((RwbpType)WorkId));
                            agent.counterAttackEnabled = false;
                            creature.Unit.room.OnWorkAllocated(agent);
                            creature.script.OnWorkAllocated(data, agent);
                            AngelaConversation.instance.MakeMessage(AngelaMessageState.MANAGE_START, new object[]{
                                                agent,
                                                data,
                                                creature
                            });
                            break;
                        }
                    case PacketType.StartWorkKit:
                        {
                            long agentId = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(agentId) ?? AgentManager.instance.GetSpareAgent(agentId);
                            CreatureModel creature = CreatureManager.instance.GetCreature(reader.GetLong());
                            if (agent.GetState() == AgentAIState.MANAGE)
                            {
                                if (creature == agent.target)
                                {
                                    if (agent.currentSkill == null)
                                    {
                                        agent.ForcelyCancelWork();
                                    }
                                    return;
                                }
                                if (agent.currentSkill == null)
                                {
                                    agent.ForcelyCancelWork();
                                }
                                else
                                {
                                    agent.StopAction();
                                }
                            }
                            agent.ManageKitCreature(creature);
                            agent.counterAttackEnabled = false;
                            creature.Unit.room.OnWorkAllocated(agent);
                            break;
                        }
                    case PacketType.ProgressWorkResult:
                        {
                            long id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id);
                            GameDataHandler.ProgressWork(agent.currentSkill, reader.GetBool());
                            break;
                        }
                    case PacketType.Pause:
                        {
                            GameDataHandler.Pause(GameManager.currentGameManager, (PAUSECALL)reader.GetByte());
                            PlaySpeedSettingUI.instance.UpdateButton();
                            break;
                        }
                    case PacketType.Resume:
                        {
                            GameDataHandler.Resume(GameManager.currentGameManager, (PAUSECALL)reader.GetByte());
                            PlaySpeedSettingUI.instance.UpdateButton();
                            break;
                        }
                    case PacketType.Speed:
                        {
                            GameDataHandler.SetPlaySpeed(GameManager.currentGameManager, reader.GetInt());
                            PlaySpeedSettingUI.instance.UpdateButton();
                            break;
                        }
                    case PacketType.SelectResearch:
                        {
                            ResearchItemModel research = ResearchDataModel.instance.GetModel(reader.GetInt());
                            if (research != null)
                            {
                                DeployUI.instance.researchWindow.selectedPanel.gameObject.SetActive(true);
                                DeployUI.instance.researchWindow.selectedPanel.SetData(research);
                                DeployUI.instance.researchWindow.OnSetPanel(true);
                                DeployUI.instance.researchWindow.panels.Find(p => p.CurrentModel == research).ActiveControl.SetActive(false);
                            }
                            break;
                        }
                    case PacketType.ConfirmResearch:
                        {
                            ResearchItemModel research = ResearchDataModel.instance.GetModel(reader.GetInt());
                            if (research != null)
                            {
                                DeployUI.instance.researchWindow.selectedPanel.gameObject.SetActive(true);
                                DeployUI.instance.researchWindow.selectedPanel.SetData(research);
                                DeployUI.instance.researchWindow.OnSetPanel(true);
                            }
                            GameDataHandler.ConfirmResearch(DeployUI.instance.researchWindow);
                            break;
                        }
                    case PacketType.WorkerDamage:
                        {
                            long workerId = reader.GetLong();
                            WorkerModel worker = AgentManager.instance.GetAgent(workerId) ?? AgentManager.instance.GetSpareAgent(workerId) ?? (WorkerModel)OfficerManager.instance.GetOfficerList().FirstOrDefault(o => o.instanceId == workerId);
                            long actorId = reader.GetLong();
                            UnitModel actor = AgentManager.instance.GetAgent(actorId) ?? AgentManager.instance.GetSpareAgent(actorId) ?? (UnitModel)OfficerManager.instance.GetOfficerList().FirstOrDefault(o => o.instanceId == actorId) ?? CreatureManager.instance.GetCreature(actorId);
                            GameDataHandler.ApplyWorkerDamage(worker, actor, reader.GetFloat(), reader.GetBool(), reader.GetBool(), reader.GetBool(), (RwbpType)reader.GetByte(), reader.GetString());
                            break;
                        }
                    case PacketType.WorkerDamageNoEffect:
                        {
                            long workerId = reader.GetLong();
                            WorkerModel worker = AgentManager.instance.GetAgent(workerId) ?? AgentManager.instance.GetSpareAgent(workerId) ?? (WorkerModel)OfficerManager.instance.GetOfficerList().FirstOrDefault(o => o.instanceId == workerId);
                            long actorId = reader.GetLong();
                            UnitModel actor = AgentManager.instance.GetAgent(actorId) ?? AgentManager.instance.GetSpareAgent(actorId) ?? (UnitModel)OfficerManager.instance.GetOfficerList().FirstOrDefault(o => o.instanceId == actorId) ?? CreatureManager.instance.GetCreature(actorId);
                            if (worker == null) break;
                            GameDataHandler.ApplyWorkerDamageNoEffect(worker, actor, reader.GetFloat(), reader.GetBool(), reader.GetBool(), (RwbpType)reader.GetByte(), reader.GetString());
                            break;
                        }
                    case PacketType.CreatureObserve:
                        {
                            long creatureId = reader.GetLong();
                            long modelId = reader.GetLong();
                            CreatureModel creature = CreatureManager.instance.GetCreature(creatureId);
                            CreatureObserveInfoModel model = CreatureManager.instance.GetObserveInfo(modelId);
                            string areaName = reader.GetString();
                            int oldLevel = reader.GetInt();
                            if (CreatureInfoWindow.CurrentWindow.CurrentModel == creature && CreatureInfoWindow.CurrentWindow.IsEnabled)
                            {
                                CreatureInfoController controller = null;
                                CreatureInfoOpenArea OpenArea = null;
                                switch (areaName)
                                {
                                    case "stat":
                                        {
                                            controller = CreatureInfoWindow.CurrentWindow.statRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.statRoot.Open;
                                            break;
                                        }
                                    case "work_r":
                                        {
                                            controller = CreatureInfoWindow.CurrentWindow.workRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.workSlots[0].Open;
                                            break;
                                        }
                                    case "work_w":
                                        {
                                            controller = CreatureInfoWindow.CurrentWindow.workRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.workSlots[1].Open;
                                            break;
                                        }
                                    case "work_b":
                                        {
                                            controller = CreatureInfoWindow.CurrentWindow.workRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.workSlots[2].Open;
                                            break;
                                        }
                                    case "work_p":
                                        {
                                            controller = CreatureInfoWindow.CurrentWindow.workRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.workSlots[3].Open;
                                            break;
                                        }
                                    case "defense":
                                        {
                                            controller = CreatureInfoWindow.CurrentWindow.escapeRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.escapeRoot.Open;
                                            break;
                                        }
                                    default:
                                        {
                                            if (!areaName.StartsWith("care_")) break;
                                            if (!int.TryParse(areaName.Replace("care_", ""), out int neededArea)) break;
                                            controller = CreatureInfoWindow.CurrentWindow.caretakingRoot;
                                            OpenArea = CreatureInfoWindow.CurrentWindow.caretakingRoot.slots[neededArea].Open;
                                            break;
                                        }
                                }
                                if (creature.TransactionCube(model.GetObserveCost(areaName)))
                                {
                                    OpenArea?.AnimTriggerSet("Open");
                                    CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(7);
                                    OverlayManager.Instance.ClearOverlay();
                                    GameDataHandler.Purchase(CreatureInfoWindow.CurrentWindow, areaName, model, oldLevel, controller);
                                }
                                else
                                {
                                    OpenArea?.AnimTriggerSet("Fail");
                                    CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
                                }
                            }
                            else if (creature.TransactionCube(model.GetObserveCost(areaName)))
                            {
                                model.OnObserveRegion(areaName);
                                if (oldLevel != model.GetObservationLevel()) creature.AddObservationLevel();
                            }
                            break;
                        }
                    case PacketType.CreateEquipment:
                        {
                            int id = reader.GetInt();
                            int cost = reader.GetInt();
                            long creatureId = reader.GetLong();
                            long modelId = reader.GetLong();
                            CreatureModel creature = CreatureManager.instance.GetCreature(creatureId);
                            CreatureObserveInfoModel model = CreatureManager.instance.GetObserveInfo(modelId);
                            if (cost > model.cubeNum)
                            {
                                CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
                                return;
                            }
                            if (!InventoryModel.Instance.CheckEquipmentCount(id))
                            {
                                CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
                                return;
                            }
                            if (CreatureInfoWindow.CurrentWindow.CurrentModel == creature && CreatureInfoWindow.CurrentWindow.IsEnabled)
                            {
                                CreatureInfoWindow.CurrentWindow.PurchaseAnim(cost);
                                EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(id);
                                if (equipmentModel != null)
                                {
                                    model.Transaction(-cost);
                                }
                                CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(2);
                                CreatureInfoWindow.CurrentWindow.equipmentRoot.Initialize();
                            }
                            else
                            {
                                EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(id);
                                if (equipmentModel != null)
                                {
                                    model.Transaction(-cost);
                                }
                            }
                            break;
                        } 
                    case PacketType.GetEGO:
                        {
                            long agentId = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(agentId) ?? AgentManager.instance.GetSpareAgent(agentId);
                            int id = reader.GetInt();
                            EGOgiftModel gift = EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(id));
                            if (UnitEGOgiftSpace.IsUniqueLock(gift.metaInfo.id))
                            {
                                agent.Equipment.gifts.SetLockState(gift, false);
                            }
                            agent.Equipment.gifts.AttachGift(agent, gift);
                            if (agent.hp > agent.maxHp)
                            {
                                agent.hp = agent.maxHp;
                            }
                            if (agent.hp < 1f)
                            {
                                agent.hp = 1f;
                            }
                            if (agent.mental > agent.maxMental)
                            {
                                agent.mental = agent.maxMental;
                            }
                            agent.GetUnit()?.UpdateGiftModel();
                            break;
                        }
                    case PacketType.Overload:
                        {
                            int count = reader.GetInt();
                            List<CreatureModel> creatures = new List<CreatureModel>();
                            for (int i = 0; i < count; i++)
                            {
                                creatures.Add(CreatureManager.instance.GetCreature(reader.GetLong()));
                            }
                            foreach (CreatureModel creature in creatures)
                            {
                                creature.ActivateOverload(CreatureOverloadManager.instance.GetQliphothOverloadLevel(), reader.GetFloat(), (OverloadType)reader.GetByte());
                            }
                            break;
                        }
                    case PacketType.CreatureMoveToNode:
                        {
                            CreatureModel creature = CreatureManager.instance.GetCreature(reader.GetLong());
                            MapNode node = DeserializePacket.GetMapNode(reader);
                            if (node != null) creature.commandQueue.SetAgentCommand(CreatureCommand.MakeMove(node));
                            else if (creature.script is SmallBird) creature.commandQueue.SetAgentCommand(CreatureCommand.MakeMove(creature.GetRoomNode()));
                            else if (node == null) Debug.Log("node null");
                            break;
                        } //
                    case PacketType.CreatureMoveToMovable:
                        {
                            CreatureModel creature = CreatureManager.instance.GetCreature(reader.GetLong());
                            MovableObjectNode node = DeserializePacket.GetMovableNode(reader);
                            creature.commandQueue.SetAgentCommand(CreatureCommand.MakeMove(node));
                            break;
                        } //
                    case PacketType.WaitingPassage:
                        {
                            long Id = reader.GetLong();
                            string passageId = reader.GetString();
                            AgentModel agent = AgentManager.instance.GetAgent(Id) ?? AgentManager.instance.GetSpareAgent(Id);
                            GameDataHandler.SetWaitingPassage(agent, SefiraManager.instance.GetSefira((SefiraEnum)reader.GetByte()).passageList.Find(p => p.GetId() == passageId));
                            agent.StopAction();
                            agent.counterAttackEnabled = false;
                            break;
                        }
                    case PacketType.InitSelectUI:
                        {
                            lock (MultiplayerController.instance.locks.selectUILock)
                            {
                                MultiplayerController.instance.selectUIData = new long[3];
                                MultiplayerController.instance.selectUIData[0] = reader.GetLong();
                                MultiplayerController.instance.selectUIData[1] = reader.GetLong();
                                MultiplayerController.instance.selectUIData[2] = reader.GetLong();
                            }
                            break;
                        }
                    case PacketType.SelectAno:
                        {
                            long id = reader.GetLong();
                            GameDataHandler.OnClickUnit(CreatureSelectUI.instance, CreatureSelectUI.instance.Units.FirstOrDefault(u => u.CreatureID == id));
                            break;
                        }
                    case PacketType.SelectToExpand:
                        {
                            SefiraEnum sefira = (SefiraEnum)reader.GetByte();
                            GameDataHandler.OnSelected(ExpandUI.instance.panels.Find(p => p.sefira == sefira).script);
                            break;
                        }
                    case PacketType.Suppress:
                        {
                            long id = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id);
                            long targetId = reader.GetLong();
                            UnitModel target = CreatureManager.instance.GetCreature(targetId) ?? (UnitModel)OfficerManager.instance.GetOfficerList().First(o => o.instanceId == targetId);
                            GameDataHandler.Suppress(agent, target, reader.GetBool());
                            break;
                        }
                    case PacketType.NewTitles:
                        {
                            long agentId = reader.GetLong();
                            int prefixId = reader.GetInt();
                            int suffixId = reader.GetInt();
                            AgentModel agent = AgentManager.instance.GetAgent(agentId) ?? AgentManager.instance.GetSpareAgent(agentId);
                            GameDataHandler.UpdateAgentTitles(agent, prefixId, suffixId);
                            break;
                        } //
                    case PacketType.BestRwbp:
                        {
                            long agentId = reader.GetLong();
                            AgentModel agent = AgentManager.instance.GetAgent(agentId) ?? AgentManager.instance.GetSpareAgent(agentId);
                            GameDataHandler.UpdateAgentBestRwbp(agent, (RwbpType)reader.GetByte());
                            break;
                        } 
                    case PacketType.CreatureTakeDamage:
                        {
                            CreatureModel creature = CreatureManager.instance.GetCreature(reader.GetLong());
                            long actorId = reader.GetLong();
                            UnitModel actor = AgentManager.instance.GetAgent(actorId) ?? AgentManager.instance.GetSpareAgent(actorId) ?? (UnitModel)OfficerManager.instance.GetOfficerList().FirstOrDefault(o => o.instanceId == actorId) ?? CreatureManager.instance.GetCreature(actorId);
                            GameDataHandler.CreatureTakeDamage(creature, actor, reader.GetFloat(), (RwbpType)reader.GetByte());
                            break;
                        } 
                    case PacketType.CreatureChangeQliphoth:
                        {
                            CreatureModel creature = CreatureManager.instance.GetCreature(reader.GetLong());
                            GameDataHandler.CreatureChangeQliphoth(creature, reader.GetBool());
                            break;
                        } 
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", "\nPacketType " + type + " " + ex.ToString());
            }
            finally
            {
                reader.Recycle();
            }
        }
    }
}
