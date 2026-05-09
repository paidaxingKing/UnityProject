using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterDuration = 0.1f;
    public bool CounterAttack()
    {
        foreach(Collider2D target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null) continue;
            if (counterable.canBeCountered)
            {
                counterable.BeCountered();
                return true;
            }
        }
        return false;
    }

    public float GetCounterDuration()
    {
        return counterDuration;
    }
}
