using HackF5.UnitySpy.HearthstoneLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverwolfUnitySpy
{
    class MindVisionPlugin
    {
        // a global event that triggers with two parameters:
        //
        // plugin.get().onGlobalEvent.addListener(function(first, second) {
        //  ...
        // });
        public event Action<object, object> onGlobalEvent;

        private object mindvisionLock = new object();

        private MindVision _mindVision;
        private MindVision mindVision
        {
            get
            {
                lock (mindvisionLock)
                {
                    if (_mindVision == null)
                    {
                        try
                        {
                            Logger.Log = onGlobalEvent;
                            _mindVision = new MindVision();
                            Logger.Log("MinVision created", "");
                        }
                        catch (Exception e)
                        {
                            Logger.Log("Could not instantiate MindVision: " + e.Message, e.StackTrace);
                        }
                    }
                }
                return _mindVision;
            }
        }

        public void getCollection(Action<object> callback)
        {
            //Logger.Log = onGlobalEvent;
            //Logger.Log("Ready to get collection", "");
            callUnitySpy(() => mindVision?.GetCollectionCards(), "getCollection", callback);
        }

        public void getMatchInfo(Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetMatchInfo(), "getMatchInfo", callback);
        }

        public void getDungeonInfo(Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetDungeonInfoCollection(), "getDungeonInfo", callback);
        }

        public void getActiveDeck(Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetActiveDeck(), "getAciveDeck", callback);
        }

        public void getBattlegroundsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetBattlegroundsInfo(), "getBattlegroundsInfo", callback, resetMindvision);
        }

        public void getArenaInfo(Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetArenaInfo(), "getArenaInfo", callback);
        }

        public void getDuelsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetDuelsInfo(), "getDuelsInfo", callback, resetMindvision, true);
        }

        public void getCurrentScene(Action<object> callback)
        {
            callUnitySpy(() => mindVision?.GetSceneMode(), "getCurrentScene", callback);
        }

        private void callUnitySpy(Func<object> action, string service, Action<object> callback, bool resetMindvision = false, bool debug = false, int retriesLeft = 2)
        {
            Task.Run(() =>
            {
                //Logger.Log("Calling unityspy 1", callback);
                try
                {
                    Logger.Log = onGlobalEvent;
                    if (resetMindvision)
                    {
                        lock (mindvisionLock)
                        {
                            this._mindVision = null;
                        }
                        Logger.Log("Reset mindvision", service);
                    }
                    if (debug)
                    {
                        Logger.Log("Calling unityspy 2", service);
                    }

                    if (callback == null)
                    {
                        Logger.Log("No callback, returning", service);
                        return;
                    }

                    if (retriesLeft <= 0)
                    {
                        Logger.Log("Could not manage to complete task", service);
                        callback(null);
                        return;
                    }
                    object result = action != null ? action() : null;
                    if (debug)
                    {
                        Logger.Log("result " + service, result);
                    }
                    string serializedResult = result != null ? JsonConvert.SerializeObject(result) : null;
                    if (debug)
                    {
                        Logger.Log("Serialized ", service);
                    }
                    callback(serializedResult);
                }
                catch (Exception e)
                {
                    Logger.Log("Raised when rertieving " + service + ", resetting MindVision: " + e.Message, e.StackTrace);
                    // Reinit the plugin
                    lock (mindvisionLock)
                    {
                        this._mindVision = null;
                    }
                    //callUnitySpy(action, service, callback, retriesLeft - 1);
                    callback(null);
                    return;
                }
            });
        }
    }
}
