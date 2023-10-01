using System;
using System.Collections;
using _Scripts.Gameplay;
using Audio;
using UnityEngine;

namespace Managers
{
    public class PerformanceManager : MonoBehaviour
    {
        // Passed a particular performance data, let's set up the stage and performance system
        [SerializeField] private StageManager stageManager;
        [SerializeField] private AudioBuilderSystem audioBuilderSystem;
        [SerializeField] private PerformanceDataSO testPerformanceData;

        private PerformanceDataSO _thisPerformance;

        public static event Action<float> OnPerformanceComplete;

        private void OnEnable()
        {
            // Register for game events so we correctly associate data
            StagePlacement.OnInstrumentPlaced += StagePlacementOnInstrumentPlaced;
            StagePlacement.OnMusicianPlaced += StagePlacementOnInstrumentPlaced;
        }

        private void OnDisable()
        {
            StagePlacement.OnInstrumentPlaced -= StagePlacementOnInstrumentPlaced;
            StagePlacement.OnMusicianPlaced -= StagePlacementOnInstrumentPlaced;
        }

        private void StagePlacementOnInstrumentPlaced(StagePlacement placement)
        {
            // Handle only if the spot is ready (an instrument and musician is there).
            if (placement.IsOccupied() != (true, true))
            {
                return;
            }
            
            // Here we work out which track should be played based on the data we have and add it to the builder
            Musician musician = placement.GetMusician();
            Instrument instrument = placement.GetInstrument();

            // First - is this instrument part of this performance track?
            foreach (TrackInstrumentPairs correct in _thisPerformance.trackData.correctTrackInstrumentPairsList)
            {
                if (correct.instrumentType == instrument.instrumentType)
                {
                    // Then we just need to confirm this musician is not terrible at this instrument
                    if (!musician.GetAllMusicianData().badInstruments.Contains(correct.instrumentType))
                    {
                        audioBuilderSystem.AddClipToBuilder(correct.audioClip);
                    }
                    else // If they are, they will play terribly
                    {
                        AudioClip badClip = _thisPerformance.trackData.intentionalBadInstrumentPairsList
                            .Find(pairs => pairs.instrumentType == instrument.instrumentType).audioClip;
                        audioBuilderSystem.AddClipToBuilder(badClip ? badClip : instrument.data.backupBadClip);
                    }

                    return;
                }
            }

            //TODO What if there is no musician, instruments can be placed before musicians.
            // If this is not the correct musical instrument we use the backup clips only for now
            audioBuilderSystem.AddClipToBuilder(
                musician.GetAllMusicianData().badInstruments.Contains(instrument.instrumentType)
                    ? instrument.data.backupBadClip
                    : instrument.data.backupGoodClip);
        }

        public void SetUpPerformance(PerformanceDataSO performanceData)
        {
            if (stageManager is null)
            {
                Debug.LogError("Stage Manager is null so nothing can be set up");
                return;
            }

            _thisPerformance = performanceData;
            stageManager.ClearStage();
            stageManager.GenerateStage(_thisPerformance.trackData.numberOfMusiciansToPlay);

            foreach (MusicianDataSO musician in _thisPerformance.musicians)
            {
                stageManager.AddMusician(musician);
            }

            foreach (InstrumentDataSO instrument in _thisPerformance.instruments)
            {
                stageManager.AddInstrument(instrument);
            }
        }

        [ContextMenu("Test setup")]
        private void TestSetup()
        {
            if (testPerformanceData is not null)
            {
                SetUpPerformance(testPerformanceData);
            }
        }

        public void StartPerformance()
        {
            Debug.LogWarning("The Show is Starting!");
            float performanceDuration = audioBuilderSystem.PlayBuiltClips();
            StartCoroutine(EPerformance(performanceDuration));
        }

        IEnumerator EPerformance(float performanceDuration)
        {
            yield return new WaitForSeconds(performanceDuration);
            PerformanceComplete();
            yield return null;
        }

        public void PerformanceComplete()
        {
            Debug.LogWarning("The Show has Ended!");
            float rating = 69;
            OnPerformanceComplete?.Invoke(rating);
        }
    }
}