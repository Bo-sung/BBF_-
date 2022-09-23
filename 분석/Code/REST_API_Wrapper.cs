using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Toast.Gamebase;

namespace BBF
{
    public enum GameBase_REST_API_TYPE
    {
        AUTHERNTICATION = 1,
        GET_IDP_TOKEN_AND_PROFILES = 2,
    }


    public class REST_API_MANAGER
    {
        private static string m_appkey;
        public static string APPKEY => m_appkey;
        private static string m_secretkey;
        public static string SECRET_KEY => m_secretkey;
        private static string m_host;
        public static string HOST => m_host;
        private static System.Int32 m_timeout;
        public static System.Int32 TimeOut => m_timeout;
        /// <summary>
        /// REST_API의 uri 파라미터를 담은 딕셔너리를 담은 딕셔너리.
        /// </summary>
        private static Dictionary<GameBase_REST_API_TYPE, Dictionary<string, string>> m_dic_apiParameters;


        public static REST_API AUTHERNTICATION = new REST_API(REST_API.METHOD.GET, "/tcgb-gateway/v1.3/apps/{appId}/members/{userId}/tokens/{accessToken}?linkedIdP=false");
        public static REST_API GET_IDP_TOKEN_AND_PROFILES = new REST_API(REST_API.METHOD.GET, "/tcgb-gateway/v1.3/apps/{appId}/members/{userId}/idps/{idPCode}?accessToken={accessToken}");
    
    
    }

    public class REST_API
    {
        public enum STATE
        {
            NOT_INITED = 0,
            INITED = 1,
            READY_FOR_REQUEST,
            WAITTING_FOR_RESPONSE,
            RESPONSE_RECIEVED,
        }

        public enum METHOD
        {
            GET,
            POST,
            PUT,
            DELETE,
        }


        protected string m_appkey;
        protected string m_secretkey;
        protected string m_host;
        protected string m_uri;
        protected METHOD m_method;
        protected bool m_inited = false;
        //protected Dictionary<string, string> m_dic_uri_parameters;

        protected UnityEngine.Networking.UnityWebRequest webRequest;

        private STATE m_state;
        public STATE State => m_state;

        public REST_API(METHOD _method, string _uri)
        {
            m_method = _method;
            m_appkey = REST_API_MANAGER.APPKEY;
            m_secretkey = REST_API_MANAGER.SECRET_KEY;
            m_host = REST_API_MANAGER.HOST;
            m_uri = _uri;
            m_inited = false;
        }

        private UnityEngine.Networking.UnityWebRequest GetRequest(Dictionary<string,string> _parameters_head, string _parameters_body = null)
        {
            //URI 생성
            var uriBuilder = new UriBuilder("https", m_host) { Path = URI_Replacer(_parameters_head) };
            uriBuilder.UserName = m_appkey;
            uriBuilder.Password = m_secretkey;

            UnityEngine.Networking.UnityWebRequest request;
            switch (m_method)
            {
                case METHOD.GET:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Get(uriBuilder.Uri);

                    }break;

                case METHOD.POST:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Post(uriBuilder.Uri, _parameters_body);

                    }
                    break;
                case METHOD.PUT:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Put(uriBuilder.Uri, _parameters_body);
                    }
                    break;
                case METHOD.DELETE:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Delete(uriBuilder.Uri);
                    }
                    break;
                default:
                    {
                        return null;
                    }
            }
            request.timeout = REST_API_MANAGER.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            request.SetRequestHeader("X-Secret-Key", m_secretkey);
            return request;
        }

        private string URI_Replacer(Dictionary<string, string> _parameters)
        {
            string result = m_uri;
            foreach(var key in _parameters.Keys)
            {
                result = result.Replace(key, _parameters[key]);
            }
            return result;
        }



        public virtual async Task Request(Action<string> _callback, Dictionary<string, string> _parameters_head, string _parameters_body = null)
        {
            webRequest = GetRequest(_parameters_head, _parameters_body);
            IEnumerator task = WebRequest(webRequest, _callback);
            task.MoveNext();
            for (int time = REST_API_MANAGER.TimeOut; time <= 0; --time)
            {
                await Task.Delay(1);
                if (webRequest.isDone)
                {
                    task.MoveNext();
                    break;
                }
            }
        }

        private IEnumerator WebRequest(UnityEngine.Networking.UnityWebRequest _request, Action<string> _callback)
        {
            m_state = STATE.WAITTING_FOR_RESPONSE;
            yield return _request.SendWebRequest();
            m_state = STATE.RESPONSE_RECIEVED;
            Debug.Log("SendWebRequest. Finished");

            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                        webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError ||
                        webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError("UnityWebRequest_completed, Error : " + webRequest.error);
            }
            else
            {
                _callback?.Invoke(webRequest.downloadHandler.text);
            }
        }

        private IEnumerator WebRequest(string _appkey, string _host, string _path, System.Int32 _timeout, string _secretkey, Action<string> _callback)
        {
            //변경 전 버전.

            //URI 생성
            var uriBuilder = new UriBuilder("https", _host) { Path = _path };
            uriBuilder.UserName = _appkey;
            uriBuilder.Password = _secretkey;

            //Request 설정.
            webRequest = UnityEngine.Networking.UnityWebRequest.Get(uriBuilder.Uri);
            webRequest.timeout = _timeout;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("X-Secret-Key", _secretkey);

            m_state = STATE.WAITTING_FOR_RESPONSE;

            yield return webRequest.SendWebRequest();
            m_state = STATE.RESPONSE_RECIEVED;
            Debug.Log("SendWebRequest. Finished");

            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                        webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError ||
                        webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError("UnityWebRequest_completed, Error : " + webRequest.error);
                webRequest = null;
            }
            else
            {
                _callback?.Invoke(webRequest.downloadHandler.text);
                webRequest = null;
            }
        }
    }
}
