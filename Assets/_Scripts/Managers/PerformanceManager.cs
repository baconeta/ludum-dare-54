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
        [Header("System References")]
        [SerializeField] private StageManager stageManager;
        [SerializeField] private AudioBuilderSystem audioBuilderSystem;

        [Header("Crowd Reaction Variables")]
        [Tooltip("If the player gets THIS score OR HIGHER, then the crowd will cheer.")]
        [SerializeField] private ReviewManager.StarRating cheerThreshold = ReviewManager.StarRating.Good;
        [Tooltip("If the player gets THIS score OR LOWER, then the crowd will boo.")]
        [SerializeField] private ReviewManager.StarRating booThreshold = ReviewManager.StarRating.Bad;
        [Tooltip("The time, in seconds, added to the end of the music audio before the crowd audibly reacts.")]
        [SerializeField] private float crowdReactionDelay = 0.25f;
        [Tooltip("The time, in seconds, that the crowd sits in silence if no reaction is played.")]
        [SerializeField] private float silenceDuration = 2.5f;
        [SerializeField] private AudioClip cheeringCrowdReaction;
        [SerializeField] private AudioClip booingCrowdReaction;

        [Header("Testing Variables")]
        [SerializeField] private PerformanceDataSO testPerformanceData;
        [SerializeField] private bool SkipPerformanceAudio;
        [SerializeField] private bool SkipCrowdReactionAudio;

        private PhaseManager phaseManager;
        private ReviewManager reviewManager;

        private PerformanceDataSO _thisPerformance;
        private AffinityScores _affinityScores;


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
            if(phaseManager)
                phaseManager.SetCurrentPhase(PhaseManager.GamePhase.Performance);

            float performanceDuration = audioBuilderSystem.PlayBuiltClips();
            StartCoroutine(EPerformance(performanceDuration + crowdReactionDelay));

            reviewManager.UpdatePerformanceData(_affinityScores, _thisPerformance.GetMaxScore(), _thisPerformance.GetMinScore());
        }

        private IEnumerator EPerformance(float performanceDuration)
        {
            if (SkipPerformanceAudio) Debug.LogWarning("Skipping performance audio.");
            yield return new WaitForSeconds(SkipPerformanceAudio ? 0 : performanceDuration);
            PlayCrowdReaction();
            yield return null;
        }

        private void PlayCrowdReaction()
        {
            float crowdReactionDuration;
            ReviewManager.StarRating perfQual = reviewManager.GetPerformanceRating();
            if (perfQual >= cheerThreshold)
            {
                audioBuilderSystem.AddClipToBuilder(cheeringCrowdReaction);
                // TODO select and build a crowd reaction.
                crowdReactionDuration = audioBuilderSystem.PlayBuiltClips();
            } else if (perfQual <= booThreshold)
            {
                audioBuilderSystem.AddClipToBuilder(booingCrowdReaction);
                // TODO select and build a crowd reaction.
                crowdReactionDuration = audioBuilderSystem.PlayBuiltClips();
            } else
            {
                // The crowd has no reaction, just silence.
                crowdReactionDuration = silenceDuration;
            }
            StartCoroutine(ECrowdReaction(crowdReactionDuration));
        }

        private IEnumerator ECrowdReaction(float crowdReactionDuration)
        {
            if (SkipCrowdReactionAudio) Debug.LogWarning("Skipping crowd reaction audio.");
            yield return new WaitForSeconds(SkipCrowdReactionAudio ? 0 : crowdReactionDuration);
            EndPerformance();
            yield return null;
        }

        private void EndPerformance()
        {
            // Notify other systems that the game state has changed.
            if(phaseManager) // Phase manager exists in normal scene
                phaseManager.SetCurrentPhase(PhaseManager.GamePhase.Review);

            if(TutorialController.IsTutorial) FindObjectOfType<TutorialController>().PerformanceEnded();
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
