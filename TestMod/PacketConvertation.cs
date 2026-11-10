using Customizing;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorkerSprite;

namespace TestMod.PacketConvertation
{
    public static class SerializePacket
    {
        //Реализация записи различных игровых данных в NetDataWriter
        public static void Put(this NetDataWriter writer, MapNode node)
        {
            if (node == null)
            {
                writer.Put(false);
                return;
            }
            else writer.Put(true);
            if (node.GetId() != "-1")
            {
                writer.Put(true);
                writer.Put(node.GetId());
            }
            else
            {
                writer.Put(false);
                writer.Put(node.GetPosition().x);
                writer.Put(node.GetPosition().y);
                writer.Put(node.GetPosition().z);
                writer.Put(node.GetAreaName());
                writer.Put(node.activate);
                writer.Put(node.GetEdges().Length);
                foreach (MapEdge edge in node.GetEdges())
                {
                    writer.Put(edge);
                }
            }
        }
        public static void Put(this NetDataWriter writer, MapEdge edge)
        {
            if (edge == null)
            {
                writer.Put(false);
                return;
            }
            writer.Put(true);
            writer.Put(edge.node1);
            writer.Put(edge.node2);
            writer.Put(edge.type);
            writer.Put(edge.cost);
        }
        public static void Put(this NetDataWriter writer, MovableObjectNode node)
        {
            if (node.GetUnit() != null)
            {
                writer.Put(true);
                writer.Put(node.GetUnit().instanceId);
                return;
            }
            writer.Put(false);
            writer.Put(node.currentNode);
            writer.Put(node.currentEdge);
            writer.Put(node.edgePosRate);
            writer.Put((int)node.edgeDirection);
        }
        public static void InitPlayer(NetDataWriter writer, string roomCode, string nickname, bool isHost, bool needConfirm)
        {
            writer.Reset();
            writer.Put((int)PacketType.InitPlayer);
            writer.Put(roomCode);
            writer.Put(nickname);
            writer.Put(isHost);
            writer.Put(needConfirm);
        }
        public static void Message(NetDataWriter writer, string message)
        {
            writer.Reset();
            writer.Put((int)PacketType.Message);
            writer.Put(message);
        }
        public static void ChatMessage(NetDataWriter writer, string sender, string message)
        {
            writer.Reset();
            writer.Put((int)PacketType.ChatMessage);
            writer.Put(sender);
            writer.Put(message);
        }
        public static void KickUser(NetDataWriter writer, string nickname)
        {
            writer.Reset();
            writer.Put((int)PacketType.KickUser);
            writer.Put(nickname);
        }
        public static void ConfirmUser(NetDataWriter writer, string nickname)
        {
            writer.Reset();
            writer.Put((int)PacketType.ConfirmUser);
            writer.Put(nickname);
        }
        public static void PlayerPermissions(NetDataWriter writer, Player player)
        {
            writer.Reset();
            writer.Put((int)PacketType.PlayerPermisions);
            writer.Put(player.Nickname);
            writer.Put(player.Permissions.perm1);
        }
        public static void NewAgent(NetDataWriter writer, AgentData data, WorkerPrimaryStat stats, int prefixId, int suffixId, int cost)
        {
            writer.Reset();
            writer.Put((int)PacketType.NewAgent);
            writer.Put(data.isCustomName);
            if (data.isCustomName) writer.Put(data.CustomName);
            else writer.Put(data.agentName.id);
            writer.Put(data.appearance.spriteSet.saveData.Eye.a);
            writer.Put(data.appearance.spriteSet.saveData.Eye.b);
            writer.Put(data.appearance.spriteSet.saveData.EyeClose.a);
            writer.Put(data.appearance.spriteSet.saveData.EyeClose.b);
            writer.Put(data.appearance.spriteSet.saveData.EyePanic.a);
            writer.Put(data.appearance.spriteSet.saveData.EyePanic.b);
            writer.Put(data.appearance.spriteSet.saveData.EyeDead.a);
            writer.Put(data.appearance.spriteSet.saveData.EyeDead.b);
            writer.Put(data.appearance.spriteSet.saveData.EyeBrow.a);
            writer.Put(data.appearance.spriteSet.saveData.EyeBrow.b);
            writer.Put(data.appearance.spriteSet.saveData.BattleEyeBrow.a);
            writer.Put(data.appearance.spriteSet.saveData.BattleEyeBrow.b);
            writer.Put(data.appearance.spriteSet.saveData.PanicEyeBrow.a);
            writer.Put(data.appearance.spriteSet.saveData.PanicEyeBrow.b);
            writer.Put(data.appearance.spriteSet.saveData.EyeColor.r);
            writer.Put(data.appearance.spriteSet.saveData.EyeColor.g);
            writer.Put(data.appearance.spriteSet.saveData.EyeColor.b);
            writer.Put(data.appearance.spriteSet.saveData.EyeColor.a);
            writer.Put(data.appearance.spriteSet.saveData.Mouth.a);
            writer.Put(data.appearance.spriteSet.saveData.Mouth.b);
            writer.Put(data.appearance.spriteSet.saveData.BattleMouth.a);
            writer.Put(data.appearance.spriteSet.saveData.BattleMouth.b);
            writer.Put(data.appearance.spriteSet.saveData.PanicMouth.a);
            writer.Put(data.appearance.spriteSet.saveData.PanicMouth.b);
            writer.Put(data.appearance.spriteSet.saveData.FrontHair.a);
            writer.Put(data.appearance.spriteSet.saveData.FrontHair.b);
            writer.Put(data.appearance.spriteSet.saveData.RearHair.a);
            writer.Put(data.appearance.spriteSet.saveData.RearHair.b);
            writer.Put(data.appearance.spriteSet.saveData.AttachmentHair.a);
            writer.Put(data.appearance.spriteSet.saveData.AttachmentHair.b);
            writer.Put(data.appearance.spriteSet.saveData.HairColor.r);
            writer.Put(data.appearance.spriteSet.saveData.HairColor.g);
            writer.Put(data.appearance.spriteSet.saveData.HairColor.b);
            writer.Put(data.appearance.spriteSet.saveData.HairColor.a);
            writer.Put(stats.hp);
            writer.Put(stats.mental);
            writer.Put(stats.work);
            writer.Put(stats.battle);
            writer.Put(prefixId);
            writer.Put(suffixId);
            writer.Put(cost);
        }
        public static void ChangeAgent(NetDataWriter writer, AgentModel agent, WorkerPrimaryStat stats, int prefixId, int suffixId, int cost)
        {
            writer.Reset();
            writer.Put((int)PacketType.ChangeAgent);
            writer.Put(agent.instanceId);
            writer.Put(stats.hp);
            writer.Put(stats.mental);
            writer.Put(stats.work);
            writer.Put(stats.battle);
            writer.Put(prefixId);
            writer.Put(suffixId);
            writer.Put(cost);
        }
        public static void EquipEGO(NetDataWriter writer, AgentModel agent, EquipmentModel equipment)
        {
            writer.Reset();
            writer.Put((int)PacketType.EquipEGO);
            writer.Put(agent.instanceId);
            writer.Put(equipment.instanceId);
        }
        public static void AddAgent(NetDataWriter writer, SefiraEnum sefira, AgentModel agent, MapNode node)
        {
            writer.Reset();
            writer.Put((int)PacketType.AddAgent);
            writer.Put((byte)sefira);
            writer.Put(agent.instanceId);
            writer.Put(node);
        }
        public static void InitAgentAdd(NetDataWriter writer, AgentModel agent, MapNode node)
        {
            writer.Reset();
            writer.Put((int)PacketType.InitAgentAdd);
            writer.Put(agent.instanceId);
            writer.Put(node);
        }
        public static void AgentRemove(NetDataWriter writer, SefiraPanel panel, int index)
        {
            writer.Reset();
            writer.Put((int)PacketType.RemoveAgent);
            writer.Put((byte)panel.Sefira.sefiraEnum);
            writer.Put(index);
        }
        public static void DeployAgent(NetDataWriter writer, AgentModel agent)
        {
            writer.Reset();
            writer.Put((int)PacketType.DeployAgent);
            writer.Put(agent.instanceId);
        }
        public static void OfficersInit(NetDataWriter writer, InitOfficers officers)
        {
            writer.Reset();
            writer.Put((int)PacketType.InitOfficers);
            writer.Put((byte)officers.currSefira.sefiraEnum);
            writer.Put(officers.officers.Count);
            foreach (InitOfficers.OfficerData data in officers.officers)
            {
                writer.Put(data.id);
                writer.Put(data.name);
                writer.Put(data.MaxHp);
                writer.Put(data.MaxMental);
                writer.Put(data.baseMovement);
                writer.Put(data.setData.Eye.a);
                writer.Put(data.setData.Eye.b);
                writer.Put(data.setData.EyeClose.a);
                writer.Put(data.setData.EyeClose.b);
                writer.Put(data.setData.EyePanic.a);
                writer.Put(data.setData.EyePanic.b);
                writer.Put(data.setData.EyeDead.a);
                writer.Put(data.setData.EyeDead.b);
                writer.Put(data.setData.EyeBrow.a);
                writer.Put(data.setData.EyeBrow.b);
                writer.Put(data.setData.BattleEyeBrow.a);
                writer.Put(data.setData.BattleEyeBrow.b);
                writer.Put(data.setData.PanicEyeBrow.a);
                writer.Put(data.setData.PanicEyeBrow.b);
                writer.Put(data.setData.EyeColor.r);
                writer.Put(data.setData.EyeColor.g);
                writer.Put(data.setData.EyeColor.b);
                writer.Put(data.setData.EyeColor.a);
                writer.Put(data.setData.Mouth.a);
                writer.Put(data.setData.Mouth.b);
                writer.Put(data.setData.BattleMouth.a);
                writer.Put(data.setData.BattleMouth.b);
                writer.Put(data.setData.PanicMouth.a);
                writer.Put(data.setData.PanicMouth.b);
                writer.Put(data.setData.FrontHair.a);
                writer.Put(data.setData.FrontHair.b);
                writer.Put(data.setData.RearHair.a);
                writer.Put(data.setData.RearHair.b);
                writer.Put(data.setData.AttachmentHair.a);
                writer.Put(data.setData.AttachmentHair.b);
                writer.Put(data.setData.HairColor.r);
                writer.Put(data.setData.HairColor.g);
                writer.Put(data.setData.HairColor.b);
                writer.Put(data.setData.HairColor.a);
                writer.Put(data.currNodeId);
                writer.Put(data.nodeId);
            }
        }
        public static void WorkerMoveToMovable(NetDataWriter writer, WorkerModel worker, MovableObjectNode targetMovable, bool resetCommand)
        {
            writer.Reset();
            writer.Put((int)PacketType.WorkerMoveToMovable);
            writer.Put(worker.instanceId);
            writer.Put(targetMovable);
            writer.Put(resetCommand);
        }
        public static void WorkerMoveById(NetDataWriter writer, WorkerModel worker, string targetId)
        {
            writer.Reset();
            writer.Put((int)PacketType.WorkerMoveById);
            writer.Put(worker.instanceId);
            writer.Put(targetId);
        }
        public static void WorkerMoveByLink(NetDataWriter writer, WorkerModel worker, MapNode node, bool resetCommand)
        {
            writer.Reset();
            writer.Put((int)PacketType.WorkerMoveByLink);
            writer.Put(worker.instanceId);
            writer.Put(node);
            writer.Put(resetCommand);
        }
        public static void CancelWork(NetDataWriter writer, AgentModel agent)
        {
            writer.Reset();
            writer.Put((int)PacketType.CancelWork);
            writer.Put(agent.instanceId);
        }
        public static void CancelSuppress(NetDataWriter writer, AgentModel agent)
        {
            writer.Reset();
            writer.Put((int)PacketType.CancelSuppress);
            writer.Put(agent.instanceId);
        }
        public static void StartWorkCreature(NetDataWriter writer, AgentModel agent, CreatureModel creature, long WorkId)
        {
            writer.Reset();
            writer.Put((int)PacketType.StartWorkCreature);
            writer.Put(agent.instanceId);
            writer.Put(creature.instanceId);
            writer.Put(WorkId);
        }
        public static void StartWorkKit(NetDataWriter writer, AgentModel agent, CreatureModel creature)
        {
            writer.Reset();
            writer.Put((int)PacketType.StartWorkKit);
            writer.Put(agent.instanceId);
            writer.Put(creature.instanceId);
        }
        public static void ProgressWorkResult(NetDataWriter writer, UseSkill skill, bool success)
        {
            writer.Reset();
            writer.Put((int)PacketType.ProgressWorkResult);
            writer.Put(skill.agent.instanceId);
            writer.Put(success);
        }
        public static void Pause(NetDataWriter writer, PAUSECALL caller)
        {
            writer.Reset();
            writer.Put((int)PacketType.Pause);
            writer.Put((byte)caller);
        }
        public static void Resume(NetDataWriter writer, PAUSECALL caller)
        {
            writer.Reset();
            writer.Put((int)PacketType.Resume);
            writer.Put((byte)caller);
        }
        public static void Speed(NetDataWriter writer, int level)
        {
            writer.Reset();
            writer.Put((int)PacketType.Speed);
            writer.Put(level);
        }
        public static void SelectResearch(NetDataWriter writer, int id)
        {
            writer.Reset();
            writer.Put((int)PacketType.SelectResearch);
            writer.Put(id);
        }
        public static void ConfirmResearch(NetDataWriter writer, int id)
        {
            writer.Reset();
            writer.Put((int)PacketType.ConfirmResearch);
            writer.Put(id);
        }
        public static void WorkerDamageNoEffect(NetDataWriter writer, WorkerModel worker, UnitModel actor, float damage, bool revialHp, bool revialMental, RwbpType type, string specialDeadScene)
        {
            writer.Reset();
            writer.Put((int)PacketType.WorkerDamageNoEffect);
            writer.Put(worker.instanceId);
            writer.Put(actor.instanceId);
            writer.Put(damage);
            writer.Put(revialHp);
            writer.Put(revialMental);
            writer.Put((byte)type);
            writer.Put(specialDeadScene);
        }
        public static void WorkerDamage(NetDataWriter writer, WorkerModel worker, UnitModel actor, float damage, bool makeSpatteredBlood, bool revialHp, bool revialMental, RwbpType type, string specialDeadScene)
        {
            writer.Reset();
            writer.Put((int)PacketType.WorkerDamage);
            writer.Put(worker.instanceId);
            writer.Put(actor.instanceId);
            writer.Put(damage);
            writer.Put(makeSpatteredBlood);
            writer.Put(revialHp);
            writer.Put(revialMental);
            writer.Put((byte)type);
            writer.Put(specialDeadScene);
        }
        public static void CreatureObserve(NetDataWriter writer, CreatureModel creature, CreatureObserveInfoModel model, string areaName, int oldLevel)
        {
            writer.Reset();
            writer.Put((int)PacketType.CreatureObserve);
            writer.Put(creature.instanceId);
            writer.Put(model.creatureTypeId);
            writer.Put(areaName);
            writer.Put(oldLevel);
        }
        public static void CreateEquipment(NetDataWriter writer, int id, int cost, CreatureModel creature, CreatureObserveInfoModel model)
        {
            writer.Reset();
            writer.Put((int)PacketType.CreateEquipment);
            writer.Put(id);
            writer.Put(cost);
            writer.Put(creature.instanceId);
            writer.Put(model.creatureTypeId);
        }
        public static void GetEGO(NetDataWriter writer, UnitModel unit, int id)
        {
            writer.Reset();
            writer.Put((int)PacketType.GetEGO);
            writer.Put(unit.instanceId);
            writer.Put(id);
        }
        public static void OverloadedCreatures(NetDataWriter writer, List<CreatureModel> creatures, float overloadTime, OverloadType type)
        {
            writer.Reset();
            writer.Put((int)PacketType.Overload);
            writer.Put(creatures.Count);
            foreach (CreatureModel creature in creatures)
            {
                writer.Put(creature.instanceId);
            }
            writer.Put(overloadTime);
            writer.Put((byte)type);
        }
        public static void CreatureMoveToNode(NetDataWriter writer, CreatureModel creature, MapNode node)
        {
            writer.Reset();
            writer.Put((int)PacketType.CreatureMoveToNode);
            writer.Put(creature.instanceId);
            writer.Put(node);
        }
        public static void CreatureMoveToMovable(NetDataWriter writer, CreatureModel creature, MovableObjectNode node)
        {
            writer.Reset();
            writer.Put((int)PacketType.CreatureMoveToMovable);
            writer.Put(creature.instanceId);
            writer.Put(node);
        }
        public static void WaitingPassage(NetDataWriter writer, AgentModel agent, PassageObjectModel passage, SefiraEnum sefira)
        {
            writer.Reset();
            writer.Put((int)PacketType.WaitingPassage);
            writer.Put(agent.instanceId);
            writer.Put(passage.GetId());
            writer.Put((byte)sefira);
        }
        public static void Suppress(NetDataWriter writer, AgentModel agent, UnitModel target, bool isAuto)
        {
            writer.Reset();
            writer.Put((int)PacketType.Suppress);
            writer.Put(agent.instanceId);
            writer.Put(target.instanceId);
            writer.Put(isAuto);
        }
        public static void SelectAno(NetDataWriter writer, long id)
        {
            writer.Reset();
            writer.Put((int)PacketType.SelectAno);
            writer.Put(id);
        }
        public static void InitSelectUI(NetDataWriter writer, long id1, long id2, long id3)
        {
            writer.Reset();
            writer.Put((int)PacketType.InitSelectUI);
            writer.Put(id1);
            writer.Put(id2);
            writer.Put(id3);
        }
        public static void SelectToExpand(NetDataWriter writer, SefiraEnum sefira)
        {
            writer.Reset();
            writer.Put((int)PacketType.SelectToExpand);
            writer.Put((byte)sefira);

        }
        public static void BestRwbp(NetDataWriter writer, AgentModel agent, RwbpType type)
        {
            writer.Reset();
            writer.Put((int)PacketType.BestRwbp);
            writer.Put(agent.instanceId);
            writer.Put((byte)type);
        }
        public static void NewTitles(NetDataWriter writer, AgentModel agent, int prefixId, int suffixId)
        {
            writer.Reset();
            writer.Put((int)PacketType.NewTitles);
            writer.Put(agent.instanceId);
            writer.Put(prefixId);
            writer.Put(suffixId);
        }
        public static void CreatureTakeDamage(NetDataWriter writer, CreatureModel creature, UnitModel actor, float damage, RwbpType type)
        {
            writer.Reset();
            writer.Put((int)PacketType.CreatureTakeDamage);
            writer.Put(creature.instanceId);
            writer.Put(actor.instanceId);
            writer.Put(damage);
            writer.Put((byte)type);
        }
        public static void CreatureChangeQliphoth(NetDataWriter writer, CreatureModel creature, bool AddOrSub)
        {
            writer.Reset();
            writer.Put((int)PacketType.CreatureChangeQliphoth);
            writer.Put(creature.instanceId);
            writer.Put(AddOrSub);
        }
    }
    public static class DeserializePacket
    {
        public static MapNode GetMapNode(this NetPacketReader reader)
        {
            if (!reader.GetBool()) return null;
            if (reader.GetBool())
            {
                return MapGraph.instance.GetNodeById(reader.GetString());
            }
            MapNode result = new MapNode("-1", new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat()), reader.GetString())
            {
                activate = reader.GetBool()
            };
            int edgesCount = reader.GetInt();
            for (int i = 0; i < edgesCount; i++)
            {
                result.AddEdge(GetMapEdge(reader));
            }
            return result;
        }
        public static MapEdge GetMapEdge(this NetPacketReader reader)
        {
            if (!reader.GetBool()) return null;
            return new MapEdge(GetMapNode(reader), GetMapNode(reader), reader.GetString(), reader.GetFloat());
        }
        public static MovableObjectNode GetMovableNode(this NetPacketReader reader)
        {
            if (reader.GetBool())
            {
                long id = reader.GetLong();
                return AgentManager.instance.GetAgent(id).GetMovableNode() ?? OfficerManager.instance.GetOfficerList().FirstOrDefault(officer => officer.instanceId == id).GetMovableNode() ?? CreatureManager.instance.GetCreature(id).GetMovableNode();
            }
            MapNode node = GetMapNode(reader);
            MapEdge edge = GetMapEdge(reader);
            float posRate = reader.GetFloat();
            EdgeDirection direction = (EdgeDirection)reader.GetInt();
            MovableObjectNode result = new MovableObjectNode(false);
            if (node != null) result.SetCurrentNode(node);
            else if (edge != null) result.SetCurrentEdge(edge, posRate, direction);
            return result;
        }
        public static (AgentData, int, int, int) NewAgent(NetPacketReader reader)
        {
            bool customName = reader.GetBool();
            AgentData data = new AgentData
            {
                isCustomName = customName
            };
            if (data.isCustomName)
            {
                data.CustomName = reader.GetString();
                data.agentName = AgentNameList.instance.GetCustomNameByInfo(data.CustomName);
            }
            else data.agentName = AgentNameList.instance.GetName(reader.GetInt());
            data.appearance.spriteSet = new WorkerSprite.WorkerSprite();
            data.appearance.spriteSet.saveData.Eye.a = reader.GetLong();
            data.appearance.spriteSet.saveData.Eye.b = reader.GetInt();
            data.appearance.spriteSet.saveData.EyeClose.a = reader.GetLong();
            data.appearance.spriteSet.saveData.EyeClose.b = reader.GetInt();
            data.appearance.spriteSet.saveData.EyePanic.a = reader.GetLong();
            data.appearance.spriteSet.saveData.EyePanic.b = reader.GetInt();
            data.appearance.spriteSet.saveData.EyeDead.a = reader.GetLong();
            data.appearance.spriteSet.saveData.EyeDead.b = reader.GetInt();
            data.appearance.spriteSet.saveData.EyeBrow.a = reader.GetLong();
            data.appearance.spriteSet.saveData.EyeBrow.b = reader.GetInt();
            data.appearance.spriteSet.saveData.BattleEyeBrow.a = reader.GetLong();
            data.appearance.spriteSet.saveData.BattleEyeBrow.b = reader.GetInt();
            data.appearance.spriteSet.saveData.PanicEyeBrow.a = reader.GetLong();
            data.appearance.spriteSet.saveData.PanicEyeBrow.b = reader.GetInt();
            data.appearance.spriteSet.saveData.EyeColor.r = reader.GetFloat();
            data.appearance.spriteSet.saveData.EyeColor.g = reader.GetFloat();
            data.appearance.spriteSet.saveData.EyeColor.b = reader.GetFloat();
            data.appearance.spriteSet.saveData.EyeColor.a = reader.GetFloat();
            data.appearance.spriteSet.saveData.Mouth.a = reader.GetLong();
            data.appearance.spriteSet.saveData.Mouth.b = reader.GetInt();
            data.appearance.spriteSet.saveData.BattleMouth.a = reader.GetLong();
            data.appearance.spriteSet.saveData.BattleMouth.b = reader.GetInt();
            data.appearance.spriteSet.saveData.PanicMouth.a = reader.GetLong();
            data.appearance.spriteSet.saveData.PanicMouth.b = reader.GetInt();
            data.appearance.spriteSet.saveData.FrontHair.a = reader.GetLong();
            data.appearance.spriteSet.saveData.FrontHair.b = reader.GetInt();
            data.appearance.spriteSet.saveData.RearHair.a = reader.GetLong();
            data.appearance.spriteSet.saveData.RearHair.b = reader.GetInt();
            data.appearance.spriteSet.saveData.AttachmentHair.a = reader.GetLong();
            data.appearance.spriteSet.saveData.AttachmentHair.b = reader.GetInt();
            data.appearance.spriteSet.saveData.HairColor.r = reader.GetFloat();
            data.appearance.spriteSet.saveData.HairColor.g = reader.GetFloat();
            data.appearance.spriteSet.saveData.HairColor.b = reader.GetFloat();
            data.appearance.spriteSet.saveData.HairColor.a = reader.GetFloat();
            WorkerSpriteManager.instance.LoadWorkerSpriteSetBySaveData(data.appearance.spriteSet);
            int hp = reader.GetInt();
            int mental = reader.GetInt();
            int work = reader.GetInt();
            int battle = reader.GetInt();
            data.stat = new WorkerPrimaryStat
            {
                hp = hp,
                mental = mental,
                work = work,
                battle = battle
            };
            int prefixId = reader.GetInt();
            int suffixId = reader.GetInt();
            int cost = reader.GetInt();
            return (data, prefixId, suffixId, cost);
        }
        public static (AgentModel, WorkerPrimaryStat, int, int, int) ChangeAgent(NetPacketReader reader)
        {
            long id = reader.GetLong();
            AgentModel agent = AgentManager.instance.GetAgent(id) ?? AgentManager.instance.GetSpareAgent(id);
            int hp = reader.GetInt();
            int mental = reader.GetInt();
            int work = reader.GetInt();
            int battle = reader.GetInt();
            WorkerPrimaryStat stats = new WorkerPrimaryStat
            {
                hp = hp,
                mental = mental,
                work = work,
                battle = battle
            };
            int prefixId = reader.GetInt();
            int suffixId = reader.GetInt();
            int cost = reader.GetInt();
            return (agent, stats, prefixId, suffixId, cost);
        }
        public static InitOfficers InitOfficers(NetPacketReader reader)
        {
            InitOfficers officers = new InitOfficers(SefiraManager.instance.GetSefira((SefiraEnum)reader.GetByte()));
            int count = reader.GetInt();
            for (int i = 0; i < count; i++)
            {
                long id = reader.GetLong();
                string name = reader.GetString();
                int maxHp = reader.GetInt();
                int maxMental = reader.GetInt();
                float baseMovement = reader.GetFloat();
                WorkerSpriteSaveData data = new WorkerSpriteSaveData();
                data.Eye.a = reader.GetLong();
                data.Eye.b = reader.GetInt();
                data.EyeClose.a = reader.GetLong();
                data.EyeClose.b = reader.GetInt();
                data.EyePanic.a = reader.GetLong();
                data.EyePanic.b = reader.GetInt();
                data.EyeDead.a = reader.GetLong();
                data.EyeDead.b = reader.GetInt();
                data.EyeBrow.a = reader.GetLong();
                data.EyeBrow.b = reader.GetInt();
                data.BattleEyeBrow.a = reader.GetLong();
                data.BattleEyeBrow.b = reader.GetInt();
                data.PanicEyeBrow.a = reader.GetLong();
                data.PanicEyeBrow.b = reader.GetInt();
                data.EyeColor.r = reader.GetFloat();
                data.EyeColor.g = reader.GetFloat();
                data.EyeColor.b = reader.GetFloat();
                data.EyeColor.a = reader.GetFloat();
                data.Mouth.a = reader.GetLong();
                data.Mouth.b = reader.GetInt();
                data.BattleMouth.a = reader.GetLong();
                data.BattleMouth.b = reader.GetInt();
                data.PanicMouth.a = reader.GetLong();
                data.PanicMouth.b = reader.GetInt();
                data.FrontHair.a = reader.GetLong();
                data.FrontHair.b = reader.GetInt();
                data.RearHair.a = reader.GetLong();
                data.RearHair.b = reader.GetInt();
                data.AttachmentHair.a = reader.GetLong();
                data.AttachmentHair.b = reader.GetInt();
                data.HairColor.r = reader.GetFloat();
                data.HairColor.g = reader.GetFloat();
                data.HairColor.b = reader.GetFloat();
                data.HairColor.a = reader.GetFloat();
                string currNodeId = reader.GetString();
                string nodeId = reader.GetString();
                officers.officers.Add(new InitOfficers.OfficerData
                {
                    id = id,
                    name = name,
                    MaxHp = maxHp,
                    MaxMental = maxMental,
                    baseMovement = baseMovement,
                    setData = data,
                    currNodeId = currNodeId,
                    nodeId = nodeId
                });
            }
            return officers;
        }
    }
}
