using TestMod.PacketConvertation;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace TestMod
{
    public class MultiplayerController : MonoBehaviour
    {
        static MultiplayerController _instance;
        static readonly AutoResetEvent ClearSession = new AutoResetEvent(false);
        static readonly string serverIP = "192.168.100.2";
        static readonly int serverPort = 8080;
        static readonly string LogPath = Application.dataPath + $"/BaseMods/Test_MOD/";
        public static NetDataWriter SharedWriter { get; } = new NetDataWriter(true, 256);
        public static MultiplayerController instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("MultiplayerController");
                    DontDestroyOnLoad(gameObject);
                    _instance = gameObject.AddComponent<MultiplayerController>();
                    _instance.listener = new EventBasedNetListener();
                    _instance.listener.NetworkReceiveEvent += (peer, reader, deliveryMethod) => _instance.ReceivePacket(peer, reader, deliveryMethod);
                    _instance.listener.PeerConnectedEvent += (peer) => _instance.PeerConnected(peer);
                    _instance.listener.PeerDisconnectedEvent += (peer, disconnectInfo) => _instance.PeerDisconnected(disconnectInfo);
                    _instance.netManager = new NetManager(_instance.listener);
                }
                return _instance;
            }
        }
        public string roomCode = "";
        EventBasedNetListener listener = null;
        NetManager netManager = null;
        NetPeer currPeer = null;
        public string nickname { get; private set; }
        bool confirm = false;
        public List<Player> playerList = new List<Player>();
        string EndConnectionState = "";
        int n = 0;
        public bool Host { get; private set; }
        public bool IsConnected { get; private set; } = false;
        volatile bool exit = false;
        readonly Queue<NetPacketReader> packets = new Queue<NetPacketReader>();
        public long[] selectUIData = null;
        public Locks locks = new Locks();
        public void PeerConnected(NetPeer peer)
        {
            currPeer = peer;
            SerializePacket.InitPlayer(SharedWriter, roomCode, nickname, Host, confirm);
            SendPacket(SharedWriter, 0);
        }
        public void PeerDisconnected(DisconnectInfo info)
        {
            try
            {
                currPeer = null;
                if (info.AdditionalData != null && info.AdditionalData.AvailableBytes > 0)
                {
                    if ((PacketType)info.AdditionalData.GetByte() == PacketType.Message)
                    {
                        EndConnectionState = info.AdditionalData.GetString();
                    }
                }
                exit = true;
            }
            catch (Exception ex)
            {
                File.WriteAllText(LogPath + "Bleba_Log.txt", ex.ToString());
            }
        }
        public void ConnectWithServer(bool host)
        {
            try
            {
                DisconnectFromServer();
                MultiplayerWindowController.instance.CreateButtonText.SetText("Disconnect");
                MultiplayerWindowController.instance.ConnectButtonText.SetText("Disconnect");
                ChatController.instance.SendButton.interactable = true;
                Host = host;
                netManager.Start();
                netManager.Connect(serverIP, serverPort, "LCorpMultiplayer");
                Thread pollingThread = new Thread(PollEventsThread);
                pollingThread.Start();
            }
            catch (Exception ex)
            {
                File.WriteAllText(LogPath + $"Bleba_Log{n++}.txt", ex.ToString());
            }
        }
        void PollEventsThread()
        {
            try
            {
                while (!exit)
                {
                    netManager.PollEvents();
                    Thread.Sleep(15);
                }
            }
            finally
            {
                ClearSession.Set();
            }
        }
        void Update()
        {
            try
            {
                if (packets.Count > 0) lock (packets)
                {
                    NetPacketReader packet = packets.Dequeue();
                    PacketHandler.Handler(packet);
                }
                if (exit)
                {
                    ClearSession.WaitOne();
                    exit = false;
                    MultiplayerWindowController.instance.CreateButtonText.SetText("CreateRoom");
                    MultiplayerWindowController.instance.ConnectButtonText.SetText("Join");
                    SaveHandler.SetOldSave();
                    if (IsConnected)
                    {
                        EscapeUI.instance.OnClickButton(2);
                        MultiplayerWindowController.instance.OpenMenu();
                    }
                    netManager.Stop();
                    ChatController.instance.SendButton.interactable = false;
                    switch (EndConnectionState)
                    {
                        case "Kick":
                            {
                                ErrorWindow.instance.GetError("Kicked");
                                break;
                            }
                        case "ServerClosed":
                            {
                                ErrorWindow.instance.GetError("ServerClosed");
                                break;
                            }
                        case "RoomNotFound":
                            {
                                ErrorWindow.instance.GetError("RoomNotFound");
                                break;
                            }
                        case "RoomOverflow":
                            {
                                ErrorWindow.instance.GetError("RoomOverflow");
                                break;
                            }
                        case "NicknameExists":
                            {
                                ErrorWindow.instance.GetError("NicknameExists");
                                break;
                            }
                        case "GameStarted":
                            {
                                ErrorWindow.instance.GetError("GameStarted");
                                break;
                            }
                        case "":
                            {
                                ErrorWindow.instance.GetError("ConnectionClosed");
                                break;
                            }
                    }
                    EndConnectionState = "";
                    MultiplayerWindowController.instance.CreateCodeField.text = "";
                    MultiplayerWindowController.instance.ConnectCodeField.text = "";
                    PlayerList.instance.ClearList();
                }
                IsConnected = currPeer != null && currPeer.ConnectionState == ConnectionState.Connected;
            }
            catch (Exception ex)
            {
                File.WriteAllText(LogPath + $"Bleba_Log{n}.txt", ex.ToString());
            }
        }
        public void StartGame()
        {
            try
            {
                (byte[], int) savePacket = SaveHandler.GetSave();
                if (currPeer == null || currPeer.ConnectionState != ConnectionState.Connected) return;
                currPeer.Send(savePacket.Item1, 0, savePacket.Item2, 0, DeliveryMethod.ReliableUnordered);
            }
            catch (Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", ex.ToString());
            }
        }
        public void OnInputEndNickname(string text)
        {
            bool needContinue = true;
            switch (text)
            {
                case "Server":
                    {
                        ErrorWindow.instance.GetError("NicknameServer");
                        needContinue = false;
                        break;
                    }
                case "":
                    {
                        ErrorWindow.instance.GetError("NicknameNull");
                        needContinue = false;
                        break;
                    }
            }
            if (!needContinue)
            {
                MultiplayerWindowController.instance.HostNickname.text = "";
                MultiplayerWindowController.instance.ClientNickname.text = "";
                return;
            }
            MultiplayerWindowController.instance.HostNickname.text = text;
            MultiplayerWindowController.instance.ClientNickname.text = text;
            nickname = text;
        }
        public void NeededConfirm(bool res)
        {
            confirm = res;
        }
        public void OnInputEndRoomCode(string text)
        {
            roomCode = text;
        }
        public void SendPacket(NetDataWriter writer, byte DeliveryType)
        {
            try
            {
                if (currPeer == null)
                {
                    Debug.LogError("[SendPacket] currPeer == null");
                    return;
                }
                if (currPeer.ConnectionState != ConnectionState.Connected)
                {
                    Debug.LogError($"[SendPacket] not Connected, state = {currPeer.ConnectionState}");
                    return;
                }
                if (DeliveryType == 0) currPeer.Send(writer, 0, DeliveryMethod.ReliableOrdered);
                if (DeliveryType == 1) currPeer.Send(writer, 0, DeliveryMethod.ReliableUnordered);
                if (DeliveryType == 2) currPeer.Send(writer, 0, DeliveryMethod.ReliableSequenced);
                if (DeliveryType == 3) currPeer.Send(writer, 0, DeliveryMethod.Unreliable);
            }
            catch (Exception ex)
            {
                File.WriteAllText(LogPath + $"Bleba_Log{n}.txt", ex.ToString());
            }
        }
        public void DisconnectFromServer()
        {
            if (IsConnected && !exit)
            {
                EndConnectionState = "ConnectionClosed";
                netManager.DisconnectAll();
                return;
            }
        }
        void ReceivePacket(NetPeer peer, NetPacketReader netReader, DeliveryMethod deliveryMethod)
        {
            try
            {
                lock (packets) packets.Enqueue(netReader);
            }
            catch (Exception ex)
            {
                File.WriteAllText(LogPath + $"Bleba_Log{n}.txt", ex.ToString());
            }
        }
        public IEnumerator AwaitSelectUIDataCoroutine(CreatureSelectUI selectUI)
        {
            bool isDataExist = false;
            while (!isDataExist)
            {
                isDataExist = selectUIData != null;
                yield return null;
            }
            lock (locks.selectUILock)
            {  
                selectUI.Units[0].Init(selectUIData[0]);
                selectUI.Units[1].Init(selectUIData[1]);
                selectUI.Units[2].Init(selectUIData[2]);
            }
            selectUIData = null;
        }
    }
    public enum PacketType
    {
        None,
        InitPlayer,
        RoomCode,
        Message,
        PlayerPermisions,
        ConfirmUser,
        KickUser,
        PlayerList,
        ChatMessage,
        Save,
        NewAgent,
        ChangeAgent,
        EquipEGO,
        AddAgent,
        InitAgentAdd,
        RemoveAgent,
        DeployAgent,
        InitOfficers,
        WorkerMoveToMovable,
        WorkerMoveById,
        WorkerMoveByLink,
        CancelWork,
        CancelSuppress,
        StartWorkCreature,
        StartWorkKit,
        Suppress,
        ProgressWorkResult,
        Pause,
        Resume,
        Speed,
        SelectResearch,
        ConfirmResearch,
        WorkerDamageNoEffect,
        WorkerDamage,
        CreatureObserve,
        CreateEquipment,
        GetEGO,
        Overload,
        CreatureMoveToNode,
        CreatureMoveToMovable,
        WaitingPassage,
        SelectAno,
        InitSelectUI,
        SelectToExpand,
        BestRwbp,
        NewTitles,
        CreatureTakeDamage,
        CreatureChangeQliphoth,
    }
    public class Player
    {
        public string Nickname;
        public bool IsHost;
        public bool IsConfirmed;
        public PlayerPermissions Permissions;
    }
    public struct PlayerPermissions
    {
        public bool perm1;
    }
    public class Locks
    {
        public readonly object selectUILock = new object();
    }
}