using BBF.REST_API;
using BBF.REST_API.Response.Gamebase;

namespace BBF
{
    public class Gamebase_REST_API_Wrapper
    {
        //구조 : public static REST_API_wrapper<RESPONSE> PROTOCOL_NAME = new REST_API_wrapper<RESPONSE>
        //(
        //    REST_API_MANAGER.METHOD, : HTTP 메소드
        //    PATH_ORIGIN,  : 원본 패스
        //    QUERY : 원본 쿼리
        //);

        public static REST_API_wrapper<TokenAuthentication> TOKEN_AUTHERNTICATION = new REST_API_wrapper<TokenAuthentication>
            (
                REST_API_MANAGER.METHOD.GET,
                "/tcgb-gateway/v1.3/apps/{appId}/members/{userId}/tokens/{accessToken}",
                "linkedIdP={linkedIdP}"
            );
        public static REST_API_wrapper<Get_IdP_Token_and_Profiles> GET_IDP_TOKEN_AND_PROFILES = new REST_API_wrapper<Get_IdP_Token_and_Profiles>
            (
                REST_API_MANAGER.METHOD.GET,
                "/tcgb-gateway/v1.3/apps/{appId}/members/{userId}/idps/{idPCode}",
                "accessToken={accessToken}"
            );
        public static REST_API_wrapper<Get_Simple_Launching> GET_SIMPLE_LAUNCHING = new REST_API_wrapper<Get_Simple_Launching>
            (
                REST_API_MANAGER.METHOD.GET,
                "/tcgb-launching/v1.3/apps/{appId}/launching/simple",
                "osCode={osCode}&storeCode={storeCode}&clientVersion={clientVersion}"
             );
        public static REST_API_wrapper<Get_Member> GET_MEMBER = new REST_API_wrapper<Get_Member>
            (
                REST_API_MANAGER.METHOD.GET,
                "/tcgb-member/v1.3/apps/{appId}/members/{userId}",
                ""
            );
        public static REST_API_wrapper<Get_Members> GET_MEMBERS = new REST_API_wrapper<Get_Members>
            (
                REST_API_MANAGER.METHOD.POST,
                "/tcgb-member/v1.3/apps/{appId}/members",
                ""
            );
        public static REST_API_wrapper<Get_IDP_Information> GET_IDP_INFORMATION = new REST_API_wrapper<Get_IDP_Information>
            (
                REST_API_MANAGER.METHOD.POST,
                "/tcgb-member/v1.3/apps/{appId}/auth/authKeys",
                ""
            );
        public static REST_API_wrapper<Get_UserId_Information_with_Auth_key> GET_USERID_INFORMATION_WITH_AUTH_KEY = new REST_API_wrapper<Get_UserId_Information_with_Auth_key>
            (
                REST_API_MANAGER.METHOD.POST,
                "/tcgb-member/v1.3/apps/{appId}/members/userIds/authKeys",
                "authSystem={authSystem}"
            );
    }

    public enum Gamebase_REST_API_Types
    {
        TOKEN_AUTHERNTICATION,
        GET_IDP_TOKEN_AND_PROFILES,
        GET_SIMPLE_LAUNCHING,
        GET_MEMBER,
        GET_MEMBERS,
        GET_IDP_INFORMATION,
        GET_USERID_INFORMATION_WITH_AUTH_KEY,
    }

}
