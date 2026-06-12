# OOPS-Project

Unity로 만드는 공장 시뮬레이션 게임입니다. 벨트 위로 흐르는 아이템을 기계로 가공해
스탯(공격력·내구도·화려함)을 올리고, 판매 수익으로 벨트와 기계를 업그레이드하면서
라운드 목표를 달성합니다. 파산 게이지가 가득 차면 게임 오버입니다.

## 개발 환경

- Unity 6.3 LTS (6000.3.x)
- C#

## 핵심 루프

```
아이템 스폰 → 벨트 이동 → 기계 가공(스탯 강화) → 판매 → 라운드 정산 → 업그레이드
```

## 폴더 구조

| 경로 | 내용 |
|---|---|
| `Assets/Scripts/Backend/` | 게임 로직 (스탯, 벨트, 기계, 판매, 라운드, 게임 흐름 FSM) |
| `Assets/Scripts/FrontEnd/` | UI·입력·씬 블록 (Observer 패턴으로 백엔드와 연결) |
| `Assets/Scripts/DESIGN.md` | 설계 결정 기록 |
| `docs/uml/` | 자동 생성된 UML (SVG/PNG) |

## 설계 문서

- [DESIGN.md](Assets/Scripts/DESIGN.md) — 레이어 구조와 설계 결정
- [UML.pdf](UML.pdf) — 전체 다이어그램 통합 PDF (벡터)

## UML

`.puml` 수정을 push하면 GitHub Actions가 아래 다이어그램을 자동 갱신합니다.

### Backend

![Backend UML](docs/uml/BACKEND.svg)

### FrontEnd 1/3 — Observer 브리지 & UI 위젯

![FrontEnd View UML](docs/uml/FRONTEND_VIEW.svg)

### FrontEnd 2/3 — 씬 블록 · 입력 · 기계 데이터

![FrontEnd Block UML](docs/uml/FRONTEND_BLOCK.svg)

### FrontEnd 3/3 — UI 패널 & 버튼

![FrontEnd Panel UML](docs/uml/FRONTEND_PANEL.svg)
