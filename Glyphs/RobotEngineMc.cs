using System;
using System.Threading;
using NKH.MindSqualls;
using NKH.MindSqualls.MotorControl;

namespace Glyphs
{
    // Управление роботом через MotorControl (не реализовано)
    class RobotEngineMc //: IRobotEngine
    {
        #region Ctor

        private readonly McNxtBrick _brick;
        private readonly McNxtMotorSync _motorSync;

        public RobotEngineMc()
        {
            _brick = new McNxtBrick(NxtCommLinkType.Bluetooth, Config.SerialPortNo)
            {
                MotorA = new McNxtMotor(),
                MotorB = new McNxtMotor()
            };

            _motorSync = new McNxtMotorSync(_brick.MotorA, _brick.MotorB);
        }

        #endregion

        #region Private Methods

        private void ResetMotorSync()
        {
            _motorSync.Idle();
            _motorSync.ResetMotorPosition(true);
        }

        private void ResetRunMotors()
        {
            _brick.MotorA.Idle();
            _brick.MotorA.ResetMotorPosition(true);
            _brick.MotorB.Idle();
            _brick.MotorB.ResetMotorPosition(true);
        }

        #endregion

        #region IRobotEngine Interface Implementation

        public void Connect()
        {
            do
            {
                _brick.Connect();
            } while (!_brick.IsConnected);
            
            ResetMotorSync();
        }

        public bool IsConnected()
        {
            return _brick.IsConnected;
        }

        public void Turn(double angle)
        {
            ////MotorControlProxy.CONTROLLED_MOTORCMD(_brick.CommLink, MotorControlMotorPort.PortA, "30", "30", '2');
            ////MotorControlProxy.CLASSIC_MOTORCMD(_brick.CommLink, MotorControlMotorPort.PortA,
            ////    Config.TurnPower.ToString(), tachoLimit.ToString(), '0');
            //MotorControlProxy.CLASSIC_MOTORCMD(_brick.CommLink, MotorControlMotorPort.PortB,
            //    "130", tachoLimit.ToString(), '0');

            //_motorSync.Run(Config.TurnPower, tachoLimit, (sbyte)(Math.Sign(angle) * 100));

            //_brick.MotorA.Run((sbyte)(Math.Sign(angle) * Config.TurnPower), tachoLimit);
            //_brick.MotorB.Run((sbyte)(-Math.Sign(angle) * Config.TurnPower), tachoLimit);
        }

        public void Run(double units)
        {
            _motorSync.Run(Config.RunPower, 0, 0);
        }

        #endregion
    }
}
