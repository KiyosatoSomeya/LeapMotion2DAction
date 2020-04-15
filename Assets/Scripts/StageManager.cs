using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private Animator uiAnim;

    [SerializeField]
    private Text titleText, timeText, missText, scoreText;

    public bool gameEnd = false;
    private int missCount = 0;

    public void IncMiss() {
        missCount++;
    }

    public void GameStart() {
        uiAnim.SetBool("Start", true);
    }

    public void GameEnd() {
        uiAnim.SetTrigger("End");
        gameEnd = true;

        // 以下表示更新
        titleText.text = "Stage Clear!";

        float clearTime = Time.timeSinceLevelLoad;
        timeText.text = ": " + clearTime.ToString("0.00");
        missText.text = ": " + missCount.ToString();
        int score = 20000 - (int)(clearTime * 100);  // 時間ポイント
        if(missCount == 0) {
            score += 2000;
        } else {
            score -= 100 * missCount;
        }

        if(score < 0) {
            score = 0;
        }

        score += 1000;

        scoreText.text = ": " + score.ToString();

        StartCoroutine(WaitKeyToRestart());
    }

    public void Restart() {
        SceneManager.LoadScene("HandControlGame");
    }

    public IEnumerator WaitKeyToRestart() {
        while (Input.anyKey) {
            yield return null;
        }
        while (!Input.anyKey) {
            yield return null;
        }

        Restart();
    }
}
