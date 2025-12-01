using System;
using System.Threading;
using Buffalo.BuffaloDiff;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Audio;
using DefaultNamespace.UI;
using Person;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FirstBuffalo buffalo;
        [SerializeField] private RightBuffalo rightBuffalo;
        [SerializeField] private LeftBuffalo leftBuffalo;
        [SerializeField] private CenterBuffalo centerBuffalo;
        [SerializeField] private Player player;
        [SerializeField] private ManageScoreUpText scoreUpText;
        [SerializeField] private CountDownManager countDownManager;
        
        [SerializeField] private GameObject flagPrefab;
        private FlagManager flagManager;
        
        [SerializeField] PlaySceneSoundManager playSceneSoundManager;
        
        private float playStartTime;
        
        public static float score = 0;
        public static int comboCount = 0;
        
        private bool isGetFlagDuringTackle = false;

        private CancellationTokenSource TackleDodgeScoreCheckCancelTokenSource;
        private CancellationTokenSource rightTackleDodgeScoreCheckCancelTokenSource;
        private CancellationTokenSource leftTackleDodgeScoreCheckCancelTokenSource;
        private CancellationTokenSource centerTackleDodgeScoreCheckCancelTokenSource;
        
        private bool isRightFree = false;
        private bool isLeftFree = false;
        private bool isCenterFree = false;
        
        private bool isFinishStarted = false;

        [SerializeField] private GameObject rightCage;
        [SerializeField] private GameObject leftCage;
        [SerializeField] private GameObject centerCage;
        private Animator centerCageAnimator;

        [SerializeField] private CinemachineCamera rightCamera;
        [SerializeField] private CinemachineCamera leftCamera;
        [SerializeField] private CinemachineCamera centerCamera;

        private void Awake()
        {
            // Debug.Log("Null? player" + (player == null) + " playerTransform: " + (player.transform == null) + " buffalo: " + (buffalo == null) + " buffaloTransform: " + (buffalo.transform == null) + "");
        }

        private void Start()
        {
            Time.timeScale = 1f;
            score = 0;
            comboCount = 0;
            GameStats.Reset();
            playStartTime = Time.time;
            isRightFree = false;
            isLeftFree = false;
            isCenterFree = false;
            isFinishStarted = false;
            
            rightTackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
            leftTackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
            centerTackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
            TackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
            
            subscribeAll();
            
            flagManager = new FlagManager(buffalo, player);
            // EmergeFlagRandomPoint();
            centerCageAnimator = centerCage.GetComponent<Animator>();
            centerCageAnimator.SetBool("OpenCage", true);
            
            buffalo.isBooted = true;
            CheckBuffaloAndOpenCage().Forget();
            countDownManager.PlayGo();
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                FreeCenter();
            }
            #endif
            if (Time.time - playStartTime >= GameConst.BuffaloRightFreeTime && !isRightFree)
            {
                // Debug.Log("Free Right");
                isRightFree = true;
                FreeRight();
            }else if (Time.time - playStartTime >= GameConst.BuffaloLeftFreeTime && !isLeftFree)
            {
                // Debug.Log("Free Left");
                isLeftFree = true;
                FreeLeft();
            }else if (Time.time - playStartTime >= GameConst.BuffaloCenterFreeTime && !isCenterFree)
            {
                // Debug.Log("Free Center");
                isCenterFree = true;
                FreeCenter();
            }
            else
            // ゲーム終了判定
            if (Time.time - playStartTime >= GameConst.EndTime && !isFinishStarted)
            {
                // Debug.Log("Finish Game");
                isFinishStarted = true;
                FinishGame().Forget();
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Debug.Log("Finish Game");
                isFinishStarted = true;
                FinishGame().Forget();
            }
            
#endif
        }
        
        private async UniTask FinishGame()
        {
            await countDownManager.StartCountDownAndFinish();   
            TackleDodgeScoreCheckCancelTokenSource.Cancel();
            rightTackleDodgeScoreCheckCancelTokenSource.Cancel();
            leftTackleDodgeScoreCheckCancelTokenSource.Cancel();
            centerTackleDodgeScoreCheckCancelTokenSource.Cancel();
            SceneManager.LoadScene("Result");
        }

        private async UniTask CheckBuffaloAndOpenCage()
        {
            while (true)
            {
                if (buffalo.transform.position.z < 13f)
                {
                    centerCageAnimator.SetBool("CloseCage", true);
                    return;
                }
                await UniTask.Yield();
            }
        }
        
        private async UniTask FreeRight()
        {
            // Debug.Log("Free Right");
            rightCamera.Priority = 11;
            Time.timeScale = 0.01f;
            await UniTask.WaitForSeconds(1f, true);
            rightCage.SetActive(false);
            rightBuffalo.isBooted = true;
            await UniTask.WaitForSeconds(1f, true);
            rightCamera.Priority = 0;
            Time.timeScale = 1f;
        }

        private async UniTask FreeLeft()
        {
            // Debug.Log("Free Left");
            leftCamera.Priority = 11;
            Time.timeScale = 0.01f;
            await UniTask.WaitForSeconds(1f, true);
            leftCage.SetActive(false);
            leftBuffalo.isBooted = true;
            await UniTask.WaitForSeconds(1f, true);
            leftCamera.Priority = 0;
            Time.timeScale = 1f;
        }
        
        private async UniTask FreeCenter()
        {
            // Debug.Log("Free Center");
            centerBuffalo.isBooted = true;
            await UniTask.WaitForSeconds(6f, true);
            centerCamera.Priority = 11;
            Time.timeScale = 0.01f;
            playSceneSoundManager.PlayGameBgm2();
            await UniTask.WaitForSeconds(1f, true);
            centerCage.SetActive(false);
            await UniTask.WaitForSeconds(1f, true);
            centerCamera.Priority = 0;
            playSceneSoundManager.PlayBuffaloChargeSound();
            Time.timeScale = 1f;
        }

        private void subscribeAll()
        {
            buffalo.OnGetFlag.Subscribe(flag => 
                {
                    Debug.Log("Buffalo got flag: " + flag.index);
                    flagManager.PopFlag(flag);
                    flag.Destroy();
                    // スコア加算などの処理
                    score += GameConst.scoreIncreaseForFlag;
                    // EmergeFlagRandomPoint();
                    
                    // コンボ判定に使う
                    isGetFlagDuringTackle = true;
                }
            );
           
            buffalo.OnTackleStart.Subscribe(_ =>
            {
                TackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
                ScoreManager.CheckTackleDog(player.transform, player, buffalo.GetTackleScoreUp,
                    AddScore, playSceneSoundManager,TackleDodgeScoreCheckCancelTokenSource.Token);
            });
            rightBuffalo.OnTackleStart.Subscribe(_ =>
            {
                rightTackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
                ScoreManager.CheckTackleDog(player.transform, player, rightBuffalo.GetTackleScoreUp,
                    AddScore, playSceneSoundManager,rightTackleDodgeScoreCheckCancelTokenSource.Token);
            });
            leftBuffalo.OnTackleStart.Subscribe(_ =>
            {


                leftTackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
                ScoreManager.CheckTackleDog(player.transform, player, leftBuffalo.GetTackleScoreUp,
                    AddScore, playSceneSoundManager, leftTackleDodgeScoreCheckCancelTokenSource.Token);
            });
            centerBuffalo.OnTackleStart.Subscribe(_ =>
            {
                centerTackleDodgeScoreCheckCancelTokenSource = new CancellationTokenSource();
                ScoreManager.CheckTackleDog(player.transform, player, centerBuffalo.GetTackleScoreUp,
                    AddScore, playSceneSoundManager, centerTackleDodgeScoreCheckCancelTokenSource.Token);
            });
            buffalo.OnTackleEnd.Subscribe(_ =>
            {
                TackleDodgeScoreCheckCancelTokenSource?.Cancel();
            });
            rightBuffalo.OnTackleEnd.Subscribe(_ =>
            {
                rightTackleDodgeScoreCheckCancelTokenSource?.Cancel();
            });
            leftBuffalo.OnTackleEnd.Subscribe(_ =>
            {
                leftTackleDodgeScoreCheckCancelTokenSource?.Cancel();
            });
            centerBuffalo.OnTackleEnd.Subscribe(_ =>
            {
                centerTackleDodgeScoreCheckCancelTokenSource?.Cancel();
            });
            player.OnAttacked.Subscribe(_ =>
            {
                AddScore(GameConst.AttackedScoreDown);
                scoreUpText.ShowScoreDownText(((int)GameConst.AttackedScoreDown).ToString());
            });
        }
        
        public void AddScore(float scoreDelta)
        {
            if (scoreDelta > 0.1f)
            {
                comboCount++;
                scoreUpText.ShowScoreUpText("+"+(int)scoreDelta);
            }
            else if (scoreDelta < -0.1f)
            {
                comboCount = 0;
                scoreUpText.ShowScoreDownText(((int)scoreDelta).ToString());
                GameStats.damegeSum += (int)scoreDelta;
            }
            UpdateMaxComboCount();
            score += scoreDelta * GetComboBonus() * GetWalkBonus();
        }

        public void UpdateMaxComboCount()
        {
            GameStats.maxCombo = Mathf.Max(GameStats.maxCombo, comboCount);
        }

        public static float GetComboBonus()
        {
            return (float)comboCount / 20 + 1;
        }

        public float GetWalkBonus()
        {
            return !player.wasSoonDodged ? GameConst.DodgeBonus : 1f;
        }
        
        private void EmergeFlagRandomPoint()
        {
            Vector3 emergePos = new Vector3(Random.Range(-GameConst.StageEnd.x, GameConst.StageEnd.x),0f,Random.Range(-GameConst.StageEnd.y, GameConst.StageEnd.y));
            flagManager.AddFlag(new Flag(flagManager.GetFlags().Count, emergePos, Instantiate(flagPrefab, emergePos, Quaternion.identity)));
        }
    }
}