using Person;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Buffalo.Buffalo buffalo;
        [SerializeField] private Player player;
        
        [SerializeField] private GameObject flagPrefab;
        private FlagManager flagManager;
        
        public float score = 0;
        public int comboCount = 0;
        
        private bool isGetFlagDuringTackle = false;

        private void Awake()
        {
            Debug.Log("Null? player" + (player == null) + " playerTransform: " + (player.transform == null) + " buffalo: " + (buffalo == null) + " buffaloTransform: " + (buffalo.transform == null) + "");
            buffalo.playerTransform = player.transform;
            
            player.buffaloTransform = buffalo.transform;
        }

        private void Start()
        {
            buffalo.OnGetFlag.Subscribe(flag => 
                {
                    Debug.Log("Buffalo got flag: " + flag.index);
                    flagManager.PopFlag(flag);
                    flag.Destroy();
                    // スコア加算などの処理
                    score += GameConst.scoreIncreaseForFlag;
                    EmergeFlagRandomPoint();
                    
                    // コンボ判定に使う
                    isGetFlagDuringTackle = true;
                }
            );
            buffalo.OnTackleEnd.Subscribe(_ =>
            {
                if (isGetFlagDuringTackle)
                {
                    comboCount++;
                    score += GameConst.scoreIncreaseForCombo * comboCount;
                    buffalo.SpeedUpRate += GameConst.SpeedUpPerCombos;
                    isGetFlagDuringTackle = false;
                }
                else
                {
                    comboCount = 0;
                    buffalo.SpeedUpRate = 1f;
                }
            });
            flagManager = new FlagManager(buffalo, player);
            EmergeFlagRandomPoint();
        }

        private void EmergeFlagRandomPoint()
        {
            Vector3 emergePos = new Vector3(Random.Range(-GameConst.StageEnd.x, GameConst.StageEnd.x),0f,Random.Range(-GameConst.StageEnd.y, GameConst.StageEnd.y));
            flagManager.AddFlag(new Flag(flagManager.GetFlags().Count, emergePos, Instantiate(flagPrefab, emergePos, Quaternion.identity)));
        }
    }
}