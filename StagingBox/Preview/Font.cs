﻿using SkiaSharp;
using StagingBox.Utils;
using Topten.RichTextKit;

namespace StagingBox.Preview
{
    public static class Font
    {
        static Font()
        {
            CharacterMatcher.Initialize();
        }

        public class TextBlock : Topten.RichTextKit.TextBlock
        {
        }


        public class CharacterMatcher : ICharacterMatcher
        {
            private readonly SKTypeface[] typeFaces;

            static CharacterMatcher()
            {
                SKTypeface[] typeFaces =
                {
                    SKTypeface.FromStream(Resources.ReadEmbeddedFile(FunctionUtils.GetApplicationAssembly(), "Resources/Fonts/UbuntuMono-R.ttf")),
                    SKTypeface.FromStream(Resources.ReadEmbeddedFile(FunctionUtils.GetApplicationAssembly(), "Resources/Fonts/NotoSansCJKsc-Regular.otf"))
                };
                FontFallback.CharacterMatcher = new CharacterMatcher(typeFaces);
            }

            public CharacterMatcher(SKTypeface[] typeFaces)
            {
                this.typeFaces = typeFaces;
            }

            public SKTypeface? MatchCharacter(string familyName, int weight, int width, SKFontStyleSlant slant,
                string[] bcp47, int character)
            {
                foreach (SKTypeface typeFace in typeFaces)
                    if (typeFace.ContainsGlyph(character))
                        return typeFace;
                return null;
            }

            public static void Initialize()
            {
            }
        }
    }
}