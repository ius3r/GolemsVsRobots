using UnityEngine;

public sealed class TeamAffiliation : MonoBehaviour
{
    [SerializeField] private Team team = Team.Neutral;

    public Team Team => team;
}
