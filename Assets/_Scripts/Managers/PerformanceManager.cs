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

        private PhaseManager phaseManager;
        private ReviewManager reviewManager;

        private PerformanceDataSO _thisPerformance;
        private AffinityScores _affinityScores;

        [SerializeField] private bool SkipPerformanceAudio;

        private void OnEnable()
        {
            // Register for game events so we correctly associate data
            StagePlacement.OnInstrumentPlaced += OnStagePlacement;
            StagePlacement.OnMusicianPlaced += OnStagePlacement;
            NightSelection.OnPerformanceSelected += SetUpPerformance;

            // Get a reference to the phase manager.
            phaseManager = GetComponent<PhaseManager>();
            if (phaseManager == null)
            {
                Debug.LogError("PerformanceManager.cs couldn't get PhaseManager!");
            }
            // Get a reference to the review manager.
            reviewManager = GetComponent<ReviewManager>();
            if (reviewManager == null)
            {
                Debug.LogError("PerformanceManager.cs couldn't get ReviewManager!");
            }
        }

        private void OnDisable()
        {
            StagePlacement.OnInstrumentPlaced -= OnStagePlacement;
            StagePlacement.OnMusicianPlaced -= OnStagePlacement;
            NightSelection.OnPerformanceSelected -= SetUpPerformance;
        }

        private void OnStagePlacement(StagePlacement placement)
        {
            // Handle only if the spot is ready (an instrument and musician is there).
            if (placement.IsOccupied() != (true, true))
            {
                return;
            }

            // Here we work out which track should be played based on the data we have and add it to the builder
            Musician musician = placement.GetMusician();
            Instrument instrument = placement.GetInstrument();

            // Check for correct and incorrect musicians.
            foreach (MusicianDataSO cm in _thisPerformance.correctMusicians)
            {
                if (musician == cm)
                {
                    _affinityScores.synergisticMusicianCount++;
                }
            }
            foreach (MusicianDataSO im in _thisPerformance.incorrectMusicians)
            {
                if (musician == im)
                {
                    _affinityScores.unsuitableMusicianCount++;
                }
            }

            // First - is this instrument part of this performance track?
            foreach (TrackInstrumentPairs correct in _thisPerformance.trackData.correctTrackInstrumentPairsList)
            {
                if (correct.instrumentType == instrument.instrumentType)
                {
                    // Correctly selected instrument
                    _affinityScores.correctInstrumentCount++;
                    // Then we just need to confirm this musician is not terrible at this instrument
                    if (!musician.GetAllMusicianData().badInstruments.Contains(correct.instrumentType))
                    {
                        audioBuilderSystem.AddClipToBuilder(correct.audioClip);

                        // Check for Musician-Instrument Proficiency.
                        if (musician.GetAllMusicianData().proficientInstruments.Contains(correct.instrumentType))
                        {
                            _affinityScores.instrumentExpertiseCount++;
                        }
                    }
                    else // If they are, they will play terribly
                    {
                        AudioClip badClip = _thisPerformance.trackData.intentionalBadInstrumentPairsList
                            .Find(pairs => pairs.instrumentType == instrument.instrumentType).audioClip;
                        audioBuilderSystem.AddClipToBuilder(badClip ? badClip : instrument.data.backupBadClip);

                        // Musician-Instrument Fumble.
                        _affinityScores.instrumentFumbleCount++;
                    }

                    return;
                }
            }

            // If this is not the correct musical instrument we use the backup clips only for now            
            if (musician.GetAllMusicianData().badInstruments.Contains(instrument.instrumentType))
            {
                audioBuilderSystem.AddClipToBuilder(instrument.data.backupBadClip);
                // Musician-Instrument Fumble.
                _affinityScores.instrumentFumbleCount++;
            } else
            {
                audioBuilderSystem.AddClipToBuilder(instrument.data.backupGoodClip);
                // Check for Musician-Instrument Proficiency.
                if (musician.GetAllMusicianData().proficientInstruments.Contains(instrument.instrumentType))
                {
                    _affinityScores.instrumentExpertiseCount++;
                }
            }
        }

        public void SetUpPerformance(PerformanceDataSO performanceData)
        {
            if (stageManager is null)
            {
                Debug.LogError("Stage Manager is null so nothing can be set up");
                return;
            }

            _affinityScores = new AffinityScores();
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

        /**
         * Start the Performance phase.
         */
        public void StartPerformance()
        {
            // Notify other systems that the game state has changed.
            phaseManager.SetCurrentPhase(PhaseManager.GamePhase.Performance);

            // TODO Bug on second performance, CustomAudioSource in audioBuilderSystem is null.

            float performanceDuration = audioBuilderSystem.PlayBuiltClips();
            StartCoroutine(EPerformance(performanceDuration));

            reviewManager.UpdatePerformanceData(_affinityScores, _thisPerformance.GetMaxScore(), _thisPerformance.GetMinScore());
        }

        private IEnumerator EPerformance(float performanceDuration)
        {
            if (SkipPerformanceAudio) Debug.LogWarning("Skipping performance audio.");
            yield return new WaitForSeconds(SkipPerformanceAudio ? 0 : performanceDuration);
            EndPerformance();
            yield return null;
        }

        private void EndPerformance()
        {
            // Notify other systems that the game state has changed.
            phaseManager.SetCurrentPhase(PhaseManager.GamePhase.Review);
        }

        public struct AffinityScores
        {
            public int correctInstrumentCount;
            public int instrumentExpertiseCount;
            public int instrumentFumbleCount;
            public int synergisticMusicianCount;
            public int unsuitableMusicianCount;
        }
    }
}
