# IMPLEMENTATION_REPORT.md — 백엔드 프로토타입 구현 최종 보고서

본 보고서는 Factory Simulation Game의 핵심 백엔드 시스템 프로토타입(Step 1 ~ Step 7) 구현 내용과 결과에 대해 기록합니다. 

---

## 1. 구현된 클래스 및 인터페이스 목록

모든 신규 소스 코드는 지침에 따라 `Assets/Scripts/Backend/` 디렉토리 하위에 배치되었으며, C#의 1파일 1클래스 원칙을 준수하여 작성되었습니다.

| 번호 | 분류 | 파일/클래스명 | 절대 경로 | 설명 |
| :--- | :--- | :--- | :--- | :--- |
| 1 | **Event** | [GameEvent.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/GameEvent.cs) | `Assets/Scripts/Backend/GameEvent.cs` | 모든 이벤트의 최상위 추상 클래스 |
| 2 | **Event** | [RoundEndEvent.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/RoundEndEvent.cs) | `Assets/Scripts/Backend/RoundEndEvent.cs` | 라운드 종료 통계 및 통과 여부를 담은 이벤트 |
| 3 | **Event** | [ItemSoldEvent.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/ItemSoldEvent.cs) | `Assets/Scripts/Backend/ItemSoldEvent.cs` | 아이템 판매 가격, 불량 여부 및 스탯을 담은 이벤트 |
| 4 | **Event** | [MarketEvent.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/MarketEvent.cs) | `Assets/Scripts/Backend/MarketEvent.cs` | 시장 상황 변동(호황/불황)을 전달하는 이벤트 |
| 5 | **Event** | [BankruptcyEvent.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/BankruptcyEvent.cs) | `Assets/Scripts/Backend/BankruptcyEvent.cs` | 파산 게이지 도달 정도를 통지하는 이벤트 |
| 6 | **Enum** | [EventType.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/EventType.cs) | `Assets/Scripts/Backend/EventType.cs` | 시장 호황(Boom) 및 불황(Recession) 상태 정의 |
| 7 | **Enum** | [StatType.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/StatType.cs) | `Assets/Scripts/Backend/StatType.cs` | 아이템 강화 스탯(공격력, 내구도, 화려함) 정의 |
| 8 | **Enum** | [MachineType.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/MachineType.cs) | `Assets/Scripts/Backend/MachineType.cs` | 설치 가능한 기계 종류(Grinder, Welder, Painter) 정의 |
| 9 | **Interface** | [IGameEventListener.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/IGameEventListener.cs) | `Assets/Scripts/Backend/IGameEventListener.cs` | 이벤트 버스 구독을 위한 공통 리스너 인터페이스 |
| 10 | **Interface** | [IManager.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/IManager.cs) | `Assets/Scripts/Backend/IManager.cs` | 모든 매니저 클래스들이 구현할 관리 인터페이스 |
| 11 | **Interface** | [IUpgradable.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/IUpgradable.cs) | `Assets/Scripts/Backend/IUpgradable.cs` | 기계에 의해 강화 가능한 대상의 인터페이스 |
| 12 | **Interface** | [ISellable.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/ISellable.cs) | `Assets/Scripts/Backend/ISellable.cs` | 판매 시스템을 통해 상점 거래가 가능한 대상의 인터페이스 |
| 13 | **Core** | [EventBus.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/EventBus.cs) | `Assets/Scripts/Backend/EventBus.cs` | 단방향 이벤트 전달을 처리하는 Pure C# 싱글톤 |
| 14 | **Core** | [FactoryStatus.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/FactoryStatus.cs) | `Assets/Scripts/Backend/FactoryStatus.cs` | 재화 및 파산, 브랜드 단계를 총괄하는 Pure C# 싱글톤 |
| 15 | **Core** | [Item.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/Item.cs) | `Assets/Scripts/Backend/Item.cs` | 강화 및 판매가 가능한 아이템의 Pure C# 최상위 추상 클래스 |
| 16 | **Core** | [StandardItem.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/StandardItem.cs) | `Assets/Scripts/Backend/StandardItem.cs` | ItemSpawner가 스폰하는 실제 구체화 아이템 클래스 |
| 17 | **Mono** | [Machine.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/Machine.cs) | `Assets/Scripts/Backend/Machine.cs` | 벨트 위 아이템을 강화하는 기계류 최상위 추상 MonoBehaviour |
| 18 | **Mono** | [Grinder.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/Grinder.cs) | `Assets/Scripts/Backend/Grinder.cs` | 공격력(AP)을 강화하는 구체 기계 (불량 발생 가능) |
| 19 | **Mono** | [Welder.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/Welder.cs) | `Assets/Scripts/Backend/Welder.cs` | 내구도(Durability)를 강화하는 구체 기계 (불량 발생 가능) |
| 20 | **Mono** | [Painter.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/Painter.cs) | `Assets/Scripts/Backend/Painter.cs` | 화려함(Splendor)을 강화하는 구체 기계 (불량 발생 안함) |
| 21 | **Mono** | [BeltTrack.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/BeltTrack.cs) | `Assets/Scripts/Backend/BeltTrack.cs` | 아이템들을 물리적인 거리에 따라 이동 및 관리하는 MonoBehaviour |
| 22 | **Mono** | [ItemSpawner.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/ItemSpawner.cs) | `Assets/Scripts/Backend/ItemSpawner.cs` | 설정 주기마다 기초 아이템을 스폰하여 벨트에 배치하는 MonoBehaviour |
| 23 | **Mono** | [SellManager.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/SellManager.cs) | `Assets/Scripts/Backend/SellManager.cs` | 판매 가격 산출, 벌금 부과 및 이벤트를 발행하는 MonoBehaviour |
| 24 | **Mono** | [RoundManager.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/RoundManager.cs) | `Assets/Scripts/Backend/RoundManager.cs` | 라운드 시간 측정, AP 목표치 비교 및 보상/벌칙을 총괄하는 MonoBehaviour |
| 25 | **Mono** | [GameManager.cs](file:///c:/Codes/OOPS/OOPS-Project/Assets/Scripts/Backend/GameManager.cs) | `Assets/Scripts/Backend/GameManager.cs` | 씬 전체의 기계 배치/삭제/업그레이드 및 라이프사이클을 조율하는 MonoBehaviour |

---

## 2. 주요 설계 결정 사항 (Key Design Decisions)

1. **C# 표준 프로퍼티 적극 활용**:
   * `AGENTS.md`의 읽기 전용 상태 노출 규칙(`{ get; private set; }` 또는 `{ get; protected set; }`)을 준수하여 불필요한 Getter/Setter 메소드의 중복 생성을 배제하고 깔끔하고 직관적인 C# 스타일의 변수를 활용했습니다.
2. **FactoryStatus 재화 음수 허용**:
   * 마이너스 통장 상태를 구현하여, 재화가 0 미만으로 떨어졌을 때 파산 바 게이지가 가산될 수 있도록 `ModifyMoney()`의 `0` 클램핑을 완전 제거하였습니다.
3. **`Configure(BeltTrack)` 동적 초기화 도입**:
   * `Machine` 추상 클래스에 `Configure(BeltTrack)` 메소드를 구현하여, `GameManager`를 통해 기계를 동적으로 생성하고 슬롯에 배치할 때 인스펙터 참조 없이 씬 상의 `BeltTrack`과 유연하게 엮이도록 설계했습니다.
4. **아이템 스탯 전달 구조 개선**:
   * `RoundManager`가 라운드 통과 여부를 검증하기 위해 판매된 아이템들의 평균 공격력(AP)을 알아야 하므로, `ItemSoldEvent`에 `AttackPower` 속성을 확장하여 누락 없는 루프 통신을 가능케 했습니다.

---

## 3. DESIGN.md 및 BACKEND.puml과의 차이점 및 이유

1. **`StandardItem` 구체 클래스 추가**:
   * **차이**: `BACKEND.puml`에는 `Item` 추상 클래스만 기술되어 있었으나, 실제 `ItemSpawner`가 아이템 인스턴스를 인스턴스화해야 하므로 상속받은 구체 클래스인 `StandardItem`을 신규 설계 및 구현했습니다.
2. **UML Getter 메소드 제거 및 프로퍼티 치환**:
   * **차이**: 다이어그램 상의 `getMoney()`, `getBrandLevel()` 등 Java 스타일의 메소드들을 C#의 프로퍼티인 `Money`, `BrandLevel` 등으로 치환하여 `BACKEND.puml` 다이어그램 명세를 함께 업데이트했습니다.
3. **`MachineType`과 `StatType` Enum 신규 명문화**:
   * **차이**: UML상에서 생략되었거나 타입 이름으로만 명시되어 있던 Enum들을 신규 C# 파일로 작성하고 `BACKEND.puml`에 명시화하여 프로토타입의 컴파일 및 로직 완결성을 높였습니다.

---

## 4. 최종 컴파일 결과 (Final Compilation Status)

* **빌드 결과**: **정상 컴파일 완료 (Tundra build success)**
* **에러 및 경고**: **0 Errors, 0 Warnings**
* **안정성 검증**: Unity 6+ (6000.3.15f1) 엔진 CLI 빌드 환경과 백그라운드 엔진 프로세스 두 경로 모두에서 전혀 컴파일 오류 없이 정상 로드됨을 확인하였습니다.
