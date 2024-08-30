using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftwareKingdom.Common;
namespace SoftwareKingdom.RemoteConfig
{
    public interface IRemoteConfig
    {
        void InitGameParameters(GameParameters parameters);

        object LoadParameter(string parameterName);
        object GetValue(string parameterName);
    }
}

