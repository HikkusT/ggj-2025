using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private int _startTime;
    [SerializeField] private Text _timeText;
    private int _currentTime;
    private float _timer = 0f;

    void Start()
    {
        _currentTime = _startTime;
    }
    
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1f && _currentTime > 0)
        {
            _timer = 0f;
            _currentTime -= 1;
            _timeText.text = _currentTime + " sec";
        }

        if (_currentTime <= 0)
        {
            _currentTime = 0;
        }
    }

    public void IncrementTime(int time)
    {
        _currentTime += time;
    }
}