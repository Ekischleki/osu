// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Concurrent;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Tests.Rulesets
{
    /// <summary>
    /// Test implementation of a <see cref="IRulesetConfigCache"/>, which ensures isolation between test scenes.
    /// </summary>
    public class TestRulesetConfigCache : IRulesetConfigCache
    {
        private readonly ConcurrentDictionary<string, IRulesetConfigManager?> configCache = new ConcurrentDictionary<string, IRulesetConfigManager?>();

        private readonly ConcurrentDictionary<string, CommonRulesetConfigManager> commonConfigCache = new ConcurrentDictionary<string, CommonRulesetConfigManager>();

        public CommonRulesetConfigManager GetCommonConfigFor(RulesetInfo ruleset) => commonConfigCache.GetOrAdd(ruleset.ShortName, _ => new CommonRulesetConfigManager(null, ruleset));

        public IRulesetConfigManager? GetConfigFor(Ruleset ruleset) => configCache.GetOrAdd(ruleset.ShortName, _ => ruleset.CreateConfig(null));
    }
}
