using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBF.REST_API
{
    //작성자 서보성.

    //사용시 주의사항.
    //클래스 목적 : REST API 대응을 위해 제작한 래핑 클래스 
    //UnityEngine.Networking.UnityWebRequest을 사용해 제작. 
    //System.Net.Http.HttpClient 를 사용한 버전과, HttpWebRequest를 사용한 버전도 있지만 
    //.Net Core 사용시에는 GET 메소드를 사용해도 body를 보낼수 있지만, 유니티, .NET Framework 상에서는 보낼수 없는 이유로 인해
    //그나마 직관적이고 빠른 UnityWebRequest를 사용.
    //현재 GET 메소드는 정상적으로 작동하는것을 확인했으나 POST 메소드를 사용하는 부분은 검증 및 테스트 필요
    //최대한 게임베이스 의존성을 최소화하려고 노력은 했으나 다른 환경에서의 테스트는 진행하지 못함. 다른환경에서 사용시 적절한 수정 필요.

    //사용법 : BBF.REST_API.REST_API_MANAGER.Initiallize 호출하여 초기화.

    //REST API란?
    //HTTP URI(Uniform Resource Identifier)를 통해 자원(Resource)을 명시하고,
    //HTTP Method(POST, GET, PUT, DELETE, PATCH 등)를 통해
    //해당 자원(URI)에 대한 CRUD Operation을 적용하는 것을 의미.

    //REST API 구성
    //Method 주의사항.
    //크게 따져봤을떄는 GET, POST 둘로 나뉜다.
    //GET은 서버로부터 데이터를 받을떄 사용.
    //POST는 서버에 데이터를 전달하고 결과값을 받을떄 사용. 이떄 전달하는 데이터는 body에 넣음.
    //URI 구조 https://[USERNAME]:[SECRETKEY]@[HOST]/[PATH]?[QUERY]
    //USERNAME : 사용자 이름 (서버 접속시 필요한 유저를 뜻함. 게임베이스에서는 AppKey에 대응.)
    //SECRETKEY : 패스워드 (서버 접속시 필요한 패스워드를 뜻함. 게임베이스에서는 콘솔에 있는 SecretKey에 대응)
    //HOST : 실행할 주소 (서버의 주소를 뜻함. 게임베이스에서는 api-gamebase.nhncloudservice.com주소. 주의!! UriBuilder 클래스 사용시에는 https:// 제거하고 추가할것. )
    //PATH : 호출할 API가 존재하는 경로.
    //QUERY : 호출시 사용할 쿼리. (주의!! UriBuilder 사용시 path에 쿼리 포함해서 넣는경우 ?가 %3으로 변경됨. 그렇기떄문에 해당 클래스의 Query값에 직접 Query문을 추가해줘야한다.)

    /// <summary>
    /// REST API의 필수 키를 들고있는 클래스.
    /// </summary>
    public static class REST_API_MANAGER
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
        private static string m_appkey;
        public static string APPKEY => m_appkey;
        private static string m_secretkey;
        public static string SECRET_KEY => m_secretkey;
        private static string m_host;
        public static string HOST => m_host;
        private static System.Int32 m_timeout;
        public static System.Int32 TimeOut => m_timeout;

        public static void Initiallize(string _appkey, string _secretkey, string _host, System.Int32 _timeout)
        {
            m_appkey = _appkey;
            m_secretkey = _secretkey;
            m_host = _host;
            m_timeout = _timeout;
        }
    }

    public class REST_API_wrapper<T> where T : Response.Response_Base
    {
        protected string m_appkey => REST_API_MANAGER.APPKEY;
        protected string m_secretkey => REST_API_MANAGER.SECRET_KEY;
        protected string m_host => REST_API_MANAGER.HOST;
        /// <summary>
        /// URI 기본 포멧
        /// </summary>
        protected string m_uri;
        protected REST_API_MANAGER.METHOD m_method;
        protected bool m_inited = false;
        /// <summary>
        /// Query 기본 포멧. 없는경우 빈 문자열
        /// </summary>
        protected string m_query;
        /// <summary>
        /// 유니티 웹 리퀘스트 코루틴 밖에서도 확인하기 위해서 멤버변수로 선언.
        /// </summary>
        protected UnityEngine.Networking.UnityWebRequest m_webRequest;
        private REST_API_MANAGER.STATE m_state;
        public REST_API_MANAGER.STATE State => m_state;


        public REST_API_wrapper(REST_API_MANAGER.METHOD _method, string _uri, string _query)
        {
            m_method = _method;
            m_uri = _uri;
            m_inited = false;
            m_query = _query;
        }
        
        protected string URI_Replacer(Dictionary<string, string> _parameters)
        {
            if (_parameters == null)
                return m_uri;

            string result = m_uri;
            foreach (var key in _parameters.Keys)
            {
                result = result.Replace(key, _parameters[key]);
            }
            return result;
        }

        protected string Query_Replacer(Dictionary<string, string> _parameters)
        {
            if (_parameters == null)
                return m_query;

            string result = m_query;
            foreach (var key in _parameters.Keys)
            {
                result = result.Replace(key, _parameters[key]);
            }
            return result;
        }
        
        protected T ConvertJSON(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// REST API 호출.
        /// </summary>
        /// <param name="_callback"> 완료 콜백 호출</param>
        /// <param name="_path_value"> 유저네임, 패스워드등 URI 상 파라미터 값 </param>
        /// <param name="_query_parameter">쿼리 파라미터 값</param>
        /// <param name="_POST_body"> POST 호출시 필요한 값</param>
        /// <param name="_head_parameter"> 헤더 값</param>
        /// <returns></returns>
        public virtual async Task Request(Action<T> _callback, Dictionary<string, string> _path_value = null, Dictionary<string, string> _head_parameter = null,Dictionary<string, string> _query_parameter = null, string _POST_body = null)
        {
            await Request_UnityWebRequest(_callback,_path_value, _head_parameter, _query_parameter,_POST_body);
        }
        protected virtual async Task Request_UnityWebRequest(Action<T> _callback, Dictionary<string, string> _path_value = null, Dictionary<string, string> _head_parameter = null, Dictionary<string, string> _query_parameter = null, string _post_data = null)
        {
            m_webRequest = GetRequest_UnityWebRequest(_path_value, _head_parameter, _query_parameter, _post_data);
            IEnumerator task = WebRequest_UnityWebRequest(m_webRequest, _callback);
            task.MoveNext();
            for (int time = REST_API_MANAGER.TimeOut; time >= 0; --time)
            {
                await Task.Delay(1);
                if (m_webRequest.isDone)
                {
                    task.MoveNext();
                    break;
                }
            }
        }

        protected IEnumerator WebRequest_UnityWebRequest(UnityEngine.Networking.UnityWebRequest _request, Action<T> _callback)
        {
            m_state = REST_API_MANAGER.STATE.WAITTING_FOR_RESPONSE;
            yield return _request.SendWebRequest();
            m_state = REST_API_MANAGER.STATE.RESPONSE_RECIEVED;
            UnityEngine.Debug.Log("SendWebRequest. Finished");

            if (m_webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                        m_webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError ||
                        m_webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.DataProcessingError)
            {
                UnityEngine.Debug.LogError("UnityWebRequest_completed, Error : " + m_webRequest.error);
            }
            else
            {
                UnityEngine.Debug.Log(m_webRequest.downloadHandler.text);
                _callback?.Invoke(ConvertJSON(m_webRequest.downloadHandler.text));
            }
        }

        protected UnityEngine.Networking.UnityWebRequest GetRequest_UnityWebRequest(Dictionary<string, string> _path_value, Dictionary<string, string> _header_parameter = null, Dictionary<string, string> _query_parameter = null, string _post_param = null)
        {
            //URI 생성
            var uriBuilder = new UriBuilder("https", m_host) { Path = URI_Replacer(_path_value) };
            uriBuilder.UserName = m_appkey;
            uriBuilder.Password = m_secretkey;
            if (_query_parameter != null)
                uriBuilder.Query = Query_Replacer(_query_parameter);


            UnityEngine.Networking.UnityWebRequest request;

            switch (m_method)
            {
                case REST_API_MANAGER.METHOD.GET:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Get(uriBuilder.Uri);
                    }
                    break;
                case REST_API_MANAGER.METHOD.POST:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Post(uriBuilder.Uri, _post_param);
                    }
                    break;
                case REST_API_MANAGER.METHOD.PUT:
                    {
                        request = UnityEngine.Networking.UnityWebRequest.Put(uriBuilder.Uri, "");
                    }
                    break;
                case REST_API_MANAGER.METHOD.DELETE:
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
            if(_header_parameter != null)
            {
                foreach( var key in _header_parameter.Keys)
                {
                    request.SetRequestHeader(key, _header_parameter[key]);
                }
            }

            UnityEngine.Debug.Log(request.url);
            //Debug.Log(request.uri);

            return request;
        }

        protected UnityEngine.Networking.UnityWebRequest GetRequest_UnityWebRequest(Dictionary<string, string> _path_value, Dictionary<string, string> _query_parameter = null, byte[] _post_param = null)
        {
            //URI 생성
            var uriBuilder = new UriBuilder("https", m_host) { Path = URI_Replacer(_path_value) };
            uriBuilder.UserName = m_appkey;
            uriBuilder.Password = m_secretkey;
            if (_query_parameter != null)
                uriBuilder.Query = Query_Replacer(_query_parameter);

            UnityEngine.Networking.UnityWebRequest request = new UnityEngine.Networking.UnityWebRequest(uriBuilder.Uri);

            request.method = m_method.ToString();
            request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(_post_param);
            request.uploadHandler.contentType = "application/json; charset=utf8";
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            request.timeout = REST_API_MANAGER.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            request.SetRequestHeader("X-Secret-Key", m_secretkey);

            UnityEngine.Debug.Log(request.url);

            return request;
        }

        ///HttpWebRequest, HttpClient 사용한 래핑. 삭제 또는 개편 필요.
        /// httpClient 사용시  . Net Core 사용시에는 GET 메소드를 사용해도 body를 보낼수 있지만, 유니티, .NET Framework 상에서는 보낼수 없음. 해당상황 해소시 변경 가능성 있음.
        /*
        protected static byte[] ToJsonBinary<T>(T data)
        {
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(stream1, data);

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            var jsonBody = sr.ReadToEnd();

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonBody);
            return byteArray;
        }
        public virtual async Task Request_DOTNET_HttpClient(Action<T> _callback, Dictionary<string, string> _parameters_head, Dictionary<string, string> _query_parameter, string _parameters_body = null)
        {
            await WebRequest_DOTNET_HttpClient(GetRequest_DOTNET_HttpClient(_parameters_head, _query_parameter, _parameters_body), _callback);
        }

        private System.Net.Http.HttpRequestMessage GetRequest_DOTNET_HttpClient(Dictionary<string, string> _parameters_head, Dictionary<string, string> _query_parameter, string _parameters_body = null)
        {
            //URI 생성
            var uriBuilder = new UriBuilder("https", m_host) { Path = URI_Replacer(_parameters_head) };
            uriBuilder.UserName = m_appkey;
            uriBuilder.Password = m_secretkey;
            if (_query_parameter != null)
                uriBuilder.Query = Query_Replacer(_query_parameter);

            System.Net.Http.HttpMethod method;
            switch (m_method)
            {
                case REST_API.METHOD.GET:
                    method = System.Net.Http.HttpMethod.Get;
                    break;
                case REST_API.METHOD.POST:
                    method = System.Net.Http.HttpMethod.Post;
                    break;
                case REST_API.METHOD.DELETE:
                    method = System.Net.Http.HttpMethod.Delete;
                    break;
                case REST_API.METHOD.PUT:
                    method = System.Net.Http.HttpMethod.Put;
                    break;
                default:
                    method = System.Net.Http.HttpMethod.Get;
                    break;

            }

            var request = new HttpRequestMessage(method, uriBuilder.Uri)
            {
                Content = new StringContent(_parameters_body, Encoding.UTF8, "application/json; charset=utf8"),
            };
            request.Headers.Add("X-Secret-Key", m_secretkey);
            return request;
        }

        private async Task WebRequest_DOTNET_HttpClient(HttpRequestMessage _request, Action<T> _callback)
        {
            m_state = REST_API.STATE.WAITTING_FOR_RESPONSE; var stream1 = new MemoryStream();
            System.Net.Http.HttpClient _client = new HttpClient();
            //_client.Timeout = new TimeSpan(1000);
            HttpResponseMessage response = null;
            try
            {
                response = await _client.SendAsync(_request);
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nHttpRequestException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (ArgumentNullException e)
            {
                Debug.Log("\nArgumentNullException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (InvalidOperationException e)
            {
                Debug.Log("\nInvalidOperationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log("\nTaskCanceledException Caught!");
                Debug.Log("Message :" + e.Message);
            }

            if (response == null)
                return;

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nHttpRequestException Caught!");
                Debug.Log("Message :" + e.Message);
            }

            try
            {
                string str = await response.Content.ReadAsStringAsync();
                Debug.Log(str);
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nHttpRequestException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (ArgumentNullException e)
            {
                Debug.Log("\nArgumentNullException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (InvalidOperationException e)
            {
                Debug.Log("\nInvalidOperationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log("\nTaskCanceledException Caught!");
                Debug.Log("Message :" + e.Message);
            }
        }

        public virtual async Task Request_DOTNET_WebRequest(Action<T> _callback, Dictionary<string, string> _parameters_head, Dictionary<string, string> _query_parameter = null, byte[] _parameters_body = null)
        {
            await WebRequest_DOTNET_WebRequest(GetRequest_DOTNET_WebRequest(_parameters_head, _query_parameter, _parameters_body), _callback);
        }

        private HttpWebRequest GetRequest_DOTNET_WebRequest(Dictionary<string, string> _parameters_head, Dictionary<string, string> _query_parameter, byte[] _parameters_body = null)
        {
            //URI 생성
            var uriBuilder = new UriBuilder("https", m_host) { Path = URI_Replacer(_parameters_head) };
            uriBuilder.UserName = m_appkey;
            uriBuilder.Password = m_secretkey;
            if (_query_parameter != null)
                uriBuilder.Query = Query_Replacer(_query_parameter);

            HttpWebRequest request = HttpWebRequest.CreateHttp(uriBuilder.Uri);
            request.Method = m_method.ToString();
            request.ContentType = "application/json; charset=utf8";
            request.Headers.Add("X-Secret-Key", m_secretkey);
            request.ContentLength = _parameters_body == null ? 0 : _parameters_body.Length;

            try
            {
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(_parameters_body, 0, _parameters_body.Length);
                }
            }
            catch (ProtocolViolationException e)
            {
                Debug.Log("\nProtocolViolationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (InvalidOperationException e)
            {
                Debug.Log("\nInvalidOperationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (NotSupportedException e)
            {
                Debug.Log("\nNotSupportedException Caught!");
                Debug.Log("Message :" + e.Message);
            }


            return request;
        }

        private async Task WebRequest_DOTNET_WebRequest(WebRequest _request, Action<T> _callback)
        {
            WebResponse resp = null;
            try
            {
                resp = await _request.GetResponseAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nHttpRequestException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (ArgumentNullException e)
            {
                Debug.Log("\nArgumentNullException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (InvalidOperationException e)
            {
                Debug.Log("\nInvalidOperationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log("\nTaskCanceledException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            if (resp == null)
                return;
            Stream stream = null;
            try
            {
                stream = resp.GetResponseStream();
            }
            catch (ProtocolViolationException e)
            {
                Debug.Log("\nProtocolViolationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (ObjectDisposedException e)
            {
                Debug.Log("\nObjectDisposedException Caught!");
                Debug.Log("Message :" + e.Message);
            }

            if (stream == null)
                return;
            try
            {
                string result = "";
                StreamReader streamReader = new StreamReader(stream);
                char[] read = new char[256];
                int count = streamReader.Read(read, 0, 256);
                while (count > 0)
                {
                    string str = new string(read, 0, count);
                    result += str;
                    count = streamReader.Read(read, 0, 256);
                }
                Debug.Log(result);
                resp.Close();
                streamReader.Close();
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nHttpRequestException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (ArgumentNullException e)
            {
                Debug.Log("\nArgumentNullException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (InvalidOperationException e)
            {
                Debug.Log("\nInvalidOperationException Caught!");
                Debug.Log("Message :" + e.Message);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log("\nTaskCanceledException Caught!");
                Debug.Log("Message :" + e.Message);
            }
        }
        */
    }


}
