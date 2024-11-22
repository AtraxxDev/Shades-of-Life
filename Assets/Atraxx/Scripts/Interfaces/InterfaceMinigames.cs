using UnityEngine;

public interface IMinigame
{
    bool IsComplete { get; }
    void StartMinigame();
    void EndMinigame();
}