<div align="center">
<h1><옥담>🎮🌳</h1>
이 프로젝트는 전통 한옥의 아름다움과 구조를 미니게임 3가지와 함께 흥미롭게 소개하는 3D 콘텐츠입니다. </br>
Unity로 제작되었으며, 플레이어는 미니게임을 통해 자연스럽게 한옥의 구성과 문화를 체험할 수 있습니다.</br>
해당 프로젝트는 기존 `마루담` 프로젝트를 발전시킨 프로젝트입니다. 
</div><br><br>

## 작품 개요
최근 한류 열풍은 외국인뿐 아니라 내국인의 관심까지 이끌고 있습니다. 그러나 정작 전통문화보다는 현대문화에 집중된 관심이 아쉽게 느껴졌습니다. 이에 우리는 한국의 전통건축물인 한옥을 주제로한 콘텐츠를 기획했습니다. 3D월드에 설치된 각종 설명과 AI 연동 NPC로 교육적인면을 챙기고, 3가지 미니게임으로 재미도 챙겼습니다.

<br><br>

## [마루담](https://github.com/HSU-Capstone)에 비해 달라진 점 
- 온돌 팩맨과 기와쌓기 미니게임을 새로 만들어 기존의 온돌 미로 / 구들장 수리 미니게임 대체
- 윷놀이 미니게임: 부품에 따른 설명 추가
- NPC 프롬프트: 다양한 언어와 상황 대응
- 게임 안정성: 미니게임 전후 Scene 연결
- 카메라 기능: 사진 찍은 후 링크를 통해 해당 사진이 찍힌 한옥 홈페이지로 연결해주는 기능

<br><br>

## 팀원 소개

| 이름 | 역할 |
|------|------|
| [이승언](https://github.com/unvictory2) | - 플레이어 <br>- NPC <br> - 윷놀이 <br> - 온돌 팩맨 |
| [조연우](https://github.com/yeonwoo616) | - 서버(클라우드 및 네트워크 관리) <br> - 씬 관리(상태) <br> - 지붕쌓기 |

<br><br>

## 주요 콘텐츠

### 🧭 한옥 탐방

|![스크린샷 2025-05-18 010739](https://github.com/user-attachments/assets/89cb3e51-0fa8-456f-b4ac-a6afef877d84)|![스크린샷 2025-05-23 235852](https://github.com/user-attachments/assets/bd63eac7-45b0-4e6a-9b5d-a1642674e643)
|:---:|:---:|
|맵 탐방|설명 콘텐츠|


- **한옥 맵 탐방**  
  현실 한옥을 기반으로 제작된 3D 맵을 자유롭게 탐험할 수 있습니다.
- **설명 콘텐츠**  
  상호작용이 표시된 곳에서 한옥 요소에 대한 설명을 UI 텍스트로 제공합니다.

<br>

|![Image](https://github.com/user-attachments/assets/d8378c92-54fe-4a81-b654-94f75b53d70f)|![Image](https://github.com/user-attachments/assets/07f4f51b-1143-4746-a17e-388bade2244e)|![Image](https://github.com/user-attachments/assets/258c36b7-570a-4caf-8fab-544b4c37b8cc)
|:---:|:---:|:---:|
|사진 찍기 전|찍은 후|링크 접속|

- **카메라 기능**  
  카메라가 표시된 위치에서 사진을 찍으면, 동일한 구도의 실제 사진이 나타납니다.

  <br><br>

### 🎲 미니게임
- **윷놀이**
<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/3c466b68-ced9-4592-8955-d43dbd3df479" width="420" height="250"/><br/>
      윷 던지기
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/119834fa-b63e-427e-8caa-4194d402e2a1"  width="420" height="250"/><br/>
      한옥 퀴즈
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/0d61b39d-1fbd-49f1-abd2-f0c7b6be5815"  width="420" height="250"/><br/>
      부품 원리 설명
    </td>
  </tr>
</table>

  + 구현된 한옥을 위에서 보면 네모 모양인데, 이를 윷놀이 판에 맞춰서 결합한 미니게임
  + 기존 윷놀이처럼 윷을 던져 말을 이동. 업기, 빽도 같은 기존의 규칙 전부 구현
  + 윷놀이 판은 구현된 한옥 위에 떠있는데, 각 칸은 위치상 일치하는 한옥 방이 존재
  + 각 칸에 도착하면 일치하는 방에 대한 퀴즈가 진행 -> 퀴즈 내용은 기존에 맵을 돌아다니며 얻은 정보에 기반
  + 대응하는 방이 없는 중앙 부근의 칸들에선 말의 부품 문양에 따른 한옥의 원리 설명 
  <details>
  <summary>윷놀이 평면도</summary>
  <table>
    <tr>
      <td align="center">
        <img src="https://github.com/user-attachments/assets/3b8c668f-909a-4c44-9028-09b88e13d1a2"><br>
        한옥 평면도
      </td>
      <td align="center">
        <img src="https://github.com/user-attachments/assets/e9721154-65ac-4584-b3d2-85399402254e"><br>
        윷놀이와 결합안
      </td>
    </tr>
  </table>
</details>

<br>

- **기와 쌓기**
<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/8edc55d1-e022-412d-b6f6-4b0639848659"/><br/>
      게임 시작
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/d17a1e52-590e-4a2c-b5a5-b2bbcf7cda9e"/><br/>
      기와 쌓기
    </td>
  </tr>
</table>
  
  + 점점 좁아지는 기와를 쌓아 올리는 게임
  + 블록이 좌우로 이동할 때 스페이스바를 누르면 아래 층과 겹치는 부분만 유지, 나머지는 제거
  + 겹치는 부분이 없으면 실패

<br>

 - **온돌 팩맨**
<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/6f6da9f2-389e-411d-ab8c-33b733bc606c"/><br/>
      게임 설명
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/84f8efe8-52f3-4d0e-a912-6c970b8b93c8"/><br/>
      인게임
    </td>
  </tr>
</table>
  
  + 온돌의 허튼 고래와 팩맨 게임의 형태가 닮았음에서 기인
  + 온돌의 원리를 팩맨 + 스네이크 게임을 통해 자연스럽게 학습할 수 있게 구성
      - 목표: 고장난 굴뚝을 고치기 위한 5개의 부품 수집 + 고래를 최대한 골고루 데우기
      - 플레이어: 구들개자리에서 들어오는 열기(불)
      - 구들개자리: 실제 온돌에서 열기가 들어오는 곳이자 플레이어가 열기를 모을 수 있는 곳
      - 적: 고장난 굴뚝에서 들어오는 한기. 플레이어의 열기와 충돌 시 소멸(열기와 한기가 만나면 소멸)
      - 플레이어가 지나간 길은 데워지며, 빨간 색으로 표시됨 > 많을 수록 추가 점수


<br><br>

### 🧠 생성형 AI & 멀티플레이

- **생성형 AI NPC**
<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/daa559f5-eb91-426a-b632-d776b243faae"/><br/>
      한국어
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/4a9ae540-34fc-4988-b3a1-8b2a463d6b43"/><br/>
      외국어
    </td>
  </tr>
</table>
  채팅으로 NPC와 대화하면 chatGPT를 거쳐 상호작용합니다.
  
  <br>
  
- **멀티플레이어 지원**  
  Photon 서버를 통해 플레이어 간 실시간 상호작용이 가능합니다.
  
  <br><br>
  
### 시연 영상 링크
- [간단한 시연 영상](https://www.youtube.com/watch?v=szIfeak0OT0)

<br><br>
  
## 개발 도구 및 환경
| 구분 | 설명 |
|------|------|
| 🎮 Unity | 게임 엔진 (3D 맵, 인터랙션, UI 구현) |
| ☁️ Photon Cloud | 서버, 멀티플레이어 네트워크 연동 |
| 🧠 ChatGPT| AI 대화 시스템 구현 |
| 🧱 Meshy | 3D 모델링 생성 AI |
| 🧑‍💻 Visual Studio | 코드 작성 및 디버깅 도구 |
| 🌐 Node.js | GPT용 서버 |

  <br><br>
## 기술 스택  
![Image](https://github.com/user-attachments/assets/9b12bca8-68d6-485d-a819-3a443c069293)

