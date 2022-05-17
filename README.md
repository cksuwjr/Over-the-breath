# over-the-breath
<br>

잃어버린 종족의 숨결을 찾아 떠나는 용의 모험<br>
<br>

Unity 2020.3.22f1 버전에서 작업되었습니다.<br>
Playable폴더 다운로드하셔서 플레이 해 보실 수 있습니다.<br>
그림소스는 모두 직접 제작하였습니다.<br>

<br><br><br>
<h1>2022 / 05 / 17 이전</h1>
<br><br><br>


<br><br><br>
<h2>Player 기본 움직임 </h2><br>

```
  방향키 : ←  →  
  점프 : ↑   
```
![ezgif com-gif-maker](https://user-images.githubusercontent.com/63836325/168753585-29783111-5f53-430d-91f1-d9d9d109928c.gif)
<br><br>
<h2>Player 기본 공격</h2><br>

Q
```
   
  <기본용>
  불덩이 발사
  적중시 적을 약간 밀어내고 피해를 입힙니다.

  <대지용> [임시이름]
  기본 공격의 종류가 3종류로 나뉩니다. 
  각각 공격력의 1배, 2배, 3배 데미지를 입히며 각 공격마다 짧게 돌진합니다.
  마지막 공격은 적을 뒤로 밀치며 뛰어올립니다.
```
![ezgif com-gif-maker (1)](https://user-images.githubusercontent.com/63836325/168754184-66f22f50-24a2-47b6-ab70-c4cddd635206.gif)<br>
![play2](https://user-images.githubusercontent.com/63836325/159896440-4964f471-c4c8-4e0c-b32a-3215ba1d8c7e.gif)<br>

<br><br>
<h2>Player 스킬</h2>

E
```

  <기본용>
  없음
  
  <대지용> [임시이름]
  짧은 거리를 순간이동하며 원래 위치로부터 이동한 거리 사이의 모든 적들은
  공격력의 4배에 해당하는 피해를 입습니다.
  
```

<br><br>
<h2> UI </h2>

스토리 진행을 위한 UI를 만들었습니다.<br>
![image](https://user-images.githubusercontent.com/63836325/168755175-d6cf16eb-679f-4bfe-8121-2af4e6e8fa31.png)<br>
```
  UI는 플레이어 사망씬을 나타낼 Die 와 스토리 진행을 도와줄 ScenarioTeller로 나누었으며 모습은 각각 아래와 같습니다.
```
<br>

Die<br>
![ezgif com-gif-maker (4)](https://user-images.githubusercontent.com/63836325/168758739-7edac6a0-0013-4a9b-9f87-9f490695640b.gif)<br>
<br>
ScenarioTeller<br>
![ezgif com-gif-maker (2)](https://user-images.githubusercontent.com/63836325/168754773-2237ea37-81c5-4f7f-b62f-e7b6ec38391b.gif)<br>



<br><br>

<hr>



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
<hr>
<h2>자연 환경 소스 제작</h2><br><br>

```
  Ground처럼 디딜 수 있는 나무 지형과 수풀, 덤불 함정 등 추가하였습니다.
```
![image](https://user-images.githubusercontent.com/63836325/168757697-41b305e8-caa4-4540-bc33-ec36be9c9967.png)


<br><br><br>
<h1> 2022 / 05 / 17 진행</h1>






























<br><br><br>
<hr>
<h2>음원 출처</h2><br><br>

```
  == The-Beginning-w-Caturday ==

  The Beginning (w Caturday) by Babasmas | https://soundcloud.com/babasmasmoosic
  Music promoted by https://www.chosic.com/free-music/all/
  Creative Commons CC BY-SA 3.0
  https://creativecommons.org/licenses/by-sa/3.0/


  == 희망의 산호초 ==

  음원제공 - BGM팩토리 (https://bgmfactory.com)
  사용음원 - 희망의 산호초



  == punch-deck-the-soul-crushing-monotony-of-isolation-instrumental-mix==

  The Soul-Crushing Monotony Of Isolation (Instrumental Mix) by Punch Deck | https://soundcloud.com/punch-deck
  Music promoted by https://www.chosic.com/free-music/all/
  Creative Commons Attribution 3.0 Unported License
  https://creativecommons.org/licenses/by/3.0/deed.en_US 
```


<br><br><br>
<hr>
  <h2>기능 구현에 도움받은 사이트 링크</h2><br><br>


<a href="https://unitybeginner.tistory.com/105"> 지면 관통 2D Platform Effector2D </a><br><br>
<a href="https://junsugi.tistory.com/12"> 효과음, 음악 넣기 AudioSource </a><br><br>
