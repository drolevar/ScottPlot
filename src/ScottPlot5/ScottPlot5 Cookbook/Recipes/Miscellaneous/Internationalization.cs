﻿namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class Internationalization : ICategory
{
    public string Chapter => "Miscellaneous";
    public string CategoryName => "Internationalization";
    public string CategoryDescription => "Using ScottPlot across cultures with different text and numeric requirements.";

    public class FontDetection : RecipeBase
    {
        public override string Name => "Supported Font Detection";
        public override string Description => "ScottPlot comes with font detection methods which help identify " +
            "the best installed font for displaying text which may contain international characters.";

        [Test]
        public override void Execute()
        {
            string chinese = "测试";
            myPlot.Axes.Title.Label.Text = chinese;
            myPlot.Axes.Title.Label.FontName = Fonts.Detect(chinese);

            string japanese = "試験";
            myPlot.Axes.Left.Label.Text = japanese;
            myPlot.Axes.Left.Label.FontName = Fonts.Detect(japanese);

            string korean = "테스트";
            myPlot.Axes.Bottom.Label.Text = korean;
            myPlot.Axes.Bottom.Label.FontName = Fonts.Detect(korean);
        }
    }

    public class AutomaticFontDetection : RecipeBase
    {
        public override string Name => "Automatic Font Detection";
        public override string Description => "The Plot's Style class contains a method " +
            "which automatically sets the fonts of common plot objects to the font " +
            "most likely able to display the characters they contain.";

        [Test]
        public override void Execute()
        {
            myPlot.Title("测试"); // Chinese
            myPlot.YLabel("試験"); // Japanese
            myPlot.XLabel("테스트"); // Korean
            myPlot.Style.SetBestFonts();
        }
    }
}
