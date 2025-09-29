using TMPro;
using UnityEngine;

public class Score_Singelton : Singleton<Score_Singelton>
{

    public TMP_Text _score_txt;
    public TMP_Text _scrap_txt;

    int _score;
    int _scrap;

    private void Start()
    {
        _score = 0;
        _score_txt.text = "0";
        _scrap_txt.text = "0";
    }


    public void fn_AddToScore(int valueToAdd)
    {
        _score += valueToAdd;
        _score_txt.text = _score.ToString();
    }

    public void fn_AddToScrap(int valueToAdd)
    {
        _scrap += valueToAdd;
        _score_txt.text = _scrap.ToString();
    }



}
