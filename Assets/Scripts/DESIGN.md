# DESIGN.md — Factory Simulation Game (Unity)

이 문서는 백엔드·프런트엔드 설계 결정과 그 이유, 그리고 두 레이어를 잇는 브리지 구조를 기록한다.
구현 세부사항(클래스 목록, 메서드 시그니처)은 레이어별 UML을 참고하라:

- 백엔드: `BACKEND.puml`
- 프런트엔드: `FRONTEND_VIEW.puml`(브리지·UI 위젯) / `FRONTEND_BLOCK.puml`(블록·입력·기계
  데이터) / `FRONTEND_PANEL.puml`(패널·버튼) — 모두 Assets/Scripts/

에이전트는 클래스 생성/수정 시 반드시 **해당 레이어의 puml**도 함께 업데이트해야 한다.

> 최신화: 2026-06-11 (main, PR #8·#9 머지 직후 기준)

---

## Architecture

### Layer Separation

백엔드와 프런트엔드(UI)는 폴더 단위로 분리된다.

```
Assets/Scripts/Backend/   — 게임 로직 (namespace Backend)
Assets/Scripts/FrontEnd/  — 렌더링·입력·UI (글로벌 namespace)
```

레이어 간 브리지는 두 가지다:

- **EventBus** — 백엔드 매니저 간 publish/subscribe. `ItemSoldEvent`, `RoundEndEvent` 등
  `GameEvent` 파생 이벤트를 전달한다
- **Observer 패턴 (ISubject/IObserver)** — `FrontEnd/ObserverPattern/`에 정의된
  Subject/Observer 인터페이스 쌍. 백엔드 Subject(FactoryStatus, RoundManager, BeltTrack)가
  상태 변화 시 `UIUpdateArgs`(읽기 전용 값 래퍼)를 만들어 UI Observer에게 push한다.
  UI는 백엔드 필드를 직접 읽지 않는다

> 초기 설계의 "Snapshot 구조체"는 구현되지 않았고, `UIUpdateArgs`가 그 역할(읽기 전용
> 상태 복사본 전달)을 대신한다.

### MonoBehaviour vs Pure C#

| 분류 | 클래스 | 이유 |
|---|---|---|
| **MonoBehaviour** | Machine 계열, BeltTrack, ItemSpawner, SellManager, RoundManager, GameManager, Item 계열, FactoryStatus, FrontEnd 전체(UIView·패널·블록 등) | 씬에 배치되거나 좌표 이동/시각 표현/물리 충돌/프레임별 갱신이 필요함 |
| **Pure C#** | EventBus, GameEvent 계열, UIUpdateArgs 계열, RoundParameters, 인터페이스 | 데이터 전용 또는 씬과 무관. MonoBehaviour 오버헤드 불필요 |
| **ScriptableObject** | MachineInfo, MachineInfoList | 기계 타입→프리팹 매핑을 에셋 데이터로 관리 |

모든 클래스를 MonoBehaviour로 만들지 말 것. 위 표를 기준으로 판단하라.

---

## Backend Design Decisions

### 1. EventBus (Observer Pattern)

**결정**: 매니저 간 직접 참조 대신 EventBus를 통한 단방향 통신

**이유**: 매니저들이 서로를 직접 참조하면 의존성이 복잡해지고 테스트가 어려워진다.
EventBus를 경유하면 발행자와 구독자가 서로를 모른 채 통신할 수 있다.

**트레이드오프**: 이벤트 흐름이 코드에서 직접 보이지 않아 디버깅이 다소 어려울 수 있다.
이벤트 타입을 명확하게 네이밍하여 보완한다.

### 2. IManager 인터페이스 (폐기 — refactor/oop-solid Phase 2)

**폐기 사유**: GameManager의 `_managers` 리스트는 채우기만 하고 호출처가 없는 dead code였다.
매니저 간 통신은 EventBus 구독(`IGameEventListener`)으로 이미 일원화되어 있어
`IManager`는 `IGameEventListener`와 시그니처까지 중복이었다. 인터페이스와
RoundManager/SellManager의 구현 선언을 삭제했다.

### 3. BeltTrack + ItemSpawner 분리

**결정**: 기존 ConveyorBelt 하나를 BeltTrack(이동/공간)과 ItemSpawner(생성)로 분리

**이유**: Single Responsibility Principle. 하나의 클래스가 아이템 생성, 이동, 공간 관리를 모두 담당하면
변경 이유가 여러 개가 된다. 분리하면 각자의 레벨업 로직과 동작이 독립적으로 변경 가능하다.

### 4. Item의 MonoBehaviour 전환

**결정**: Item을 Pure C#에서 MonoBehaviour 클래스로 전환

**이유**: 게임 화면상 물리 좌표 이동(MoveItem) 및 시각적 표현(불량 발생 시 머티리얼 색상 빨간색 변경 등)을 컴포넌트와 직접 연계하고, 물리 충돌(Trigger)을 통해 기계 가공 처리를 매끄럽게 처리하기 위함입니다.

**트레이드오프**: 아이템 수가 많아지면 GameObject 관리 비용이 커진다. 성능 문제가 생기면
오브젝트 풀링을 우선 검토할 것.

### 5. FactoryStatus의 MonoBehaviour 전환 및 UI Observer 패턴 적용

**결정**: FactoryStatus를 MonoBehaviour 싱글톤으로 전환하고, FrontEnd UI 화면에 데이터를 전파할 수 있도록 IFactoryStatusSubject(Observer 패턴)를 구현

**이유**: 프레임별로 자금이 음수일 때 파산바 게이지를 실시간으로 가산(Update)하고, 프런트엔드 UI 컴포넌트들이 실시간으로 상태 변경을 구독하여 화면을 갱신하기 위함입니다. 또한 내부 상태 변수를 Dictionary<FactoryStatusType, float>로 추상화하여 상태의 유연한 관리와 전파를 용이하게 하였습니다.

### 6. Machine Configure(BeltTrack) 동적 초기화 (폐기 — refactor/oop-solid Phase 3)

**폐기 사유**: 기계 배치 주체가 GameManager에서 FrontEnd의 BeltBlock으로 이관되면서
`Configure(BeltTrack)`의 유일한 호출처(GameManager.PlaceMachine)가 사라졌다. 실제
씬/프리팹에서도 `_beltTrack`은 전부 null이어서 Update()의 GetNearestItem 폴링 가공
경로는 죽은 코드였다. 가공은 OnTriggerStay 물리 충돌 단일 경로로 일원화하고
Machine의 BeltTrack 의존을 제거했다 — Machine은 이제 벨트 구조를 전혀 모른다.

### 7. 벌금 정책 및 파산 게이지 관리 일원화

**결정**: 불량품 판매 시 고정 벌금액 대신 **아이템 가치 비례 차감**으로 강화하고, 즉발성 파산바 페널티 대신 `FactoryStatus`의 `Update()` 루프를 통해 돈이 마이너스 상태일 때 마이너스 자금 비율에 비례해 매 프레임 파산 게이지가 자연 가산(natural penalty)되도록 일원화함.

### 8. Stat 클래스 — 스탯의 단일 표현 (OCP)

**결정**: 아이템 스탯을 개별 float 필드(AP/DU/SP) 대신 `Stat` 클래스(Pure C#,
`[Serializable]`, 내부 `Dictionary<StatType, float>`)로 일원화. `Item`과 `ItemSpawner`가
Stat을 has-a로 보유하고, 스폰 시 `item.Initialize(_baseStat.Clone())`으로 값을 전달.
`ItemSoldEvent`도 개별 프로퍼티 대신 Stat 스냅샷을 탑재한다.

**이유**: 스탯을 하나 추가할 때 Item(필드+switch 2곳)·ItemSpawner·SellManager·
ItemSoldEvent·RoundManager까지 수정이 번졌다(OCP 위반). Stat 도입 후에는
`StatType` enum 멤버 추가 + `Item.PriceCoefficients` 계수 등록만으로 끝난다.
가격 공식도 StatType→계수 매핑 합산으로 일반화했다.

**직렬화**: 인스펙터에는 `StatEntry{StatType, float}` 리스트로 노출하고
`ISerializationCallbackReceiver`로 Dictionary와 동기화한다 (Dictionary는 Unity 직렬화 불가).

### 9. 게임 흐름 FSM + 게임오버 이벤트화

**결정**: GameManager의 게임 흐름을 `IGameState{Enter/Update/Exit}`(Pure C#) +
`GameStateMachine`으로 관리. 상태는 Ready → Playing ⇄ Paused, Playing → GameOver.
기존 StartGame/PauseGame/ResetGame의 `Time.timeScale` 조작은 상태 Enter로 흡수했다.
게임오버는 매 프레임 `IsGameOver()` 폴링 대신, `FactoryStatus`가 파산바 1.0 도달 시
`BankruptcyEvent`를 1회 발행하고 GameManager가 수신해 GameOver로 전이한다.

**이유**: 게임 흐름 상태가 timeScale 값에 암묵적으로 흩어져 있어 상태 추가(예: 일시정지
메뉴, 라운드 전환 연출)가 어려웠다. FSM으로 상태와 전이를 명시하면 새 상태 추가가
클래스 추가로 끝난다(OCP). 폴링 제거로 GameManager의 Update는 상태 위임만 남는다.

**함께 정리**: GameManager의 기계 배치/강화/제거 경로(PlaceMachine/LevelUpMachine/
RemoveMachine/LevelUpBelt/_placedMachines/기계 프리팹 필드)는 FrontEnd의
BeltBlock + MachineManager + 패널 UI가 대체 완료한 죽은 경로라 전부 삭제했다.

### 10. BeltTrack 타일화

**결정**: 연속 길이(`_trackLength: float`) 대신 타일 수 기반으로 전환.
`TrackLength = _tileCount × _tileSize`, `MachineSpaces = _tileCount`(기계 타일 수),
레벨업 = 타일 +1, 판매 타일(SellBlock)은 트랙 끝(`TrackLength + _tileSize`)에 위치.

**이유**: 벨트의 공간 단위(기계 슬롯·블록 배치·판매 지점)가 모두 1타일 격자로
움직이는데 길이만 연속값이라 `_machineSpaces`/`_trackLength`/SellBlock 위치를 따로
증가시키며 동기화해야 했다. 타일 수 하나로 일원화하면 파생값(길이·슬롯 수·판매 위치)이
모두 계산 프로퍼티가 된다. 함께 삭제: `GetNearestItem`(폴링 가공 폐기), 미사용
`_itemPositions`, 주석 처리된 끝 도달 판매 로직(판매는 SellBlock 트리거 담당),
디버그 `Input.GetKeyDown(P)`, `MachineSpaces` 프로퍼티와 중복인 `GetMachineSpaces()`.

---

## FrontEnd Design

### 영역별 책임

| 영역 | 클래스 | 책임 |
|---|---|---|
| `ObserverPattern/` | `ISubject`/`IObserver` + 파생 6쌍 | 레이어 간·UI 내부 통지 인터페이스 정의 |
| `UI/UIView/` | `UIView` + Text/Slider/Button 파생, `UIUpdateArgs` 계열 | `UIUpdateArgs`를 받아 개별 위젯(TMP 텍스트, 슬라이더) 갱신 |
| `UI/` | `FactoryStatusUI`, `RoundUI` | 백엔드 Subject 구독, 받은 값을 UIView들로 분배 |
| `UI/MachineUI/UIPanel/` | `UIPanelBase` 파생(MachineSelectUI, MachineModifyUI, TrackModifyUI) | 블록 선택 시 열리는 패널. 블록 상태 표시 + 버튼 입력 처리 |
| `UI/MachineUI/UIContents/` | `UIPanelButtonBase` 파생 버튼, 패널용 UIView 파생 | 버튼 클릭을 Subject로 패널에 통지 |
| `BeltBlock/` | `BlockBase` 파생(BeltBlock, TrackBlock, SellBlock), `BeltBlockManager` | 씬 상의 선택 가능한 블록. 기계 설치/판매/강화, 트랙 강화, 판매 지점 감지 |
| `InteractEvent/` | `BlockSelect`, `CameraMove` | 마우스 레이캐스트로 블록 선택→패널 표시, 카메라 조작 |
| `MachineData/` | `MachineManager`, `MachineInfo(List)` (ScriptableObject) | 기계 타입→프리팹 매핑 데이터 제공 |

### F1. Block 시스템 — 선택 가능한 씬 오브젝트의 공통화

**결정**: 씬에서 클릭으로 선택되는 모든 오브젝트(기계 슬롯, 트랙, 판매 지점)를
`BlockBase` 추상 클래스로 통일하고, 블록마다 `UIType()`으로 자신이 열어야 할 패널
종류(`BlockUIType`)를 선언하게 함

**이유**: `BlockSelect`는 레이캐스트로 `BlockBase` 하나만 찾으면 되고, 어떤 패널을 열지는
블록 스스로 답한다. 새 블록 종류를 추가할 때 BlockSelect를 수정할 필요가 없다 (OCP).

**구매/지불 로직의 위치**: 기계 설치·강화·판매(`BeltBlock`), 트랙 강화(`TrackBlock`)의
비용 지불은 블록이 `FactoryStatus.ModifyMoney()`를 직접 호출해 처리한다. UI 패널은
버튼 입력을 블록 메서드 호출로 변환만 한다.

### F2. 패널-버튼 Observer 구조

**결정**: 버튼(`UIPanelButtonBase` = IUIPanelButtonSubject)이 클릭 시 자신을 구독 중인
패널(`UIPanelBase` = IUIPanelButtonObserver)에 통지하고, 패널이 버튼 타입으로 분기해
대상 블록의 메서드를 호출

**이유**: Unity Button의 onClick을 인스펙터에 직접 묶는 대신 코드에서 옵저버로 연결해,
패널이 어떤 버튼이 눌렸는지(타입 매칭)와 현재 대상 블록(TargetBlock)을 함께 알 수 있다.

### F3. UIView + UIUpdateArgs — 위젯 갱신의 단일 통로

**결정**: 모든 위젯 갱신은 `UIView.SetValue(UIUpdateArgs)` 하나로 통일.
값 종류별 파생(`TextUpdateArgs`, `SliderUpdateArgs`)과 위젯별 파생(TextUIView 계열,
SliderUIView 계열)으로 확장

**이유**: 상위 UI(FactoryStatusUI, RoundUI, 패널)는 위젯의 구체 타입을 모른 채
UIView 리스트에 값만 흘려보내면 된다. 라벨 접두/접미사는 파생 클래스(`MoneyUIView` 등)가
Awake에서 설정한다.

### F4. MachineData — ScriptableObject 기반 프리팹 매핑

**결정**: 기계 타입→프리팹 매핑을 `MachineInfo`/`MachineInfoList` ScriptableObject 에셋으로
관리하고, `MachineManager` 싱글톤이 Dictionary로 캐시해 제공

**이유**: 새 기계 추가 시 코드 수정 없이 에셋 추가만으로 확장 가능. 씬과 분리된
데이터 에셋이라 브랜치 충돌도 적다.

---

## Layer Bridge (Backend ↔ FrontEnd)

백엔드 상태 → UI 갱신은 `FrontEnd/ObserverPattern/`의 Subject/Observer 쌍으로 연결된다:

| Subject | Observer | 전달 내용 |
|---|---|---|
| `FactoryStatus : IFactoryStatusSubject` (백엔드) | `FactoryStatusUI : IFactoryStatusObserver` | `FactoryStatusType` + `UIUpdateArgs` (돈/브랜드/파산바) |
| `RoundManager : IRoundSubject` (백엔드) | `RoundUI : IRoundObserver` | `RoundParameters` (라운드 번호, 남은 시간, 목표/현재 AP) |
| `BeltTrack : IBeltTrackLevelSubject` (백엔드) | `ItemSpawner` (백엔드), `BeltBlockManager` (FrontEnd) | 벨트 레벨업 통지 → 스폰 강화 / 블록 추가 |
| `SellBlock : ISellBlockSubject` (FrontEnd) | `BeltTrack : ISellBlockObserver` (백엔드) | 판매 지점 도달 아이템 (OnTriggerEnter 감지) |
| `BlockBase : IBlockSubject` (FrontEnd) | `UIPanelBase : IBlockObserver` (FrontEnd) | 블록 상태 변경 → 패널 텍스트 갱신 |
| `UIPanelButtonBase : IUIPanelButtonSubject` (FrontEnd) | `UIPanelBase : IUIPanelButtonObserver` (FrontEnd) | 버튼 클릭 |

**규칙**: Observer는 `Start()`에서 `RegisterObserver`, `OnDestroy()`에서
`UnregisterObserver`를 반드시 호출할 것.

### Communication Flow

```
ItemSpawner ──(item 프리팹 Instantiate + AddItem)──► BeltTrack
                                                        │ Update()마다 아이템 이동
                                                   Machine들 (OnTriggerStay() 충돌 가공)
                                                        ▼
                                                    Item 강화 (불량 발생 가능)

SellBlock(FrontEnd) ──OnTriggerEnter──► BeltTrack ──► SellManager.SellItem()
SellManager ──ModifyMoney / AddBrandPoints──► FactoryStatus
            └─(ItemSoldEvent)──► EventBus ──► RoundManager (판매 AP 집계)

RoundManager ──라운드 정산(보상/벌금)──► FactoryStatus
             └─(RoundEndEvent)──► EventBus

FactoryStatus ──(IFactoryStatusSubject)──► FactoryStatusUI ──► UIView들
RoundManager  ──(IRoundSubject)─────────► RoundUI ──► RoundUIView들
BeltTrack     ──(IBeltTrackLevelSubject)─► ItemSpawner / BeltBlockManager

[입력] BlockSelect ──레이캐스트──► BlockBase.UIType() ──► UIPanelBase.OpenUI()
       패널 버튼 클릭 ──(IUIPanelButtonSubject)──► 패널 ──► 블록 메서드 호출
```

---

## Class Responsibilities (Backend)

| 클래스 | 단일 책임 | 변경 이유 |
|---|---|---|
| `BeltTrack` | 아이템 이동, 기계 공간 관리, 판매 지점 도달 처리 | 벨트 이동 로직 변경 시 |
| `ItemSpawner` | 스폰 타이밍 및 아이템 프리팹 인스턴스 생성 | 스폰 로직 변경 시 |
| `SellManager` | 가격 계산, 판매/벌금 처리, ItemSoldEvent 발행 | 판매 공식 변경 시 |
| `RoundManager` | 라운드 타이머, 평균 AP 기준치, 보상/패널티 | 라운드 규칙 변경 시 |
| `GameManager` | 게임 흐름 FSM 관리 (Ready/Playing/Paused/GameOver 전이) | 게임 진입/종료 흐름 변경 시 |
| `FactoryStatus` | 전역 상태 읽기/쓰기 + 변경 통지 + 파산바 자연 가감 | 전역 상태 항목 추가/제거 시 |
| `EventBus` | 이벤트 publish/subscribe | 이벤트 시스템 교체 시 |
| `Item` 계열 | 스탯 보유/강화/가격 계산 + 자체 시각 표현 | 아이템 스탯·불량 규칙 변경 시 |
| `Machine` 계열 | 근접 아이템 스탯 강화 (Grinder=AP·불량가능, Welder=DU, Painter=SP·불량가능) | 강화 규칙 변경 시 |

---

## 알려진 어긋남 (정리 필요)

코드가 본 문서/AGENTS.md의 원칙과 어긋나 있는 지점. 새 작업 시 악화시키지 말 것.

1. **백엔드 → FrontEnd 역방향 의존**: `BeltTrack`이 FrontEnd의 `BlockBase`(`_sellBlock`)를
   SerializeField로 참조하고 `ISellBlockObserver`를 구현한다. `RoundManager`도 FrontEnd의
   `UIUpdateArgs`/`RoundParameters`를 직접 생성한다. 브리지 인터페이스와 UIUpdateArgs가
   FrontEnd 폴더에 있어 백엔드가 UI 레이어 코드 없이 컴파일되지 않는다 — 중립 위치(예:
   `Assets/Scripts/Common/`)로 옮기는 정리가 필요. 폴더 재편이므로 승인 후 진행할 것
2. **싱글톤 제한 위반**: 원칙은 FactoryStatus, EventBus 둘만인데
   `RoundManager.Instance`, `SellManager.Instance`, `MachineManager.Instance`가 추가됨.
   사용처가 있어 단순 삭제 불가(RoundUI, BeltTrack, BeltBlock이 의존) — 인스펙터 참조로
   바꾸려면 씬 재연결 필요
> 1파일 1클래스 위반(Operation/UIUpdateArgs 계열/RoundParameters/BlockUIType 동거,
> `MachineModifybutton_Sell.cs` 파일명 오타)과 불필요/위험 using
> (`UnityEditor.UIElements`, NUnit, Newtonsoft 등)은 refactor/oop-solid Phase 0에서,
> `GameManager.Awake()`의 `FindAnyObjectByType` 검색은 Phase 2(_managers 삭제)에서 해소됨.

---

## UML Diagrams

소스(.puml)는 `Assets/Scripts/`, 생성물(SVG/PNG)은 `docs/uml/`에 있다:

| 다이어그램 | 소스 | 생성물 |
|---|---|---|
| Backend | `BACKEND.puml` | `docs/uml/BACKEND.svg` / `.png` |
| FrontEnd 1/3 — 브리지·UI 위젯 | `FRONTEND_VIEW.puml` | `docs/uml/FRONTEND_VIEW.svg` / `.png` |
| FrontEnd 2/3 — 블록·입력·기계 데이터 | `FRONTEND_BLOCK.puml` | `docs/uml/FRONTEND_BLOCK.svg` / `.png` |
| FrontEnd 3/3 — 패널·버튼 | `FRONTEND_PANEL.puml` | `docs/uml/FRONTEND_PANEL.svg` / `.png` |

- 통합 PDF: 저장소 루트 `UML.pdf` — SVG 기반 벡터(확대해도 선명), 다이어그램당 1페이지
- `.puml` push 시 GitHub Actions(`.github/workflows/uml.yml`)가 자동 생성한다.
  main에서는 생성물을 커밋하고, 그 외 브랜치에서는 워크플로 아티팩트로 업로드한다
  (PR 브랜치에 봇 커밋이 쌓이며 생기는 충돌 방지)
- **다이어그램에는 한글을 넣지 않는다** — CI 러너에 한글 폰트가 없어 깨진다.
  PlantUML이 에러 다이어그램을 내면 워크플로가 실패하도록 검사한다
