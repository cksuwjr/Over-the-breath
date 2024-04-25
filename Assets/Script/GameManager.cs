using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {

                if (FindObjectOfType<GameManager>())
                    instance = FindObjectOfType<GameManager>();
                else
                {
                    var newGameManager = new GameObject();
                    newGameManager.name = "Game Manager";
                    instance = newGameManager.AddComponent<GameManager>();
                }
            }
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }
    }

    private UIManager uIManager;
    public UIManager UIManager
    {
        get
        {
            uIManager = FindObjectOfType<UIManager>();
            if (uIManager == null)
            {
                Debug.Log("UI");
            }
            return uIManager;
        }
    }

    private CameraManager cameraManager;
    public CameraManager CameraManager
    {
        get
        {
            if (cameraManager == null)
            {
                var findCameraManager = FindObjectOfType<CameraManager>();
                if (findCameraManager)
                {
                    cameraManager = findCameraManager;
                }
                else
                {
                    var newCameraManager = new GameObject();
                    newCameraManager.name = "Camera Manager";
                    newCameraManager.AddComponent<CameraManager>();
                    DontDestroyOnLoad(newCameraManager.gameObject);
                    cameraManager = newCameraManager.GetComponent<CameraManager>();
                }
                
            }
            return cameraManager;
        }
    }

    private StoryManager storyManager;
    public StoryManager StoryManager
    {
        get
        {
            if (storyManager == null)
            {
                var findStoryManager = FindObjectOfType<StoryManager>();
                if (findStoryManager)
                {
                    storyManager = findStoryManager;
                }
                else
                {
                    var newStoryManager = new GameObject();
                    newStoryManager.name = "Story Manager";
                    newStoryManager.AddComponent<StoryManager>();
                    DontDestroyOnLoad(newStoryManager.gameObject);
                    storyManager = newStoryManager.GetComponent<StoryManager>();
                }
            }
            return storyManager;
        }
    }

    private Player player;
    public Player Player
    {
        get
        {
            if(player == null)
                player = FindObjectOfType<Player>();
            return player;
        }
        set { player = value; }
    }


    public void MonsterFreeze(bool freeze)
    {
        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Enemy"))
            a.GetComponent<Monster>().binded = freeze;
        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Neutrality"))
            a.GetComponent<Monster>().binded = freeze;
    }
    
    public void SaveGame()
    {
        if (DataManager.Instance.data == null)
            DataManager.Instance.data = new GameData();

        DataManager.Instance.data.sceneName = SceneManager.GetActiveScene().name;
        if (Player)
        {
            DataManager.Instance.data.position = Player.transform.position;
            if (DataManager.Instance.data.status == null)
                DataManager.Instance.data.status = new StatData();
            DataManager.Instance.data.status.SetData(Player.GetComponent<Status>());

            if(DataManager.Instance.data.skillData == null)
                DataManager.Instance.data.skillData = new SkillData();

            DataManager.Instance.data.skillData.skillPoint = Player.skill.SkillPoint;

            string[] skillName = new string[Player.skill.skillBook.GetComponentsInChildren<Skill>().Length];
            int[] skillPoint = new int[Player.skill.skillBook.GetComponentsInChildren<Skill>().Length];
            int count = 0;
            foreach(Skill skill in Player.skill.skillBook.GetComponentsInChildren<Skill>())
            {
                skillName[count] = skill.info.name;
                skillPoint[count] = skill.SkillLevel;
                count++;
            }
            DataManager.Instance.data.skillData.skillNames = skillName;
            DataManager.Instance.data.skillData.skillPoints = skillPoint;
            DataManager.Instance.data.skillData.keySetting = Player.skill.GetKeySetting();

            DataManager.Instance.SaveGameData();
        }
    }
    public void SaveSetting()
    {
        if (DataManager.Instance.settingData == null)
            DataManager.Instance.settingData = new SettingData();

        DataManager.Instance.settingData.bgmVolume = SoundManager.Instance.BackgroundSoundSource.volume;
        DataManager.Instance.settingData.effectVolume = SoundManager.Instance.effectSoundSource.volume;
        DataManager.Instance.settingData.resolutionOption = this.resolutionOption;

        Debug.Log("GameManager,, BGM: " + DataManager.Instance.settingData.bgmVolume + "/  Effect: " + DataManager.Instance.settingData.effectVolume);


        DataManager.Instance.SaveSettingData();
    }
    public void LoadGame()
    {
        DataManager.Instance.LoadGameData();
        StoryManager.StoryLoad();

        var sceneName = "new1-1";
        if (DataManager.Instance.data != null)
            sceneName = DataManager.Instance.data.sceneName;
        else
            DataManager.Instance.data = new GameData();
        LoadScene(sceneName);
    }
    public void LoadScene(string sceneName)
    {
        SaveGame();
        DataManager.Instance.LoadGameData();
        player = null;
        Player = null;

        LoadingSceneManager.LoadScene(sceneName);
    }

    public void PlayerPositionInit()
    {
        var data = DataManager.Instance.data;
        if (data == null) return;
        if(data.position == null) return;

        Player.transform.position = data.position;
    }
    public void PlayerStatInit()
    {

        var data = DataManager.Instance.data;
        if (data == null) return;
        if (data.status == null) return;
        var stat = Player.GetComponent<Status>();

        stat.MaxHp = data.status._maxhp;
        stat.HP = data.status._hp;
        stat.Level = data.status._level;
        stat.MaxExp = data.status._maxExp;
        stat.Exp = data.status._exp;
        stat.AttackPower = data.status._attackpower;
        stat.BasicJumpPower = data.status._basicjumppower;
        stat.BasicSpeed = data.status._basicspeed;
        stat.MoveSpeed = data.status._movespeed;
        stat.JumpPower = data.status._jumppower;

        Player.ChangeDragon(data.status.dragonMode);
    }

    public void PlayerSkillInit()
    {
        var data = DataManager.Instance.data;
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        skillBook.GetComponentInChildren<SkillTree>().AllSkillUIUpdate();
        if (data == null) return;
        if (data.skillData == null) return;

        GameManager.instance.Player.skill.SkillPoint = data.skillData.skillPoint;


        var skills = skillBook.GetComponentsInChildren<Skill>();

        foreach(Skill skill in skills)
        {
            int i = 0;
            foreach(string skillName in data.skillData.skillNames)
            {
                if(skill.info.name == skillName)
                {
                    skill.SkillLevel = data.skillData.skillPoints[i];
                    break;
                }
                i++;
            }
        }
        skillBook.GetComponentInChildren<SkillTree>().AllSkillUIUpdate();
        foreach (Skill skill in skills)
        {
            int i = 0;
            foreach (string skillName in data.skillData.keySetting)
            {
                if (skill.info.name == skillName)
                {
                    if (i == 0) GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.Q);
                    if (i == 1) GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.W);
                    if (i == 2) GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.E);
                    if (i == 3) GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.R);
                    break;
                }
                i++;
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneLoadedEvent;
    }
    private void SceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {
        if (Player == null)
        {
            if(GameObject.FindGameObjectWithTag("Respawn"))
            {
                GameObject.FindGameObjectWithTag("Respawn").GetComponent<Spawner>().PlayerSpawn();
            }
        }
        if (Player == null)
            return;
        PlayerPositionInit();
        PlayerStatInit();
        PlayerSkillInit();
    }

    public int resolutionOption = 0;

    public void SetResolution(int resolutionOption)
    {
        this.resolutionOption = resolutionOption;
        switch (resolutionOption)
        {
            case 0:
                Screen.SetResolution(1280, 720, false);
                break;
            case 1:
                Screen.SetResolution(1366, 768, false);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, true);
                break;
        }
    }
}
