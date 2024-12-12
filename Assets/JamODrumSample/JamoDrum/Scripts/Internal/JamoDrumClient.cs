/*
 * ETC JAM-0-DRUM Interface
 *
 * An Windows Forms application which uses the Win32
 * Raw Input API to read messages from multiple 
 * 
 * Author:  Bryan Maher (bm3n@andrew.cmu.edu)
 * Date:    27-JAN-2012
 */

using System;
using System.Net.Sockets;
using OscJack;
using UnityEngine;

namespace ETC.Platforms
{
    /// <summary>
    /// Delegate for the Spin event.
    /// </summary>
    /// <param name="controllerID">A number from 1 to 4 indicating the controller that generated the event.</param>
    /// <param name="delta">The amount the spinner has moved since the last event.</param>
    public delegate void SpinEventHandler(int controllerID, int delta);

    /// <summary>
    /// Delegate for the Hit event.
    /// </summary>
    /// <param name="controllerID">A number from 1 to 4 indicating the controller that generated the event.</param>
    public delegate void HitEventHandler(int controllerID);


    /// <summary>
    /// A network interface to the ETC Jam-0-Drum platform.
    /// </summary>
    /// <remarks>
    /// This application uses OscJack by Keijiro Takahashi.
    /// See https://github.com/keijiro/OscJack to learn more about OSC protocol.
    /// </remarks>
    public class JamoDrumClient : MonoBehaviour
    {
        #region STATICS

        /// <summary>
        /// Contains the Singleton instance of the JamoDrumClient.
        /// </summary>
        private static JamoDrumClient instance;

        /// <summary>
        /// Returns a pointer to the one and only instance of the JamoDrumClient.
        /// </summary>
        public static JamoDrumClient Instance
        {
            get
            {
                // Lazy Instantiation
                if (instance == null)
                {
                    // Create game object
                    GameObject clientGo = new GameObject("[JamoDrumClient]");
                    clientGo.AddComponent<JamoDrumClient>();
                }

                return JamoDrumClient.instance;
            }
        }

        /// <summary>
        /// Print debug message into console.
        /// </summary>
        public static bool ShowDebugMessage = false;

        #endregion STATICS

        #region PROPERTIES & EVENTS

        /// <summary>
        /// The OSC server used to receive messages from the Jam-0-Drum server.
        /// </summary>        
        private OscServer receiver;

        /// <summary>
        /// Raised when a controller spin message is received from the Jam-0-Drum server.
        /// </summary>
        public event SpinEventHandler Spin;

        /// <summary>
        /// Raised when a controller hit message is received from the Jam-0-Drum server.
        /// </summary>
        public event HitEventHandler Hit;

        public event HitEventHandler Release;

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            if (instance != null)
            {
                throw new NotSupportedException("Only one JamoDrumClient instance can be created!");
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeOscListener();
        }

        private void OnDestroy()
        {
            DestroyOscListener();

            instance = null;
        }

        #endregion

        #region OSC SERVER

        /// <summary>
        /// Initializes the OscServer to listen for known Jam-0-Drum messages.
        /// </summary>
        private void InitializeOscListener()
        {
            const int listenPort = 9000;
            try
            {
                receiver = new OscServer(listenPort);

                receiver.MessageDispatcher.AddCallback("/jamodrum/controller/spin", OnMessageSpin);
                receiver.MessageDispatcher.AddCallback("/jamodrum/controller/hit", OnMessageHit);
                receiver.MessageDispatcher.AddCallback("/jamodrum/controller/release", OnMessageRelease);

                PrintLogFormat("Server Started on Port [{0}].", listenPort);
            }
            catch (SocketException socketException)
            {
                PrintErrorFormat(
                    "Server Failed to Start. Check if there is any other game or editor running and occupying port {0}. " +
                    "Then check if the network connection is blocked by the firewall. " +
                    "If not, find a TA for help. \nError Message: {1}",
                    listenPort, socketException.Message);

                throw;
            }
        }

        private void DestroyOscListener()
        {
            // Stop the server and release the resources
            if (receiver != null)
            {
                receiver.Dispose();

                PrintLog("Server Terminated.");
            }
        }

        /// <summary>
        /// Handles incoming Hit messages from OscServer.
        /// Note that no exception is allowed here because it will suspend the osc receiver.
        /// </summary>
        private void OnMessageHit(string address, OscDataHandle data)
        {
            if (data.GetElementCount() != 1)
            {
                PrintError("Invalid hit message received in JamoDrumClient");
                return;
            }

            // The first data item is teh controller id.
            // Note: The Jam-o-Drum server is using an older version of OSC protocol,
            // which uses little-endian by default. So we have to convert the endianness here
            // to get the correct result.
            int controllerId = JamoDrumClientUtility.RevertEndian(data.GetElementAsInt(0));

            if (controllerId < 1 || controllerId > 4)
            {
                PrintErrorFormat("Invalid hit message received in JamoDrumClient: Invalid controllerId {0}", controllerId);
                return;
            }

            PrintLogFormat("Drum {0} Hit", controllerId);

            if (Hit != null)
            {
                Hit.Invoke(controllerId);
            }

        }

        private void OnMessageRelease(string address, OscDataHandle data)
        {
            if (data.GetElementCount() != 1)
            {
                PrintError("Invalid hit message received in JamoDrumClient");
                return;
            }

            // The first data item is teh controller id.
            // Note: The Jam-o-Drum server is using an older version of OSC protocol,
            // which uses little-endian by default. So we have to convert the endianness here
            // to get the correct result.
            int controllerId = JamoDrumClientUtility.RevertEndian(data.GetElementAsInt(0));

            if (controllerId < 1 || controllerId > 4)
            {
                PrintErrorFormat("Invalid hit message received in JamoDrumClient: Invalid controllerId {0}", controllerId);
                return;
            }

            PrintLogFormat("Drum {0} Release", controllerId);

            if (Hit != null)
            {
                Release.Invoke(controllerId);
            }

        }

        /// <summary>
        /// Handles incoming Spin messages from OscServer.
        /// </summary>
        private void OnMessageSpin(string address, OscDataHandle data)
        {
            if (data.GetElementCount() != 2)
            {
                PrintError("Invalid spin message received in JamoDrumClient");
                return;
            }

            // The first data item is the controller id, the second is the delta value.
            // Note: The Jam-o-Drum server is using an older version of OSC protocol,
            // which uses little-endian by default. So we have to convert the endianness here
            // to get the correct result.
            int controllerId = JamoDrumClientUtility.RevertEndian(data.GetElementAsInt(0));
            int delta = JamoDrumClientUtility.RevertEndian(data.GetElementAsInt(1));

            if (controllerId < 1 || controllerId > 4)
            {
                PrintErrorFormat("Invalid spin message received in JamoDrumClient: Invalid controllerId {0}", controllerId);
                return;
            }

            // Delta should be a very small value
            if (Mathf.Abs(delta) > 128)
            {
                PrintErrorFormat("Invalid spin message received in JamoDrumClient: Invalid spin delta {0}", delta);
                return;
            }

            PrintLogFormat("Drum {0} Spin: Delta {1}", controllerId, delta);

            if (Spin != null)
            {
                Spin.Invoke(controllerId, delta);
            }
        }

        #endregion

        #region LOG

        private void PrintLog(string log)
        {
            if (ShowDebugMessage)
            {
                Debug.Log("[JamoDrumClient] " + log);
            }
        }

        private void PrintLogFormat(string log, params object[] args)
        {
            if (ShowDebugMessage)
            {
                Debug.LogFormat("[JamoDrumClient] " + log, args);
            }
        }

        private void PrintError(string log)
        {
            Debug.LogError("[JamoDrumClient] " + log);
        }

        private void PrintErrorFormat(string log, params object[] args)
        {
            Debug.LogErrorFormat("[JamoDrumClient] " + log, args);
        }

        #endregion

    }
}
