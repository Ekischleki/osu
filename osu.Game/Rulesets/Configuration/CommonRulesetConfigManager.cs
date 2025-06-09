// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Configuration;
using osu.Game.Configuration;

namespace osu.Game.Rulesets.Configuration
{
    public class CommonRulesetConfigManager : RulesetConfigManager<CommonRulesetConfig>
    {
        public CommonRulesetConfigManager(SettingsStore? store, RulesetInfo ruleset, int? variant = null) : base(store, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            SetDefault(CommonRulesetConfig.DifficultyFilterLowerBound, 0.0, 0, 10, 0.1);
            SetDefault(CommonRulesetConfig.DifficultyFilterUpperBound, 10.1, 0, 10.1, 0.1);
        }
    }

    public enum CommonRulesetConfig
    {
        DifficultyFilterLowerBound,
        DifficultyFilterUpperBound,
    }
}
