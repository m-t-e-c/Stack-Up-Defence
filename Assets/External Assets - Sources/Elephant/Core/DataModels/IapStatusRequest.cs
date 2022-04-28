using System;
using UnityEngine;

namespace ElephantSDK
{
    [Serializable]
    public class IapStatusRequest
    {
        public string user_id;
        public string bundle;

        private IapStatusRequest() { }

        public static IapStatusRequest Create()
        {
            var iapStatusRequest = new IapStatusRequest
            {
                user_id = ElephantCore.Instance.userId, 
                bundle = Application.identifier
            };
            
            return iapStatusRequest;
        }
    }
}