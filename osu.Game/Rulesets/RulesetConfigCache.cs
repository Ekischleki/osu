// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Configuration;
using osu.Game.Database;
using osu.Game.Extensions;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets
{
    public partial class RulesetConfigCache : Component, IRulesetConfigCache
    {
        private readonly RealmAccess realm;
        private readonly RulesetStore rulesets;

        private readonly Dictionary<string, IRulesetConfigManager?> configCache = new Dictionary<string, IRulesetConfigManager?>();
        private readonly Dictionary<string, CommonRulesetConfigManager> commonConfigCache = new Dictionary<string, CommonRulesetConfigManager>();

        public RulesetConfigCache(RealmAccess realm, RulesetStore rulesets)
        {
            this.realm = realm;
            this.rulesets = rulesets;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var settingsStore = new SettingsStore(realm);

            // let's keep things simple for now and just retrieve all the required configs at startup..
            foreach (var ruleset in rulesets.AvailableRulesets)
            {
                if (string.IsNullOrEmpty(ruleset.ShortName))
                    continue;

                var rulesetInstance = ruleset.CreateInstance();
                configCache[ruleset.ShortName] = rulesetInstance.CreateConfig(settingsStore);
                commonConfigCache[ruleset.ShortName] = new CommonRulesetConfigManager(settingsStore, ruleset);
            }
        }

        private T getConfigFrom<T>(RulesetInfo ruleset, Dictionary<string, T> dict)
        {
            if (!IsLoaded)
                throw new InvalidOperationException($@"Cannot retrieve {typeof(T).Name} before {nameof(RulesetConfigCache)} has loaded");
            if (!dict.TryGetValue(ruleset.ShortName, out var config))
                throw new InvalidOperationException($@"Attempted to retrieve {typeof(T).Name} for an unavailable ruleset {ruleset.GetDisplayString()}");

            return config;
        }

        public IRulesetConfigManager? GetConfigFor(Ruleset ruleset) => getConfigFrom(ruleset.RulesetInfo, configCache);

        public CommonRulesetConfigManager GetCommonConfigFor(RulesetInfo ruleset) => getConfigFrom(ruleset, commonConfigCache);

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            // ensures any potential database operations are finalised before game destruction.
            foreach (var c in configCache.Values)
                c?.Dispose();
        }
    }
}
