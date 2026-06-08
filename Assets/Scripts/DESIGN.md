# DESIGN.md — Factory Simulation Game (Unity) Backend

이 문서는 백엔드 설계 결정과 그 이유를 기록한다.
구현 세부사항(클래스 목록, 메서드 시그니처)은 `BACKEND.puml`을 참고하라.
에이전트는 클래스 생성/수정 시 반드시 `BACKEND.puml`도 함께 업데이트해야 한다.

---

## Architecture

### Layer Separation

백엔드와 UI는 엄격히 분리된다.

```
Backend (이 문서의 범위)
└── 순수 C# 클래스 및 MonoBehaviour 로직 클래스

UI (별도 담당)
└── MonoBehaviour 렌더링 전담 클래스
```

레이어 간 유일한 브리지:
- **EventBus** — 백엔드 → UI 단방향 이벤트 전달
- **Snapshot 구조체** — 읽기 전용 상태 복사본. UI는 직접 백엔드 필드를 읽지 않는다

### MonoBehaviour vs Pure C#

| 분류 | 클래스 | 이유 |
|---|---|---|
| **MonoBehaviour** | Machine 계열, BeltTrack, ItemSpawner, SellManager, RoundManager, GameManager | 씬에 배치되는 오브젝트. 자체 Update() 루프 필요 |
| **Pure C#** | Item 계열, FactoryStatus, EventBus, GameEvent 계열, Snapshot 구조체, 인터페이스 | 다수 인스턴스 또는 데이터 전용. MonoBehaviour 오버헤드 불필요 |

모든 클래스를 MonoBehaviour로 만들지 말 것. 위 표를 기준으로 판단하라.

---

## Key Design Decisions

### 1. EventBus (Observer Pattern)

**결정**: 매니저 간 직접 참조 대신 EventBus를 통한 단방향 통신

**이유**: 매니저들이 서로를 직접 참조하면 의존성이 복잡해지고 테스트가 어려워진다.
EventBus를 경유하면 발행자와 구독자가 서로를 모른 채 통신할 수 있다.

**트레이드오프**: 이벤트 흐름이 코드에서 직접 보이지 않아 디버깅이 다소 어려울 수 있다.
이벤트 타입을 명확하게 네이밍하여 보완한다.

### 2. IManager 인터페이스

**결정**: `abstract Manager` 클래스 대신 `IManager` 인터페이스 사용

**이유**: C#은 단일 상속만 지원한다. MonoBehaviour를 상속하면서 공통 추상 클래스도 상속하는 것은 불가능하다.
인터페이스는 다중 구현이 가능하므로 MonoBehaviour 상속과 충돌하지 않는다.

**다형성 유지**: GameManager가 `List<IManager>`를 보유하고 `onEvent()`를 타입 분기 없이 호출한다.

### 3. BeltTrack + ItemSpawner 분리

**결정**: 기존 ConveyorBelt 하나를 BeltTrack(이동/공간)과 ItemSpawner(생성)로 분리

**이유**: Single Responsibility Principle. 하나의 클래스가 아이템 생성, 이동, 공간 관리를 모두 담당하면
변경 이유가 여러 개가 된다. 분리하면 각자의 레벨업 로직과 동작이 독립적으로 변경 가능하다.

### 4. Item을 Pure C#으로 유지

**결정**: Item은 MonoBehaviour가 아닌 Pure C# 클래스

**이유**: 벨트 위에 다수의 아이템이 동시에 존재한다. 모두 MonoBehaviour로 만들면
Unity 내부 오브젝트 관리 비용이 크게 증가한다.
아이템의 시각적 표현은 ItemSpawner가 Prefab으로 별도 관리한다.

### 5. FactoryStatus Singleton

**결정**: 재화, 브랜드, 파산바 등 전역 상태를 FactoryStatus 싱글톤 하나로 관리

**이유**: 여러 매니저가 동일한 전역 상태에 접근해야 한다. 각 매니저가 개별적으로
상태를 들고 있으면 동기화 문제가 생긴다.

**주의**: 싱글톤 남용을 방지하기 위해 FactoryStatus와 EventBus 외에는 싱글톤을 추가하지 말 것.

### 6. Machine Configure(BeltTrack) 동적 초기화

**결정**: Machine 추상 클래스에 `Configure(BeltTrack)` 메서드를 구현하여 런타임에 벨트 트랙을 수동으로 연결할 수 있도록 함

**이유**: 씬 구성 단계에서 인스펙터 레퍼런스를 미리 잡아두는 방식은 정적 배치에 유리하지만, 게임 런타임 중에 GameManager가 기계를 동적으로 인스턴스화하고 벨트의 특정 슬롯에 동적으로 임포트/배치할 때 인스펙터 연결이 불가능합니다. `Configure()` 메서드를 통한 의존성 주입 구조를 도입함으로써 씬 배치 및 런타임 동적 생성을 모두 깔끔하게 지원할 수 있게 됩니다.

---

## Communication Flow

```
ItemSpawner ──(spawnItem)──► BeltTrack ──(아이템 끝 도달)──► SellManager
                                  │
                             Machine들
                           (Update()마다
                            가장 가까운
                            아이템 강화)

SellManager ──(ItemSoldEvent)──► EventBus ──► RoundManager
                                          └──► UI Layer (Snapshot)

RoundManager ──(RoundEndEvent)──► EventBus ──► FactoryStatus
                                          └──► UI Layer (Snapshot)
```

---

## Class Responsibilities

| 클래스 | 단일 책임 | 변경 이유 |
|---|---|---|
| `BeltTrack` | 아이템 이동 및 기계 공간 관리 | 벨트 이동 로직 변경 시 |
| `ItemSpawner` | 스폰 타이밍 및 아이템 인스턴스 생성 | 스폰 로직 변경 시 |
| `SellManager` | 가격 계산 및 판매 처리 | 판매 공식 변경 시 |
| `RoundManager` | 라운드 타이머, 기준치, 보상/패널티 | 라운드 규칙 변경 시 |
| `GameManager` | 씬 라이프사이클, IManager 조율 | 게임 진입/종료 흐름 변경 시 |
| `FactoryStatus` | 전역 상태 읽기/쓰기 | 전역 상태 항목 추가/제거 시 |
| `EventBus` | 이벤트 publish/subscribe | 이벤트 시스템 교체 시 |

---

## UML Diagram

백엔드 클래스 다이어그램은 `BACKEND.puml`을 참고하라.
렌더링된 PNG는 `docs/uml/BACKEND.png`에서 확인할 수 있다 (push 시 자동 생성).