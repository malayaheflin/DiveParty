/*
 * ETC JAM-0-DRUM Server Simulator
 *
 * A script simulating server messages
 * as if it was from a real server.
 *
 * Useful to test network related problems.
 * 
 * Author:  Yikai Han (yikaih@andrew.cmu.edu)
 * Date:    04-OCT-2019
 */

using System;
using System.Net.Sockets;
using OscJack;
using UnityEngine;

namespace ETC.Platforms
{
    /// <summary>
    /// Server Message Simulator.
    /// You do not need to run this script inside your Jam-o-Drum game. You can
    /// run it in other editors or instances on the same computer.
    /// </summary>
    public class JamoDrumServerSimulator : MonoBehaviour
    {
        [Header("Config")]
        public int spinDelta = 3;
        
        [Header("Drum 1")]
        public bool hitDrum1 = false;
        public bool spinPlus1 = false;
        public bool spinMinus1 = false;

        [Header("Drum 2")]
        public bool hitDrum2 = false;
        public bool spinPlus2 = false;
        public bool spinMinus2 = false;

        [Header("Drum 3")]
        public bool hitDrum3 = false;
        public bool spinPlus3 = false;
        public bool spinMinus3 = false;

        [Header("Drum 4")]
        public bool hitDrum4 = false;
        public bool spinPlus4 = false;
        public bool spinMinus4 = false;

        #region OSC Client

        // OSC Client to send messages.
        private OscClient client;

        private void Start()
        {
            const int listenPort = 9000;
            try
            {
                client = new OscClient("127.0.0.1", listenPort);
            }
            catch (SocketException)
            {
                Debug.LogError("[JamoDrumServerSimulator] Failed to start client. Check if the network connection is blocked by firewall.");
                throw;
            }
        }

        private void OnDestroy()
        {
            if (client != null)
            {
                client.Dispose();
            }
        }

        /// <summary>
        /// Send a fake hit message to the client.
        /// </summary>
        public void SendDrumHit(int controllerId)
        {
            // Jam-o-Drum Server is using an older version of OSC protocol
            // which uses little endian, so we need to convert it manually here.
            controllerId = JamoDrumClientUtility.RevertEndian(controllerId);

            client.Send("/jamodrum/controller/hit", controllerId);
        }

        /// <summary>
        /// Send a fake spin message to the client.
        /// </summary>
        public void SendDrumSpin(int controllerId, int spin)
        {
            // Jam-o-Drum Server is using an older version of OSC protocol
            // which uses little endian, so we need to convert it manually here.
            controllerId = JamoDrumClientUtility.RevertEndian(controllerId);
            spin = JamoDrumClientUtility.RevertEndian(spin);

            client.Send("/jamodrum/controller/spin", controllerId, spin);
        }

        #endregion

        #region Input Update

        private void Update()
        {
            if (hitDrum1) {
                SendDrumHit(1);
                hitDrum1 = false;
            }

            if (hitDrum2) {
                SendDrumHit(2);
                hitDrum2 = false;
            }

            if (hitDrum3) {
                SendDrumHit(3);
                hitDrum3 = false;
            }

            if (hitDrum4) {
                SendDrumHit(4);
                hitDrum4 = false;
            }

            if (spinPlus1) {
                SendDrumSpin(1, spinDelta);
                spinPlus1 = false;
            }

            if (spinPlus2) {
                SendDrumSpin(2, spinDelta);
                spinPlus2 = false;
            }

            if (spinPlus3) {
                SendDrumSpin(3, spinDelta);
                spinPlus3 = false;
            }

            if (spinPlus4) {
                SendDrumSpin(4, spinDelta);
                spinPlus4 = false;
            }

            if (spinMinus1) {
                SendDrumSpin(1, -spinDelta);
                spinMinus1 = false;
            }

            if (spinMinus2) {
                SendDrumSpin(2, -spinDelta);
                spinMinus2 = false;
            }

            if (spinMinus3) {
                SendDrumSpin(3, -spinDelta);
                spinMinus3 = false;
            }

            if (spinMinus4) {
                SendDrumSpin(4, -spinDelta);
                spinMinus4 = false;
            }
        }

        #endregion
    }
}
