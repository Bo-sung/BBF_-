# SDK분석.
## SDKSettings 파일 

| 타입 | 변수명 | 설명 | SDK |
|------|--------|------|-----|
| bool | isEnableLog| 로그 OnOff여부 |-|
| bool | isTestAds| 테스트 AD 여부 |-|
| string | facebookID| 페북 SDK ID |FaceBook|
| string | applovinSDKKey| appLovin SDK ID |applovin|
| string | appsFlyerKey| AppsFlyer SDK Key |appsFlyer|
| string | oneSignalAppID| OneSignal 앱아이디 |oneSignal|
| string | appleAppID| Apple 앱아이디 |apple|
| string | ironSource_AppKey| 아이언소스 AppKey |IronSource|
| int | banner_Placement_Size| 배너 Placement 수량 설정 |-|
| int | interstitial_Placement_Size| 전면광고 Placement 수량 설정 |-|
| int | reward_Placement_Size | 리워드 Placement 수량 설정 |-|
| SDKPlatformSettings | admobAppID | AdMob 앱아이디 |AdMob|
| SDKPlatformSettings[] | admobNativeID | AdMobNative 앱아이디 | AdMob|
| SDKPlatformSettings[] | maxBannerID | MAX 배너 광고 Placement ID |applovin|
| SDKPlatformSettings[] | maxInterstitialID | MAX 전면 광고 Placement ID |applovin|
| SDKPlatformSettings[] | maxRewardID | MAX 리워드 광고 Placement ID |applovin|
| SDKPlatformSettings[] | ironSourceBannerID | 아이언소스 배너 광고 Placement ID |IronSource|
| SDKPlatformSettings[] | ironSourceInterstitialID | 아이언소스 전면 광고 Placement ID |IronSource|
| SDKPlatformSettings[] | ironSourceRewardID | 아이언소스 리워드 광고 Placement ID |IronSource|

[코드](/분석/Code/SDKSetting.cs)

# SDK 사용처 및 대체 여부  
| SDK | 사용처 | 상태 | 대체제 |비고|
|----|----|----|----|----|
|파이어베이스 애널리틱스|레벨 및 아이템 로그 저장 및 전송|대체 예정|게임베이스|-|   
|파이어베이스 크래쉬리틱스| 크래시 및 로그 저장 및 전송|제거 예정|-|-|  
|GPGS(구글 플레이 게임 서비스)| 안드로이드 플랫폼상 게임 진행 정보 저장 및 랭킹 표시를 위해 필수적| 활성화|-|IOS 플랫폼을 위한 대체제 추가 필요.|     
|애드몹 네이티브| 설정 및 종료창 배너|대체 혹은 제거 예정|아이언소스|아이언소스에 MREC가 있지만 유니티 Ads가 지원하지 않음.|  
|페이스북|사용위치 없음|제거|-|-|
|MAX|배너/전면광고/보상형 광고|대체 예정|아이언소스|-|
|OneSignal|푸시알림 및 푸시 송수신|대체 예정|게임베이스|-|
|Appsflyer|결제/광고 정보 전송|유지 혹은 대체 예정|게임베이스|게임베이스로 대체 가능하지만 기능적인 차이 존재|
|GameAnalyticsSDK| 레벨 및 아이템 로그 저장 및 전송| 대체 예정| 게임베이스| 전달받은 메일에 미포함되어있음.|
|GameBase|푸시알림,매출통계,소셜로그인,|추가중|-|추가작업중|
|IronSource|전면/배너/리워드광고|추가중|-|-|

*****

# 기타이슈
1. ~~애드몹 네이티브의 경우 사용처가 종료여부 물어보는 팝업 및 설정 팝업에서만 사용되므로 제거 또는 컨텐츠 수정 등으로 대처가 가능할것으로 예상.  
아이언 소스 내 적응형 배너 기능이 있으나 단일 화면 내 복수의 배너 표시가능 여부에 대한 **검증이 필요한바** 완전한 대체 여부를 확정지을수 없음.~~  
    A. 해당사항 검증 결과  
    복수의 배너 출력 불가능. 아이언소스의 경우 배너광고를 직접적으로 제공하지 않지만 타 업체의 배너를 받아와 대신 출력할수 있음.  
    -> Unity Ads를 배너를 위해 사용.  
    -> 문제점 발생. 유니티 Ads의 경우 중앙 사각형 광고(MREC) 를 사용할수 없음. <sup id="a2">[참고자료](#2)</sup>

2. 게임베이스 analytics에는 구매 및 유저접속 관련 통계 및 분석 기능은 많지만. 스테이지 달성도. 아이템 보유량, 아이템 사용시간과 같은 항목를 따로 추가할수가 없음. 그렇기에 AppsFlyer 혹은 파이어베이스 에널리틱스. 등의 분석관련된 서비스를 이용하거나, 같은 NHN클라우드의 서비스인 Log & Crash Serch 서비스를 사용하여 로그를 남기는 방법이 있음. 두 방법 모두 추가 비용 발생가능.

3. GPGS(구글플레이 게임서비스)의 경우 iOS 지원이 중단되어 클라우드 서비스 사용이 불가능함. 그렇기에 iOS의 경우 GameKit을 사용해야함. 만약 안드로이드에서 저장한 유저가  IOS 기기에서 동일한 구글계정으로 로그인하는 경우 저장 데이터를 불러올수가 없음.
같은 문제로 안드로이드 기기는 구글. iOS기기는 애플 서비스를 사용하므로 랭킹 및 세이브가 플랫폼 종속적이게 됨. -> 이를 해결하기 위해서는 별도의 랭킹 및 세이브 기능이 있는 서버가 필요하므로 해당사항 검토 필요.


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
메인페이지로 [돌아가기](/README.md)