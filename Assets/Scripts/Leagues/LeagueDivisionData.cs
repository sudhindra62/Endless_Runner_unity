using System;
using System.Collections.Generic;

[Serializable]
public class LeagueDivisionData
{
    public string TierName;         // e.g., "Gold"
    public int TierLevel;           // e.g., 2 for "Gold II"
    public long EntryScore;           // Minimum score to be in this division
    public long PromotionScore;       // Score needed to enter promotion zone
    public long DemotionScore;        // Score below which enters demotion zone
}

[Serializable]
public class LeagueTierDefinition
{
    public List<LeagueDivisionData> Divisions;
}
