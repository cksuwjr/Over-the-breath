# over-the-breath
잃어버린 종족의 숨결을 찾아 떠나는 용의 모험<br>
<br>
플레이는 Playable폴더 다운로드 하셔서 해보실 수 있습니다.<br>
<br>
<br>
<br>
<h2>☆플레이어(Player) 기본 움직임 </h4><br>

방향키 : ←  →  <br>
점프 : ↑   <br>

<h2>플레이어 기본 공격</h4><br>
  
Q : 불덩이 발사 <br>
적중시 적을 약간 밀어내고 피해를 입힙니다. <br><br>


![play2](https://user-images.githubusercontent.com/63836325/159896440-4964f471-c4c8-4e0c-b32a-3215ba1d8c7e.gif)
<br><br><br>

<h2>적 유닛(Enemy) 기본 AI 구현</h4><br>

기본적으로 랜덤확률에 따라 이동, 점프 합니다.<br>
<br>
외에도 지형이동시 계단지형을 점프해 도약할 수 있습니다..<br>
<br>
다만, 플레이어의 공격에 피격당할 시 플레이어를 향해 이동하며<br>
플레이어에 근접했을 때 플레이어가 자신보다 높은 곳에 위치하면 점프합니다.<br><br>
![play](https://user-images.githubusercontent.com/63836325/159894667-ae98f061-ad63-4642-b10e-6370284673da.gif)
<br><br><br>
