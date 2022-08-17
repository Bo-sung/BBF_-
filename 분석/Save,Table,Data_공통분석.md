# 공통점

1. Type.GetNestedTypes  
    파생 클래스 내 선언된 클래스들을 Type 배열로 받을수 있는 메소드.  
    사용예)  
        var fieldinfos = typeof(T).GetNestedTypes();

2. Type.GetFields()  
    해당 타입 내 모든 필드를 가져와 FieldInfo 배열로 받을수 있는 메소드  
    사용예)  
        FieldInfo[] fieldInfos = target.GetFields();

3. GetCustomAttribute<T>  
    해당 타입이 지정된 어트리뷰션을 가지고 있다면 들고옴. 만약 없다면 NULL 리턴.  

4. Save/Title/Data 구조 상세  
    Save,Title,Data 클래스는 각각의 테이블,세이브,데이터 정보를 선언한 클래스들의 정의, 로딩을 위한 메소드만 들고있으며, 각 관련 클래스들은 세이브의 경우 SaveLoad를 상속받은 어트리뷰션, 테이블의 경우 TableLoad 어트리뷰션, 데이터의 경우 DataLoad를 상속받은 어트리뷰션을 사용한 static 형태의 리스트와 처리 관련된 정적 메소드를 가지고 있음.  
    굳이 이런 구조를 가지고 있는 이유는 GetNestedTypes과 GetFields와 GetCustomAttribute를 사용한 일괄 처리가 쉽기 때문이다.  
    이를 사용한 일괄 처리 루틴  
    >    예시) Save 클래스  
            GetNestedTypes를 사용해 Save 클래스내 선언된 클래스들의 타입들을 불러옴.  
            불러온 Save 클래스 내 타입들을 GetFields를 사용해 불러옴.  
            불러온 타입에 각각의 어트리뷰션을 키로 데이터를 찾아 넣음.   
    각각 데이터들은 JSON형식으로 PlayerPref에 각 타입의 name값을 키로 저장됨.


# 장점
1. 데어터 타입 추가시 데이터 로드 로직을 수정할 필요가 없음.
2. 파셜 클래스를 사용했기떄문에 빌드시 하나의 데이터 클래스를 여러 파일로 분리해 각각의 폴더에 저장할수 있음 == 파일 분리를 통한 관리의 용이 

# 단점
1. JSON형식의 경우 데이터를 직접적으로 읽을수 있기 떄문에 보안문제 발생 가능.
    ㄴ> JSON파일을 직접 저장하는것이 아닌 PlayerPref에 저장하는 형식이라 직접적으로 읽을수 없음. 만약 이를 읽더라도 직렬화 하기 전 AES 등의 암호화를 거치는경우 어느정도 해결 가능.
2. 특정한 데이터의 로딩 구조등을 확인하기 위해 찾아가기에 복잡함.
    ㄴ> 구조 파악시 시간이 걸릴뿐 큰 문제는 아님
3. JSON 형식을 사용하기떄문에 엑셀을 사용할수 없음
    ㄴ> 컨버터 등을 사용가능.