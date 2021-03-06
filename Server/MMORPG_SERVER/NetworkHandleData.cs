﻿using System;
using System.Collections.Generic;

namespace MMORPG_SERVER
{
    class NetworkHandleData
    {
        private delegate void Packet_(int Index, byte[] Data);
        private Dictionary<int, Packet_> Packets;

        public void InitMessages()
        {
            Packets = new Dictionary<int, Packet_>();
            Packets.Add(1, HandleNewAccount);
            Packets.Add(2, HandleLogin);
        }

        public void HandleData(int index, byte[] data)
        {
            int packetnum;
            Packet_ Packet;
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(data);
            packetnum = buffer.ReadInteger();
            buffer = null;

            if (packetnum == 0)
                return;

            if (Packets.TryGetValue(packetnum, out Packet))
            {
                Packet.Invoke(index, data);
            }

        }

        void HandleNewAccount(int index, byte[]data)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(data);
            int packetNum = buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();

            Globals.database.AddAccount(username,password);
        }

        void HandleLogin(int index, byte[]data)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(data);
            int packetNum = buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();

            if (!Globals.database.AccountExist(index,username))
            {
                //SendUserNotExists
                return;
            }

            if(!Globals.database.PasswordOK(index,username, password))
            {
                //SendPasswordwasNotCorrect
                return;
            }

            Console.WriteLine("Player " + username + " logged in succesfully.");
            //SendPlayerIntoTheGame
        }

      
    }
}
