using _Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace UI.Popups
{
    public class PerformanceInfoPopup : MonoBehaviour
    {
        [SerializeField] private NightSelection nightSelection;
        
        [SerializeField] private TMP_Text performanceName;
        [SerializeField] private TMP_Text composer;
        [SerializeField] private TMP_Text info;
        [SerializeField] private TMP_Text performanceTime;

        private PerformanceDataSO _performanceData;
        
        public void SetPerformanceCardInfo(PerformanceDataSO data)
        {
            _performanceData = data;
            TrackDataSO trackData = data.trackData;
            performanceName.text = trackData.questName;
            composer.text = trackData.composerName;
            info.text = trackData.info;
            // performanceTime.text =  TODO later as data comes from night not performance
        }

        public void PressStartNightButton()
        {
            nightSelection.SelectNight(_performanceData);
        }
    }
}