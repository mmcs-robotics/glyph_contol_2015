namespace Glyphs
{
    public class Robot
    {
        #region Ctor

        private readonly IRobotEngine _engine;

        public Robot(IRobotEngine engine)
        {
            _engine = engine;
            _engine.Connect();
        }

        #endregion

        public void Idle()
        {
            _engine.Idle();
        }

        public void Run(int direction)
        {
            _engine.Run(direction);
        }

        public void Turn(int direction)
        {
            _engine.Turn(direction);
        }

        public void Shoot()
        {
            _engine.Shoot();
        }
    }
}