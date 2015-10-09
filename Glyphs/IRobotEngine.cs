namespace Glyphs
{
    public interface IRobotEngine
    {
        void Connect();

        // Остановка
        void Idle();

        // Движение вперед / назад
        void Run(int direction);

        // Поворот влево / вправо
        void Turn(int direction);

        // Стрельба
        void Shoot();
    }
}
