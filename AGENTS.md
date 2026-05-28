# AGENTS.md — Factory Simulation Game (Unity)

> **Antigravity 2.0 (I/O 2026) 기준으로 작성됨.**
> 이 파일은 모든 서브에이전트에 적용된다. 병렬 실행 중에도 각 에이전트는 이 규칙을 독립적으로 준수해야 한다.

---

## Project Overview

- **Engine**: Unity 6+
- **Language**: C#
- **GitHub**: https://github.com/dotori235/OOPS-Project

---

## Build & Test

```bash
# Unity 프로젝트는 Editor에서 빌드. CLI 빌드가 필요한 경우:
unity -batchmode -quit -projectPath . -buildTarget StandaloneWindows64 -logFile build.log

# 컴파일 에러 확인 (Unity MCP 연결 시)
# MCP 툴로 에디터 콘솔 로그를 확인하라
```

- 스크립트 변경 후 반드시 Unity Editor에서 컴파일 에러를 확인하라
- 플레이모드 실행으로 런타임 동작을 검증한 뒤 다음 작업으로 넘어가라

---

## File Structure

> **초기화 지시 (최초 1회만):**
> 아래 명령으로 프로젝트를 스캔한 뒤, 실제 결과를 `<!-- STRUCTURE -->` 블록 안에 채워넣어라.
> 이후에는 구조 변경이 있을 때만 이 섹션을 업데이트하라. 매 작업마다 재스캔하지 말 것.
>
> ```bash
> find Assets/Scripts -type f -name "*.cs" | sort
> find Assets/Scenes  -type f -name "*.unity"
> find Assets/Prefabs -type f -name "*.prefab"
> ```

<!-- STRUCTURE: 에이전트가 초기화 후 아래에 실제 구조를 작성 -->

```
(초기화 전 — 에이전트가 위 명령을 실행한 뒤 여기에 채워넣을 것)
```

<!-- /STRUCTURE -->

### 신규 파일 배치 규칙

- 파일 생성 전에 위 구조를 확인하고 적절한 위치를 먼저 명시할 것
- 폴더가 없으면 생성해도 되지만, 생성 전에 명시할 것
- 한 파일에 하나의 클래스만 작성. 파일명은 클래스명과 일치시킬 것

---

## Coding Conventions

### C# Style

- **네이밍**: 클래스·public 멤버 `PascalCase`, private 필드 `_camelCase`, 로컬 변수 `camelCase`. 의도가 드러나도록 지을 것 — 주석이 필요하다면 먼저 네이밍을 개선할 것
- **접근 제한자**: 항상 명시. 암묵적 `private` 사용 금지
- **필드**: 모든 필드는 `private` 또는 `protected`. `public` 필드 금지
- **프로퍼티**: 읽기 전용 상태 노출 시 `{ get; private set; }` 사용
- **인터페이스**: `I` 접두사 (예: `IManager`, `IEventListener`)
- **주석**: "왜(why)"에만 작성. "무엇(what)"과 "어떻게(how)"는 코드 자체가 말하게 할 것
- **함수 크기**: 하나의 함수는 하나의 일만. 길어지면 의미 있는 이름으로 분리할 것
- **XML Doc**: 자명한 코드에는 달지 말 것. 설계 의도와 배경은 `DESIGN.md`에 작성

### Unity-Specific Rules

- `[SerializeField]`로 인스펙터 노출. `public` 필드로 노출하지 말 것
- `FindObjectOfType` / `GameObject.Find` 사용 금지 → 인스펙터 레퍼런스 사용
- 싱글톤은 `DontDestroyOnLoad` + static instance 패턴 사용
- MonoBehaviour가 이벤트를 구독하는 경우: `Start()`에서 구독, `OnDestroy()`에서 반드시 해제
- MonoBehaviour는 로직 진입점 역할만. 복잡한 로직은 Pure C# 클래스에 위임할 것
- 모놀리식 컴포넌트 지양 — 책임은 작은 단위로 분리할 것

---

## Non-Obvious Patterns

이 프로젝트에서 에이전트가 프레임워크 지식만으로는 알 수 없는 규칙들.
설계 결정의 세부 내용은 `Assets/Scripts/` 내 `DESIGN.md`를 참고하라.

- **레이어 분리**: 백엔드와 UI는 엄격히 분리된다. 레이어 간 직접 참조를 추가하기 전에 `DESIGN.md`를 먼저 확인할 것
- **크로스 클래스 통신**: 클래스 간 직접 의존을 추가하기 전에 `DESIGN.md`의 통신 패턴을 먼저 확인할 것
- **Pure C# vs MonoBehaviour**: 모든 클래스를 MonoBehaviour로 만들지 말 것. 어느 쪽이어야 하는지는 `DESIGN.md`에 명시되어 있다
- **의존성 추가**: 새로운 클래스 간 의존성을 추가하기 전에 `DESIGN.md`의 설계 원칙 위반 여부를 먼저 확인할 것

---

## Safety Rules

### 일반

- `Assets/Scenes/` 내 파일은 명시적 지시 없이 삭제하거나 덮어쓰지 말 것
- `ProjectSettings/` 파일은 명시적 지시 없이 수정하지 말 것
- Asset Store 임포트는 명시적 승인 없이 하지 말 것
- 기존 파일 수정 전에 무엇을 어떻게 바꾸는지 먼저 명시할 것

### Antigravity 2.0 병렬 에이전트 관련

- 각 서브에이전트는 **하나의 파일 또는 하나의 기능 단위**만 담당한다
- 서브에이전트가 같은 파일을 동시에 수정하지 않도록, 작업 시작 전 담당 파일을 명시할 것
- 쿼터 소진 경고 없이 작업이 중단될 수 있으므로, 각 작업은 단일 단위로 분리할 것
- 긴 작업은 시작 전에 단계를 나열하고 승인을 받은 뒤 진행할 것

### Unity MCP 사용 범위

**원칙**: read-only 조회 먼저, 수정은 작은 단위로, 검증 후 다음 단계로 확장.

**허용:**
- 씬 상태 조회 (스크립트 연결 확인, 컴파일 에러 확인)
- MonoBehaviour가 올바른 GameObject에 연결됐는지 검증
- 새 스크립트 생성 후 컴파일 에러 및 플레이모드 동작 확인

**금지:**
- 계획 없이 `.unity` 씬 파일을 직접 대량 수정
- 승인 없이 `Assets/` 폴더 구조 재편
- 검증 없이 연속적인 수정 적용

---

## Workflow

### 작업 시작 시

1. `File Structure` 섹션을 읽어 현재 구조를 파악한다 (재스캔 금지)
2. `Assets/Scripts/DESIGN.md`가 있으면 먼저 읽는다
3. 담당할 파일을 명시하고 작업을 시작한다

### 작업 단위

- 한 번에 하나의 클래스 또는 하나의 기능 단위만 구현한다
- 구현 → Unity MCP로 컴파일 확인 → 플레이모드 검증 → 다음 단계

### 체크리스트

- [ ] `public` 필드 없음
- [ ] 파일명과 클래스명 일치
- [ ] 접근 제한자 모두 명시됨
- [ ] MonoBehaviour인 경우: `OnDestroy()`에서 이벤트 구독 해제
- [ ] 네이밍만으로 의도가 드러남 (주석 없이 읽히는가)
- [ ] 함수가 하나의 일만 하고 있음
- [ ] 클래스 생성/수정 시 BACKEND.puml도 함께 업데이트됨
- [ ] `DESIGN.md`의 책임 범위를 벗어나지 않음
- [ ] 컴파일 에러 없음 (Unity MCP 확인)