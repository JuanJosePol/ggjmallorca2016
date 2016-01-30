using System;
using UnityEditor;
using UnityEngine;

namespace Beholder.Editor
{
    public class VersionChecker
    {
        public VersionChecker()
        {
            _normalHostUrl = EditorSettings.webSecurityEmulationHostUrl;
            EditorSettings.webSecurityEmulationHostUrl = VERSION_CHECK_URL;
            _versionRequest = new WWW(VERSION_CHECK_URL);
            EditorSettings.webSecurityEmulationHostUrl = _normalHostUrl;
        }

        public const string LOCAL_VERSION = "5.1.1";
        private const string VERSION_CHECK_URL = @"https://www.virtual-mirror.com/version_check.txt";
        private const string PRODUCT_KEY = "BeholdR";

        private int _localVersion = -1;
        public int LocalVersion { get { return _localVersion > 0 ? _localVersion : (_localVersion = int.Parse(LOCAL_VERSION.Replace(".", ""))); } }

        private int _remoteVersion = -1;
        public int RemoteVersion {
            get {
                if (_remoteVersion <= 0)
                    int.TryParse(RemoteVersionString.Replace(".", ""), out _remoteVersion);
                return _remoteVersion;
            }
        }

        private string _versionString;
        public string RemoteVersionString {
            get {
                if (string.IsNullOrEmpty(_versionString)) {
                    if(_versionRequest == null) {
                        _normalHostUrl = EditorSettings.webSecurityEmulationHostUrl;
                        EditorSettings.webSecurityEmulationHostUrl = VERSION_CHECK_URL;
                        _versionRequest = new WWW(VERSION_CHECK_URL);
                        EditorSettings.webSecurityEmulationHostUrl = _normalHostUrl;
                    }

                    if (!RetrievedVersion) {
                        _versionString = string.Empty;
                    }
                    else {
                        string[] productVersions = _versionRequest.text.Split(new []{ '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        for(int i = 0; i < productVersions.Length; i++) {
                            string[] split = productVersions[i].Split(';');
                            if (split[0].Equals(PRODUCT_KEY))
                                _versionString = split[1]; 
                        }
                    }
                }

                return _versionString;
            }
        }

        public bool RetrievedVersion { get { return _versionRequest != null && _versionRequest.isDone && string.IsNullOrEmpty(_versionRequest.error); } }

        private WWW _versionRequest;
        private string _normalHostUrl;
    }
}