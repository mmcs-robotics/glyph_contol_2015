using AForge.Vision.GlyphRecognition;

namespace Glyphs
{
    class GlyphDatabaseBuilder
    {
        public GlyphDatabase Database { get; private set; }

        public GlyphDatabaseBuilder()
        {
            Database = new GlyphDatabase(5);

            // 1
            Database.Add(new Glyph("Влево", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 1, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 2
            Database.Add(new Glyph("Вправо", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 3
            Database.Add(new Glyph("Вперед", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 4
            Database.Add(new Glyph("Назад", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 5
            Database.Add(new Glyph("Бум", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 1, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            //// 6
            //Database.Add(new Glyph("Динозавр", new byte[,]
            //{
            //    {0, 0, 0, 0, 0},
            //    {0, 1, 1, 1, 0},
            //    {0, 1, 0, 1, 0},
            //    {0, 1, 1, 1, 0},
            //    {0, 0, 0, 0, 0}
            //}));

            //// 7
            //Database.Add(new Glyph("Корова", new byte[,]
            //{
            //    {0, 0, 0, 0, 0},
            //    {0, 1, 1, 1, 0},
            //    {0, 0, 1, 0, 0},
            //    {0, 1, 1, 1, 0},
            //    {0, 0, 0, 0, 0}
            //}));

            //// 8
            //Database.Add(new Glyph("Дельфин", new byte[,]
            //{
            //    {0, 0, 0, 0, 0},
            //    {0, 0, 1, 0, 0},
            //    {0, 1, 1, 0, 0},
            //    {0, 1, 1, 0, 0},
            //    {0, 0, 0, 0, 0}
            //}));
        }
    }
}
