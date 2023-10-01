using _Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace UI.Popups
{
    public class PerformanceInfoPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text performanceName;
        [SerializeField] private TMP_Text composer;
        [SerializeField] private TMP_Text info;
        [SerializeField] private TMP_Text performanceTime;
        
        public void SetPerformanceCardInfo(PerformanceDataSO data)
        {
            //TODO
        }
    }
}