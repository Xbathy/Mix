using System;
using Proyecto26;
using UnityEngine;

namespace Mix.Restful
{
    public class Client : MonoBehaviour
    {
        /// <summary>
        /// post 字节序列
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="action">response 回调</param>
        public static void PostBytes<TResponse>(RequestHelper request, Action<TResponse> action)
        {
            RestClient.Post<TResponse>(request).Then(action);
        }
    }
}