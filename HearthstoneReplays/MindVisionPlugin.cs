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

        private object mindvisionListenerLock = new object();
        private MindVision _mindVisionListener;

        private int instantiationFailures = 0;
        private int nextNotifThreshold = 5;

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
                            _mindVision.MessageReceived += _mindVisionListener_MessageReceived;
                            Logger.Log("MinVision created", "");
                            instantiationFailures = 0;
                            //if (onMemoryUpdate != null)
                            //{
                            //    //Logger.Log("Resetting memory updates", "");
                            //    onMemoryUpdate("reset");
                            //}
                        }
                        catch (Exception e)
                        {
                            Logger.Log("Could not instantiate MindVision: " + e.Message, e.StackTrace);
                            instantiationFailures++;
                            if (instantiationFailures >= nextNotifThreshold)
                            {
                                Logger.Log("mindvision-instantiate-error", e.Message);
                                instantiationFailures = 0;
                                nextNotifThreshold *= 2;
                            }
                        }
                    }
                }
                return _mindVision;
            }
        }

        private MindVision MindVisionListener
        {
            get
            {
                lock (mindvisionListenerLock)
                {
                    if (_mindVisionListener == null)
                    {
                        try
                        {
                            Logger.Log = onGlobalEvent;
                            _mindVisionListener = new MindVision();
                            _mindVisionListener.MessageReceived += _mindVision_MessageReceived;
                            Logger.Log("MinVision Listener created", "");
                            // Don't send a reset event here, as it might not be a reset. Do it explicitly instead
                            //if (onMemoryUpdate != null)
                            //{
                            //    //Logger.Log("Resetting memory updates", "");
                            //    onMemoryUpdate("reset");
                            //}
                        }
                        catch (Exception e)
                        {
                            Logger.Log("Could not instantiate MindVision Listener: " + e.Message, e.StackTrace);

                        }
                    }
                }
                return _mindVisionListener;
            }
        }

        private void _mindVisionListener_MessageReceived(object sender, dynamic e)
        {
            Logger.Log("MindVision Listener log", e?.Message);
        }

        private void _mindVision_MessageReceived(object sender, dynamic e)
        {
            Logger.Log("MindVision log", e?.Message);
        }

        public void getCollection(Action<object> callback)
        {
            //Logger.Log = onGlobalEvent;
            //Logger.Log("Ready to get collection", "");
            callUnitySpy(() => MindVision?.GetCollectionCards(), "getCollection", callback);
        }

        public void getBattlegroundsOwnedHeroSkinDbfIds(Action<object> callback)
        {
            //Logger.Log = onGlobalEvent;
            //Logger.Log("Ready to get collection", "");
            callUnitySpy(() => MindVision?.GetCollectionBattlegroundsHeroSkins(), "getBattlegroundsOwnedHeroSkinDbfIds", callback);
        }

        public void getCardBacks(Action<object> callback)
        {
            //Logger.Log = onGlobalEvent;
            //Logger.Log("Ready to get collection", "");
            callUnitySpy(() => MindVision?.GetCollectionCardBacks(), "getCardBacks", callback);
        }

        public void getCoins(Action<object> callback)
        {
            //Logger.Log = onGlobalEvent;
            //Logger.Log("Ready to get collection", "");
            callUnitySpy(() => MindVision?.GetCollectionCoins(), "getCoins", callback);
        }

        public void getMatchInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMatchInfo(), "getMatchInfo", callback);
        }

        public void getDungeonInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDungeonInfoCollection(), "getDungeonInfo", callback);
        }

        public void getActiveDeck(long selectedDeckId, bool resetMindvision, Action<object> callback)
        {
            long? finalSelectedDeckId = selectedDeckId;
            if (finalSelectedDeckId == 0)
            {
                finalSelectedDeckId = null;
            }
            callUnitySpy(() => MindVision?.GetActiveDeck(finalSelectedDeckId), "getActiveDeck", callback, resetMindvision);
        }

        public void getWhizbangDeck(long deckId, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetWhizbangDeck(deckId), "getWhizbangDeck", callback);
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

        public void getBoostersInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetBoostersInfo(), "getBoostersInfo", callback);
        }

        public void getMercenariesInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMercenariesInfo(), "getMercenariesInfo", callback, resetMindvision);
        }

        public void getMercenariesCollectionInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMercenariesCollectionInfo(), "getMercenariesCollectionInfo", callback, resetMindvision);
        }

        public void getMemoryChanges(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMemoryChanges(), "getMemoryChanges", callback);
        }

        public void isMaybeOnDuelsRewardsScreen(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.IsMaybeOnDuelsRewardsScreen(), "isMaybeOnDuelsRewardsScreen", callback);
        }

        private int listenRetries = 10;
        private int listenTimeout = 2000;

        public void listenForUpdates(Action<object> callback)
        {
            Task.Run(async () =>
            {
                Logger.Log("Starting to listen for updates", "");
                var listener = MindVisionListener;
                if (listener == null)
                {
                    if (listenRetries <= 0)
                    {
                        Logger.Log("Retried too many times, increasing timeout", "");
                        listenTimeout = 10000;
                    }
                    listenRetries--;
                    await Task.Delay(listenTimeout);
                    listenForUpdates(callback);
                    return;
                }
                else
                {
                    listenRetries = 10;
                    listenTimeout = 2000;
                    listener.ListenForChanges(200, (changes) =>
                    {
                        string serializedResult = changes != null ? JsonConvert.SerializeObject(changes) : null;
                        //Logger.Log("Memory changes", serializedResult); 
                        onMemoryUpdate(serializedResult);
                    });
                    Logger.Log("activated listenForUpdates", "");
                    callback("ok");

                }
            });
        }

        public void stopListenForUpdates()
        {
            Task.Run(() =>
            {
                MindVisionListener?.StopListening();
            });
        }

        public void isRunning(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.IsRunning(), "isRunning", callback);
        }

        public void reset(Action<object> callback)
        {
            if (this._mindVision != null)
            {
                this._mindVision = null;
            }
            if (this._mindVisionListener != null)
            {
                this._mindVisionListener.StopListening();
                Logger.Log("Resetting memory updates", "");
                this._mindVisionListener = null;
                this.onMemoryUpdate("reset");
            }
            if (callback != null)
            {
                callback("reset done");
            }
        }

        public void resetListening(Action<object> callback)
        {
            if (this._mindVisionListener != null)
            {
                this._mindVisionListener.StopListening();
                Logger.Log("Resetting memory updates", "");
                this._mindVisionListener = null;
            }
            if (callback != null)
            {
                callback("resetListening done");
            }
        }

        public void stopListening(Action<object> callback)
        {
            if (this._mindVisionListener != null)
            {
                this._mindVisionListener.StopListening();
                Logger.Log("Stopping memory updates", "");
                this._mindVisionListener = null;
            }
            if (callback != null)
            {
                callback("stopListening done");
            }
        }

        public void resetMain()
        {
            if (this._mindVision != null)
            {
                this._mindVision = null;
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
                        resetMain();
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
                    //Logger.Log("Raised when rertieving " + service + ", resetting MindVision: " + e.Message, e.StackTrace);
                    // Don't automatically reset, as exceptions can occur when we try to read the memory at the wrong moment
                    Logger.Log("Raised when rertieving " + service + ": " + e.Message, e.StackTrace);
                    // Reinit the plugin
                    //resetMain();
                    //callUnitySpy(action, service, callback, retriesLeft - 1);
                    callback(null);
                    return;
                }
            });
        }
    }
}
