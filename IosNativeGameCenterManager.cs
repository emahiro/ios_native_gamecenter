using UnityEngine;
using System.Collections;

public class IosNativeGameCenterManager : MonoBehaviour {

	public bool DontDestroyObjectOnLoad = true;
	public const string LEADER_BOAD_ID = "xxxxxxx";

	// リーダーボード全体のスコア
	static long reportScore;
	public static GCLeaderboard leaderBoard;

	// LeaderBoard
	public static int userRange;

	// 個人データ
	public static string playerId;
	public static GCScore playerScoreData;

	public static void Auth()
	{
		// 認証のためProcessAuthentucationをコールバックとして登録
		Debug.Log("ゲームセンターの認証");

		GameCenterManager.OnAuthFinished += OnAuthFinished;
		GameCenterManager.init();
	}

	public static void OnAuthFinished(ISN_Result result)
	{
		if(result.IsSucceeded)
		{
			IOSNativePopUpManager.showMessage("Player Authored ", "ID: " + GameCenterManager.Player.PlayerId + "\n" + "Alias: " + GameCenterManager.Player.Alias);
		}
		else
		{
			IOSNativePopUpManager.showMessage("Game Center ", "player auth failed");
		}
	}

	// デフォルトのリーダーボードを表示する
	public static void ShowLeaderBoadUI()
	{
		GameCenterManager.ShowLeaderboard(LEADER_BOAD_ID);
	}

	void Start()
	{
		if(DontDestroyObjectOnLoad)
		{
			DontDestroyOnLoad(gameObject);
		}

		// スコアをロードする
		LoadScores ();
		ScoreListLoad ();
	}

	void Update()
	{
		
	}

	// リーダーボードにスコアを送信する
	public static void ReportScore(long score)
	{
		Debug.Log("スコア" + score + "を次のリーダーボードに報告します。" + LEADER_BOAD_ID);
		reportScore = score;
		SubmittedScore(reportScore);
	}

	public static void SubmittedScore(long score)
	{
		GameCenterManager.OnScoreSubmitted += OnScoreSubmitted;
		GameCenterManager.ReportScore(score, LEADER_BOAD_ID);
	}

	public static void OnScoreSubmitted(ISN_Result result)
	{
		GameCenterManager.OnScoreSubmitted -= OnScoreSubmitted;

		if(result.IsSucceeded)
		{
			Debug.Log("Score Submitted");
		}
		else
		{
			Debug.Log("Score submit failed");
		}
	}

	// スコア全体
	public static void LoadScores()
	{
		GameCenterManager.OnPlayerScoreLoaded += OnPlayerScoreLoaded;
		GameCenterManager.LoadCurrentPlayerScore(LEADER_BOAD_ID);
	}

	public static void OnPlayerScoreLoaded(ISN_PlayerScoreLoadedResult result)
	{
		GameCenterManager.OnPlayerScoreLoaded -= OnPlayerScoreLoaded;

		if(result.IsSucceeded)
		{
			playerScoreData = result.loadedScore;

			// playerIdの取得
			playerId = playerScoreData.playerId;

			// IDからユーザー名を取得
			GameCenterPlayerTemplate player = GameCenterManager.GetPlayerById(playerId);

			Debug.Log(
				"PlayerId"+ playerScoreData.playerId + "\n" +
				"PlayerName " + player.Alias + "\n" +
				"PlayerScore " + playerScoreData.GetLongScore().ToString() + "\n" +
				"PlayerRank " + playerScoreData.rank + "\n" +
				"PlayerImage" + player.Avatar);
		}

		Debug.Log("double score representation: " + playerScoreData.GetDoubleScore());
		Debug.Log("long score representation: " + playerScoreData.GetLongScore());
	}

	// リーダーボード全体のデータをロードする
	public static void ScoreListLoad()
	{
		// 1 ~ 100位までをロードする
		const int START  = 1;
		const int TO_END = 100;

		GameCenterManager.OnScoresListLoaded += OnScoresListLoaded;
		GameCenterManager.LoadScore(LEADER_BOAD_ID, START, TO_END, GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL);
	}

	public static void OnScoresListLoaded(ISN_Result result)
	{
		GameCenterManager.OnScoresListLoaded -= OnScoresListLoaded;

		if(result.IsSucceeded)
		{
			Debug.Log("Scores loaded");
			leaderBoard = GameCenterManager.GetLeaderboard(LEADER_BOAD_ID);

			// SocreListLoadで他のユーザーのデータを含んだleaderBoardを呼ぶ
			if(leaderBoard != null)
			{
				const int START = 1;
				const int TO_END = 100;

				for(int rankIndex = START; rankIndex < TO_END ; rankIndex++)
				{
					GCScore score = leaderBoard.GetScore(rankIndex, GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL);
					Debug.Log(score);

					if(score != null)
					{
						// userDataを取得するための変数を用意する。
						GameCenterPlayerTemplate user = GameCenterManager.GetPlayerById(score.playerId);

						if(user != null)
						{
							Debug.Log(
								"userId"+ score.playerId + "\n" +
								"UserName " + user.Alias + "\n" +
								"UserScore " + score.GetLongScore().ToString() + "\n" +
								"UserRank " + score.rank + "\n" +
								"UserImage" + user.Avatar);
						}
						else
						{
							// Nullの場合は処理しない
							Debug.Log("user is Null -> continue");
							continue;
						}
					}
					else
					{
						// Score == NULL でループを抜ける
						break;
					}
				}
			}
			else
			{
				Debug.Log("leaderBoard is Null");
			}
		}
		else
		{
			Debug.Log("Score Load failed");
		}
	}
}
