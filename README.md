<div align="center">
<h1><마루담>🎮🌳</h1>
이 프로젝트는 전통 한옥의 아름다움과 구조를 미니게임 3가지와 함께 흥미롭게 소개하는 3D 콘텐츠입니다.  
Unity로 제작되었으며, 플레이어는 미니게임을 통해 자연스럽게 한옥의 구성과 문화를 체험할 수 있습니다.
</div><br>

## 작품 개요
최근 한류 열풍은 외국인뿐 아니라 내국인의 관심까지 이끌고 있습니다. 그러나 정작 전통문화보다는 현대문화에 집중된 관심이 아쉽게 느껴졌습니다. 이에 우리는 한국의 전통건축물인 한옥을 주제로한 콘텐츠를 기획했습니다. 3D월드에 설치된 각종 설명과 AI 연동 NPC로 교육적인면을 챙기고, 3가지 미니게임으로 재미도 챙겼습니다.

<br><br>

## 팀원 소개

| 이름 | 역할 |
|------|------|
| [이승언](https://github.com/unvictory2) | - 플레이어 <br>- 윷놀이<br> - NPC |
| [김나영](https://github.com/kny02) | - 맵<br>- UI|
| [조연우](https://github.com/yeonwoo616) | - 서버(클라우드 및 네트워크 관리) |
| [최서영](https://github.com/CSY5316) | - 구들장 보수 미니게임 <br>- 구들 미로 미니게임|

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

|![Image](https://github.com/user-attachments/assets/f051c312-6134-4e55-9769-89177e1b5883)|![Image](https://github.com/user-attachments/assets/759c907b-38c4-4441-8562-debd2380144a)
|:---:|:---:|
|사진 찍기 전|찍은 후|

- **카메라 기능**  
  카메라가 표시된 위치에서 사진을 찍으면, 동일한 구도의 실제 사진이 나타납니다.

  <br><br>

### 🎲 미니게임
<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/46927874-d768-41be-8454-c09c78ad2769" width="420" height="250"/><br/>
      윷놀이
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/828a5b8b-7fbb-41e7-9487-4692c627efdb"  width="420" height="250"/><br/>
      구들 미로 게임
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/6211ccc8-3303-47d7-b280-622bb25f66c3"  width="420" height="250"/><br/>
      구들장 보수 게임
    </td>
  </tr>
</table>


- **윷놀이**
  
  + 구현된 한옥을 위에서 보면 네모 모양인데, 이를 윷놀이 판에 맞춰서 결합한 미니게임
  + 기존 윷놀이처럼 윷을 던져 말을 이동
  + 윷놀이 판은 구현된 한옥 위에 떠있는데, 각 칸은 위치상 일치하는 한옥 방이 존재
  + 각 칸에 도착하면 일치하는 방에 대한 퀴즈가 진행된다. 퀴즈 내용은 기존에 맵을 돌아다니며 얻은 정보에 기반
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
  
- **구들 미로 게임**

  + 전통 온돌의 구조를 학습하며 퀴즈를 푸는 1인칭 미로 탐험 게임
  + 미로의 벽이 온돌의 구들 구조이며 벽면에는 온돌의 구조와 원리에 대한 정보 탑재
  + 어두운 환경 속에서 손전등을 활용한 정보 확인으로 긴장감 형성
  + 벽면에 탑재된 정보 관련 퀴즈 제공으로 학습 요소 제공
   
- **구들장 보수 게임**

  + 전통 온돌 시공 과정 중 구들장 보수 공사 단계를 모티브로 제작한 체험형 게임
  + 금이 간 구들장을 망치로 두들겨 수리 후 새 구들장 설치 및 구들장 사이로 새어나오는 연기 제거하는 보수 공사 진행
  + 제한 시간 및 연기와 금 간 구들장의 랜덤 위치 발생으로 몰입감 형성 

<br><br>

### 🧠 생성형 AI & 멀티플레이

- **생성형 AI NPC**  
  채팅으로 NPC와 대화하면 chatGPT를 거쳐 상호작용합니다.

  
- **멀티플레이어 지원**  
  Photon 서버를 통해 플레이어 간 실시간 상호작용이 가능합니다.
  <br><br>
  
## 개발 도구 및 환경
| 구분 | 설명 |
|------|------|
| 🎮 Unity | 게임 엔진 (3D 맵, 인터랙션, UI 구현) |
| ☁️ Photon Cloud | 서버, 멀티플레이어 네트워크 연동 |
| 🧠 ChatGPT| AI 대화 시스템 구현 |
| 👽️ Mixamo | 애니메이션 생성 |
| 🎵 Suno | 배경 음악 생성 |
| 🧱 Meshy | 3D 모델링 생성 AI |
| 🧑‍💻 Visual Studio | 코드 작성 및 디버깅 도구 |
| 🌐 Node.js | GPT용 서버 |

  <br><br>
## 기술 스택  
![Image](https://github.com/user-attachments/assets/da5c5af5-506b-40a5-9ec9-40539b4a3ef7)

