using System;
using System.Threading;
using NKH.MindSqualls;

namespace Glyphs
{
    // Управление роботом через стандартный интерфейс
    public class RobotEngine : IRobotEngine
    {
        #region Ctor

        private readonly NxtBrick _brick;
        private readonly NxtMotorSync _motorSync;

        public RobotEngine()
        {
            _brick = new NxtBrick(NxtCommLinkType.Bluetooth, Config.SerialPortNo)
            {
                MotorA = new NxtMotor(),
                MotorB = new NxtMotor(),
                MotorC = new NxtMotor()
            };

            _motorSync = new NxtMotorSync(_brick.MotorA, _brick.MotorB);
        }

        #endregion

        #region Private Methods

        private void ConnectSafe()
        {
            try
            {
                _brick.Connect();
            }
            catch (Exception e)
            {
                Logger.Warn(string.Format("Ошибка подключения к роботу: {0}", e.Message));
            }
        }

        private void ResetMotorSync()
        {
            _motorSync.Idle();
            _motorSync.ResetMotorPosition(true);
        }

        #endregion

        #region IRobotEngine Interface Implementation

        public void Connect()
        {
            ConnectSafe();

            while (!_brick.IsConnected)
            {
                Thread.Sleep(2000);
                ConnectSafe();
            }

            Logger.Success("Подключился к роботу");

            ResetMotorSync();
        }

        public void Idle()
        {
            _motorSync.Idle();
        }

        public bool IsConnected()
        {
            return _brick.IsConnected;
        }
        
        public void Turn(int direction)
        {
            _motorSync.Run(Config.TurnPower, 0, (sbyte) (Math.Sign(direction) * 100));
        }

        public void Run(int direction)
        {
            _motorSync.Run((sbyte) (Math.Sign(direction) * Config.RunPower), 0, 0);
        }

        public void Shoot()
        {
            _brick.MotorC.Run(Config.ShootPower, 3600);
        }

        #endregion
    }
}
