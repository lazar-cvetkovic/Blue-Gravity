using UnityEngine;
using EasyTransition;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] TransitionSettings[] _transitions;

    private TransitionSettings GetRandomTransition() => _transitions[Random.Range(0, _transitions.Length)];

    public void LoadScene(int index)
    {
        var transition = GetRandomTransition();
        TransitionManager.Instance.Transition(index, transition, 0.5f);
    }

    public void RestartScene()
    {
        var transition = GetRandomTransition();
        int index = SceneManager.GetActiveScene().buildIndex;
        TransitionManager.Instance.Transition(index, transition, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
