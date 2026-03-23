
using System.Collections.Generic;
using System.Linq;


    /// <summary>
    /// A single, stateless modifier to be applied at runtime.
    /// </summary>
    public class LiveOpsModifier
    {
        public ModifierType Type { get; }
        public float Value { get; }
        public string Source { get; }

        public LiveOpsModifier(ModifierType type, float value, string source)
        {
            Type = type;
            Value = value;
            Source = source; // e.g., "RemoteConfig", "EventBoost"
        }
    }

    /// <summary>
    /// Manages the collection and application of modifiers from various sources.
    /// This pipeline ensures that modifiers are applied in a predictable, non-destructive way.
    /// This is a conceptual class; its logic will be embedded within LiveOpsManager.
    /// </summary>
    public class LiveOpsModifierPipeline
    {
        private readonly Dictionary<ModifierType, List<LiveOpsModifier>> _modifiers = new Dictionary<ModifierType, List<LiveOpsModifier>>();

        public void AddModifier(LiveOpsModifier modifier)
        {
            if (!_modifiers.ContainsKey(modifier.Type))
            {
                _modifiers[modifier.Type] = new List<LiveOpsModifier>();
            }
            if (!_modifiers[modifier.Type].Any(m => m.Source == modifier.Source))
            {
                _modifiers[modifier.Type].Add(modifier);
            }
        }

        public void RemoveModifiersFromSource(string source)
        {
            foreach (var key in _modifiers.Keys)
            {
                _modifiers[key].RemoveAll(m => m.Source == source);
            }
        }

        public float GetCombinedMultiplier(ModifierType type)
        {
            if (!_modifiers.ContainsKey(type) || _modifiers[type].Count == 0)
            {
                return 1.0f;
            }
            return _modifiers[type].Aggregate(1.0f, (acc, mod) => acc * mod.Value);
        }

        public float GetDirectValue(ModifierType type, float defaultValue)
        {
            if (!_modifiers.ContainsKey(type) || _modifiers[type].Count == 0)
            {
                return defaultValue;
            }
            return _modifiers[type].Last().Value;
        }
    }

