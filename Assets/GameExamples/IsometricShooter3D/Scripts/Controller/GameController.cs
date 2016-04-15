﻿using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{

    public class GameController : BaseGameController
    {

        #region Properties
        // Reference to our player.
        [SerializeField]
        PlayerModel player;
        public PlayerModel Player
        {
            get { return player; }
        }

        // Used to check if the player is camping in a spot.
        [SerializeField]
        float campingThresholdDistance = 1.5f;
        public float CampingThresholdDistance
        {
            get { return campingThresholdDistance; }
        }
        [SerializeField]
        float timeBetweenCampingChecks = 2f;
        public float TimeBetweenCampingChecks
        {
            get { return timeBetweenCampingChecks; }
            set { timeBetweenCampingChecks = value; }
        }

        float nextCampCheckTime;
        public float NextCampCheckTime
        {
            get { return nextCampCheckTime; }
            set { nextCampCheckTime = value; }
        }

        bool playerIsCamping;
        public bool PlayerIsCamping
        {
            get { return playerIsCamping; }
            set { playerIsCamping = value; }
        }

        Vector3 campPositionOld;
        public Vector3 CampPositionOld
        {
            get { return campPositionOld; }
            set { campPositionOld = value; }
        }

        // Number of enemies alive.
        [SerializeField]
        int enemiesAlive;
        public int EnemiesAlive
        {
            get { return enemiesAlive; }
        }

        bool gamePlaying = false;
        #endregion

        void OnEnable()
        {
            this.AddObserver(OnGameOverNotification, GameplayState.GameOverNotification);
            this.AddObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnGameOverNotification, GameplayState.GameOverNotification);
            this.RemoveObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        void OnGameOverNotification(object sender, object args)
        {
            gamePlaying = false;
        }

        void OnNextWaveNotification (object sender, object args)
        {
            gamePlaying = true;
        }

        void Update()
        {
            if (gamePlaying)
            {
                if (Time.time > nextCampCheckTime)
                {
                    nextCampCheckTime = Time.time + timeBetweenCampingChecks;
                    playerIsCamping = (Vector3.Distance(player.transform.position, campPositionOld) < campingThresholdDistance);
                    campPositionOld = player.transform.position;
                }
            }
        }

        public override void Start()
        {
            ChangeState<InitState>();
        }

        public void ModifyEnemyCount(int amount)
        {
            enemiesAlive += amount;
        }
    }
}
