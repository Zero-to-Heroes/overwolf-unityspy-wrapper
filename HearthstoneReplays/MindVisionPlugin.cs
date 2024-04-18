using HackF5.UnitySpy.HearthstoneLib;
using HackF5.UnitySpy.HearthstoneLib.Detail;
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
        public event Action<object, object> onGlobalEvent;
        public event Action<object> onMemoryUpdate;

        private object mindvisionLock = new object();
        private MindVision _mindVision;

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

        private void _mindVisionListener_MessageReceived(object sender, dynamic e)
        {
            Logger.Log("MindVision Listener log", e?.Message);
        }

        private void _mindVision_MessageReceived(object sender, dynamic e)
        {
            Logger.Log("MindVision log", e?.Message);
        }

        public void getCollection(bool throwException, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetCollectionCards(), "getCollection", callback, throwException);
        }

        public void getBattlegroundsOwnedHeroSkinDbfIds(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetCollectionBattlegroundsHeroSkins(), "getBattlegroundsOwnedHeroSkinDbfIds", callback);
        }

        public void getCardBacks(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetCollectionCardBacks(), "getCardBacks", callback);
        }

        public void getCoins(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetCollectionCoins(), "getCoins", callback);
        }

        public void getMatchInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMatchInfo(), "getMatchInfo", callback);
        }

        public void getCurrentBoard(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetBoard(), "getCurrentBoard", callback);
        }

        public void getAdventuresInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetAdventuresInfo(), "getAdventuresInfo", callback);
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
            callUnitySpy(() => MindVision?.GetActiveDeck(finalSelectedDeckId), "getActiveDeck", callback);
        }

        public void getDuelsDeck(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsDeck(), "getDuelsDeck", callback);
        }

        public void getDuelsDeckFromCollection(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsDeckFromCollection(), "getDuelsDeckFromCollection", callback);
        }

        public void getSelectedDeckId(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetSelectedDeckId(), "getSelectedDeckId", callback);
        }

        public void getWhizbangDeck(long deckId, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetWhizbangDeck(deckId), "getWhizbangDeck", callback);
        }

        public void getBattlegroundsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetBattlegroundsInfo(), "getBattlegroundsInfo", callback);
        }
        
        public void getBattlegroundsSelectedMode(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetBattlegroundsSelectedGameMode(), "getBattlegroundsSelectedMode", callback);
        }

        public void getArenaInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetArenaInfo(), "getArenaInfo", callback);
        }

        public void getArenaDeck(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetArenaDeck(), "getArenaDeck", callback);
        }

        public void getDuelsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsInfo(), "getDuelsInfo", callback);
        }

        public void getRewardsTrackInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetRewardTrackInfo(), "getRewardsTrackInfo", callback);
        }

        public void getDuelsRewardsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsRewardsInfo(), "getDuelsRewardsInfo", callback);
        }

        public void getAchievementsInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetAchievementsInfo(), "getAchievementsInfo", callback);
        }

        public void getAchievementCategories(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetAchievementCategories(), "getAchievementCategories", callback);
        }

        public void getInGameAchievementsProgressInfo(int[] achievementIds, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetInGameAchievementsProgressInfo(achievementIds), "getInGameAchievementsProgressInfo", callback);
        }

        public void getInGameAchievementsProgressInfoByIndex(int[] indexes, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetInGameAchievementsProgressInfoByIndex(indexes), "getInGameAchievementsProgressInfo", callback);
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
            callUnitySpy(() => MindVision?.GetMercenariesInfo(), "getMercenariesInfo", callback);
        }

        public void getMercenariesCollectionInfo(bool resetMindvision, Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMercenariesCollectionInfo(), "getMercenariesCollectionInfo", callback);
        }

        public void getMemoryChanges(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetMemoryChanges(), "getMemoryChanges", callback);
        }

        public void getDuelsHeroOptions(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsHeroOptions(), "getDuelsHeroOptions", callback);
        }

        public void getDuelsHeroPowerOptions(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsHeroPowerOptions(), "getDuelsHeroPowerOptions", callback);
        }

        public void getDuelsSignatureTreasureOptions(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetDuelsSignatureTreasureOptions(), "getDuelsSignatureTreasureOptions", callback);
        }

        public void getActiveQuests(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetQuests(), "getActiveQuests", callback);
        }

        public void getPlayerProfileInfo(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.GetPlayerProfileInfo(), "getPlayerProfileInfo", callback);
        }

        private int listenRetries = 10;
        private int listenTimeout = 2000;

        public void listenForUpdates(Action<object> callback)
        {
            Task.Run(async () =>
            {
                Logger.Log("Starting to listen for updates", "");
                var listener = MindVision;
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
                        string serializedResult = changes != null 
                            ? JsonConvert.SerializeObject(changes, new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                }) 
                            : null;
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
                MindVision?.StopListening();
            });
        }

        public void isRunning(Action<object> callback)
        {
            callUnitySpy(() => MindVision?.IsRunning(), "isRunning", callback);
        }

        public void reset(Action<object> callback)
        {
            Task.Run(() =>
            {
                if (this._mindVision != null)
                {
                    this._mindVision.StopListening();
                    this._mindVision = null;
                    for (var i = 0; i < 5; i++)
                    {
                        var unused = MindVision;
                        if (this._mindVision != null)
                        {
                            break;
                        }
                        else
                        {
                            Logger.Log("Could not instantiate MindVision", "");
                        }
                    }
                }
                if (callback != null)
                {
                    callback("reset done");
                }
            });
        }

        public void tearDown(Action<object> callback)
        {
            Task.Run(() =>
            {
                if (this._mindVision != null)
                {
                    this._mindVision.StopListening();
                    Logger.Log("Stopping memory updates", "");
                    this._mindVision = null;
                }
                if (callback != null)
                {
                    callback("tearDown done");
                }
            });
        }

        private void callUnitySpy(Func<object> action, string service, Action<object> callback, bool throwException = false)
        {
            Task.Run(() =>
            {
                try
                {
                    Logger.Log = onGlobalEvent;

                    if (callback == null)
                    {
                        Logger.Log("No callback, returning", service);
                        return;
                    }

                    object result = action != null ? action() : null;
                    string serializedResult = result != null ? JsonConvert.SerializeObject(result) : null;
                    callback(serializedResult);
                }
                catch (Exception e)
                {
                    // Don't automatically reset, as exceptions can occur when we try to read the memory at the wrong moment
                    Logger.Log("Raised when rertieving " + service + ": " + e.Message, e.StackTrace);
                    // Sometimes we really want to know if there was an exception, so that we can trigger a reset without going
                    // through the logs handling events
                    if (throwException)
                    {
                        callback("exception");
                        return;
                    }
                    if (Utils.IsMemoryReadingIssue(e))
                    {
                        Logger.Log("Memory reading issue, calling reset", "");
                        Logger.Log("reset", "");
                    }
                    callback(null);
                    return;
                }
            });
        }
    }
}
