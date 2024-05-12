// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModSetBpm : ModRateAdjust, IApplicableToBeatmap
    {
        public override Type[] IncompatibleMods => [
            typeof(ModRateAdjust)
            ];
        public override string Name => "Set Bpm";
        public override string Acronym => "SB";
        public override ModType Type => ModType.Conversion;
        public override LocalisableString Description => "Adjust the speed to a set bpm";
        public override bool Ranked => false;
        public override BindableNumber<double> SpeedChange { get; } = new BindableDouble(1);

        [SettingSource("New BPM", "The actual decrease to apply", SettingControlType = typeof(SettingsSlider<int>))]

        public BindableNumber<int> NewBpm { get; } = new BindableNumber<int>(120)
        {
            MinValue = 1,
            MaxValue = 400,
            Precision = 1,
        };

        [SettingSource("Adjust pitch", "Should pitch be adjusted with speed")]
        public virtual BindableBool AdjustPitch { get; } = new BindableBool();

        private readonly RateAdjustModHelper rateAdjustHelper;

        private Bindable<double> beatmapBPM = new(120);


        protected ModSetBpm()
        {
            rateAdjustHelper = new RateAdjustModHelper(SpeedChange);
            rateAdjustHelper.HandleAudioAdjustments(AdjustPitch);
            NewBpm.BindValueChanged((v) =>
            {
                SpeedChange.Value = NewBpm.Value / beatmapBPM.Value;
            }, true);
            beatmapBPM.BindValueChanged((v) =>
            {
                SpeedChange.Value = NewBpm.Value / beatmapBPM.Value;
            }, true);
        }

        public override void ApplyToTrack(IAdjustableAudioComponent track)
        {
            SpeedChange.Value = NewBpm.Value / beatmapBPM.Value;
            rateAdjustHelper.ApplyToTrack(track);
        }

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            beatmapBPM.Value = beatmap.BeatmapInfo.BPM;
            SpeedChange.Value = NewBpm.Value / beatmapBPM.Value;
        }
        /*
        public override Mod DeepClone()
        {
            ModSetBpm mod = (ModSetBpm)base.DeepClone();
            mod.beatmapBPM = beatmapBPM;
            return mod;
        }
        */
        public override double ScoreMultiplier => rateAdjustHelper.ScoreMultiplier;
    }
}
