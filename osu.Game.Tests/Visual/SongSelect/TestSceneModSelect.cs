// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osu.Game.Overlays.Mods;

namespace osu.Game.Tests.Visual.SongSelect
{
    [TestFixture]
    public partial class TestSceneModSelect : TestScene
    {
        private ModSelectOverlay overlay = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = overlay = new UserModSelectOverlay(Overlays.OverlayColourScheme.Green);
        }


        [Test]
        public void DisplaySelect()
        {
            AddStep("Show mod select", () => overlay.Show());
        }
    }
}
