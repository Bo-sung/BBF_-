using System;
using System.Collections.Generic;

namespace Assets.REST_API.TestCodes
{
    public class TestGET : UnityEngine.MonoBehaviour
    {
        /// <summary>
        /// 사용시 주의사항. Dictionary가 인스펙터창에 안뜨는 관계로 key와 value의 크기가 일치하지 않는경우 null로 기입되므로 조심.
        /// </summary>
        [UnityEngine.Header("Keys")]
        [UnityEngine.SerializeField]
        string appkey;
        [UnityEngine.SerializeField]
        string host;
        [UnityEngine.SerializeField]
        string userid;
        [UnityEngine.SerializeField]
        float timeout;
        [UnityEngine.SerializeField]
        string secretkey;
        [UnityEngine.SerializeField]
        BBF.Gamebase_REST_API_Types type;

        [UnityEngine.Header("Header")]
        [UnityEngine.SerializeField]        List<string> m_header_key;
        [UnityEngine.SerializeField]        List<string> m_header_value;

        [UnityEngine.Header("Path")]
        [UnityEngine.SerializeField]        List<string> m_path_key;
        [UnityEngine.SerializeField]        List<string> m_path_value;

        [UnityEngine.Header("Query")]
        [UnityEngine.SerializeField]        List<string> m_query_key;
        [UnityEngine.SerializeField]        List<string> m_query_value;

        [UnityEngine.Header("Request")]
        [UnityEngine.SerializeField]        List<string> requestList;

        [UnityEngine.ContextMenu("TestGET")]
        private void GETTest()
        {
            BBF.REST_API.REST_API_MANAGER.Initiallize(appkey, secretkey, host, (Int32)timeout);

            Dictionary<string, string> header_parameter = new Dictionary<string, string>();
            if(m_header_key.Count == 0 || m_header_key.Count != m_header_value.Count)
                header_parameter = null;
            else
                for(int i = 0; i < m_header_key.Count; ++i)
                {
                    header_parameter.Add(m_header_key[i], m_header_value[i]);
                }

            Dictionary<string, string> path_value = new Dictionary<string, string>();
            if (m_path_key.Count == 0 || m_path_key.Count != m_path_value.Count)
                path_value = null;
            else
                for (int i = 0; i < m_path_key.Count; ++i)
                {
                    path_value.Add(m_path_key[i], m_path_value[i]);
                }
            Dictionary<string, string> query_parameter = new Dictionary<string, string>();
            if (m_query_key.Count == 0 || m_query_key.Count != m_query_value.Count)
                query_parameter = null;
            else
                for (int i = 0; i < m_query_key.Count; ++i)
                {
                    query_parameter.Add(m_query_key[i], m_query_value[i]);
                }

            switch (type)
            {
                case BBF.Gamebase_REST_API_Types.TOKEN_AUTHERNTICATION:
                    {
                        BBF.Gamebase_REST_API_Wrapper.TOKEN_AUTHERNTICATION.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;

                case BBF.Gamebase_REST_API_Types.GET_IDP_TOKEN_AND_PROFILES:
                    {
                        BBF.Gamebase_REST_API_Wrapper.GET_IDP_TOKEN_AND_PROFILES.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;
                case BBF.Gamebase_REST_API_Types.GET_SIMPLE_LAUNCHING:
                    {
                        BBF.Gamebase_REST_API_Wrapper.GET_SIMPLE_LAUNCHING.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;
                case BBF.Gamebase_REST_API_Types.GET_MEMBER:
                    {
                        BBF.Gamebase_REST_API_Wrapper.GET_MEMBER.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;
                case BBF.Gamebase_REST_API_Types.GET_MEMBERS:
                    {
                        BBF.Gamebase_REST_API_Wrapper.GET_MEMBERS.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;
                case BBF.Gamebase_REST_API_Types.GET_IDP_INFORMATION:
                    {
                        BBF.Gamebase_REST_API_Wrapper.GET_IDP_INFORMATION.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;
                case BBF.Gamebase_REST_API_Types.GET_USERID_INFORMATION_WITH_AUTH_KEY:
                    {
                        BBF.Gamebase_REST_API_Wrapper.GET_USERID_INFORMATION_WITH_AUTH_KEY.Request
                            (
                            _callback: callback,
                            _path_value: path_value,
                            _head_parameter: header_parameter,
                            _query_parameter: query_parameter,
                            _POST_body: requestList.Count != 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(requestList.ToArray()) : null
                            ).GetAwaiter();
                    }
                    break;
            }
        }

        public void callback(BBF.REST_API.Response.Response_Base response)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

           UnityEngine.Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.None, settings));
        }
    }
}
