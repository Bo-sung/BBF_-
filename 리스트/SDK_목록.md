# SDK 목록
## 단순 목록
    파이어베이스 애널리틱스,  
    파이어베이스 크래쉬리틱스,  
    구글 플레이 게임 서비스,  
    애드몹 네이티브,  
    MAX(applovin 광고),  
    페이스북,  
    OneSignal,  
    Appsflyer  
    GameAnalyticsSDK,  

## 1. 광고 플러그인
    MAX
        상단 배너
        매일 무료 코인광고(1일3회)
        별 보상 3배 광고
        볼 50개 추가 광고
        코인 50개 광고
        상점 코인 50개 무료 광고
        인게임 코인 보상 광고
        불시 광고

    AdMob
        설정창 중앙 배너
        종료창 중앙 배너
## 2. AppsFlyer
    결제 정보 전송
    광고 시청 정보 전송
    광고 클릭 정보 전송

## 3. Firebase Analize
    레벨 시작 로그 저장 및 전송
    레벨 완료 로그 저장 및 전송
    레벨 실패 로그 저장 및 전송
    아이템 획득 로그 저장 및 전송
    아이템 사용 로그 저장 및 전송
## 4. Firebase Crashlytics
    게임 크래시 및 로그 저장 및 전송

## 5. Google Play Game Service (GPGS)
    5.1. GooglePlayGames.BasicApi.SavedGame
        게임 진행상황 저장 및 구글서버 전송      
    5.2 소셜
        랭킹 표시를 위한 호출

## 6. OneSignal
    푸쉬 송수신

## 7. FaceBook
    사용처 없음.

## 8. GameAnalyticsSDK
    레벨 시작 로그 저장 및 전송
    레벨 완료 로그 저장 및 전송
    레벨 실패 로그 저장 및 전송
    아이템 획득 로그 저장 및 전송
    아이템 사용 로그 저장 및 전송


# 대체할 SDK  
파이어베이스 애널리틱스   
 - 대체 (게임베이스)  
 
 파이어베이스 크래쉬리틱스  
 - 제거  
 
구글 플레이 게임 서비스  
- 유지 (게임 진행 정보 저장 및 랭킹 표시를 위해 필수적)  

애드몹 네이티브  
- 보류 (중앙 배너 광고용. 아이언 소스에 동일한 광고 존재하지 않음).  

페이스북  
- 제거 (현재 사용중인 기능 없음.)  

MAX  
- 대체 (아이언소스)  

OneSignal  
- 대체 (게임베이스) 

Appsflyer  
- 보류 (게임베이스로 대체 가능하지만 기능적인 차이 존재)  

GameAnalyticsSDK  
- 대체 (게임베이스)(전달받은 메일에 미포함되어있음.)  

*****

## 결론
게임베이스로 대체 
- 파이어베이스 애널리틱스 
- OneSignal  
- Appsflyer(유지 가능성 존재).  
- GameAnalyticsSDK 

아이언소스로 대체
- 애드몹 네이티브,  
- MAX(applovin 광고), 

유지
- 구글 플레이 게임 서비스,  
- Appsflyer(대체 가능성 존재)

제거
- 파이어베이스 크래쉬리틱스, 
- 페이스북,  

# 기타의견.
1. ~~애드몹 네이티브의 경우 사용처가 종료여부 물어보는 팝업 및 설정 팝업에서만 사용되므로 제거 또는 컨텐츠 수정 등으로 대처가 가능할것으로 예상.  
아이언 소스 내 적응형 배너 기능이 있으나 단일 화면 내 복수의 배너 표시가능 여부에 대한 **검증이 필요한바** 완전한 대체 여부를 확정지을수 없음.~~  
1.1 해당사항 검증 결과  
    복수의 배너 출력 불가능.  
    아이언소스의 경우 배너광고를 직접적으로 제공하지 않지만 타 업체의 배너를 받아와 대신 출력할수 있음.  
    -> Unity Ads를 배너를 위해 사용.  
    -> 문제점 발생. 유니티 Ads의 경우 중앙 사각형 광고(MREC) 를 사용할수 없음. <sup id="a2">[참고자료](#2)</sup>

2. 게임베이스 analytics에는 구매 및 유저접속 관련 통계 및 분석 기능은 많지만. 스테이지 달성도. 아이템 보유량, 아이템 사용시간과 같은 항목를 따로 추가할수가 없음. 그렇기에 AppsFlyer 혹은 파이어베이스 에널리틱스. 등의 분석관련된 서비스를 이용하거나, 같은 NHN클라우드의 서비스인 Log & Crash Serch 서비스를 사용하여 로그를 남기는 방법이 있음. 두 방법 모두 추가 비용 발생가능.

3. GPGS(구글플레이 게임서비스)의 경우 iOS 지원이 중단되어 클라우드 서비스 사용이 불가능함. 그렇기에 iOS의 경우 GameKit을 사용해야함. 만약 안드로이드에서 저장한 유저가  IOS 기기에서 동일한 구글계정으로 로그인하는 경우 저장 데이터를 불러올수가 없음.
같은 문제로 안드로이드 기기는 구글. iOS기기는 애플 서비스를 사용하므로 랭킹 및 세이브가 플랫폼 종속적이게 됨. -> 이를 해결하기 위해서는 별도의 랭킹 및 세이브 기능이 있는 서버가 필요하므로 해당사항 검토 필요.

---

<br>

<b id="1"><sup>참고자료</sup></b>
https://developers.is.com/ironsource-mobile/android/banner-integration-android/#step-4..[↩](#a1)<br>

<b id="2"><sup>표</sup></b> 
|     |BANNER|LARGE|RECTANGLE|SMART|   
|---|---|---|---|---|   
|ironSource |   Banner  |   Large Banner    |   Medium Rectangle    |   Banner / Leaderboard    |  
|AdColony   |   Banner  |   Banner  |   Medium Rectangle 	|   Banner / Leaderboard    |
|AdMob      |   Banner  | 	Large Banner*   | 	Medium Rectangle    |   Banner / Leaderboard    |
|AppLovin   | 	Banner  | 	Banner  |   Medium Rectangle    |   Banner / Leaderboard    |
|Chartboost | 	Banner  | 	Banner  |	Medium Rectangle    | 	Leaderboard |
|Meta Audience Network| Banner  |   Large Banner    |   Medium Rectangle    |   Banner / Leaderboard    |  
|Digital Turbine|   Banner  |   Banner  |   Medium Rectangle    |   Banner / Leaderboard    |
|HyprMX     | 	Banner  | – |   Medium Rectangle    |   Banner / Leaderboard    |  
|InMobi     |   Banner  |   Banner  |   Medium Rectangle    |   Banner / Leaderboard|  
|Smaato     | 	Banner  |   –   |   Medium Rectangle    |   Banner / –  |  
|UnityAds   | 	Banner  |   Banner  |   –   |   Banner / Leaderboard    |
|Vungle     | 	Banner  | 	Banner  | 	Medium Rectangle    |   Banner / Leaderboard|

[↩](#a2)<br>

<br>


------
전반적인 분석 [문서로](/전반적인_분석.md)