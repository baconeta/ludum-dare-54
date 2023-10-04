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

        [Header("Testing Variables")]
        [SerializeField] private PerformanceDataSO testPerformanceData;
        [SerializeField] private bool SkipPerformanceAudio;
        [SerializeField] private bool SkipCrowdReactionAudio;

        private PhaseManager phaseManager;
        private ReviewManager reviewManager;
        public AudioWrapper audioWrapper;

        private PerformanceDataSO _thisPerformance;
        private AffinityScores _affinityScores;
        public static event Action OnPerformanceStarted; 


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
                Debug.LogWarning("PerformanceManager.cs couldn't get PhaseManager!");
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
                if (musician.GetAllMusicianData() == cm)
                {
                    _affinityScores.synergisticMusicianCount++;
                }
            }
            foreach (MusicianDataSO im in _thisPerformance.incorrectMusicians)
            {
                if (musician.GetAllMusicianData() == im)
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
                            musician.worldObject.UpdateAffinitySprite(true);
                        }
                    }
                    else // If they are, they will play terribly
                    {
                        AudioClip badClip = _thisPerformance.trackData.intentionalBadInstrumentPairsList
                            .Find(pairs => pairs.instrumentType == instrument.instrumentType).audioClip;
                        audioBuilderSystem.AddClipToBuilder(badClip ? badClip : instrument.data.backupBadClip);

                        // Musician-Instrument Fumble.
                        _affinityScores.instrumentFumbleCount++;
                        musician.worldObject.UpdateAffinitySprite(false);
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

            // Spawn affinity emotions above each musician's heads.
            foreach (MusicianDataSO musician in _thisPerformance.musicians)
            {
                StartCoroutine(EShowAffinityEmotes(musician));
            }

            // Populate the review manager with data needed for scoring.
            reviewManager.UpdatePerformanceData(_affinityScores, _thisPerformance.GetMaxScore(), _thisPerformance.GetMinScore());
            OnPerformanceStarted?.Invoke();
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
            ReviewManager.StarRating performanceRating = reviewManager.GetPerformanceRating();
            if (performanceRating >= cheerThreshold)
            {
                audioWrapper.PlaySoundVoid("CrowdCheer");
            } else if (performanceRating <= booThreshold)
            {
                audioWrapper.PlaySoundVoid("CrowdBoo");
            } else
            {
                audioWrapper.PlaySoundVoid("CrowdClap");
            }
            StartCoroutine(ECrowdReaction(3));
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

            if(TutorialController.IsTutorial) FindObjectOfType<TutorialController>()?.PerformanceEnded();
        }

        private IEnumerator EShowAffinityEmotes(MusicianDataSO musicianData)
        {
            int delay = UnityEngine.Random.Range(0, 4);
            yield return new WaitForSeconds(delay);
            Sprite emote ;
            foreach (var musician in stageManager.musiciansInRound)
            {
                if (musician.GetAllMusicianData() == musicianData)
                {
                    //musician.worldObject.affinityImage.sprite = emote;
                    musician.worldObject.affinityImage.gameObject.SetActive(true);
                }
            }
            yield return null;
        }

        public PerformanceDataSO GetCurrentPerformanceData()
        {
            return _thisPerformance;
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
