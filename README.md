<h1 align="center">
  RunderLand
</h1>

<h3 align="center">
  실시간 위치 기반 AR 러닝 애플리케이션
</h3>

<p align="center">
  🏆 2023 메타버스 개발자 경진대회 우수상 (한국퀄컴대표상) 수상작
</p>

## Introduction

**RunderLand**는 AR 경험을 통해 사용자가 가상 러닝 메이트와 함께 달릴 수 있는 메타버스 러닝 애플리케이션입니다. 
위치 정보 기반 러닝, 실시간 네비게이션, 러닝 기록 저장 및 공유, 멀티플레이 등 다양한 기능을 통해 사용자에게 몰입감 있는 러닝 경험을 제공합니다.

| [![1차 시연 영상](https://img.youtube.com/vi/WNS9c8TE59s/0.jpg)](https://www.youtube.com/watch?v=WNS9c8TE59s) | [![최종 시연 영상](https://img.youtube.com/vi/O8mDsIyQ21E/0.jpg)](https://www.youtube.com/watch?v=O8mDsIyQ21E) |
|:------------:|:------------:|
| 1차 시연 영상 링크 | 최종 시연 영상 링크 |


## Key Features

- 아바타 운동: 러닝/자전거 선택, 아바타와 함께 운동
- 운동 기록 및 함께 달리기: 경로·거리·시간 기록 저장, 아바타로 과거 기록 재현
- 러닝 보조: 현재 위치, 날씨, 일간 목표, 지도 API를 통한 러닝 지원
- 플레이 모드: 싱글, 기록 대결, 멀티 플레이 지원
- 실시간 네비게이션: GPS 기반 아바타 동기화, 목적지 설정 및 실시간 경로 안내

## My Contribution

- Figma를 이용한 UI 디자인 및 AR/안드로이드 Dual Render 화면 설계 ([Figma Design Link](https://www.figma.com/design/XDMRxLTHciWtYpouNx2f9a/RunderLand?node-id=0-1&t=EFOmWJxTi6Te9CLP-1))
- Unity 기반 프론트엔드 개발 (ThinkReality A3 기기 최적화)
- 지도, 날씨, 위치 기반 기능 개발 (네이버 Reverse Geocoding, 기상청 API, Bing Map API)

## Challenges and Solutions

### 1. GPS 좌표 -> 기상청 격자 좌표 변환 로직

**어려웠던 점**

러닝 중 사용자의 현재 위치를 기반으로 기상청 초단기 예보 데이터를 제공하기 위해, `WGS84`	기반 GPS 좌표를 기상청이 요구하는 `LCC` 기반 격자 좌표로 변환해야 했습니다. 하지만 이를 수행할 적절한 변환 라이브러리를 찾기 어려웠고, 직접 구현이 필요한 상황이었습니다.

**해결 방법**

- 다행히도 `JavaScript`로 작성된 오픈소스 변환 모듈을 참고하여 `C#`으로 직접 변환해 사용했습니다.
- GPS 좌표 수신이 완료되지 않은 상태에서 변환이 시도될 경우를 방지하기 위해, 좌표 수신 완료까지 대기하는 로직을 추가하여 안정성을 높였습니다.

**관련 코드**

- [KmaWeater Source File](./Assets/Scripts/Main/Weather/KmaWeather.cs)

### 2. 퀄컴 SDK와 지도 API 호환성 이슈

**어려웠던 점**

AR 글래스 화면 상에 지도를 띄우기 위해 MapBox API를 사용하려 했으나, 퀄컴 SDK와의 호환성 문제로 지도가 정상적으로 렌더링 되지 않는 현상이 발생했습니다. 

**해결 방법**

- 여러 테스트와 조사 끝에 Bing Map API로 대체하여 문제를 해결했습니다.
- 외부 프로젝트에서 Bing Map 모듈을 독립적으로 개발한 뒤, 메인 프로젝트에 통합하는 방식으로 적용했습니다.
- 이를 통해 AR 글래스와 스마트폰 환경 모두에서 안정적인 지도 렌더링과 네비게이션 기능을 제공할 수 있었습니다.
