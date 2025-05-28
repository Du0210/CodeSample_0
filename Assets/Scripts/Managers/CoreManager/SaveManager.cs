namespace HDU.Managers
{
    using UnityEngine;
    //using ES3Internal;
    using System.Collections.Generic;
    using HDU.Define;
    using ESaveType = HDU.Define.CoreDefine.ESaveType;
    //using CodeStage.AntiCheat.ObscuredTypes;

    public class SaveManager : IManager
    {
        #region Key
        private readonly string KEY_PASS = "HONGDONGUK";
        private readonly string KEY_PATH = "Data";
        #endregion

        public UserData UserData { get; private set; }

        private const int AUTO_SAVE_INTERVAL = 300;
        private int _saveTimer = 0;
        private HashSet<ESaveType> _dirtySet;

        //private ES3Settings _cacheSettings;

        public void Clear()
        {
        }

        public void Init()
        {
            _dirtySet = new HashSet<ESaveType>();

#if UNITY_EDITOR
            Debug.Log("로드");
//            _cacheSettings = new ES3Settings
//            {
//                location = ES3.Location.Cache,
//                encryptionType = ES3.EncryptionType.None,
//                path = KEY_PATH,
//            };
//#else
//            _cacheSettings = new ES3Settings
//            {
//                location = ES3.Location.Cache,
//                encryptionType = ES3.EncryptionType.AES,
//                encryptionPassword = KEY_PASS,
//                path = KEY_PATH,
//            };
#endif

            Load();
        }

        #region Runtime Callback
        public void AutoSaveUpdate()
        {
            if (_dirtySet.Count > 0)
            {
                _saveTimer++;

                if (_saveTimer >= AUTO_SAVE_INTERVAL)
                {
                    Save();
                    _dirtySet.Clear();
                }
            }
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Save();
            }
        }

        public void OnApplicationQuit()
        {
            Save();
        }
        #endregion

        #region Save / Load
        private void Save()
        {
            SaveToCache_All();
            //ES3.StoreCachedFile(KEY_PATH, _cacheSettings); // 디스크에 저장
            //_isDirty = false;
            _saveTimer = 0;
            Debug.Log("저장");
        }

        private void SaveToCache_Single(ESaveType saveType)
        {
            //switch (saveType)
            //{
            //    case ESaveType.SettingData:
            //        ES3.Save(nameof(ESaveType.SettingData), UserData.Setting, _cacheSettings);
            //        break;
            //    case ESaveType.StageData:
            //        ES3.Save(nameof(ESaveType.StageData), UserData.Stage, _cacheSettings);
            //        break;
            //    case ESaveType.UnitData:
            //        ES3.Save(nameof(ESaveType.UnitData), UserData.Unit, _cacheSettings);
            //        break;
            //    case ESaveType.GoodsData:
            //        ES3.Save(nameof(ESaveType.GoodsData), UserData.Goods, _cacheSettings);
            //        break;
            //}
        }

        private void SaveToCache_All()
        {
            //ES3.Save(nameof(ESaveType.SettingData), UserData.Setting, _cacheSettings);
            //ES3.Save(nameof(ESaveType.StageData), UserData.Stage, _cacheSettings);
            //ES3.Save(nameof(ESaveType.UnitData), UserData.Unit, _cacheSettings);
            //ES3.Save(nameof(ESaveType.GoodsData), UserData.Goods, _cacheSettings);

            //ES3.Save("Version", UserData.Version, _cacheSettings);
        }

        public void Load()
        {
            //if(ES3.FileExists(KEY_PATH))
            //{
            //    try
            //    {
            //        UserData = new UserData
            //        {
            //            Setting = ES3.Load<SettingData>(nameof(ESaveType.SettingData), _cacheSettings),
            //            Stage = ES3.Load<StageData>(nameof(ESaveType.StageData), _cacheSettings),
            //            Unit = ES3.Load<UnitData>(nameof(ESaveType.UnitData), _cacheSettings),
            //            Goods = ES3.Load<GoodsData>(nameof(ESaveType.GoodsData), _cacheSettings),
            //            Version = ES3.Load<string>("Version", _cacheSettings),
            //        };
            //    }
            //    catch (System.Exception)
            //    {
            //        // 세이브파일 에러 예외처리
            //    }
            //}
            //else
            //{
            //    UserData = CreateDefaultUserData();
            //    SaveToCache_All();
            //}
        }
        #endregion

        #region Public API
        public void CacheDirtyData(ESaveType saveType)
        {
            SaveToCache_Single(saveType);
            //_isDirty = true;
        }
        #endregion

        #region Private API
        private UserData CreateDefaultUserData()
        {
            UserData userData = new UserData();
            userData.Initialize();

            return userData;
        }
        #endregion
    }
}