using System.Collections.Generic;

public class TargetDatabase
{
    static Dictionary<int, TargetScript> targets = new Dictionary<int, TargetScript>();

    public static void RegisterTarget(int id, TargetScript target)
    {
        targets[id] = target;
    }

    public static void UnregisterTarget(int id)
    {
        targets.Remove(id);
    }

    public static TargetScript GetTarget(int id)
    {
        targets.TryGetValue(id, out TargetScript target);
        return target;
    }
}
