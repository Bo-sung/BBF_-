namespace BBF.REST_API.Response
{
    namespace Gamebase
    {
        public class Response_GameBase : Response_Base
        {
            public Header header;
        }

        public class LinkedIdP
        {
            public string idPCode;
            public string idPId;
        }

        public class Header
        {
            public int resultCode;
            public string resultMessage;
            public string transactionId;
            public TraceError traceError;
            public bool isSuccessful;
        }

        public class TraceError
        {
            public long trackingTime;
            public string throwPoint;
            public string uri;
        }

        public class Member
        {
            public string appId;
            public string userId;
            public string valid;
            public System.DateTime regDate;
            public string lastLoginDate;
            public AuthList[] authList;
        }

        public class AuthList
        {
            public string userId;
            public string authSystem;
            public string idPCode;
            public string authKey;
            public System.DateTime regDate;
        }

        public class TemporaryWithdrawal
        {
            public string gracePeriodDate;
        }

        public class IdpProfile
        {
            public string sub;
            public string name;
            public string given_name;
            public string locale;
            public string picture;
        }

        public class IdpToken
        {
            public string idPCode;
            public string accessToken;
        }

        public class AccessInfo
        {
            public string serverAddress;
            public string csInfo;
        }

        public class RelatedUrls
        {
            public string termsUrl;
            public string csUrl;
            public string punishRuleUrl;
            public string personalInfoCollectionUrl;
        }

        public class Install
        {
            public string url;
        }

        public class Status
        {
            public int code;
            public string message;
        }

        public class App
        {
            public string storeCode;
            public AccessInfo accessInfo;
            public RelatedUrls relatedUrls;
            public Install install;
        }

        public class Maintenance
        {
            public string typeCode;
            public System.DateTime beginDate;
            public System.DateTime endDate;
            public string url;
            public string reason;
            public string message;
        }

        public class MemberInfo
        {
            public string deviceCountryCode;
            public string usimCountryCode;
            public string language;
            public string osCode;
            public string telecom;
            public string storeCode;
            public string network;
            public string deviceModel;
            public string osVersion;
            public string sdkVersion;
            public string clientVersion;
        }

        public class MemberList
        {
            public string userId;
            public string valid;
            public string appId;
            public System.DateTime regDate;
        }

        public class Result
        {
            public string authKey;
            public string idPCode;
            public string authSystem;
        }

    }
}