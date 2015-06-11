# GameCenterにて独自リーダーボードを作る

1. [iOSNativePlugin](https://www.assetstore.unity3d.com/en/#!/content/7421)をDLする
1. Unityの該当のプロジェクトにImportする。

実装方法の詳細はUnionAssetsのiOSのGameCenterの[ドキュメント](https://unionassets.com/iosnative/manage-game-center-7)を参照

本パッケージの簡単な使い方。
Unityのパッケージファイルを展開後、iOS_GC_Controlerをプロジェクトのヒエラルキー上に配置する。

##メソッドの説明


1. Auth()  
ゲームセンターの認証
1. ShowLeaderBoadUI()  
iOSのデフォルトのリーダーボードを表示する
1. ReportScore(long score)  
スコアを送信する。記録するときに使用。
1. LoadScores（）  
プレイヤーのスコアとそれに紐づくデータを取得する
1. ScoreListLoad()  
リーダーボード全体のスコアとそれに紐づくユーザーのデータを取得する。100位まで取得可能。但し取得する際は必ず1位からしか取得できず
途中から取得するということはできない。
