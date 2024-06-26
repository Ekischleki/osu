// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Mods
{
    /// <summary>
    /// A mod that changes its settings/behaviour based on the beatmap it is applied to.
    /// </summary>
    public interface IRequiresBeatmapInfo
    {
        /// <summary>
        /// Called when the mod is being applied to a beatmap. This will be called before functions from other <see cref="IApplicableMod"/> interfaces will be called.
        /// </summary>
        /// <param name="beatmapInfo">The beatmap info of the beatmap this mod will be applied to</param>
        void AppliedToBeatmap(IBeatmapInfo beatmapInfo);
    }
}
