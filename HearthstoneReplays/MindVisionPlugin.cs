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
        public event Action<object> onMemoryUpdate;

        private object mindvisionLock = new object();

        private MindVision _mindVision;

        private MindVision MindVision
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
                            onMemoryUpdate("reset");
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
            callUnitySpy(() => MindVision?.GetCollectionCards(), "getCollection", callback);
        }

        public void getMatchInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMatchInfo(), "getMatchInfo", callback);
        }

        public void getDungeonInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDungeonInfoCollection(), "getDungeonInfo", callback);
        }

        public void getActiveDeck(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetActiveDeck(), "getAciveDeck", callback);
        }

        public void getBattlegroundsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetBattlegroundsInfo(), "getBattlegroundsInfo", callback, resetMindvision);
        }

        public void getArenaInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetArenaInfo(), "getArenaInfo", callback);
        }

        public void getDuelsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsInfo(), "getDuelsInfo", callback, resetMindvision, true);
        }

        public void getRewardsTrackInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetRewardTrackInfo(), "getRewardsTrackInfo", callback);
        }

        public void getDuelsRewardsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsRewardsInfo(), "getDuelsRewardsInfo", callback, resetMindvision, true);
        }

        public void getAchievementsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetAchievementsInfo(), "getAchievementsInfo", callback, resetMindvision, true);
        }

        public void getInGameAchievementsProgressInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetInGameAchievementsProgressInfo(), "getInGameAchievementsProgressInfo", callback, resetMindvision, true);
        }

        public void getCurrentScene(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetSceneMode(), "getCurrentScene", callback);
        }

        public void listenForUpdates(Action<object> callback)
        {
            Task.Run(() =>
            {
                MindVision?.ListenForChanges(500, (changes) =>
                {
                    string serializedResult = changes != null ? JsonConvert.SerializeObject(changes) : null;
                    onMemoryUpdate(changes);
                });
            });
        }

        public void stopListenForUpdates()
        {
            Task.Run(() =>
            {
                MindVision?.StopListening();
            });
        }

        public void isRunning(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.IsRunning(), "isRunning", callback);
        }

        public void reset(Action<object> callback)
        {
            lock (mindvisionLock)
            {
                this.MindVision?.StopListening();
                this._mindVision = null;
            }
            if (callback != null)
            {
                isRunning(callback);
            }
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
                            reset(null);
                        }
                        Logger.Log("Reset mindvision", service);
                    }
                    if (debug)
                    {
                        //Logger.Log("Calling unityspy 2", service);
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
                        //Logger.Log("result " + service, result);
                    }
                    string serializedResult = result != null ? JsonConvert.SerializeObject(result) : null;
                    if (debug)
                    {
                        //Logger.Log("Serialized ", service);
                    }
                    callback(serializedResult);
                }
                catch (Exception e)
                {
                    Logger.Log("Raised when rertieving " + service + ", resetting MindVision: " + e.Message, e.StackTrace);
                    // Reinit the plugin
                    lock (mindvisionLock)
                    {
                        reset(null);
                    }
                    //callUnitySpy(action, service, callback, retriesLeft - 1);
                    callback(null);
                    return;
                }
            });
        }
    }
}
